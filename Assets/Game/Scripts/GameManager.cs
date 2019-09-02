using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public GameState currentState = GameState.PresentationMenu;
    private bool bCanAdvanceScreen = true;
    private bool bCanActiveSetup = true;

    public static GameManager Instance;

    public GameObject setup;
    public GameObject loggin;
    public GameObject[] QRS;
    public GameObject kirby;
    public GameObject music;


    private int numLightBulb;
    private int totalLightBulb;

     private System.Random random = new System.Random();


    public Text textNumLightBulb;
    public Text textTotalLightBulb;

    public InputField inputId;
    public InputField inputPassword;

    public EasyTween presentationMenu;
    public EasyTween mainScreen;
    public EasyTween qRScene;
    public EasyTween insertBom;

    private IEnumerator StopVideCoroutine;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }


    void Start()
    {

        print("tamaño del arreglo QRS: " + QRS.Length);
        presentationMenu.OpenCloseObjectAnimation();
        currentState = GameState.PresentationMenu;
        StopVideCoroutine =  stopVideo();
        StartCoroutine(StopVideCoroutine);
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (bCanActiveSetup)
            {
                kirby.SetActive(false);
                loggin.SetActive(true);
                bCanActiveSetup = false;

            }
            else
            {
                loggin.SetActive(false);
                bCanActiveSetup = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && bCanAdvanceScreen)
        {

            switch (currentState)
            {
                case GameState.PresentationMenu:
                    MainMenu();
                    currentState = GameState.MainMenu;

                    break;
                case GameState.MainMenu:
                    InsertBom();
                    currentState = GameState.insertBom;

                    break;
                case GameState.insertBom:
                    QRScene();
                    currentState = GameState.QRScene;

                    break;

                case GameState.QRScene:
                    PresentationMenu();
                    currentState = GameState.PresentationMenu;
                    break;
            }

        }
    }


    public void PresentationMenu()
    {
        music.SetActive(false);
        if (bCanAdvanceScreen == false)
            return;
        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 3.0f);
        qRScene.OpenCloseObjectAnimation();
        presentationMenu.OpenCloseObjectAnimation();
    }

    public void MainMenu()
    {
      
        StopCoroutine(StopVideCoroutine);
        Invoke("PlaySong", 1.5f);

        if (bCanAdvanceScreen == false)
            return;
        //variable contando el texto de cuantas botellas ha entrado la persona
        bCanAdvanceScreen = false;
        print("Entro a main menu");
        Invoke("AllowAdvanceScreen", 3.0f);
        presentationMenu.OpenCloseObjectAnimation();
        mainScreen.OpenCloseObjectAnimation();
    }

    private void PlaySong()
    {
        music.SetActive(true);
    }

    public void InsertBom()
    {
        if (bCanAdvanceScreen == false)
            return;

        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 3.0f);
        mainScreen.OpenCloseObjectAnimation();
        insertBom.OpenCloseObjectAnimation();
    }
    public void QRScene()
    {
        if (bCanAdvanceScreen == false)
            return;

        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 5.0f);
        insertBom.OpenCloseObjectAnimation();
        qRScene.OpenCloseObjectAnimation();
        RandomQR();
        currentState = GameState.QRScene;
        numLightBulb = 0;
        textNumLightBulb.text = "0";
        StartCoroutine(Reset());

    }

    public void InsertLightBulb()
    {
        numLightBulb += 1;
        totalLightBulb += 1;
        textTotalLightBulb.text = "Total de bombillos recolectados: " + totalLightBulb;
        textNumLightBulb.text = numLightBulb.ToString();
    }


    public void Loggin()
    {

        if (inputId.text.Equals("123") && inputPassword.text.Equals("123"))
        {
            print("entro al loggin");
            setup.SetActive(true);

        }

    }
    public void Exit()
    {
       
        setup.SetActive(false);
        loggin.SetActive(false);
        inputId.text = "";
        inputPassword.text = "";
        kirby.SetActive(true);

    }

    public void RandomQR()
    {
        int randomNumber = random.Next(0, QRS.Length);
        print("Random number: " + randomNumber);
        QRS[randomNumber].SetActive(true);
    }

    IEnumerator stopVideo()
    {
        yield return new WaitForSeconds(33);
        MainMenu();
    }
    IEnumerator Reset()
    {

        yield return new WaitForSeconds(5);
        PresentationMenu();

    }
    public void RaceFinished()
    {

        QRScene();


    }



    private void AllowAdvanceScreen()
    {
        bCanAdvanceScreen = true;
    }

}







public enum GameState
{
    PresentationMenu,
    MainMenu,
    insertBom,
    QRScene

}
