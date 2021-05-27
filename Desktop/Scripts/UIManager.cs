using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject firmMainUI;
    public GameObject adminMainUI;
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
        adminMainUI.SetActive(false);
        scoreboardUI.SetActive(false);
        firmMainUI.SetActive(false);
        LoadingUI.SetActive(false);
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
    public void RegisterScreen()
    {
        ClearScreen();
        registerUI.SetActive(true);
    }
    public void AdminMainScreen()
    {
        ClearScreen();
        adminMainUI.SetActive(true);
    }
    public void FirmMainScreen()
    {
        ClearScreen();
        firmMainUI.SetActive(true);
    }
    public void ScoreboardScreen()
    {
        ClearScreen();
        scoreboardUI.SetActive(true);
    }
}
