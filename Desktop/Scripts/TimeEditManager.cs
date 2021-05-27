using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeEditManager : MonoBehaviour
{
    public GameObject Worker;
    public GameObject ListContent;
    public GameObject EditableGameObject;
    public int EditableGameObjectWeekId;
    List<GameObject> Elements = new List<GameObject>();
    public List<string> hours = new List<string>();

    public void GetAllContentElements()
    {
        foreach(Transform child in ListContent.transform)
        {
            Elements.Add(child.gameObject);
        }
    }
    
    public void GetSelectedHours()
    {
        hours.Clear();
        if (Elements.Count > 0)
        {
            foreach (GameObject child in Elements)
            {
                if (child.transform.Find("Toggle").GetComponent<Toggle>().isOn)
                {
                    TMP_Text hour = child.transform.Find("Hour").GetComponent<TMP_Text>();
                    string fixed_hour = hour.text.Substring(0, hour.text.Length - 3);
                    if (hour.text.StartsWith("0")) fixed_hour.Replace("0", "");
                    hours.Add(fixed_hour);
                }
            }
            if (hours.Count < 1)
            {
                hours.Clear();
                hours.Add("Nėra");
            }
        }
        else hours.Add("Nėra");

        EditableGameObject.GetComponent<WorkerWeekDay>().hours.Clear();
        foreach (string hour in hours)
        {
            EditableGameObject.GetComponent<WorkerWeekDay>().hours.Add(hour);
            Debug.Log(hour);
        }
        EditableGameObject.GetComponent<WorkerWeekDay>().FillTimeInterval();
       
        for (int i=0; i<7; i++)
        {
            if (EditableGameObjectWeekId == i)
            {
                Worker.GetComponent<WorkerElement>().SheduleList[i].Clear();
                foreach (string hour in hours)
                {
                    Worker.GetComponent<WorkerElement>().SheduleList[i].Add(hour);
                }
                break;
            }
        }

        Worker.GetComponent<WorkerElement>().SaveTimesButton();
        this.gameObject.SetActive(false);
    }

    public void LoadSelectedHours()
    {
        if (Elements.Count > 0)
        {
            foreach (GameObject child in Elements)
            {
                child.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
                foreach (string hour in hours)
                {
                    if (child.transform.Find("Hour").GetComponent<TMP_Text>().text == hour+":00")
                    {
                        child.transform.Find("Toggle").GetComponent<Toggle>().isOn = true;
                    }
                }
            }
        }
    }
}
