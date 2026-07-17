using System.Collections;
using UnityEngine;

public class puck : MonoBehaviour
{
    [Header("Boundaries")]
    [SerializeField] public GameObject leftWall;
    [SerializeField] public GameObject rightWall;

    public scoreKeeper scoreKeeper;

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
        scoreKeeper = FindObjectOfType<scoreKeeper>();
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

        transform.position = respawnPoint.position;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        scoreKeeper.resumeTimer();
    }

    //called by the score keeper 
    //when the game is over and the puck is meant to be frozen 
    //the puck is placed at the centre and the rigid body is disabled
    public void freezePuck()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        StopAllCoroutines();          // Cancel any pending respawn
        transform.position = respawnPoint.position;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = false;         // Freeze the puck completely
    }
}