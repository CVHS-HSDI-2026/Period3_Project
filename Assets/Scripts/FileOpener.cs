using UnityEngine;
using UnityEngine.SceneManagement;
using SFB;
using System.IO;
using System.Diagnostics;
using TMPro;

public class FileOpener : MonoBehaviour
{
    [Header("UI References")]
    public GameObject playButton;
    public TMP_Text statusText;

    private string selectedMp3Path;
    private string generatedJsonPath;

    public void OpenFile()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel(
            "Select a Song", "",
            new[] { new ExtensionFilter("Audio Files", "mp3", "wav", "flac", "ogg") },
            false
        );

        if (paths.Length == 0) return;

        selectedMp3Path = paths[0];
        string songName = Path.GetFileNameWithoutExtension(selectedMp3Path);
        generatedJsonPath = Path.Combine(
            Path.GetDirectoryName(selectedMp3Path),
            songName + "_rhythm.json"
        );

        statusText.text = $"Selected: {songName}\nGenerating rhythm map...";
        playButton.SetActive(false);

        GenerateRhythmMap();
    }

    void GenerateRhythmMap()
    {
        // Check if JSON already exists — skip generation if so
        if (File.Exists(generatedJsonPath))
        {
            statusText.text = "Rhythm map already exists!\nReady to play.";
            playButton.SetActive(true);
            return;
        }

        // Run the Python script to generate the JSON
        string pythonScript = Path.Combine(Application.streamingAssetsPath,
                                           "rhythm_generator.py");

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = $"\"{pythonScript}\" \"{selectedMp3Path}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        try
        {
            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0 && File.Exists(generatedJsonPath))
                {
                    statusText.text = "Rhythm map generated!\nReady to play.";
                    playButton.SetActive(true);
                }
                else
                {
                    statusText.text = $"Failed to generate rhythm map.\n{error}";
                    UnityEngine.Debug.LogError(error);
                }
            }
        }
        catch (System.Exception e)
        {
            statusText.text = "Python not found. Make sure Python is installed.";
            UnityEngine.Debug.LogError(e.Message);
        }
    }

    public void StartGame()
    {
        // Pass paths to the next scene via a static holder
        SongDataHolder.mp3Path = selectedMp3Path;
        SongDataHolder.jsonPath = generatedJsonPath;
        SceneManager.LoadScene("GameScene");
    }
}