using UnityEngine;
using UnityEngine.UIElements; // Required namespace for UI Toolkit
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public int index = 0;
    public List<Sprite> character = new List<Sprite>();
    Image characterImage;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        var root = uiDocument.rootVisualElement;

        Button left = root.Query<Button>("LeftButton");
        if (left != null)
        {
            left.clicked += OnLeftClicked;
        }
        else
        {
            Debug.LogError("Left Button not found in UXML document");
        }

        Button right = root.Query<Button>("RightButton");
        if (right != null)
        {
            right.clicked += OnRightClicked;
        }
        else
        {
            Debug.LogError("Right Button not found in UXML document");
        }

        Image image = root.Query<Image>("CharacterImage");
        if (image != null)
        {
            characterImage = image;
            characterImage.sprite = character[index];
        }
        else
        {
            Debug.LogError("Character Image not found in UXML document");
        }

        Button back = root.Query<Button>("BackButton");
        if (back != null)
        {
            back.clicked += OnBackClicked;
        } else
        {
            Debug.LogError("Back Button no found in UXML document");
        }

        Button select = root.Query<Button>("SelectButton");
        if (select != null)
        {
            select.clicked += OnSelectClicked;
        }
    }

    void OnLeftClicked()
    {
        if (index > 0)
        {
            index--;
        }
        if (characterImage != null) {
            characterImage.sprite = character[index];
        }
    }

    void OnRightClicked()
    {
        if (index <  character.Count-1)
        {
            index++;
        }
        if (characterImage != null) {
            characterImage.sprite = character[index];
        }
    }

    void OnBackClicked()
    {
        SceneManager.LoadScene("Scenes/MainMenuScene/MainMenuScene");
    }

    void OnSelectClicked()
    {

    }

    void OnDisable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        Button left = root.Query<Button>("LeftButton");
        if (left != null)
        {
            left.clicked -= OnLeftClicked;
        }

        Button right = root.Query<Button>("RightButton");
        if (right != null)
        {
            right.clicked -= OnRightClicked;
        }
    }
}
