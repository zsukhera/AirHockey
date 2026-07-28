using System.Threading.Tasks;
using Fusion;
using UnityEngine;

public class NetworkManager : MonoBehaviour
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

    public async void HostGame()
    {
        bool success = await NetworkManager.Instance.StartGame(Fusion.GameMode.Host, "TestRoom");

        Debug.Log("Success = " + success);
    }
}