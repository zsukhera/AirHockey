using Fusion;
using UnityEngine;

public class LobbyNetworkController : NetworkBehaviour
{
    public static LobbyNetworkController Instance;


    private void Awake()
    {
        Instance = this;
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_UpdateCountdown(int number)
    {
        if (LobbyManager.Instance != null)
        {
            LobbyManager.Instance.UpdateCountdown(number);
        }
    }
}