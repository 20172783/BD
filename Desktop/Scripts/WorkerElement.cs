using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkerElement : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text lastnameText;
    public TMP_Text ratingText;
    public TMP_Text name2Text;
    public TMP_Text lastname2Text;
    public TMP_Text rating2Text;

    public GameObject RemoveWorkerButton;
    public GameObject EditPanel;
    public GameObject BookedNotWorkingList;
    public string workerID;

    public List<GameObject> WeekDays = new List<GameObject>();

    public List<List<string>> SheduleList = new List<List<string>>();

    void FillSheduleList()
    {
        for (int i = 0; i < 7; i++)
        {
            List<string> List = new List<string>();
            SheduleList.Add(List);
        }
    }
    public void NewElement(string _name, string _lastname, float _rating, string workerId)
    {
        nameText.text = _name;
        lastnameText.text = _lastname;
        ratingText.text = _rating.ToString();
        name2Text.text = _name;
        lastname2Text.text = _lastname;
        rating2Text.text = _rating.ToString();
        workerID = workerId;
        FillSheduleList();
    }

    public string getWorkerId()
    {
        return workerID;
    }

    public void deleteWorker()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().DeleteWorkerFromFirm(workerID);
    }

    public void RequestButton()
    {
        try
        {
            GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().RequestButton(workerID);
        }
        catch (Exception e)
        {
            Debug.Log("Failed To Send Request" + e);
        }
    }
    public void SaveTimesButton()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().SaveWorkerTimesButton(workerID, SheduleList);
        //LoadEditData();
    }
    public void LoadEditData()
    {
        for (int i=0; i<7;i++)
        {
            SheduleList[i].Clear();
            foreach (string child in GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().SheduleList[i])
            {
                SheduleList[i].Add(child);
            }
            WeekDays[i].GetComponent<WorkerWeekDay>().LoadHours();
        }
        BookedNotWorkingList.GetComponent<BookedNotWorking>().LoadListElements();
        // SheduleList = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().SheduleList;
    }
    public void OpenEdit()
    {
        LoadEditData();
        
        EditPanel.SetActive(true);
    }

}