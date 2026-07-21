using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    
    [Header("Audio Manager")]
    public GameObject audioManager;
    public sfxManager sfxManager;
    [Header("AI")]
    [SerializeField] private Difficulty difficulty = Difficulty.Medium;
    [SerializeField] private Transform puck;
    [SerializeField] private Rigidbody2D puckRB;

    [Header("Movement Bounds")]
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject halfLine;

    [Header("Physics")]
    [SerializeField] private float friction = 0.95f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float maxHitForce = 30f;

    [Header("Puck Interaction")]
    [SerializeField] private float puckHitForce = 15f;

    [SerializeField] private float hitDistance = 1.2f;
    [SerializeField] private float hitCooldown = 0.5f;

    private float nextHitTime;

    private Rigidbody2D rb;

    private Vector2 movementBoundsMin;
    private Vector2 movementBoundsMax;

    private Vector2 targetPosition;
    private Vector2 homePosition;

    private Vector2 currentVelocity;

    private float moveSpeed;
    private float reactionDelay;
    private float aimingError;
    private float predictionTime;

    private float nextReaction;
    public bool inputEnabled = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetupDifficulty();
        SetupBounds();
        enableInput();
        homePosition = transform.position;
        targetPosition = transform.position;
    }

    public void enableInput()
    {
        inputEnabled = true;
        currentVelocity = Vector2.zero;
        rb.velocity = Vector2.zero;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!inputEnabled)
        {
            currentVelocity *= friction;
            return;
        }

        if (Time.time >= nextReaction)
        {
            nextReaction = Time.time + reactionDelay;
            DecideTarget();
        }

        CheckPuckMovement();
        engagePuck();
        MoveTowardsTarget();
        currentVelocity *= friction;
    }

    public void disableInput()
    {
        inputEnabled = false;

        currentVelocity = Vector2.zero;
        rb.velocity = Vector2.zero;

        targetPosition = transform.position;
    }

    //checks if the puck velocity is below 3
    //if it is, then the enemy player moves toward the puck, causing a hit
    void engagePuck()
    {
        // Only hit if cooldown has passed
        if (Time.time < nextHitTime)
            return;

        float distance = Vector2.Distance(transform.position, puck.position);

        // Check if AI is close enough to hit
        if (distance <= hitDistance)
        {
            nextHitTime = Time.time + hitCooldown;

            Vector2 hitDirection;

            // Aim toward player's side (downwards)
            hitDirection = Vector2.down;

            // Add difficulty-based aiming error
            hitDirection += Random.insideUnitCircle * aimingError;

            hitDirection.Normalize();

            puckRB.AddForce(
                hitDirection * puckHitForce,
                ForceMode2D.Impulse
            );

            //audioManager.GetComponent<audioManager>().playHitSound();
            sfxManager.playHitSound();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = currentVelocity;
    }

    void SetupDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                moveSpeed = 6f;
                reactionDelay = 0.35f;
                aimingError = 0.8f;
                predictionTime = 0f;
                break;

            case Difficulty.Medium:
                moveSpeed = 5f;
                reactionDelay = 0.25f;
                aimingError = 0.3f;
                predictionTime = 0.2f;
                break;

            case Difficulty.Hard:
                moveSpeed = 16f;
                reactionDelay = 0.02f;
                aimingError = 0.05f;
                predictionTime = 0.45f;
                break;
        }
    }

    void SetupBounds()
    {
        float centreY = halfLine.transform.position.y;

        float leftX = leftWall.GetComponent<BoxCollider2D>().bounds.max.x;
        float rightX = rightWall.GetComponent<BoxCollider2D>().bounds.min.x;

        float topY = topWall.GetComponent<BoxCollider2D>().bounds.min.y;
        float bottomY = bottomWall.GetComponent<BoxCollider2D>().bounds.max.y;

        // Enemy always stays on the top half
        movementBoundsMin = new Vector2(leftX, centreY);
        movementBoundsMax = new Vector2(rightX, topY);
    }

    void DecideTarget()
    {
        Vector2 predictedPuck =
            (Vector2)puck.position +
            puckRB.velocity * predictionTime;

        predictedPuck += Random.insideUnitCircle * aimingError;

        float middle =
            (movementBoundsMin.y + movementBoundsMax.y) * 0.5f;

        // Puck is on enemy side
        if (predictedPuck.y > middle)
        {
            targetPosition = predictedPuck;
        }
        else
        {
            // Return home when player has puck
            targetPosition = homePosition;
        }

        targetPosition.x = Mathf.Clamp(targetPosition.x,
            movementBoundsMin.x,
            movementBoundsMax.x);

        targetPosition.y = Mathf.Clamp(targetPosition.y,
            movementBoundsMin.y,
            movementBoundsMax.y);
    }

    void MoveTowardsTarget()
    {
        Vector2 newPosition = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);

        Vector2 delta = newPosition - (Vector2)transform.position;

        currentVelocity = delta / Time.deltaTime;

        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity =
                currentVelocity.normalized * maxSpeed;
        }

        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("puck"))
        {
            Rigidbody2D puckRigidBody =
                collision.gameObject.GetComponent<Rigidbody2D>();
            //audioManager.GetComponent<audioManager>().playHitSound();
            sfxManager.playHitSound();
            if (puckRigidBody != null)
            {
                Vector2 direction =
                    (collision.transform.position - transform.position).normalized;

                puckRigidBody.AddForce(
                    direction * puckHitForce,
                    ForceMode2D.Impulse);
            }
        }
    }

    //if the puck is on the side of the enemy, and is stationary
    //then the enemy hits it to get it moving
    void CheckPuckMovement()
    {
        // Is the puck on the enemy's side?
        if (puck.position.y > halfLine.transform.position.y)
        {
            // Is it almost stationary?
            if (puckRB.velocity.magnitude < 0.2f)
            {
                targetPosition = puck.position;

                // Keep the target inside the AI's movement bounds
                targetPosition.x = Mathf.Clamp(targetPosition.x,
                    movementBoundsMin.x,
                    movementBoundsMax.x);

                targetPosition.y = Mathf.Clamp(targetPosition.y,
                    movementBoundsMin.y,
                    movementBoundsMax.y);
            }
        }
    }
}