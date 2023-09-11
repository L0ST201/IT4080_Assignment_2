using UnityEngine;
using Unity.Netcode;

public class NetworkHandler : NetworkBehaviour
{
    private bool hasPrinted = false;
    private ulong myClientId;

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientStarted += OnClientStarted;
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += ClientOnClientDisconnected;

            myClientId = NetworkManager.Singleton.LocalClientId;
        }
    }

   public override void OnDestroy()
    {
        base.OnDestroy();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= ClientOnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= ClientOnClientDisconnected;
        }
    }

    private void OnClientStarted()
    {
        Debug.Log("Client Started!");

        NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientOnClientDisconnected;

        PrintMe();
    }

    private void OnServerStarted()
    {
        Debug.Log("Server Started!");

        NetworkManager.Singleton.OnClientConnectedCallback += ServerOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += ServerOnClientDisconnected;

        PrintMe();
    }

    private void PrintMe()
    {
        if (hasPrinted) 
        {
            return;
        }
        
        Debug.Log("I AM");
        
        hasPrinted = true;

        if (IsHost) 
        {
            Debug.Log($" the Host! {NetworkManager.ServerClientId}/{NetworkManager.Singleton.LocalClientId}");
        }
        else if (IsServer) 
        {
            Debug.Log($" the Server! {NetworkManager.ServerClientId}");
        }
        else if (IsClient) 
        {
            Debug.Log($" a Client! {NetworkManager.Singleton.LocalClientId}");
        }
        else
        {
            Debug.Log("Nothing yet");
        }
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

    private void ServerOnClientConnected(ulong clientId)
    {
        Debug.Log($"Client (ID: {clientId}) connected to the server");
    }

    private void ServerOnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client (ID: {clientId}) disconnected from the server");
    }
}
