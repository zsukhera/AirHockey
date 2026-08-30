using Fusion;
using UnityEngine;

public class GameplayManager : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef playerPrefab;

    [SerializeField] private Transform bottomSpawn;
    [SerializeField] private Transform topSpawn;


    public override void Spawned()
    {
        bottomSpawn = GameObject.Find("BottomSpawn").transform;
        topSpawn = GameObject.Find("TopSpawn").transform;

        Debug.Log("GameplayManager Spawned");

        if (Runner.IsServer)
        {
            SpawnPlayers();
        }
    }

    private void SetupLocalCamera(PlayerRef localPlayer)
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("GameplayManager: Main Camera not found.");
            return;
        }

        if (localPlayer.PlayerId == 1)
        {
            // Player 1 sees the world normally.
            cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            Debug.Log("Camera set for Player 1 - normal orientation");
        }
        else
        {
            // Player 2 sees the world upside down.
            cam.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

            Debug.Log("Camera set for Player 2 - mirrored orientation");
        }
    }

    private void SpawnPlayers()
    {
        int index = 0;

        foreach (PlayerRef player in Runner.ActivePlayers)
        {
            Transform spawnPoint =
                index == 0 ? bottomSpawn : topSpawn;


            Runner.Spawn(
                playerPrefab,
                spawnPoint.position,
                Quaternion.identity,
                player
            );

            index++;
        }
    }
}