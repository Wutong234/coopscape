using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class netmanager : NetworkManager
{
    public netscript netScript;

    public override void OnStartServer()
    {
        netScript.OnServerStart();
    }
    
    public override void OnStopServer()
    {
        netScript.OnServerStop();
    }
    
    public override void OnClientConnect(NetworkConnection connection)
    {
        ClientScene.AddPlayer(connection);
        netScript.OnServerJoin();
    }
    
    public override void OnClientDisconnect(NetworkConnection connection)
    {
        netScript.OnServerLeave();
    }
}
