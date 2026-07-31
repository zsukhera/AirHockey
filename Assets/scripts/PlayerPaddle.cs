using Fusion;
using UnityEngine;

public class PlayerPaddle : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private Camera cam;

    public override void Spawned()
    {
        Debug.Log(
            "Paddle spawned. Owner: " +
            Object.InputAuthority +
            " Local: " +
            Runner.LocalPlayer
        );

        if (Object.HasInputAuthority)
        {
            Debug.Log("I control this paddle");
            cam = Camera.main;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;

        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        transform.position = Vector3.MoveTowards(
            transform.position,
            mousePosition,
            moveSpeed * Runner.DeltaTime
        );

        Debug.Log("Paddle " + Object.InputAuthority +" Has Authority: " +Object.HasInputAuthority);
    }
}