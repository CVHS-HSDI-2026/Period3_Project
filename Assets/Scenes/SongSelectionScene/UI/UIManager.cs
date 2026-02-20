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
            newButton.clicked += () => Debug.Log($"Button {index + 1} clicked!");
            container.Add(newButton);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
