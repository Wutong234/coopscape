using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadmanager : MonoBehaviour
{
    public netscript netScript;
    public GameObject images;
    public GameObject image2;
    public Image img1;
    public Image img2;

    public bool isRotating = false;
    public bool isFlashing = false;
    public bool stopFlashing = false;
    public bool hasLoaded = false;
    public float frames = 0;

    void Awake()
    {
        img1.CrossFadeAlpha(0f, 0f, false);
        img2.CrossFadeAlpha(0f, 0f, false);
    }

    void Update()
    {
        if (isRotating)
        {
            Rotate();
        }
        if (isFlashing && !stopFlashing)
        {
            Flash();
        }
    }

    public void Rotate()
    {
        frames += Time.deltaTime;
        if(frames >= 0.05)
        {
            frames = 0;
            image2.transform.Rotate(0f, 0f, -12.85714286f, Space.Self);
        }
    }

    public async void Flash()
    {
        isFlashing = false;
        img2.CrossFadeAlpha(0.8f, 0.75f, false);
        await Task.Delay(850);
        if(!stopFlashing)
        {
            img2.CrossFadeAlpha(0.55f, 0.75f, false);
        }
        await Task.Delay(850);
        isFlashing = true;
    }

    public async void StartLoadingScreen()
    {
        await Task.Delay(50);
        isRotating = true;
        images.SetActive(true);
        img1.CrossFadeAlpha(1f, 0.2f, false);
        await Task.Delay(600);
        img2.CrossFadeAlpha(0.8f, 0.2f, false);
        await Task.Delay(300);
        isFlashing = true;
        StartScene();
        await Task.Delay(5000);
        isFlashing = false;
        stopFlashing = true;
        img2.CrossFadeAlpha(0f, 0.25f, false);
        await Task.Delay(1000);
        isRotating = false;
        // TODO Start Ambiant Music
        img1.CrossFadeAlpha(0f, 0.25f, false);
        await Task.Delay(150);
        images.SetActive(false);
        hasLoaded = true;
    }

    public void StartScene()
    {
        if(netScript.clientData[4] == "testmap")
        {
            if(netScript.playerNumber == 1)
            {
                if(netScript.clientData[7] == "0")
                {
                    SceneManager.LoadScene("TestMapP1");
                }
                if(netScript.clientData[7] == "1")
                {
                    SceneManager.LoadScene("TestMapP2");
                }
            }
            
            if(netScript.playerNumber == 2)
            {
                if(netScript.clientData[11] == "0")
                {
                    SceneManager.LoadScene("TestMapP1");
                }
                if(netScript.clientData[11] == "1")
                {
                    SceneManager.LoadScene("TestMapP2");
                }
            }
        }
        if(netScript.clientData[4] == "map1")
        {
            if(netScript.playerNumber == 1)
            {
                if(netScript.clientData[7] == "0")
                {
                    SceneManager.LoadScene("Map1P1");
                }
                if(netScript.clientData[7] == "1")
                {
                    SceneManager.LoadScene("Map1P2");
                }
            }
            if(netScript.playerNumber == 2)
            {
                if(netScript.clientData[11] == "0")
                {
                    SceneManager.LoadScene("Map1P1");
                }
                if(netScript.clientData[11] == "1")
                {
                    SceneManager.LoadScene("Map1P2");
                }
            }
        }
    }
}
