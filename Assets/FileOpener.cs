using UnityEngine;
using SFB;
using System.IO;

public class FileOpener : MonoBehaviour
{
    public void OpenFile()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Select File", "", "", false);

        if (paths.Length > 0)
        {
            string path = paths[0];
            Debug.Log("Selected file: " + path);

            // Example: read file text
            string fileContent = File.ReadAllText(path);
            Debug.Log(fileContent);
        }
    }
}
