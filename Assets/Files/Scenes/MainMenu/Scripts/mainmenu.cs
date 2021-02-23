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
    private List<string> settings = new List<string>();
    private string settingsPath = "Client/settings.txt";
    DirectoryInfo dir = new DirectoryInfo("Client");
    private byte verifSettings = 0;

    private bool clicked = false;

    public TextMeshProUGUI playerNameText;
    public TMP_InputField playerNameInput;
    public GameObject layer2;
    public Image invalidName;
    private byte verifName = 0;

    public TMP_InputField idInput;
    public Image invalidId;
    private byte verifId = 0;

    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeText;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeText;
    public Slider soundVolumeSlider;
    public TextMeshProUGUI soundVolumeText;
    public Slider fovSlider;
    public TextMeshProUGUI fovText;
    public Toggle devToggle;
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

    void Awake()
    {
        if (!dir.Exists)
        {
            dir.Create();
        }
        if (!File.Exists(settingsPath))
        {
            TextWriter tw = new StreamWriter(settingsPath, true);
            tw.WriteLine("GUEST\n75\n75\n75\n90\nfalse");
            tw.Close();
        }
        settings = File.ReadAllLines(settingsPath).ToList();
    }
    
    void Start()
    {
        mixer.SetFloat("Master", float.Parse(settings[1]) == 0 ? -9999 : Mathf.Log(float.Parse(settings[1]) / 100, 8) * 40);
        mixer.SetFloat("Music", float.Parse(settings[2]) == 0 ? -9999 : Mathf.Log(float.Parse(settings[2]) / 100, 8) * 40);
        mixer.SetFloat("Sound", float.Parse(settings[3]) == 0 ? -9999 : Mathf.Log(float.Parse(settings[3]) / 100, 8) * 40);
        playerNameText.text = settings[0];
        masterVolumeText.text = settings[1] + "%";
        musicVolumeText.text = settings[2] + "%";
        soundVolumeText.text = settings[3] + "%";
        fovText.text = settings[4] + "°";
        masterVolumeSlider.value = float.Parse(settings[1]) / 100;
        musicVolumeSlider.value = float.Parse(settings[2]) / 100;
        soundVolumeSlider.value = float.Parse(settings[3]) / 100;
        fovSlider.value = (float.Parse(settings[4]) - 60) / 60;
        fovSlider.value = (float.Parse(settings[4]) - 60) / 60;
        devToggle.isOn = bool.Parse(settings[5]);
        devHud.showGUI = bool.Parse(settings[5]);
        StartMusic();
    }

    void Update()
    {
        if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
        {
            Enter();
        }

        if (Input.GetKeyDown("escape"))
        {
            Escape();
        }
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
            backAudio.Play(0);
            menuJoin.SetActive(false);
            menuPlay.SetActive(true);
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

    public async void StartMusic()
    {
        await Task.Delay(200);
        musicAudio.Play(0);
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
                settings[0] = newName.ToUpper();
                playerNameText.text = settings[0];
                layer2.SetActive(false);
                playerNameInput.text = "";
                UpdateSettings();
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
        string oldSet = settings[1];
        settings[1] = (Math.Round(masterVolumeSlider.value * 100)).ToString("R");
        mixer.SetFloat("Master", float.Parse(settings[1]) == 0 ? -9999 : Mathf.Log(float.Parse(settings[1]) / 100, 8) * 40);
        if (oldSet != settings[1])
        {
            sliderAudio.Play(0);
        }
        masterVolumeText.text = settings[1] + "%";
        UpdateSettings();
    }
    
    public void MusicVolume()
    {
        string oldSet = settings[2];
        settings[2] = (Math.Round(musicVolumeSlider.value * 100)).ToString("R");
        mixer.SetFloat("Music", float.Parse(settings[2]) == 0 ? -9999 : Mathf.Log(float.Parse(settings[2]) / 100, 8) * 40);
        if (oldSet != settings[2])
        {
            sliderAudio.Play(0);
        }
        musicVolumeText.text = settings[2] + "%";
        UpdateSettings();
    }

    public void SoundVolume()
    {
        string oldSet = settings[3];
        settings[3] = (Math.Round(soundVolumeSlider.value * 100)).ToString("R");
        mixer.SetFloat("Sound", float.Parse(settings[3]) == 0 ? -9999 : Mathf.Log(float.Parse(settings[3]) / 100, 8) * 40);
        if (oldSet != settings[3])
        {
            sliderAudio.Play(0);
        }
        soundVolumeText.text = settings[3] + "%";
        UpdateSettings();
    }

    public void FovSlider()
    {
        string oldSet = settings[4];
        settings[4] = (Math.Round(fovSlider.value * 60 + 60)).ToString("R");
        if (oldSet != settings[4])
        {
            sliderAudio.Play(0);
        }
        fovText.text = settings[4] + "°";
        UpdateSettings();
    }

    public void ToggleDev()
    {
        if (clicked == true)
        {
            buttonAudio.Play(0);
            settings[5] = (devToggle.isOn).ToString();
            devHud.showGUI = bool.Parse(settings[5]);
            UpdateSettings();
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
        menuPlay.SetActive(false);
        menuJoin.SetActive(true);
        idInput.Select();
        idInput.ActivateInputField();
    }

    public void ConfirmId()
    {
        string newId = idInput.text.ToUpper();

        if (String.IsNullOrEmpty(newId))
        {
            buttonAudio.Play(0);
            idInput.text = "";
        }
        else
        {
            if (newId.All(char.IsLetterOrDigit) && newId.Length == 6)
            {
                buttonAudio.Play(0);
                idInput.text = "";
            }
            else
            {
                InvalidId();
                idInput.Select();
                idInput.ActivateInputField();
            }
        }
    }

    public async void InvalidId()
    {
        errorAudio.Play(0);
        verifId++;
        invalidId.CrossFadeAlpha(1f, 0.1f, false);
        await Task.Delay(1000);
        if (verifId == 1)
        {
            invalidId.CrossFadeAlpha(0f, 0.1f, false);
        }
        verifId--;
    }

    public void HostMenu()
    {
        buttonAudio.Play(0);
    }
}

//              Name
//              Master Volume
//              Music Volume
//              Sound Effect Volume
//              FOV
//              Dev HUD