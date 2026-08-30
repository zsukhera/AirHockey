
using Fusion;
using UnityEngine;

public class PlayerPaddle : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float dragSensitivity = 1f;

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
    private SpriteRenderer spriteRenderer;

    private bool isDragging;

    private Vector2 dragStartPos;
    private Vector2 dragStartWorldPos;


    public override void Spawned()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
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
                "PlayerPaddle: SpriteRenderer not found."
            );
        }

        Debug.Log(
            "Paddle spawned | Owner: " +
            Object.InputAuthority +
            " | Local: " +
            Runner.LocalPlayer
        );


        // Player 1 = Blue
        // Player 2 = Red
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
    }


    public override void FixedUpdateNetwork()
    {
        // Only State Authority changes the networked paddle position.
        if (!Object.HasStateAuthority)
            return;


        if (!GetInput<NetworkInputData>(
                out NetworkInputData input))
        {
            return;
        }


        // Start dragging
        if (input.dragStarted)
        {
            Vector2 clickPosition =
                input.pointerWorldPosition;

            if (IsClickOnPlayer(clickPosition))
            {
                StartDrag(clickPosition);
            }
        }


        // Continue dragging
        if (input.isDragging && isDragging)
        {
            UpdateDrag(
                input.pointerWorldPosition
            );
        }


        // Release
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

        rb.velocity = Vector2.zero;

        Debug.Log(
            "Started dragging paddle owned by " +
            Object.InputAuthority
        );
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


        rb.MovePosition(newPos);
    }


    private void EndDrag()
    {
        isDragging = false;

        rb.velocity = Vector2.zero;

        Debug.Log(
            "Stopped dragging paddle owned by " +
            Object.InputAuthority
        );
    }


    private bool IsClickOnPlayer(Vector2 worldPos)
    {
        if (circleCollider == null)
            return false;


        Vector2 offset =
            worldPos -
            rb.position;


        return offset.magnitude <=
               circleCollider.radius;
    }


    public void SetSide(bool top)
    {
        isTopPlayer = top;
    }


    public void SetBounds(
        Vector2 min,
        Vector2 max)
    {
        movementBoundsMin = min;
        movementBoundsMax = max;
    }


    public void Stop()
    {
        if (!Object.HasStateAuthority)
            return;

        isDragging = false;

        rb.velocity = Vector2.zero;
    }


    public void ResetPosition(Vector3 startPos)
    {
        if (!Object.HasStateAuthority)
            return;

        rb.position = startPos;

        rb.velocity = Vector2.zero;

        isDragging = false;
    }
}
