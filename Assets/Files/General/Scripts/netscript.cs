using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;

public class netscript : MonoBehaviour
{
    public mainmenu menuScript;
    public netmanager netManager;

    private string settingsPath = "Data/settings.txt";
    DirectoryInfo dir = new DirectoryInfo("Data");
    public List<string> settings = new List<string>();
    private byte verifSettings = 0;
    GameObject selfPlayer;
    netbehaviour netBehaviour;
    public bool isHost = false;
    public bool isIngame = false;
    public bool isConnected = false;
    public int playerNumber = 0;
    public int p1n = 0;
    public int p2n = 0;
    public int p3n = 0;
    public List<string> serverData = new List<string> {"0", "ERROR", "0", "4", "nomap", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false"};
    public List<string> clientData = new List<string> {"0", "ERROR", "0", "4", "nomap", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false"};

    void Awake()
    {
        if (!dir.Exists)
        {
            dir.Create();
        }
        if (!File.Exists(settingsPath))
        {
            TextWriter tw = new StreamWriter(settingsPath, true);
            tw.WriteLine("GUEST\n75\n75\n75\n90\nFalse\nTrue");
            tw.Close();
        }
        settings = File.ReadAllLines(settingsPath).ToList();
    }

    public void RecievePlayerCount(int num)
    {
        if(p1n == 0)
        {
            p1n = num;
        }
        else if(p2n == 0)
        {
            p2n = num;
        }
        else if(p3n == 0)
        {
            p3n = num;
        }
        if(clientData[2] == "2")
        {
            if(p1n != 0)
            {
                serverData[2] = "1";
                serverData[9] = "false";
                serverData[10] = "ERROR";
                serverData[11] = "0";
                serverData[12] = "false";
                SendUpdateClients();
                p1n = 0;
            }
        }
        else if(clientData[2] == "3")
        {
            if(p1n != 0 && p2n != 0)
            {
                if(p1n != 2 && p2n != 2)
                {
                    serverData[2] = "2";
                    serverData[9] = serverData[13];
                    serverData[10] = serverData[14];
                    serverData[11] = serverData[15];
                    serverData[12] = serverData[16];
                    serverData[13] = "false";
                    serverData[14] = "ERROR";
                    serverData[15] = "0";
                    serverData[16] = "true";
                }
                else if(p1n != 3 && p2n != 3)
                {
                    serverData[2] = "2";
                    serverData[13] = "false";
                    serverData[14] = "ERROR";
                    serverData[15] = "0";
                    serverData[16] = "false";
                }
                SendUpdateClients();
                p1n = 0;
                p2n = 0;
            }
        }
        else if(clientData[2] == "4")
        {
            if(p1n != 0 && p2n != 0 && p3n != 0)
            {
                if(p1n != 2 && p2n != 2 && p3n != 2)
                {
                    serverData[2] = "3";
                    serverData[9] = serverData[13];
                    serverData[10] = serverData[14];
                    serverData[11] = serverData[15];
                    serverData[12] = serverData[16];
                    serverData[13] = serverData[17];
                    serverData[14] = serverData[18];
                    serverData[15] = serverData[19];
                    serverData[16] = "true";
                    serverData[17] = "false";
                    serverData[18] = "ERROR";
                    serverData[19] = "0";
                    serverData[20] = "true";
                }
                else if(p1n != 3 && p2n != 3 && p3n != 3)
                {
                    serverData[2] = "3";
                    serverData[13] = serverData[17];
                    serverData[14] = serverData[18];
                    serverData[15] = serverData[19];
                    serverData[16] = serverData[20];
                    serverData[17] = "false";
                    serverData[18] = "ERROR";
                    serverData[19] = "0";
                    serverData[20] = "true";
                }
                else if(p1n != 4 && p2n != 4 && p3n != 4)
                {
                    serverData[2] = "3";
                    serverData[17] = "false";
                    serverData[18] = "ERROR";
                    serverData[19] = "0";
                    serverData[20] = "false";
                }
                SendUpdateClients();
                p1n = 0;
                p2n = 0;
                p3n = 0;
            }
            
        }
    }

    public async void UpdateSettings()
    {
        verifSettings++;
        await Task.Delay(1000);
        if (verifSettings == 1)
        {
            File.WriteAllText(settingsPath, string.Join("\n", settings));
        }
        verifSettings--;
    }

    public static string GetLocalIPAddress()
    {
        foreach(NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.Address.ToString();
                    }
                }
            }
        }
        return "localhost";
    }

    public void OnServerStart()
    {
        isHost = true;
        serverData = new List<string> {"0", GetLocalIPAddress(), "0", "4", "nomap", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false"};
    }

    public void OnServerStop()
    {
        isHost = false;
        serverData[0] = "0";
        playerNumber = 0;
        if (!isIngame)
        {
            menuScript.OnLeave();
        }
    }

    public void OnServerJoin()
    {
        clientData = new List<string> {"0", "ERROR", "0", "4", "nomap", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false", "false", "ERROR", "0", "false"};
        if (!isIngame)
        {
            menuScript.ClientStarted();
        }
        SendPlayerJoin();
    }

    public void OnServerLeave()
    {
        playerNumber = 0;
        if (!isIngame)
        {
            menuScript.OnLeave();
        }
    }

    public void GetPlayer()
    {
        selfPlayer = GameObject.Find("MyPlayer");
        netBehaviour = selfPlayer.GetComponent<netbehaviour>();
        netBehaviour.SendPlayerJoin();
    }
    
    public void SendUpdateClients()
    {
        netBehaviour.SendUpdateClients();
    }

    public void RecieveUpdateClients(string newData)
    {
        clientData = newData.Split(new char[] { ',' }).ToList();
        if (clientData[0] == "2")
        {
            if (!isIngame)
            {
                menuScript.StartScene();
                isIngame = true;
            }
        }
        else
        {
            isIngame = false;
        }
        if (playerNumber == 0)
        {
            playerNumber = Int32.Parse(clientData[2]);
            playerNumber++;
            if (playerNumber >= 1 && playerNumber <= 4)
            {
                SendData(2, $"{playerNumber}");
                SendData(1 + (4 * playerNumber), "true");
                SendData(2 + (4 * playerNumber), settings[0]);
                if(playerNumber <= Int32.Parse(clientData[3]))
                {
                    SendData(3 + (4 * playerNumber), $"{playerNumber - 1}");
                }
                else
                {
                    SendData(3 + (4 * playerNumber), "0");
                }
                if (!isIngame)
                {
                    menuScript.JoinedLobby();
                }
            }
            else
            {
                if (isHost)
                {
                    netManager.StopHost();
                }
                else
                {
                    netManager.StopClient();
                }
            }
        }
        if (!isIngame)
        {
            menuScript.UpdateLobby();
        }
    }

    public void SendPlayerleft()
    {
        netBehaviour.SendPlayerLeft();
    }

    public async void SendPlayerJoin()
    {
        await Task.Delay(200);
        GetPlayer();
    }

    public void RecievePlayerJoin()
    {
        SendUpdateClients();
    }

    public void SendData(int number, string data)
    {
        netBehaviour.SendData(number, data);
    }

    public void RecieveData(int number, string data)
    {
        serverData[number] = data;
        SendUpdateClients();
    }
}
// ----------------------------------------------------------------------

// 0  //        Name                        string
// 1  //        Master Volume               0-100
// 2  //        Music Volume                0-100
// 3  //        Sound Effect Volume         0-100
// 4  //        FOV                         60-120
// 5  //        Dev HUD                     true-false

// ----------------------------------------------------------------------

// 0  //        Current Status              0 = Disconected / 1 = Lobby / 2 = Ingame
// 1  //        Room ID
// 2  //        Current Player
// 3  //        Max Player
// 4  //        Map
// 5  //        P1 Presence
// 6  //        P1 Name
// 7  //        P1 Role                     0 = Role 1 / 1 = Role 2 / 2 = Role 3 / 3 = Role 4
// 8  //        P1 -
// 9  //        P2 Presence
// 10 //        P2 Name
// 11 //        P2 Role
// 12 //        P2 -
// 13 //        P3 Presence
// 14 //        P3 Name
// 15 //        P3 Role
// 16 //        P3 Change?
// 17 //        P4 Presence
// 18 //        P4 Name
// 19 //        P4 Role
// 20 //        P4 Change?

// ----------------------------------------------------------------------

// TODO:
// Add a "Click to continue" and "Waiting for other players" text on loading screen
// Player movement / physic
// Crosshair
// Escape Menu
