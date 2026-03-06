using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using UI = UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
public class SongSelectionSceneUIManager : MonoBehaviour
{
    private string serverurl = " http://127.0.0.1:5000";
    GameObject[] songs = new GameObject[20];
    
	float catVelocity = (float)(0.0);
	float maxCatVelocity = 30;
	int minCatHeight = 0;
	float catHeight = 0;
	float catAcceleration = (float)(-40);
	float catGroundToleranceRange = 1;
	Image catImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string serverurl = "http://127.0.0.1:5000";
        //for(int i = 0; i < 20; i ++){
        //  songs[i] = new GameObject();
        //songs[i].transform.SetParent(GameObject.Find("Unity-Content-Container").transform, false);

        //}
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        var container = root.Q<VisualElement>("unity-content-container");
           catImage = root.Query<Image>("JumpingCat");
        for (int i = 0; i < 20; i++)
        {
            Button newButton = new Button();
            newButton.text = $"Song NAMES!!";
            Label butText = new Label();
            butText.text = $"WORRRRRKKKKKKKK";
            int index = i; // capture loop variable
         newButton.style.width = new Length(10, LengthUnit.Percent);  // 50% width
            //newButton.style.paddingTop = new Length(20, LengthUnit.Percent);
            newButton.style.height = new Length(50, LengthUnit.Percent);
            newButton.style.fontSize = 32;
            newButton.style.unityFont = Fonts.Load<Font>("PixelifySans-Regular");
            newButton.style.marginLeft = new Length(2.5f, LengthUnit.Percent);
            newButton.style.marginRight = new Length(2.5f, LengthUnit.Percent);
            newButton.style.marginTop = new Length(5, LengthUnit.Percent);
            newButton.clicked += () => Debug.Log($"Button {index + 1} clicked!");
            butText.style.marginTop = new Length(16, LengthUnit.Percent);
            butText.style.marginLeft = new Length(10f, LengthUnit.Percent);
            butText.style.marginRight = new Length(10f, LengthUnit.Percent);
            container.Add(newButton);
            container.Add(butText);
        }
        Button addMusicButton = root.Query<Button>("AddMusicButton");
        Button backButton = root.Query<Button>("BackButton");
        backButton.clicked += () => SceneManager.LoadScene("Scenes/MainMenuScene/MainMainScene");
        if (addMusicButton != null)

        {
            addMusicButton.clicked += OnAddMusicButtonClicked; // Register the callback method
        }
        else
        {
            Debug.LogError("SongsButton not found in UXML document!");
        }
    }
    void FixedUpdate()
	{
		if (catImage != null)
		{
            if ((catHeight <= minCatHeight + catGroundToleranceRange / 2.0 && catHeight >= minCatHeight - catGroundToleranceRange / 2.0) && catVelocity != maxCatVelocity)
			{
				catHeight = minCatHeight;
				catVelocity = maxCatVelocity;
			} else {
				catVelocity = (catVelocity + catAcceleration * Time.fixedDeltaTime);				
			}
            catHeight += ((catVelocity * Time.fixedDeltaTime) + (catAcceleration * Time.fixedDeltaTime * Time.fixedDeltaTime / 2));
			//Debug.Log($"Time: {Time.fixedDeltaTime}; Velocity: {catVelocity}; Height: {catHeight}");
            catImage.style.translate = new Translate(new Length(0, LengthUnit.Pixel), new Length(-catHeight, LengthUnit.Percent));
        }
    }
    void OnAddMusicButtonClicked()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel(
            "Select Song",
            "",
            new[] { new ExtensionFilter("Audio Files", "wav", "mp3") },
            false
        );

        if (paths.Length == 0)
            return;

        string selectedPath = paths[0];

        string extension = Path.GetExtension(selectedPath).ToLower();

        if (extension != ".wav" && extension != ".mp3")
        {
            Debug.Log("Invalid file type!");
            return;
        }

        string fileName = Path.GetFileName(selectedPath);
        string destinationPath = Path.Combine(Application.persistentDataPath, fileName);
        StartUpload(destinationPath);
        File.Copy(selectedPath, destinationPath, true);

        Debug.Log("Song stored at: " + destinationPath);
        void Start()
        {

        }
    }
    void StartUpload(String file)
    {
        StartCoroutine(Upload(file));
    }
    [System.Serializable]
    public class ResponseData
    {
        public string message;
        public int score;
    }
    IEnumerator Upload(String file)
    {
        byte[] songdata = File.ReadAllBytes(file);
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string nameOfFile = Path.GetFileName(file);
        formData.Add(new MultipartFormFileSection("file", songdata, nameOfFile, "audio/wav"));
        Debug.Log("file size:" + songdata.Length);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/", formData);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.chunkedTransfer = false;
        www.SetRequestHeader("Accept", "application/json");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;

            Debug.Log("Form upload complete!");
            Debug.Log("Response length: " + www.downloadHandler.data.Length);
            Debug.Log("Raw text: [" + www.downloadHandler.text + "]");
            Debug.Log("Result: " + www.result);
            Debug.Log("Server Response: " + jsonResponse);

            ResponseData data = JsonUtility.FromJson<ResponseData>(jsonResponse);
            string jsonPath = Path.Combine(Application.persistentDataPath, (nameOfFile + ".json"));
            File.WriteAllText(jsonPath, jsonResponse);
            Debug.Log("saved to :" + jsonPath);
        }
        else
        {
            
            Debug.Log(www.error);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}

