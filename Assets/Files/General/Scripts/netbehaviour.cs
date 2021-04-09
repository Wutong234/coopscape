using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class netbehaviour : NetworkBehaviour
{
    GameObject netMan;
    netscript netScript;

    void Start()
    {
        if(isLocalPlayer)
        {
            gameObject.name = "MyPlayer";
        }
    }

    public void GetNetScript()
    {
        if (netScript == null)
        {
            netMan = GameObject.Find("NetworkManager");
            netScript = netMan.GetComponent<netscript>();
        }
    }

    public void SendData(int number, string data)
    {
        RecieveData(number, data);
    }

    [Command(ignoreAuthority = true)]
    void RecieveData(int number, string data)
    {
        GetNetScript();
        netScript.RecieveData(number, data);
    }

    public void SendPlayerJoin()
    {
        RecievePlayerJoin();
    }
    
    [Command(ignoreAuthority = true)]
    void RecievePlayerJoin()
    {
        GetNetScript();
        netScript.RecievePlayerJoin();
    }

    public void SendUpdateClients()
    {
        GetNetScript();
        RecieveUpdateClients(String.Join(",", netScript.serverData));
    }

    [ClientRpc]
    void RecieveUpdateClients(string newData)
    {
        GetNetScript();
        netScript.RecieveUpdateClients(newData);
    }
    
    public void SendPlayerLeft()
    {
        RecievePlayerLeft();
    }

    [ClientRpc]
    public void RecievePlayerLeft()
    {
        ReSendPlayerLeft();
    }

    public void ReSendPlayerLeft()
    {
        ReRecievePlayerLeft(netScript.playerNumber);
    }

    [Command(ignoreAuthority = true)]
    public void ReRecievePlayerLeft(int num)
    {
        netScript.RecievePlayerCount(num);
    }
}
