using Fusion;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [Header("Camera Rotation")]
    [SerializeField] private float normalRotation = 0f;
    [SerializeField] private float mirroredRotation = 180f;

    public override void Spawned()
    {
        // Only modify the camera belonging to this player.
        if (!Object.HasInputAuthority)
            return;

        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("PlayerCamera: Main Camera not found.");
            return;
        }

        // Player 1 stays normal.
        // Player 2 sees the world upside-down.
        if (Object.InputAuthority.PlayerId == 1)
        {
            cam.transform.rotation =
                Quaternion.Euler(0f, 0f, normalRotation);

            Debug.Log("Local camera: Player 1 - normal orientation");
        }
        else
        {
            cam.transform.rotation =
                Quaternion.Euler(0f, 0f, mirroredRotation);

            Debug.Log("Local camera: Player 2 - mirrored orientation");
        }
    }
}
