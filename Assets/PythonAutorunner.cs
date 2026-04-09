using UnityEngine;
using System.Diagnostics;
using System.IO;

public class PythonAutoRunner : MonoBehaviour
{
    void Start()
    {
        // This runs automatically when the game starts
        RunPythonExe();
    }

    void RunPythonExe()
    {
        // 1. Get the path to your .exe in StreamingAssets
        string exePath = Path.Combine(Application.streamingAssetsPath, "api.exe");

        // Check if file exists to avoid errors
        if (!File.Exists(exePath))
        {
            UnityEngine.Debug.LogError("Python .exe not found at: " + exePath);
            return;
        }

        // 2. Setup the process
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = exePath;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true; // Hidden background process

        // 3. Launch
        Process.Start(startInfo);
        UnityEngine.Debug.Log("Python process started automatically.");
    }
}