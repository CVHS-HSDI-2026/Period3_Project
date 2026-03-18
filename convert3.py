import librosa
import numpy as np
import json
import os

hop_length = 256
def run_audio(audio_file):


    y, sr = librosa.load(audio_file, sr=None)

    _, percussive = librosa.effects.hpss(y)

    onset_env = librosa.onset.onset_strength(
        y=percussive,
        sr=sr,
        hop_length=hop_length
    )

    tempo_candidates = librosa.feature.tempo(
        onset_envelope=onset_env,
        sr=sr,
        hop_length=hop_length,
        aggregate=None
    )

    tempo = float(np.median(tempo_candidates))

    if tempo < 100:
        tempo *= 2

    tempo, beat_frames = librosa.beat.beat_track(
        onset_envelope=onset_env,
        sr=sr,
        hop_length=hop_length,
        bpm=tempo
    )

    tempo = float(np.atleast_1d(tempo)[0])

    beat_times = librosa.frames_to_time(
        beat_frames,
        sr=sr,
        hop_length=hop_length
    )

    events = []

    seconds_per_beat = 60.0 / tempo

    for i in range(len(beat_times) - 1):

        start = beat_times[i]
        end = beat_times[i + 1]

        events.append(start)

        events.append(start + (end - start) * 0.5)

    events.append(beat_times[-1])

    events = sorted(list(set(events)))

    output = {
        "tempo": round(tempo, 2),
        "events": [float(round(e, 4)) for e in events]
    }

    json_path = os.path.splitext(audio_file)[0] + "_rhythm.json"

    #with open(json_path, "w") as f:
    #    json.dump(output, f, indent=4)
    return output


    print("Tempo:", tempo)
    print("Events:", len(events))
    print("Saved:", json_path)
