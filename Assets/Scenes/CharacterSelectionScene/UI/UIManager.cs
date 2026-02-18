using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using System;

public class UIManager : MonoBehaviour
{
    //    // The UXML asset should be assigned to the UIDocument component in the Inspector

    //    void OnEnable()
    //    {
    //        // Get the UIDocument component
    //        var uiDocument = GetComponent<UIDocument>();

    //        // Get the root visual element
    //        var root = uiDocument.rootVisualElement;

    //        // Get reference to button in code
    //        Button myButton = root.Query<Button>("PlayButton");
    //        if (myButton != null)
    //        {
    //            myButton.clicked += OnPlayButtonClicked; // Register the callback method
    //        }
    //        else
    //        {
    //            Debug.LogError("PlayButton not found in UXML document!");
    //        }

    //        // Get reference to button in code
    //        Button myButton = root.Query<Button>("CharacterSelectButton");
    //        if (myButton != null)
    //        {
    //            myButton.clicked += OnPlayButtonClicked; // Register the callback method
    //        }
    //        else
    //        {
    //            Debug.LogError("CharacterSelectButton not found in UXML document!");
    //        }

    //        // Get reference to button in code
    //        Button myButton = root.Query<Button>("SettingsButton");
    //        if (myButton != null)
    //        {
    //            myButton.clicked += OnPlayButtonClicked; // Register the callback method
    //        }
    //        else
    //        {
    //            Debug.LogError("SettingsButton not found in UXML document!");
    //        }

    //        // Get reference to button in code
    //        Button myButton = root.Query<Button>("SongsButton");
    //        if (myButton != null)
    //        {
    //            myButton.clicked += OnPlayButtonClicked; // Register the callback method
    //        }
    //        else
    //        {
    //            Debug.LogError("SongsButton not found in UXML document!");
    //        }

    //        // Get reference to button in code
    //        Button myButton = root.Query<Button>("RopeSkinsButton");
    //        if (myButton != null)
    //        {
    //            myButton.clicked += OnPlayButtonClicked; // Register the callback method
    //        }
    //        else
    //        {
    //            Debug.LogError("RopeSkinsButton not found in UXML document!");
    //        }
    //    }

    //    // This method is called when the button is clicked
    //    private void OnPlayButtonClicked()
    //    {
    //        Debug.Log("Button was clicked!");
    //        // Add your specific action here, e.g., load a scene, open a menu, etc.
    //    }

    //    // It's good practice to unregister callbacks when the object is disabled or destroyed
    //    void OnDisable()
    //    {
    //        var uiDocument = GetComponent<UIDocument>();
    //        var root = uiDocument.rootVisualElement;
    //        Button myButton = root.Query<Button>("MyButton");
    //        if (myButton != null)
    //        {
    //            myButton.clicked -= OnPlayButtonClicked; // Unregister to prevent memory leaks
    //        }
    //    }
}
