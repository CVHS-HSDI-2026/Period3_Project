using System.Collections;
using UnityEngine;
using TMPro;
using System.IO;

public class SongLoader : MonoBehaviour
{
    [Header("References")]
    public RhythmGameManager gameManager;
    public TMP_Text statusText;

    void Start()
    {
        if (string.IsNullOrEmpty(SongDataHolder.mp3Path))
        {
            statusText.text = "No song selected.\nReturning to menu...";
            StartCoroutine(ReturnToMenu());
            return;
        }

        StartCoroutine(LoadSong());
    }

    IEnumerator LoadSong()
    {
        statusText.text = "Loading...";

        // Load JSON
        string json = File.ReadAllText(SongDataHolder.jsonPath);
        RhythmData rhythmData = JsonUtility.FromJson<RhythmData>(json);

        // Load audio
        string audioUri = new System.Uri(SongDataHolder.mp3Path).AbsoluteUri;
        AudioType audioType = GetAudioType(SongDataHolder.mp3Path);

        using (var www = new UnityEngine.Networking.UnityWebRequest(audioUri))
        {
            www.downloadHandler = new UnityEngine.Networking.DownloadHandlerAudioClip(
                audioUri, audioType
            );
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                AudioClip clip = UnityEngine.Networking.DownloadHandlerAudioClip
                    .GetContent(www);
                statusText.text = "";
                gameManager.StartGame(rhythmData, clip);
            }
            else
            {
                statusText.text = $"Failed to load audio:\n{www.error}";
            }
        }
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SongSelectionScene");
    }

    AudioType GetAudioType(string path)
    {
        string ext = Path.GetExtension(path).ToLower();
        return ext switch
        {
            ".mp3" => AudioType.MPEG,
            ".wav" => AudioType.WAV,
            ".ogg" => AudioType.OGGVORBIS,
            _ => AudioType.UNKNOWN
        };
    }
}