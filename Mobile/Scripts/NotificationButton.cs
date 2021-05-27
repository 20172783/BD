using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NotificationButton : MonoBehaviour
{
    public int pressStatus = 0;
    public GameObject NotificationPanel;
    public void onPressButton()
    {
        if(pressStatus==0)
        {
            GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().LoadNotifications();
            NotificationPanel.SetActive(true);
            pressStatus = 1;
        }
        else
        {
            NotificationPanel.SetActive(false);
            pressStatus = 0;
        }
    }
}
