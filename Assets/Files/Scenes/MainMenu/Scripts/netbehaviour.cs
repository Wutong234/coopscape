using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;

public class netbehaviour : NetworkBehaviour
{
    public mainmenu menuManager;

    public void ServerStart()
    {
        print("I just started hosting a server");
    }

    public void ServerStop()
    {
        print("I just stoped hosting a server");
    }

    public void ClientConnect()
    {
        print("I joined a server");
    }

    public void ClientDisconnect()
    {
        print("I left a server");
    }
    
    // Called from client, executed on host
    [Command]
    void Hola()
    {
        print("Recieved HOLA");
    }

    // Called from host, executed on client
    [ClientRpc]
    void GetInfo()
    {
        print("Recieved RPC");
    }
}

    //  info[0] IP
    //  info[1] Map
    //  info[2] Player Count
    //  info[3] Max player count
    //  info[4] P1 name
    //  info[5] P1 role
    //  info[6] P1 ready
    //  info[7] P2 name
    //  info[8] P2 role
    //  info[9] P2 ready
    //  info[10] P3 name
    //  info[11] P3 role
    //  info[12] P3 ready
    //  info[13] P4 name
    //  info[14] P4 role
    //  info[15] P4 ready
    //  info[15] Game state