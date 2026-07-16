using UnityEngine;

public class puck : MonoBehaviour
{
    [Header("Boundaries")]
    [SerializeField] public GameObject leftWall;
    [SerializeField] public GameObject rightWall;
    //[SerializeField] public GameObject topWall;
    //[SerializeField] public GameObject bottomWall;

    private Vector2 movementBoundsMin;
    private Vector2 movementBoundsMax;

    private CircleCollider2D circleCollider;

    void Start()
    {
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

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Only clamp horizontally
        pos.x = Mathf.Clamp(pos.x, movementBoundsMin.x, movementBoundsMax.x);

        transform.position = pos;
    }


}