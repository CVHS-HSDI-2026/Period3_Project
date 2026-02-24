using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using UI = UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class SongSelectionSceneUIManager : MonoBehaviour
{
    GameObject[] songs = new GameObject[20];
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
        if (addMusicButton != null)
        {
            addMusicButton.clicked += OnAddMusicButtonClicked; // Register the callback method
        }
        else
        {
            Debug.LogError("SongsButton not found in UXML document!");
        }
    }

    void OnAddMusicButtonClicked()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
