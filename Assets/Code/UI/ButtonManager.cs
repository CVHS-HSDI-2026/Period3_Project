using UnityEngine;
using UnityEngine.UI; // Make sure to include this using statement

public class ButtonManager : MonoBehaviour
{
    // This is the function that will run when the button is pressed
    public void OnButtonPressed()
    {
        Debug.Log("Button was clicked! The function is running.");
        // Add your specific code here, e.g.,
        // GameObject.Find("SomeObject").SetActive(false);
        // int score = 10;
    }
}
