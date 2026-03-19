using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using UI = UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using System.Threading;

public class MainMenuSceneUIManager : MonoBehaviour
{
	// The UXML asset should be assigned to the UIDocument component in the Inspector

	void OnEnable()
	{
		// Get the UIDocument component
		var uiDocument = GetComponent<UIDocument>();

		// Get the root visual element
		var root = uiDocument.rootVisualElement;

		// Get reference to button in code
		Button playButton = root.Query<Button>("PlayButton");
		if (playButton != null)
		{
			playButton.clicked += OnPlayButtonClicked; // Register the callback method
		}
		else
		{
			Debug.LogError("PlayButton not found in UXML document!");
		}

        playButton.RegisterCallback<MouseEnterEvent>(e => playButton.style.scale = new Scale(new Vector3(1.1f, 1.1f, 1)));
        playButton.RegisterCallback<MouseLeaveEvent>(e => playButton.style.scale = new Scale(Vector3.one));

		// Get reference to button in code
		Button characterSelectButton = root.Query<Button>("CharacterSelectButton");
		if (characterSelectButton != null)
		{
			characterSelectButton.clicked += OnCharacterSelectButtonClicked; // Register the callback method
		}
		else
		{
			Debug.LogError("CharacterSelectButton not found in UXML document!");
		}
		characterSelectButton.RegisterCallback<MouseEnterEvent>(e => characterSelectButton.style.scale = new Scale(new Vector3(1.1f, 1.1f, 1)));
        characterSelectButton.RegisterCallback<MouseLeaveEvent>(e =>characterSelectButton.style.scale = new Scale(Vector3.one));
		// Get reference to button in code
		Button songsButton = root.Query<Button>("SongsButton");
		if (songsButton != null)
		{
			songsButton.clicked += OnSongsButtonClicked; // Register the callback method
		}
		else
		{
			Debug.LogError("SongsButton not found in UXML document!");
		}

		// Get reference to button in code
		Button ropeSkinsButton = root.Query<Button>("RopeSkinsButton");
		if (ropeSkinsButton != null)
		{
			ropeSkinsButton.clicked += OnRopeSkinsButtonClicked; // Register the callback method
		}
		else
		{
			Debug.LogError("RopeSkinsButton not found in UXML document!");
		}
		ropeSkinsButton.RegisterCallback<MouseEnterEvent>(e => ropeSkinsButton.style.scale = new Scale(new Vector3(1.1f, 1.1f, 1)));
        ropeSkinsButton.RegisterCallback<MouseLeaveEvent>(e => ropeSkinsButton.style.scale = new Scale(Vector3.one));
	}

	private void OnPlayButtonClicked()
	{
		SceneManager.LoadScene("Scenes/SongSelectionScene/SongSelection");
	}

	private void OnCharacterSelectButtonClicked()
	{
		SceneManager.LoadScene("Scenes/CharacterSelectionScene/CharacterSelectionScene");
	}

	//private void OnSettingsButtonClicked()
	//{
	//    Debug.Log("SettingsButton was clicked!");
	//    SceneManager.LoadScene("SettingsScene");
	//}

	private void OnSongsButtonClicked()
	{
		SceneManager.LoadScene("Scenes/SongSelectionScene/SongSelection");
	}

	private void OnRopeSkinsButtonClicked()
	{
		Debug.Log("RopeSkinsButton was clicked!");
	}

	// It's good practice to unregister callbacks when the object is disabled or destroyed
	void OnDisable()
	{
		var uiDocument = GetComponent<UIDocument>();
		var root = uiDocument.rootVisualElement;
		Button playButton = root.Query<Button>("PlayButton");
		if (playButton != null)
		{
			playButton.clicked -= OnPlayButtonClicked; // Register the callback method
		}
		else
		{
			Debug.LogError("PlayButton not found in UXML document!");
		}

		// Get reference to button in code
		Button characterSelectButton = root.Query<Button>("CharacterSelectButton");
		if (characterSelectButton != null)
		{
			characterSelectButton.clicked -= OnCharacterSelectButtonClicked; // Register the callback method
		}
		else
		{
			Debug.LogError("CharacterSelectButton not found in UXML document!");
		}

		//// Get reference to button in code
		//Button settingsButton = root.Query<Button>("SettingsButton");
		//if (settingsButton != null)
		//{
		//    settingsButton.clicked -= OnSettingsButtonClicked; // Register the callback method
		//}
		//else
		//{
		//    Debug.LogError("SettingsButton not found in UXML document!");
		//}

		// Get reference to button in code
		Button songsButton = root.Query<Button>("SongsButton");
		if (songsButton != null)
		{
			songsButton.clicked -= OnSongsButtonClicked; // Register the callback method
		}
		else
		{
			Debug.LogError("SongsButton not found in UXML document!");
		}

		// Get reference to button in code
		Button ropeSkinsButton = root.Query<Button>("RopeSkinsButton");
		if (ropeSkinsButton != null)
		{
			ropeSkinsButton.clicked -= OnRopeSkinsButtonClicked; // Register the callback method
		}
		else
		{
			Debug.LogError("RopeSkinsButton not found in UXML document!");
		}
	}

	float catVelocity = (float)(0.0);
	float maxCatVelocity = 40;
	int minCatHeight = 0;
	float catHeight = 0;
	float catAcceleration = (float)(-40);
	float catGroundToleranceRange = 1;
	Image catImage;

	void Start()
	{
        // Get the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Get the root visual element
        var root = uiDocument.rootVisualElement;

        catImage = root.Query<Image>("JumpingCat");
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
				catVelocity = catVelocity + catAcceleration * Time.fixedDeltaTime;				
			}
            catHeight += ((catVelocity * Time.fixedDeltaTime) + (catAcceleration * Time.fixedDeltaTime * Time.fixedDeltaTime / 2));
			//Debug.Log($"Time: {Time.fixedDeltaTime}; Velocity: {catVelocity}; Height: {catHeight}");
            catImage.style.translate = new Translate(new Length(0, LengthUnit.Pixel), new Length(-catHeight, LengthUnit.Percent));
        }
    }
}
