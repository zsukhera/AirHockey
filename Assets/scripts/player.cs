using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private bool useTouchInput = false; // Toggle between mouse and touch

    [Header("Movement Settings")]
    [SerializeField] private float dragSensitivity = 1f;
    [SerializeField] private Vector2 movementBoundsMin = new Vector2(-5f, -8f);
    [SerializeField] private Vector2 movementBoundsMax = new Vector2(5f, 8f);
    [SerializeField] private float maxSpeed = 20f;

    [Header("Physics")]
    [SerializeField] private float friction = 0.95f; // Velocity damping

    [Header("Puck Interaction")]
    [SerializeField] private float puckHitForce = 15f;
    [SerializeField] private float puckHitRadius = 1f;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private Vector3 lastPosition;
    private Vector2 currentVelocity = Vector2.zero;
    private bool isDragging = false;
    private Vector2 dragStartPos;
    private Vector3 dragStartWorldPos;

    private void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        if (rb == null)
        {
            Debug.LogError("Player must have a Rigidbody2D component!");
        }
        if (circleCollider == null)
        {
            Debug.LogError("Player must have a CircleCollider2D component!");
        }

        lastPosition = transform.position;

        // Auto-detect input method based on platform
#if UNITY_EDITOR
        useTouchInput = false; // Use mouse in editor
#elif UNITY_ANDROID || UNITY_IOS
            useTouchInput = true; // Use touch on mobile
#endif
    }

    private void Update()
    {
        // Handle input based on selected method
        if (useTouchInput)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }

        // Apply friction to velocity
        currentVelocity *= friction;
    }

    private void FixedUpdate()
    {
        // Apply velocity to rigidbody
        rb.velocity = currentVelocity;
    }

    /// <summary>
    /// Handle mouse input for editor testing
    /// </summary>
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            if (IsClickOnPlayer(mouseWorldPos))
            {
                StartDrag(mouseWorldPos);
            }
        }

        if (Input.GetMouseButton(0)) // Holding click
        {
            if (isDragging)
            {
                Vector3 mouseWorldPos = GetMouseWorldPosition();
                UpdateDrag(mouseWorldPos);
            }
        }

        if (Input.GetMouseButtonUp(0)) // Release click
        {
            if (isDragging)
            {
                EndDrag();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("puck"))
        {
            ApplyForceToPuck(collision.gameObject);
        }
    }

    private void ApplyForceToPuck(GameObject puck)
    {
        Rigidbody2D puckRb = puck.GetComponent<Rigidbody2D>();

        if (puckRb != null)
        {
            Vector2 direction = (puck.transform.position - transform.position).normalized;
            puckRb.AddForce(direction * puckHitForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Handle touch input for Android/iOS devices
    /// </summary>
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorldPos = GetTouchWorldPosition(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsClickOnPlayer(touchWorldPos))
                {
                    StartDrag(touchWorldPos);
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (isDragging)
                {
                    UpdateDrag(touchWorldPos);
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (isDragging)
                {
                    EndDrag();
                }
            }
        }
    }

    /// <summary>
    /// Check if click/touch is on the player circle
    /// </summary>
    private bool IsClickOnPlayer(Vector3 worldPos)
    {
        Vector2 posOffset = (Vector2)worldPos - (Vector2)transform.position;
        return posOffset.magnitude <= circleCollider.radius;
    }

    /// <summary>
    /// Start dragging the player
    /// </summary>
    private void StartDrag(Vector3 clickWorldPos)
    {
        isDragging = true;
        dragStartWorldPos = clickWorldPos;
        dragStartPos = transform.position;
        currentVelocity = Vector2.zero; // Stop momentum when grabbing
    }

    /// <summary>
    /// Update player position while dragging
    /// </summary>
    private void UpdateDrag(Vector3 currentWorldPos)
    {
        // Calculate drag delta - FIXED: Convert to Vector2 for proper math
        Vector2 dragDelta = (Vector2)currentWorldPos - (Vector2)dragStartWorldPos;
        Vector2 newPos = (Vector2)dragStartPos + dragDelta * dragSensitivity;

        // Clamp position to bounds
        newPos.x = Mathf.Clamp(newPos.x, movementBoundsMin.x, movementBoundsMax.x);
        newPos.y = Mathf.Clamp(newPos.y, movementBoundsMin.y, movementBoundsMax.y);

        // Set position (keep Z unchanged)
        Vector3 finalPos = new Vector3(newPos.x, newPos.y, transform.position.z);
        transform.position = finalPos;
    }

    /// <summary>
    /// End dragging and apply momentum
    /// </summary>
    private void EndDrag()
    {
        isDragging = false;

        // Calculate velocity based on last movement
        Vector3 positionDelta = transform.position - lastPosition;
        currentVelocity = new Vector2(positionDelta.x, positionDelta.y) / Time.deltaTime;

        // Clamp velocity to max speed
        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity = currentVelocity.normalized * maxSpeed;
        }
    }

    /// <summary>
    /// Convert mouse screen position to world position
    /// </summary>
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f; // Distance from camera
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    /// <summary>
    /// Convert touch screen position to world position
    /// </summary>
    private Vector3 GetTouchWorldPosition(Vector2 touchScreenPos)
    {
        Vector3 touchPos = new Vector3(touchScreenPos.x, touchScreenPos.y, 10f);
        return Camera.main.ScreenToWorldPoint(touchPos);
    }

    private void LateUpdate()
    {
        lastPosition = transform.position;
    }

    /// <summary>
    /// Toggle between mouse and touch input at runtime
    /// </summary>
    public void SetInputMode(bool useTouch)
    {
        useTouchInput = useTouch;
        Debug.Log($"Input mode changed to: {(useTouch ? "Touch" : "Mouse")}");
    }

    /// <summary>
    /// Get current input mode (for UI display)
    /// </summary>
    public bool GetInputMode()
    {
        return useTouchInput;
    }

    /// <summary>
    /// Optionally: Stop the player instantly (useful for game over)
    /// </summary>
    public void Stop()
    {
        currentVelocity = Vector2.zero;
        rb.velocity = Vector2.zero;
        isDragging = false;
    }

    /// <summary>
    /// Optionally: Reset player to starting position
    /// </summary>
    public void ResetPosition(Vector3 startPos)
    {
        transform.position = startPos;
        currentVelocity = Vector2.zero;
        rb.velocity = Vector2.zero;
        isDragging = false;
    }
}