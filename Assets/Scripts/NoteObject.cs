using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public float targetTime;
    public int lane;
    public bool wasHit = false;

    public static float scrollSpeed = 5f;
    private float despawnX = 10f;

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
        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);

        if (transform.position.x > despawnX && !wasHit)
        {
            Destroy(gameObject);
        }
    }
}