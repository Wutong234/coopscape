using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class mainmenu : MonoBehaviour
{
    private bool clicked = false;
    private bool isLoading = false;
    private bool isChanging = false;
    private bool hasStarted = false;

    public TextMeshProUGUI playerNameText;
    public TMP_InputField playerNameInput;
    public GameObject layer2;
    public Image invalidName;
    private byte verifName = 0;
    private byte verifPlayer = 0;
    private byte oldVerifPlayer = 0;
    private int frames = 0;

    public TMP_InputField idInput;
    public Image invalidId;
    public Image loading;
    public GameObject loadingObject;
    public GameObject grey;

    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeText;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeText;
    public Slider soundVolumeSlider;
    public TextMeshProUGUI soundVolumeText;
    public Slider fovSlider;
    public TextMeshProUGUI fovText;
    public Toggle devToggle;
    public Toggle fullToggle;
    public NetworkManagerHUD devHud;
    public GameObject changeMap;

    public GameObject menuMain;
    public GameObject menuPlay;
    public GameObject menuJoin;
    public GameObject menuLobby;
    public GameObject menuMap;
    public GameObject menuSettings;
    public GameObject menuGameplay;
    public GameObject menuVideo;
    public GameObject menuVolume;
    public GameObject menuMap2;
    public GameObject menuMap3;
    public GameObject menuMap4;

    public AudioMixer mixer;
    public AudioSource musicAudio;
    public AudioSource buttonAudio;
    public AudioSource backAudio;
    public AudioSource exitAudio;
    public AudioSource errorAudio;
    public AudioSource sliderAudio;
    public AudioSource transitionAudio;

    public NetworkManager manager;
    public netscript netScript;
    public loadmanager loadManager;

    public GameObject imageHost;
    public GameObject imageClient;
    public GameObject startButton;
    public GameObject waitingButton;
    public TMP_Dropdown drop1;
    public TMP_Dropdown drop2;
    public TMP_Dropdown drop3;
    public TMP_Dropdown drop4;
    public GameObject drop1go;
    public GameObject drop2go;
    public GameObject drop3go;
    public GameObject drop4go;
    public GameObject dropArrow1;
    public GameObject dropArrow2;
    public GameObject dropArrow3;
    public GameObject dropArrow4;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public TextMeshProUGUI name1;
    public TextMeshProUGUI name2;
    public TextMeshProUGUI name3;
    public TextMeshProUGUI name4;
    public TextMeshProUGUI roomId;
    public TMP_InputField roomIdSelect;
    public TextMeshProUGUI playerCount;
    public TextMeshProUGUI mapName;

    public Image imgHost;
    public Image imgClient;
    public Sprite imageNoMap;
    public Sprite imageMap1;
    public Sprite imageTestMap;
    
    void Start()
    {
        mixer.SetFloat("Master", float.Parse(netScript.settings[1]) == 0 ? -9999 : Mathf.Log(float.Parse(netScript.settings[1]) / 100, 8) * 40);
        mixer.SetFloat("Music", float.Parse(netScript.settings[2]) == 0 ? -9999 : Mathf.Log(float.Parse(netScript.settings[2]) / 100, 8) * 40);
        mixer.SetFloat("Sound", float.Parse(netScript.settings[3]) == 0 ? -9999 : Mathf.Log(float.Parse(netScript.settings[3]) / 100, 8) * 40);
        playerNameText.text = netScript.settings[0];
        masterVolumeText.text = netScript.settings[1] + "%";
        musicVolumeText.text = netScript.settings[2] + "%";
        soundVolumeText.text = netScript.settings[3] + "%";
        fovText.text = netScript.settings[4] + "°";
        masterVolumeSlider.value = float.Parse(netScript.settings[1]) / 100;
        musicVolumeSlider.value = float.Parse(netScript.settings[2]) / 100;
        soundVolumeSlider.value = float.Parse(netScript.settings[3]) / 100;
        fovSlider.value = (float.Parse(netScript.settings[4]) - 60) / 60;
        fovSlider.value = (float.Parse(netScript.settings[4]) - 60) / 60;
        devToggle.isOn = bool.Parse(netScript.settings[5]);
        devHud.showGUI = bool.Parse(netScript.settings[5]);
        fullToggle.isOn = bool.Parse(netScript.settings[6]);
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, bool.Parse(netScript.settings[6]));
        StartMusic();
    }

    void Update()
    {
        if(!hasStarted)
        {
            if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
            {
                Enter();
            }

            if (Input.GetKeyDown("escape"))
            {
                Escape();
            }
    
            if (grey.activeSelf)
            {
                Rotate();
            }

            if (netScript.isHost)
            {
                frames++;
                if (frames == 60)
                {
                    frames = 0;
                    CheckPlayerCount();
                }
            }
        }
    }

    public void CheckPlayerCount()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Player(Clone)");
        foreach(GameObject go in objects)
        {
            verifPlayer++;
        }
        if(verifPlayer < oldVerifPlayer)
        {
            netScript.SendPlayerleft();
        }
        oldVerifPlayer = verifPlayer;
        verifPlayer = 0;
    }

    public void Enter()
    {
        if (layer2.activeSelf)
        {
            ConfirmName();
        }
        
        if (menuJoin.activeSelf)
        {
            ConfirmId();
        }
        
    }

    public void Escape()
    {
        if (menuPlay.activeSelf)
        {
            backAudio.Play(0);
            menuPlay.SetActive(false);
            menuMain.SetActive(true);
        }
        else if (layer2.activeSelf)
        {
            CancelName();
        }
        else if (menuJoin.activeSelf)
        {
            if (isLoading == false)
            {
                backAudio.Play(0);
                menuJoin.SetActive(false);
                menuPlay.SetActive(true);
                idInput.text = "";
            }
        }
        else if (menuSettings.activeSelf)
        {
            backAudio.Play(0);
            menuSettings.SetActive(false);
            menuMain.SetActive(true);
        }
        else if (menuGameplay.activeSelf)
        {
            menuGameplay.SetActive(false);
            menuSettings.SetActive(true);
            CancelName();
        }
        else if (menuVideo.activeSelf)
        {
            backAudio.Play(0);
            menuVideo.SetActive(false);
            menuSettings.SetActive(true);
        }
        else if (menuVolume.activeSelf)
        {
            backAudio.Play(0);
            menuVolume.SetActive(false);
            menuSettings.SetActive(true);
        }
        else if (menuMap.activeSelf)
        {
            backAudio.Play(0);
            menuMap.SetActive(false);
            changeMap.SetActive(false);
            menuLobby.SetActive(true);
        }
        else if (menuMap2.activeSelf)
        {
            backAudio.Play(0);
            menuMap2.SetActive(false);
            menuMap.SetActive(true);
        }
        else if (menuMap3.activeSelf)
        {
            backAudio.Play(0);
            menuMap3.SetActive(false);
            menuMap.SetActive(true);
        }
        else if (menuMap4.activeSelf)
        {
            backAudio.Play(0);
            menuMap4.SetActive(false);
            menuMap.SetActive(true);
        }
    }

    public void Rotate()
    {
        loadingObject.transform.Rotate(0f, 0f, -300f * Time.deltaTime, Space.Self);
    }

    public void StartMusic()
    {
        musicAudio.Play(0);
    }
    
    public void ClickButton()
    {
        clicked = true;
        buttonAudio.Play(0);
    }

    public async void ExitButton()
    {
        exitAudio.Play(0);
        await Task.Delay(100);
        musicAudio.Pause();
        await Task.Delay(250);
        Application.Quit();
    }
    
    public void ChangeName()
    {
        buttonAudio.Play(0);
        invalidName.CrossFadeAlpha(0f, 0f, false);
        layer2.SetActive(true);
        playerNameInput.Select();
        playerNameInput.ActivateInputField();
    }

    public void ConfirmName()
    {
        string newName = playerNameInput.text;

        if (String.IsNullOrEmpty(newName))
        {
            buttonAudio.Play(0);
            layer2.SetActive(false);
            playerNameInput.text = "";
        }
        else
        {
            if (newName.All(char.IsLetterOrDigit))
            {
                buttonAudio.Play(0);
                netScript.settings[0] = newName.ToUpper();
                playerNameText.text = netScript.settings[0];
                layer2.SetActive(false);
                playerNameInput.text = "";
                netScript.UpdateSettings();
            }
            else
            {
                InvalidName();
                playerNameInput.Select();
                playerNameInput.ActivateInputField();
            }
        }
    }

    public async void InvalidName()
    {
        errorAudio.Play(0);
        verifName++;
        invalidName.CrossFadeAlpha(1f, 0.1f, false);
        await Task.Delay(1000);
        if (verifName == 1)
        {
            invalidName.CrossFadeAlpha(0f, 0.1f, false);
        }
        verifName--;
    }

    public void CancelName()
    {
        backAudio.Play(0);
        layer2.SetActive(false);
        playerNameInput.text = "";
    }

    public void MasterVolume()
    {
        string oldSet = netScript.settings[1];
        netScript.settings[1] = (Math.Round(masterVolumeSlider.value * 100)).ToString("R");
        mixer.SetFloat("Master", float.Parse(netScript.settings[1]) == 0 ? -9999 : Mathf.Log(float.Parse(netScript.settings[1]) / 100, 8) * 40);
        if (oldSet != netScript.settings[1])
        {
            sliderAudio.Play(0);
        }
        masterVolumeText.text = netScript.settings[1] + "%";
        netScript.UpdateSettings();
    }
    
    public void MusicVolume()
    {
        string oldSet = netScript.settings[2];
        netScript.settings[2] = (Math.Round(musicVolumeSlider.value * 100)).ToString("R");
        mixer.SetFloat("Music", float.Parse(netScript.settings[2]) == 0 ? -9999 : Mathf.Log(float.Parse(netScript.settings[2]) / 100, 8) * 40);
        if (oldSet != netScript.settings[2])
        {
            sliderAudio.Play(0);
        }
        musicVolumeText.text = netScript.settings[2] + "%";
        netScript.UpdateSettings();
    }

    public void SoundVolume()
    {
        string oldSet = netScript.settings[3];
        netScript.settings[3] = (Math.Round(soundVolumeSlider.value * 100)).ToString("R");
        mixer.SetFloat("Sound", float.Parse(netScript.settings[3]) == 0 ? -9999 : Mathf.Log(float.Parse(netScript.settings[3]) / 100, 8) * 40);
        if (oldSet != netScript.settings[3])
        {
            sliderAudio.Play(0);
        }
        soundVolumeText.text = netScript.settings[3] + "%";
        netScript.UpdateSettings();
    }

    public void FovSlider()
    {
        string oldSet = netScript.settings[4];
        netScript.settings[4] = (Math.Round(fovSlider.value * 60 + 60)).ToString("R");
        if (oldSet != netScript.settings[4])
        {
            sliderAudio.Play(0);
        }
        fovText.text = netScript.settings[4] + "°";
        netScript.UpdateSettings();
    }

    public void ToggleDev()
    {
        if (clicked == true)
        {
            buttonAudio.Play(0);
            netScript.settings[5] = (devToggle.isOn).ToString();
            devHud.showGUI = bool.Parse(netScript.settings[5]);
            netScript.UpdateSettings();
        }
    }

    public void ToggleFullscreen()
    {
        if (clicked == true)
        {
            buttonAudio.Play(0);
            netScript.settings[6] = (fullToggle.isOn).ToString();
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, bool.Parse(netScript.settings[6]));
            netScript.UpdateSettings();
        }
    }

    public void StartHoverImage()
    {
        changeMap.SetActive(true);
    }

    public void EndHoverImage()
    {
        changeMap.SetActive(false);
    }

    public void JoinMenu()
    {
        buttonAudio.Play(0);
        invalidId.CrossFadeAlpha(0f, 0f, false);
        loading.CrossFadeAlpha(0f, 0f, false);
        grey.SetActive(false);
        idInput.text = "";
        menuPlay.SetActive(false);
        menuJoin.SetActive(true);
        idInput.Select();
        idInput.ActivateInputField();
    }

    public void ConfirmId()
    {
        buttonAudio.Play(0);
        string newId = idInput.text;
        if (String.IsNullOrEmpty(newId))
        {
            idInput.text = "";
            idInput.Select();
            idInput.ActivateInputField();
        }
        else
        {
            manager.networkAddress = newId;
            CheckId();
            ClientStart();
        }
    }

    public async void ClientStart()
    {
        await Task.Delay(200);
        manager.StartClient();
    }

    public async void CheckId()
    {
        isLoading = true;
        invalidId.CrossFadeAlpha(0f, 0.1f, false);
        loading.CrossFadeAlpha(1f, 0.1f, false);
        grey.SetActive(true);
        await Task.Delay(4500);
        if (isLoading == true)
        {
            idInput.text = "";
            manager.StopClient();
            isLoading = false;
            idInput.Select();
            idInput.ActivateInputField();
            manager.networkAddress = "localhost";
            loading.CrossFadeAlpha(0f, 0.1f, false);
            grey.SetActive(false);
            InvalidId();
        }
    }

    public async void InvalidId()
    {
        await Task.Delay(100);
        invalidId.CrossFadeAlpha(1f, 0.2f, false);
        errorAudio.Play(0);
        await Task.Delay(1600);
        invalidId.CrossFadeAlpha(0f, 0.2f, false);
    }

    public void HostMenu()
    {
        buttonAudio.Play(0);
        manager.StartHost();
    }
    
    public void ClientStarted()
    {
        isLoading = false;
    }

    public void JoinedLobby()
    {
        sliderAudio.Play(0);
        menuPlay.SetActive(false);
        menuJoin.SetActive(false);
        menuLobby.SetActive(true);
        idInput.text = "";
        loading.CrossFadeAlpha(0f, 0.1f, false);
        grey.SetActive(false);
        if (netScript.isHost)
        {
            imageHost.SetActive(true);
            imageClient.SetActive(false);
            startButton.SetActive(true);
            waitingButton.SetActive(false);
            drop1.interactable = true;
            drop2.interactable = true;
            drop3.interactable = true;
            drop4.interactable = true;
            dropArrow1.SetActive(true);
            dropArrow2.SetActive(true);
            dropArrow3.SetActive(true);
            dropArrow4.SetActive(true);
        }
        else
        {
            imageHost.SetActive(false);
            imageClient.SetActive(true);
            startButton.SetActive(false);
            waitingButton.SetActive(true);
            drop1.interactable = false;
            drop2.interactable = false;
            drop3.interactable = false;
            drop4.interactable = false;
            dropArrow1.SetActive(false);
            dropArrow2.SetActive(false);
            dropArrow3.SetActive(false);
            dropArrow4.SetActive(false);
        }

    }

    public void LeaveLobby()
    {
        if(!hasStarted)
        {
            if (netScript.isHost)
            {
                manager.StopHost();
            }
            else
            {
                manager.StopClient();
            }
            backAudio.Play(0);
        }
    }

    public void OnLeave()
    {
        isLoading = false;
        idInput.text = "";
        loading.CrossFadeAlpha(0f, 0.1f, false);
        grey.SetActive(false);
        menuPlay.SetActive(false);
        menuJoin.SetActive(false);
        menuLobby.SetActive(false);
        menuMain.SetActive(true);
    }

    public void ChangeMap()
    {
        if (!hasStarted)
        {
            buttonAudio.Play(0);
            menuLobby.SetActive(false);
            menuMap.SetActive(true);
        }
    }

    public void SendTestMap()
    {
        netScript.serverData[4] = "testmap";
        Send2PlayerMap();
        SendAnyMap();
    }

    public void SendMap1()
    {
        netScript.serverData[4] = "map1";
        Send2PlayerMap();
        SendAnyMap();
    }

    public void Send2PlayerMap()
    {
        netScript.serverData[3] = "2";
        if(netScript.clientData[7] == "2" || netScript.clientData[7] == "3")
        {
            netScript.serverData[7] = "0";
        }
        if(netScript.clientData[11] == "2" || netScript.clientData[11] == "3")
        {
            netScript.serverData[11] = "0";
        }
        if(netScript.clientData[15] == "2" || netScript.clientData[15] == "3")
        {
            netScript.serverData[15] = "0";
        }
        if(netScript.clientData[19] == "2" || netScript.clientData[19] == "3")
        {
            netScript.serverData[19] = "0";
        }
    }

    public void SendAnyMap()
    {
        buttonAudio.Play(0);
        changeMap.SetActive(false);
        menuMap2.SetActive(false);
        menuLobby.SetActive(true);
        netScript.SendUpdateClients();
    }

    public void SendDrop1()
    {
        if (!isChanging && !hasStarted)
        {
            netScript.serverData[7] = drop1.value.ToString();
            netScript.SendUpdateClients();
        }
    }

    public void SendDrop2()
    {
        if (!isChanging && !hasStarted)
        {
            netScript.serverData[11] = drop2.value.ToString();
            netScript.SendUpdateClients();
        }
    }

    public void SendDrop3()
    {
        if (!isChanging && !hasStarted)
        {
            netScript.serverData[15] = drop3.value.ToString();
            netScript.SendUpdateClients();
        }
    }

    public void SendDrop4()
    {
        if (!isChanging && !hasStarted)
        {
            netScript.serverData[19] = drop4.value.ToString();
            netScript.SendUpdateClients();
        }
    }

    public void UpdateLobby()
    {
        if(!hasStarted)
        {
            if(netScript.playerNumber == 3)
            {
                if(netScript.clientData[16] == "true")
                {
                    netScript.playerNumber = 2;
                    netScript.SendData(16, "false");
                }
            }
            if(netScript.playerNumber == 4)
            {
                if(netScript.clientData[20] == "true")
                {
                    netScript.playerNumber = 2;
                    netScript.SendData(20, "false");
                }
            }
            player1.SetActive(bool.Parse(netScript.clientData[5]));
            player2.SetActive(bool.Parse(netScript.clientData[9]));
            player3.SetActive(bool.Parse(netScript.clientData[13]));
            player4.SetActive(bool.Parse(netScript.clientData[17]));
            name1.text = netScript.clientData[6];
            name2.text = netScript.clientData[10];
            name3.text = netScript.clientData[14];
            name4.text = netScript.clientData[18];
            roomId.text = netScript.clientData[1];
            roomIdSelect.text = netScript.clientData[1] + "\u0000";
            playerCount.text = netScript.clientData[2] + "/" + netScript.clientData[3];
            if (netScript.clientData[4] == "nomap")
            {
                mapName.text = "NO MAP";
                imgHost.sprite = imageNoMap;
                imgClient.sprite = imageNoMap;
                drop1go.SetActive(false);
                drop2go.SetActive(false);
                drop3go.SetActive(false);
                drop4go.SetActive(false);
            }
            else
            {
                drop1go.SetActive(true);
                drop2go.SetActive(true);
                drop3go.SetActive(true);
                drop4go.SetActive(true);
            }
            if(netScript.clientData[4] == "testmap")
            {
                mapName.text = "TEST MAP";
                imgHost.sprite = imageTestMap;
                imgClient.sprite = imageTestMap;
                drop1.options.Clear();
                drop2.options.Clear();
                drop3.options.Clear();
                drop4.options.Clear();
                List<string> list = new List<string> { "ROLE 1", "ROLE 2" };
                foreach (string t in list)
                {
                    drop1.options.Add (new TMP_Dropdown.OptionData() {text=t});
                    drop2.options.Add (new TMP_Dropdown.OptionData() {text=t});
                    drop3.options.Add (new TMP_Dropdown.OptionData() {text=t});
                    drop4.options.Add (new TMP_Dropdown.OptionData() {text=t});
                }
            }
            if (netScript.clientData[4] == "map1")
            {
                mapName.text = "MAP 1";
                imgHost.sprite = imageMap1;
                imgClient.sprite = imageMap1;
                drop1.options.Clear();
                drop2.options.Clear();
                drop3.options.Clear();
                drop4.options.Clear();
                List<string> list = new List<string> { "SCIENTIST", "PROFESSOR" };
                foreach (string t in list)
                {
                    drop1.options.Add (new TMP_Dropdown.OptionData() {text=t});
                    drop2.options.Add (new TMP_Dropdown.OptionData() {text=t});
                    drop3.options.Add (new TMP_Dropdown.OptionData() {text=t});
                    drop4.options.Add (new TMP_Dropdown.OptionData() {text=t});
                }
            }
            isChanging = true;
            drop1.value = Int32.Parse(netScript.clientData[7]) + 1;
            drop2.value = Int32.Parse(netScript.clientData[11]) - 1;
            drop3.value = Int32.Parse(netScript.clientData[15]) - 1;
            drop4.value = Int32.Parse(netScript.clientData[19]) - 1;
            drop1.value = Int32.Parse(netScript.clientData[7]);
            drop2.value = Int32.Parse(netScript.clientData[11]);
            drop3.value = Int32.Parse(netScript.clientData[15]);
            drop4.value = Int32.Parse(netScript.clientData[19]);
            isChanging = false;
        }
    }

    public void StartGame()
    {
        if(netScript.isHost && netScript.clientData[2] == netScript.clientData[3] && netScript.clientData[4] != "nomap")
        {
            if(netScript.clientData[3] == "2" && netScript.clientData[7] != netScript.clientData[11])
            {
                netScript.serverData[0] = "2";
                netScript.SendUpdateClients();
            }
            else if(netScript.clientData[3] == "3" && netScript.clientData[7] != netScript.clientData[11] && netScript.clientData[11] != netScript.clientData[15])
            {
                netScript.serverData[0] = "2";
                netScript.SendUpdateClients();
            }
            else if(netScript.clientData[3] == "4" && netScript.clientData[7] != netScript.clientData[11] && netScript.clientData[11] != netScript.clientData[15] && netScript.clientData[15] != netScript.clientData[19])
            {
                netScript.serverData[0] = "2";
                netScript.SendUpdateClients();
            }
            else
            {
                errorAudio.Play(0);
            }
        }
        else
        {
            errorAudio.Play(0);
        }
    }

    public async void StartScene()
    {
        hasStarted = true;
        await Task.Delay(250);
        loadManager.StartLoadingScreen();
        transitionAudio.Play(0);
        await Task.Delay(50);
        musicAudio.Pause();
    }
}
