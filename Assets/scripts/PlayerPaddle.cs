using Fusion;
using UnityEngine;

public class PlayerPaddle : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private Camera cam;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            cam = Camera.main;
            Debug.Log("I own this paddle");
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;


        Vector3 mousePosition =
            cam.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = 0;


        transform.position = Vector3.MoveTowards(
            transform.position,
            mousePosition,
            moveSpeed * Runner.DeltaTime
        );
    }
}