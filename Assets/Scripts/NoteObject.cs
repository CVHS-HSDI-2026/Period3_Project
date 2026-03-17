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
        GetComponent<SpriteRenderer>().color = Color.cyan;
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