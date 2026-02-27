using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using UI = UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using SFB;
public class SongSelectionSceneUIManager : MonoBehaviour
{
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
            newButton.text = $"Button {i + 1}";
            int index = i; // capture loop variable
         newButton.style.width = new Length(10, LengthUnit.Percent);  // 50% width
            newButton.style.height = new Length(50, LengthUnit.Percent);
            newButton.style.marginLeft = new Length(2.5f, LengthUnit.Percent);
            newButton.style.marginRight = new Length(2.5f, LengthUnit.Percent);
            newButton.style.marginTop = new Length(5, LengthUnit.Percent);
            newButton.clicked += () => Debug.Log($"Button {index + 1} clicked!");
            container.Add(newButton);
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

        File.Copy(selectedPath, destinationPath, true);

        Debug.Log("Song stored at: " + destinationPath);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
