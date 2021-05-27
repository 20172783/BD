using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LogInButton : MonoBehaviour
{
    public void OpenProfile()
    {
        SceneManager.LoadScene("UserPorfileScene", LoadSceneMode.Additive);
    }
}
