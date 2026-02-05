using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SongSelectionGenerator : MonoBehaviour
{
    public GameObject[] songs = new GameObject[20];
    public Button myButton;
    public Text buttonText;
    public GameObject content = GameObject.Find("Content");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            
        for (int i = 0; i < songs.Length; i++)
        {
            songs[i] = new GameObject("Button " + i);
            songs[i].transform.SetParent(GameObject.Find("Content").transform, false);
            songs[i].transform.position = new Vector3((i*200f)-400f, 287f, 0f);
        Button dynamicButton = songs[i].AddComponent<Button>();
        Image img = songs[i].AddComponent<Image>(); // Needed for Button visuals
        img.color = Color.green;

        dynamicButton.onClick.AddListener(() => Debug.Log("Dynamic Button Clicked!"));
        }
        Renderer rend = GetComponent<Renderer>();
        Vector3 size = rend.bounds.size;
        content.transform.localScale = new Vector3(size.x, 50*songs.Length, size.z);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
