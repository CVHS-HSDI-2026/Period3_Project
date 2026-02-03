using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonListGenerator : MonoBehaviour
{
public GameObject buttonPrefab;
public Transform buttonParent;
public List<string> buttonLabels = new List<string> { "Play", "Settings", "Exit" };

void Start()
{
foreach (string label in buttonLabels)
{
GameObject newButton = Instantiate(buttonPrefab, buttonParent);
newButton.GetComponentInChildren<Text>().text = label;
newButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(label));
}
}

void OnButtonClick(string label)
{
Debug.Log("Button clicked: " + label);
}
}