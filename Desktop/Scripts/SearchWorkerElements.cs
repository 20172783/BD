using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchWorkerElements : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text lastnameText;
    public TMP_Text ratingText;
    public string workerID;
    
    public void NewElement(string _name, string _lastname, float _rating, string workerId)
    {
        nameText.text = _name;
        lastnameText.text = _lastname;
        ratingText.text = _rating.ToString();
        workerID = workerId;
    }

    public string getWorkerId()
    {
        return workerID;
    }

    public void RequestButton()
    {
        try{
            GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().RequestButton(workerID);
            GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().requestMessage.SetActive(true);
        }
        catch(Exception e)
        {
            Debug.Log("Failed To Send Request" +e);
        }
    }
}
