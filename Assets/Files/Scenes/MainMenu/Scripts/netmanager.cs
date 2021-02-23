using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class netmanager : NetworkManager
{
    public netbehaviour networkBehaviour;

    public override void OnStartServer()
    {
        networkBehaviour.ServerStart();
    }

    public override void OnStopServer()
    {
        networkBehaviour.ServerStop();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        networkBehaviour.ClientConnect();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        networkBehaviour.ClientDisconnect();
    }
}
