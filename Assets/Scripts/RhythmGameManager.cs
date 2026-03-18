using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmGameManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject notePrefab;
    public Transform spawnPoint;
    public AudioSource musicSource;

    [Header("Timing")]
    public float approachTime = 2.0f;
    public float hitWindow = 0.2f;

    private List<RhythmNote> upcomingNotes = new List<RhythmNote>();
    private int nextNoteIndex = 0;
    private float songStartTime;
    private bool gameStarted = false;

    // Called by SongLoader once everything is ready
    public void StartGame(RhythmData rhythmData, AudioClip clip)
    {
        // Filter notes that are too close together
        upcomingNotes.Clear();
        nextNoteIndex = 0;
        gameStarted = false;

        float lastTime = -1f;
        foreach (var note in rhythmData.notes)
        {
            if (note.time - lastTime >= 0.2f)
            {
                upcomingNotes.Add(note);
                lastTime = note.time;
            }
        }

        // Set scroll speed based on screen distances and approach time
        float spawnY = spawnPoint.position.y;
        float hitZoneY = -4f;
        float distance = spawnY - hitZoneY;
        NoteObject.scrollSpeed = distance / approachTime;

        musicSource.clip = clip;
        StartCoroutine(BeginPlayback());
    }

    IEnumerator BeginPlayback()
    {
        yield return new WaitForSeconds(0.5f);
        musicSource.Play();
        songStartTime = Time.time - 0.5f;
        gameStarted = true;
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

            float noteY = note.transform.position.y;
            if (noteY > -2f || noteY < -6f) continue;

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