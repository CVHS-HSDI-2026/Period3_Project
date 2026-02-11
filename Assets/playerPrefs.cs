using UnityEngine;

public class playerPrefs : MonoBehaviour
{
    private void Start() // saves whatever data is inside (ex. PlayerPrefs.SetInt("levelpercent",100);) works with strings floats or ints   
    {
        // add data

        PlayerPrefs.Save();
    }
    private void Update() // loads data inside that was saved from Start() (ex. int levelpercent = PlayerPrefs.GetInt("levelpercent", 0);)
    {
        // add data
        //after adding data, debug (ex. Debug.Log(levelpercent))
    }
}
