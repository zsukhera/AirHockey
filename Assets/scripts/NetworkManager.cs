using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef gameplayManagerPrefab;
    public static NetworkManager Instance;
    public NetworkRunner Runner;
    private bool matchStarting;
    public NetworkObject lobbyControllerPrefab;
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
    private async void StartMatchCountdown()
    {
        Debug.Log("Starting countdown");

        for (int i = 3; i > 0; i--)
        {
            if (LobbyManager.Instance != null)
            {
                LobbyNetworkController.Instance.RPC_UpdateCountdown(i);
            }

            await Task.Delay(1000);
        }


        if (LobbyManager.Instance != null)
        {
            LobbyNetworkController.Instance.RPC_UpdateCountdown(0);
        }


        await Task.Delay(500);


        Debug.Log("Loading gameplay scene");

        await Runner.LoadScene(
            "Online GamePlay",
            LoadSceneMode.Single
        );
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
        if (result.Ok && mode == Fusion.GameMode.Host)
        {
            Runner.Spawn(lobbyControllerPrefab);
        }
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

        if (runner.IsServer && runner.ActivePlayers.Count() == 2 && !matchStarting)
        {
            matchStarting = true;
            StartMatchCountdown();
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
    { if (Camera.main == null) return; 
        NetworkInputData data = new NetworkInputData(); 
        bool mouseDown = Input.GetMouseButtonDown(0); 
        bool mouseHeld = Input.GetMouseButton(0); 
        bool mouseUp = Input.GetMouseButtonUp(0); 
        data.dragStarted = mouseDown; 
        data.isDragging = mouseHeld; data.dragEnded = mouseUp; 
        Vector3 mouseScreenPosition = Input.mousePosition; 
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z); 
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); 
        data.pointerWorldPosition = worldPosition; 
        input.Set(data); 
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

    public async void LoadGameScene()
    {
        if (Runner.IsServer)
        {
            Debug.Log("Loading OnlineGame scene");

            await Runner.LoadScene(
                "Online GamePlay",
                LoadSceneMode.Single
            );
        }
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("Scene loaded: " + runner.SceneManager);

        if (!runner.IsServer)
            return;

        Debug.Log("Spawning GameplayManager");

        runner.Spawn(
            gameplayManagerPrefab,
            Vector3.zero,
            Quaternion.identity
        );
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }
}