import librosa
import numpy as np
import io
import pretty_midi
import midiutil
import json
import os

SUPPORTED_FORMATS = ('.wav', '.mp3', '.flac', '.ogg', '.m4a')

audio_file = "memphis-trap-memphis-trap-wav-349366.wav"

def validate_audio_file(file_path):
    if not os.path.exists(file_path):
        raise FileNotFoundError(f"Audio file not found: {file_path}")
    ext = os.path.splitext(file_path)[1].lower()
    if ext not in SUPPORTED_FORMATS:
        raise ValueError(
            f"Unsupported audio format '{ext}'. "
            f"Supported formats: {', '.join(SUPPORTED_FORMATS)}"
        )


def separate_components(signal):
    """Separate harmonic and percussive components using HPSS."""
    harmonic, percussive = librosa.effects.hpss(signal, margin=3.0)
    return harmonic, percussive


def detect_onsets(percussive, sample_rate, hop_length=512):
    """Detect percussive onsets."""
    onset_strength = librosa.onset.onset_strength(
        y=percussive,
        sr=sample_rate,
        hop_length=hop_length,
        aggregate=np.median
    )

    onset_frames = librosa.onset.onset_detect(
        onset_envelope=onset_strength,
        sr=sample_rate,
        hop_length=hop_length,
        backtrack=True,
        pre_max=3,
        post_max=3,
        pre_avg=5,
        post_avg=5,
        delta=0.2,
        wait=10
    )

    onset_times = librosa.frames_to_time(
        onset_frames, sr=sample_rate, hop_length=hop_length
    )
    onset_strengths = onset_strength[onset_frames]

    return onset_times, onset_strengths


def classify_hit(freq_profile, freqs):
    """
    Classify hit by realistic drum frequency ranges:
        Kick   = 20-200 Hz
        Snare  = 200-2000 Hz
        HiHat  = 2000+ Hz
    """
    low  = np.mean(freq_profile[(freqs >= 20) & (freqs < 200)])
    mid  = np.mean(freq_profile[(freqs >= 200) & (freqs < 2000)])
    high = np.mean(freq_profile[(freqs >= 2000)])

    dominant = np.argmax([low, mid, high])

    return dominant  # 0=kick, 1=snare, 2=hi-hat


def snap_to_beat_grid(onset_times, beat_times, tolerance=0.05):
    snapped = []
    for t in onset_times:
        nearest = beat_times[np.argmin(np.abs(beat_times - t))]
        if abs(nearest - t) <= tolerance:
            snapped.append(float(nearest))
        else:
            snapped.append(float(t))
    return snapped


def scale_difficulty(notes, difficulty="medium"):
    if difficulty == "hard" or not notes:
        return notes

    strengths = np.array([n["strength"] for n in notes])
    percentile = {"easy": 60, "medium": 35}.get(difficulty, 0)
    threshold = np.percentile(strengths, percentile)

    return [n for n in notes if n["strength"] >= threshold]


def wav_to_rhythm_notes(audio_file, hop_length=512,
                        difficulty="medium", snap_to_beats=True):

    validate_audio_file(audio_file)

    signal, sample_rate = librosa.load(
        audio_file, sr=None, res_type='kaiser_fast'
    )

    harmonic, percussive = separate_components(signal)

    # ---- Beat tracking ----
    tempo, beat_frames = librosa.beat.beat_track(
        y=signal, sr=sample_rate
    )
    tempo = float(np.atleast_1d(tempo)[0])
    beat_times = librosa.frames_to_time(
        beat_frames, sr=sample_rate, hop_length=hop_length
    )

    # ---- Onset detection ----
    onset_times, onset_strengths = detect_onsets(
        percussive, sample_rate, hop_length
    )

    if snap_to_beats:
        onset_times = snap_to_beat_grid(onset_times, beat_times)

    # ---- Normalize strengths 0-1 ----
    if len(onset_strengths) > 0:
        min_s, max_s = onset_strengths.min(), onset_strengths.max()
        if max_s > min_s:
            onset_strengths = (onset_strengths - min_s) / (max_s - min_s)
        else:
            onset_strengths = np.ones_like(onset_strengths)

    # ---- Precompute STFT once (performance fix) ----
    stft = np.abs(librosa.stft(percussive, hop_length=hop_length))
    freqs = librosa.fft_frequencies(sr=sample_rate)

    notes = []

    for t, strength in zip(onset_times, onset_strengths):

        frame = librosa.time_to_frames(
            t, sr=sample_rate, hop_length=hop_length
        )

        start = max(0, frame - 4)
        end = min(stft.shape[1], frame + 4)

        freq_profile = np.mean(stft[:, start:end], axis=1)

        lane = classify_hit(freq_profile, freqs)

        velocity = int(60 + strength * 67)

        notes.append({
            "time": float(round(t, 4)),
            "lane": int(lane),
            "lane_name": ["kick", "snare", "hi-hat"][int(lane)],
            "velocity": int(velocity),
            "strength": float(strength)
        })

    notes = scale_difficulty(notes, difficulty)

    # Remove duplicate time/lane combos
    seen = set()
    unique_notes = []
    for note in notes:
        key = (note["time"], note["lane"])
        if key not in seen:
            seen.add(key)
            unique_notes.append(note)

    return {
        "tempo": float(round(tempo, 2)),
        "beat_times": [float(round(b, 4)) for b in beat_times],
        "difficulty": difficulty,
        "note_count": int(len(unique_notes)),
        "notes": unique_notes
    }


def rhythm_notes_to_midi(rhythm_data):
    """Convert rhythm data to proper-timed MIDI."""
    midi = midiutil.MIDIFile(1)
    track = 0
    midi.addTempo(track, 0, rhythm_data["tempo"])

    seconds_per_beat = 60.0 / rhythm_data["tempo"]

    lane_to_midi = {
        0: 36,  # Kick
        1: 38,  # Snare
        2: 42   # Hi-hat
    }

    for note in rhythm_data["notes"]:

        time_in_beats = note["time"] / seconds_per_beat

        midi.addNote(
            track=track,
            channel=9,
            pitch=lane_to_midi[note["lane"]],
            time=time_in_beats,
            duration=0.25,
            volume=note["velocity"]
        )

    midi_buffer = io.BytesIO()
    midi.writeFile(midi_buffer)
    return midi_buffer.getvalue()


# ------------------ MAIN EXECUTION ------------------

#try:
#    rhythm_data = wav_to_rhythm_notes(
#        audio_file,
#        difficulty="medium",
#        snap_to_beats=True
#    )
#
#    output_path = os.path.splitext(audio_file)[0] + "_rhythm.json"
#    with open(output_path, "w") as f:
#        json.dump(rhythm_data, f, indent=4)
#
#    print(f"Done! Rhythm notes written to {output_path}")
#    print(f"Tempo: {rhythm_data['tempo']} BPM")
#    print(f"Notes: {rhythm_data['note_count']}")
#
#    midi_bytes = rhythm_notes_to_midi(rhythm_data)
#    midi_path = os.path.splitext(audio_file)[0] + "_rhythm.mid"
#
#    with open(midi_path, "wb") as f:
#        f.write(midi_bytes)
#
#    print(f"MIDI written to {midi_path}")
#
#except Exception as e:
#    print(f"Error: {e}")
#    raise
