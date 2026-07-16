using System.Collections;
using UnityEngine;

public class puck : MonoBehaviour
{
    [Header("Boundaries")]
    [SerializeField] public GameObject leftWall;
    [SerializeField] public GameObject rightWall;
    //[SerializeField] public GameObject topWall;
    //[SerializeField] public GameObject bottomWall;
    public Transform respawnPoint;
    private Vector2 movementBoundsMin;
    private Vector2 movementBoundsMax;

    private CircleCollider2D circleCollider;

    void Start()
    {
        // Find walls using tags
        leftWall = GameObject.FindWithTag("leftWall");
        rightWall = GameObject.FindWithTag("rightWall");

        if (leftWall == null || rightWall == null)
        {
            Debug.LogError("Walls not found! Make sure leftWall and rightWall objects have the correct tags.");
            return;
        }

        circleCollider = GetComponent<CircleCollider2D>();

        float leftX = leftWall.GetComponent<BoxCollider2D>().bounds.max.x;
        float rightX = rightWall.GetComponent<BoxCollider2D>().bounds.min.x;

        //float topY = topWall.GetComponent<BoxCollider2D>().bounds.min.y;
        //float bottomY = bottomWall.GetComponent<BoxCollider2D>().bounds.max.y;


        // Keep the entire puck inside the left/right walls
        float radius = circleCollider.radius * transform.lossyScale.x;

        movementBoundsMin.x = leftX + radius;
        movementBoundsMax.x = rightX - radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("goal"))
        {
            respawn();
        }
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Only clamp horizontally
        pos.x = Mathf.Clamp(pos.x, movementBoundsMin.x, movementBoundsMax.x);

        transform.position = pos;
    }

    public void respawn()
    {
        StartCoroutine(waitThenRespawn(2));
    }

    IEnumerator waitThenRespawn(int time)
    {
        yield return new WaitForSeconds(time);
        gameObject.transform.position = respawnPoint.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
    }

}