public static class NetworkHelper
{
    public static bool IsClientAlsoHost(ulong clientId)
    {
        // Replace with actual logic to determine if the client is also the host.
        return clientId == 0;  // Assuming clientId 0 is reserved for the host for this mockup.
    }
}


/* using System.Collections;
using System.Collections.Generic;
using Unity.Netcode; 
using UnityEngine;

public class NetworkHelper : MonoBehaviour
{
    public static NetworkHelper Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InstanceStartHost()
    {
        // Your host starting logic here
    }

    public void InstanceStartClient()
    {
        // Your client starting logic here
    }

    public void InstanceStartServer()
    {
        // Your server starting logic here
    }

    public bool IsClient { get; set; } = false; // Example property. You might want to update this according to your logic.
    public bool IsServer { get; set; } = false; // Example property. You might want to update this according to your logic.

    private static void StartButtons()
    {
        if (GUILayout.Button("Host"))
            Singleton.InstanceStartHost(); 

        if (GUILayout.Button("Client"))
            Singleton.InstanceStartClient(); 

        if (GUILayout.Button("Server"))
            Singleton.InstanceStartServer(); 
    }

    private static void ServerButtons()
    {
        // Implement the server-specific buttons here.
    }

    private static void ClientButtons()
    {
        // Implement the client-specific buttons here.
    }

    public static void GUILayoutNetworkControls()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!Singleton.IsClient && !Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            if (Singleton.IsServer)
            {
                ServerButtons();
            }
            else
            {
                ClientButtons();
            }
        }

        GUILayout.EndArea();
    }
}
 */