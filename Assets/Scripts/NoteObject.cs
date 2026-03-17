using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public float targetTime;
    public int lane;
    public bool wasHit = false;

    public static float scrollSpeed = 5f; 
    private float despawnY = -6f;

    void Start()
    {
        Color[] laneColors = {
            Color.blue,
            Color.red,
            Color.yellow
        };
        GetComponent<SpriteRenderer>().color = laneColors[lane];
    }

    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        if (transform.position.y < despawnY && !wasHit)
        {
            Destroy(gameObject);
        }
    }
}