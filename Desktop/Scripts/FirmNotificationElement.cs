using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FirmNotificationElement : MonoBehaviour
{
    public TMP_Text nameText;
    public string firmid;
    public string workerid;
    public string request_id;

    public void NewElement(string _name, string _firmid, string _workerid, string _request_id)
    {
        nameText.text = _name;
        firmid = _firmid;
        workerid = _workerid;
        request_id = _request_id;
    }
    public void AcceptButton()
    {
        try
        {
            GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().AcceptReqestButton(firmid, workerid, request_id);
        }
        catch (Exception e)
        {
            Debug.Log("Failed To Send Request" + e);
        }
    }

    public void DeclineButton()
    {
        try
        {
            GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().DeclineReqestButton(request_id);
        }
        catch (Exception e)
        {
            Debug.Log("Failed To Send Request" + e);
        }
    }
    public void RequestButton()
    {
        try
        {
            GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().LoadNotifications();
        }
        catch (Exception e)
        {
            Debug.Log("Failed To Send Request" + e);
        }
    }
}
