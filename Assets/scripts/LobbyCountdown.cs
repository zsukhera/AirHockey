using Fusion;
using UnityEngine;

public class LobbyCountdown : NetworkBehaviour
{
    public static LobbyCountdown Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ShowCountdown(int seconds)
    {
        if (LobbyManager.Instance != null)
        {
            LobbyManager.Instance.UpdateCountdown(seconds);
        }
    }
}