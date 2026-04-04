using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;

public class RhythmGameManager : MonoBehaviour
{
    [System.Serializable]
    public class ResponseData
    {
        public string message;
        public int score;
    }
    [System.Serializable]
    public class SaveData
    {
        public List<song> items;
    }

    [System.Serializable]
    public class song
    {
        public string itemName;
        public string savedPath;
        public string savedJsonPath;
    }
    [Header("Setup")]
    public GameObject notePrefab;
    public Transform spawnPoint;
    public AudioSource musicSource;

    [Header("Song Files")]


    public string mp3FileName;
    public string jsonFileName = "01. Bad Apple!!.json";

    [Header("Timing")]
    public float approachTime = 2.0f;
    public float hitWindow = 0.2f;

    private List<float> upcomingEvents = new List<float>();
    private int nextEventIndex = 0;
    private float songStartTime;
    private bool gameStarted = false;
    private int score = 0;

    public static event Action OnPerfect;
    public static event Action OnGood;
    public static event Action OnOk;
    public static event Action OnMiss;
    public static event Action<int> OnScoreChanged;

    void Start()
    {
        PlayerPrefs.SetInt("current_score", 0);
        LoadRhythmData();
        StartCoroutine(LoadAudioAndStart());
    }

    void LoadRhythmData()
    {
        string readjson = File.ReadAllText(Application.persistentDataPath + "/save.json");
        SaveData loadedData = JsonUtility.FromJson<SaveData>(readjson);
        if (loadedData == null)
        {
            loadedData = new SaveData();
            loadedData.items = new List<song>();
        }

        if (loadedData.items == null)
        {
            loadedData.items = new List<song>();
        }
        
        string fileName = loadedData.items[PlayerPrefs.GetInt("currentSong")].itemName;
        mp3FileName = fileName;
        jsonFileName = fileName + ".json";
        string path = Path.Combine(Application.persistentDataPath, jsonFileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"JSON not found: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        RhythmData rhythmData = JsonUtility.FromJson<RhythmData>(json);

        // Filter events too close together
        float lastTime = -1f;
        foreach (float t in rhythmData.events)
        {
            if (t - lastTime >= 0.2f)
            {
                upcomingEvents.Add(t);
                lastTime = t;
            }
        }

        Debug.Log($"Loaded {upcomingEvents.Count} events at {rhythmData.tempo} BPM");
    }

    IEnumerator LoadAudioAndStart()
    {
        string filePath = Path.Combine(Application.persistentDataPath, mp3FileName);
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

        while (nextEventIndex < upcomingEvents.Count &&
               upcomingEvents[nextEventIndex] <= songTime + approachTime)
        {
            SpawnNote(upcomingEvents[nextEventIndex]);
            nextEventIndex++;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryHit(songTime);
        }
    }

    void SpawnNote(float eventTime)
    {
        GameObject noteObj = Instantiate(notePrefab, spawnPoint.position,
                                         Quaternion.identity);
        noteObj.layer = LayerMask.NameToLayer("NoteBar");
        NoteObject note = noteObj.GetComponent<NoteObject>();
        note.targetTime = eventTime;
        note.lane = 0;   
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
        if (timingDiff < 0.05f) { 
            //Debug.Log("PERFECT!");
            score += 3;
            OnPerfect?.Invoke();
        }
        else if (timingDiff < 0.1f) { 
            //Debug.Log("GOOD");
            score += 2;
            OnGood?.Invoke();
        }
        else { 
            //Debug.Log("OK");
            score += 1;
            OnOk?.Invoke();
        }

        //PlayerPrefs.SetInt("current_score", score);
        OnScoreChanged?.Invoke(score);
    }

    void RegisterMiss()
    {
        OnMiss.Invoke();
    }
}