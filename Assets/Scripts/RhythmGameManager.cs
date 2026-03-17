using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class RhythmGameManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject notePrefab;
    public Transform spawnPoint;
    public AudioSource musicSource;

    [Header("Timing")]
    public float approachTime = 2.0f;
    public float hitWindow = 0.2f;

    private RhythmData rhythmData;
    private List<RhythmNote> upcomingNotes;
    private int nextNoteIndex = 0;
    private float songStartTime;
    private bool gameStarted = false;

    void Start()
    {
        LoadRhythmData();
        StartCoroutine(LoadAudioAndStart());
    }

    void LoadRhythmData()
    {
        string path = Path.Combine(Application.streamingAssetsPath,
                                   "01. Bad Apple!!_rhythm.json");
        string json = File.ReadAllText(path);
        rhythmData = JsonUtility.FromJson<RhythmData>(json);

        // Filter notes too close together
        var filtered = new List<RhythmNote>();
        float lastTime = -1f;
        foreach (var note in rhythmData.notes)
        {
            if (note.time - lastTime >= 0.2f)
            {
                filtered.Add(note);
                lastTime = note.time;
            }
        }

        upcomingNotes = filtered;
        Debug.Log($"Loaded {upcomingNotes.Count} notes (filtered from {rhythmData.note_count}) at {rhythmData.tempo} BPM");
    }

    IEnumerator LoadAudioAndStart()
    {
        string filePath = Path.Combine(
            Application.streamingAssetsPath, "01. Bad Apple!!.mp3"
        );
        string audioPath = new System.Uri(filePath).AbsoluteUri;

        using (var www = new UnityEngine.Networking.UnityWebRequest(audioPath))
        {
            www.downloadHandler = new UnityEngine.Networking.DownloadHandlerAudioClip(
                audioPath, AudioType.MPEG
            );
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                musicSource.clip = UnityEngine.Networking.DownloadHandlerAudioClip
                    .GetContent(www);

                // Calculate horizontal scroll speed
                float spawnX = spawnPoint.position.x;
                float hitZoneX = 8f;
                float distance = hitZoneX - spawnX;
                NoteObject.scrollSpeed = distance / approachTime;

                Debug.Log($"Scroll speed: {NoteObject.scrollSpeed} units/sec");

                yield return new WaitForSeconds(0.5f);
                musicSource.Play();
                songStartTime = Time.time - 0.5f;
                gameStarted = true;
            }
            else
            {
                Debug.LogError("Failed to load audio: " + www.error);
            }
        }
    }

    void Update()
    {
        if (!gameStarted) return;

        float songTime = Time.time - songStartTime;

        while (nextNoteIndex < upcomingNotes.Count &&
               upcomingNotes[nextNoteIndex].time <= songTime + approachTime)
        {
            SpawnNote(upcomingNotes[nextNoteIndex]);
            nextNoteIndex++;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryHit(songTime);
        }
    }

    void SpawnNote(RhythmNote noteData)
    {
        GameObject noteObj = Instantiate(notePrefab, spawnPoint.position,
                                         Quaternion.identity);
        NoteObject note = noteObj.GetComponent<NoteObject>();
        note.targetTime = noteData.time;
        note.lane = noteData.lane;
    }

    void TryHit(float songTime)
    {
        NoteObject[] activeNotes = FindObjectsOfType<NoteObject>();
        NoteObject bestNote = null;
        float bestDiff = hitWindow;

        foreach (var note in activeNotes)
        {
            if (note.wasHit) continue;

            float noteX = note.transform.position.x;
            if (noteX < 6f || noteX > 10f) continue;

            float diff = Mathf.Abs(note.targetTime - songTime);
            if (diff < bestDiff)
            {
                bestDiff = diff;
                bestNote = note;
            }
        }

        if (bestNote != null)
        {
            bestNote.wasHit = true;
            RegisterHit(bestDiff);
            Destroy(bestNote.gameObject);
        }
        else
        {
            RegisterMiss();
        }
    }

    void RegisterHit(float timingDiff)
    {
        if (timingDiff < 0.05f) Debug.Log("PERFECT!");
        else if (timingDiff < 0.1f) Debug.Log("GOOD");
        else Debug.Log("OK");
    }

    void RegisterMiss()
    {
        Debug.Log("MISS");
    }
}