using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkManager Instance;
    public NetworkRunner Runner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<bool> StartGame(Fusion.GameMode mode, string roomName)
    {
        if (Runner == null)
        {
            Runner = gameObject.AddComponent<NetworkRunner>();
            Runner.ProvideInput = true;
            Runner.AddCallbacks(this);
            Debug.Log("NetworkRunner created.");
        }

        var sceneManager = GetComponent<NetworkSceneManagerDefault>();

        if (sceneManager == null)
            sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();

        var result = await Runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = roomName,
            PlayerCount = 2,
            SceneManager = sceneManager
        });

        Debug.Log($"StartGame Result: {result.Ok}");

        return result.Ok;
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player left: {player}");
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player joined: {player}");

        if (LobbyManager.Instance != null)
        {
            LobbyManager.Instance.PlayerJoined(player);
        }
    }
    public async void HostGame()
    {
        bool success = await NetworkManager.Instance.StartGame(Fusion.GameMode.Host, "TestRoom");

        Debug.Log("Success = " + success);
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        //throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        //throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        //throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        //throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        //throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        //throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        //throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server.");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        //throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        //throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }
}