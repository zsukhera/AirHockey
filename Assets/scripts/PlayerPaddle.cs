using Fusion;
using UnityEngine;

public class PlayerPaddle : NetworkBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private bool useTouchInput = false;

    [Header("Movement Settings")]
    [SerializeField] private float dragSensitivity = 1f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float friction = 0.95f;

    [Header("Bounds")]
    [SerializeField] private Vector2 movementBoundsMin;
    [SerializeField] private Vector2 movementBoundsMax;

    [Header("Player Side")]
    [SerializeField] private bool isTopPlayer = false;

    [Header("Visual")]
    [SerializeField] private Color player1Color = Color.blue;
    [SerializeField] private Color player2Color = Color.red;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    // SpriteRenderer is on the child Circle object.
    private SpriteRenderer spriteRenderer;

    private Vector2 currentVelocity;

    private bool isDragging;

    private Vector2 dragStartPos;
    private Vector2 dragStartWorldPos;

    private Vector2 lastPosition;


    public override void Spawned()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        // Find SpriteRenderer on the child Circle.
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (rb == null)
        {
            Debug.LogError("PlayerPaddle: Rigidbody2D not found.");
        }

        if (circleCollider == null)
        {
            Debug.LogError("PlayerPaddle: CircleCollider2D not found.");
        }

        if (spriteRenderer == null)
        {
            Debug.LogError(
                "PlayerPaddle: SpriteRenderer not found on paddle or child objects."
            );
        }

        Debug.Log(
            "Paddle spawned | Owner: " +
            Object.InputAuthority +
            " | Local: " +
            Runner.LocalPlayer
        );


        // Assign visual color based on Fusion PlayerRef.
        if (spriteRenderer != null)
        {
            if (Object.InputAuthority.PlayerId == 1)
            {
                spriteRenderer.color = player1Color;
            }
            else
            {
                spriteRenderer.color = player2Color;
            }
        }


        if (Object.HasInputAuthority)
        {
            Debug.Log("I control this paddle");
        }

        lastPosition = rb.position;
    }


    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority)
            return;

        if (GetInput<NetworkInputData>(out NetworkInputData input))
        {
            HandleNetworkInput(input);
        }

        rb.velocity = currentVelocity;

        currentVelocity *= friction;

        lastPosition = rb.position;
    }


    private void HandleNetworkInput(NetworkInputData input)
    {
        if (input.dragStarted)
        {
            Vector2 clickPosition = input.pointerWorldPosition;

            if (IsClickOnPlayer(clickPosition))
            {
                StartDrag(clickPosition);
            }
        }

        if (input.isDragging && isDragging)
        {
            UpdateDrag(input.pointerWorldPosition);
        }

        if (input.dragEnded && isDragging)
        {
            EndDrag();
        }
    }


    private void StartDrag(Vector2 clickPosition)
    {
        isDragging = true;

        dragStartWorldPos = clickPosition;
        dragStartPos = rb.position;

        currentVelocity = Vector2.zero;
        rb.velocity = Vector2.zero;
    }


    private void UpdateDrag(Vector2 currentWorldPos)
    {
        Vector2 dragDelta =
            currentWorldPos -
            dragStartWorldPos;

        Vector2 newPos =
            dragStartPos +
            dragDelta * dragSensitivity;

        newPos.x = Mathf.Clamp(
            newPos.x,
            movementBoundsMin.x,
            movementBoundsMax.x
        );

        newPos.y = Mathf.Clamp(
            newPos.y,
            movementBoundsMin.y,
            movementBoundsMax.y
        );

        rb.position = newPos;
    }


    private void EndDrag()
    {
        isDragging = false;

        Vector2 positionDelta =
            rb.position -
            lastPosition;

        currentVelocity =
            positionDelta / Runner.DeltaTime;

        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity =
                currentVelocity.normalized *
                maxSpeed;
        }
    }


    private bool IsClickOnPlayer(Vector2 worldPos)
    {
        if (circleCollider == null)
            return false;

        Vector2 offset =
            worldPos -
            rb.position;

        return offset.magnitude <= circleCollider.radius;
    }


    public void SetSide(bool top)
    {
        isTopPlayer = top;
    }


    public void SetBounds(Vector2 min, Vector2 max)
    {
        movementBoundsMin = min;
        movementBoundsMax = max;
    }


    public void Stop()
    {
        if (!Object.HasStateAuthority)
            return;

        currentVelocity = Vector2.zero;
        rb.velocity = Vector2.zero;
        isDragging = false;
    }


    public void ResetPosition(Vector3 startPos)
    {
        if (!Object.HasStateAuthority)
            return;

        rb.position = startPos;
        currentVelocity = Vector2.zero;
        rb.velocity = Vector2.zero;
        isDragging = false;

        lastPosition = rb.position;
    }
}
