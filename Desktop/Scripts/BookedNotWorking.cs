using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookedNotWorking : MonoBehaviour
{
    public GameObject Worker;
    public GameObject CreateWindow;
    public InputField Date;
    public Transform BookedNotWorkingListContent;
    public InputField Hour;
    public InputField Date1;
    public InputField Date2;
    public GameObject DayPanel;
    public GameObject PeriodPanel;
  
    public void CreateButton()
    {
        if (DayPanel.activeInHierarchy)
        {
            if (Date.text.Length > 0 && Hour.text.Length > 0)
            {
                string h = Hour.text;
                string[] HourParts = Hour.text.Split(char.Parse(":"));
                string id = Worker.GetComponent<WorkerElement>().workerID;
                GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().CreateInactiveWorkerTimeButton(id, Date.text, HourParts[0], this.gameObject);
                CreateWindow.SetActive(false);
            }
        }
        else if (PeriodPanel.activeInHierarchy)
        {
            if (Date1.text.Length > 0 && Date2.text.Length > 0)
            {
                string id = Worker.GetComponent<WorkerElement>().workerID;
                GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().CreateInactiveWorkerTimeButton(id, Date1.text, Date2.text, this.gameObject);
                CreateWindow.SetActive(false);
            }
        }
       
    }

    public void LoadListElements()
    {
        string id = Worker.GetComponent<WorkerElement>().workerID;
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().LoadInactiveWorkerTimeButton(id, BookedNotWorkingListContent);
    }
}
