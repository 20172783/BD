using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchFirmElement : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text ratingText;
    public string firmID;

    public void NewElement(string _name, float _rating, string firmId)
    {
        nameText.text = _name;
        ratingText.text = _rating.ToString();
        firmID = firmId;
    }

    public string getFirmId()
    {
        return firmID;
    }

    public void RequestButton()
    {
        try
        {
           GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().RequestButton(firmID);
           GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().requestMessage.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.Log("Failed To Send Request" + e);
        }
    }
}
