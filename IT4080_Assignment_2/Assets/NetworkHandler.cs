using UnityEngine;
using Unity.Netcode;

public class NetworkHandler : MonoBehaviour
{
    private ulong myClientId;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientOnClientDisconnected;

        myClientId = NetworkManager.Singleton.LocalClientId;
    }

    private void ClientOnClientConnected(ulong clientId)
    {
        PrintMe();

        if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.IsClient && myClientId == clientId)
        {
            Debug.Log($"I (ID: {clientId}, the host) have noticed a new client connected.");
        }
        else
        {
            Debug.Log($"I (ID: {clientId}) have connected to the server.");
        }
    }

    private void ClientOnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.IsClient && myClientId == clientId)
        {
            Debug.Log($"I (ID: {clientId}, the host) have noticed a client disconnected.");
        }
        else
        {
            Debug.Log($"I (ID: {clientId}) have disconnected from the server.");
        }
    }

    private void PrintMe()
    {
        Debug.Log("PrintMe called.");
    }
}
