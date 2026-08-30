using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector2 pointerWorldPosition;
    public NetworkBool isDragging;
    public NetworkBool dragStarted;
    public NetworkBool dragEnded;
}
