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