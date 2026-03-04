using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using UnityEngine.SceneManagement;
using System;

public class GameSceneUIManager : MonoBehaviour
{
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

        Button playButton = root.Query<Button>("PlayButton");
    }

    private void OnJumpButtonClicked()
    {
        Debug.Log("Jump");
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
