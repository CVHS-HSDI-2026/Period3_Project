using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using UnityEngine.SceneManagement;
using System;

public class GameSceneUIManager : MonoBehaviour
{
    Button playButton;
    Button pauseButton;

    private void OnEnable()
    {
        // Get the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Get the root visual element
        var root = uiDocument.rootVisualElement;

        Button jumpButton = root.Query<Button>("JumpButton");
        if (jumpButton != null)
        {
            jumpButton.clicked += OnJumpButtonClicked; // Register the callback method
        }
        else
        {
            Debug.LogError("JumpButton not found in UXML document!");
        }

        playButton = root.Query<Button>("PlayButton");
        pauseButton = root.Query<Button>("PauseButton");
        if (playButton != null)
        {
            playButton.clicked += OnPlayButtonClicked; // Register the callback method
        }
        else
        {
            Debug.LogError("PlayButton not found in UXML document!");
        }
        
        if (pauseButton != null)
        {
            pauseButton.clicked += OnPauseButtonClicked; // Register the callback method
        }
        else
        {
            Debug.LogError("PauseButton not found in UXML document!");
        }
    }

    private void OnJumpButtonClicked()
    {
        Debug.Log("Jump");
    }

    private void OnPlayButtonClicked()
    {
        playButton.style.visibility = Visibility.Hidden;
        pauseButton.style.visibility = Visibility.Visible;
    }

    private void OnPauseButtonClicked()
    {
        pauseButton.style.visibility = Visibility.Hidden;
        playButton.style.visibility = Visibility.Visible;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
