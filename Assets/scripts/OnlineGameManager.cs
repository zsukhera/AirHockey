using Fusion;
using UnityEngine;

public class OnlineGameManager : NetworkBehaviour
{
    public override void Spawned()
    {
        Debug.Log($"OnlineGameManager Spawned | IsServer: {Runner.IsServer}");
    }
}