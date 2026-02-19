using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using UI = UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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

        // Get reference to button in code
        //Button settingsButton = root.Query<Button>("SettingsButton");
        //if (settingsButton != null)
        //{
        //    settingsButton.clicked += OnSettingsButtonClicked; // Register the callback method
        //}
        //else
        //{
        //    Debug.LogError("SettingsButton not found in UXML document!");
        //}

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
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("PlayButton was clicked!");
        LoadSceneAsync("GameScene");
    }

    private void OnCharacterSelectButtonClicked()
    {
        Debug.Log("CharacterSelectButton was clicked!");
        LoadSceneAsync("CharacterSelectScene");
    }

    //private void OnSettingsButtonClicked()
    //{
    //    Debug.Log("SettingsButton was clicked!");
    //    SceneManager.LoadScene("SettingsScene");
    //}

    private void OnSongsButtonClicked()
    {
        Debug.Log("SongsButton was clicked!");
        LoadSceneAsync("SongSelectionScene");
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

    void LoadSceneAsync(string sceneName)
    {
        //$"Scenes/{sceneName}/{sceneName}"
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            //if (progressBar != null)
            //{
            //    progressBar.value = progress;
            //}
        }
    }
}
