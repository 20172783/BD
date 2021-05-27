using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkerWeekDay : MonoBehaviour
{
    public int weekid; 
    public GameObject EditPanel;
    public GameObject Worker;
    public TMP_Text TimeIntervalText;
    public List<string> hours = new List<string>();

    public void LoadList()
    {
        hours.Clear();
        foreach(string child in Worker.GetComponent<WorkerElement>().SheduleList[weekid])
        {
            hours.Add(child);
        }
        if (hours.Count < 1) hours.Add("Nėra");
    }

    public void LoadHours()
    {
        LoadList();
        FillTimeInterval();
    }

    public void AddExistingHours()
    {
        EditPanel.GetComponent<TimeEditManager>().hours.Clear();
        foreach (string hour in hours)
        {
            EditPanel.GetComponent<TimeEditManager>().hours.Add(hour);
        }
    }

    public void OpenEdit()
    {
        EditPanel.SetActive(true);
        EditPanel.GetComponent<TimeEditManager>().EditableGameObject = this.gameObject;
        EditPanel.GetComponent<TimeEditManager>().GetAllContentElements();
        EditPanel.GetComponent<TimeEditManager>().EditableGameObjectWeekId = weekid;
        AddExistingHours(); //Load
        EditPanel.GetComponent<TimeEditManager>().LoadSelectedHours(); //select all loaded hours
    }

    public void FillTimeInterval()
    {
        if (hours.Count > 1 && hours[0] != "Nėra")
        {
            int[] hoursinInt = new int[24];

            int i = 0;
            int max = 0;
            int min = 24;
            foreach (string hour in hours)
            {
                if (hour != "Nėra")
                {
                    hoursinInt[i] = int.Parse(hour);

                    int tempHour;
                    if (hoursinInt[i] < 6) tempHour = hoursinInt[i] * 100;
                    else tempHour = hoursinInt[i];

                    if (tempHour > max) max = hoursinInt[i];
                    if (tempHour < min) min = hoursinInt[i];
                    i++;
                }

            }
            TimeIntervalText.text = min + "-" + max;
        }
        else if (hours.Count == 1 && hours[0] != "Nėra") TimeIntervalText.text = hours[0];
        else TimeIntervalText.text = "Nėra";
    }
    

}
