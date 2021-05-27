using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject loginUI;
    public GameObject cientRegisterUI;
    public GameObject registerUI;
    public GameObject userDataUI;
    public GameObject clientMainUI;
    public GameObject workerMainUI;
    public GameObject scoreboardUI;
    public GameObject LoadingUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ClearScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        userDataUI.SetActive(false);
        scoreboardUI.SetActive(false);
        cientRegisterUI.SetActive(false);
        LoadingUI.SetActive(false);
        clientMainUI.SetActive(false);
        workerMainUI.SetActive(false);
    }

    public void LoginScreen()
    {
        ClearScreen();
        loginUI.SetActive(true);
    }

    public void LoadingScreen()
    {
        ClearScreen();
        LoadingUI.SetActive(true);
    }

    public void ClientRegisterScreen()
    {
        ClearScreen();
        cientRegisterUI.SetActive(true);
    }
    public void RegisterScreen()
    {
        ClearScreen();
        registerUI.SetActive(true);
    }

    public void UserDataScreen()
    {
        ClearScreen();
        userDataUI.SetActive(true);
    }

    public void ClientMainScreen()
    {
        ClearScreen();
        clientMainUI.SetActive(true);
    }

    public void WorkerMainScreen()
    {
        ClearScreen();
        workerMainUI.SetActive(true);
    }

    public void ScoreboardScreen()
    {
        ClearScreen();
        scoreboardUI.SetActive(true);
    }
}
