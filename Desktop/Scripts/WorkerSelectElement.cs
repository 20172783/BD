using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkerSelectElement : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Lastname;
    public string id;
    public Toggle Workertoggle;
    public int rating;
    // Start is called before the first frame update
    public void NewElement(string _id, string _name, string _lastname, int _rating)
    {
        Name.text = _name;
        Lastname.text = _lastname;
        id = _id;
        rating = _rating;
    }

    public void OnToggleClick()
    {
        if (Workertoggle.isOn)
        {
            GameObject.Find("WorkerSelectPanel").GetComponent<WorkerSelect>().selectedWorkers.Add(id);
        }
        else if (!Workertoggle.isOn)
        {
            GameObject.Find("WorkerSelectPanel").GetComponent<WorkerSelect>().selectedWorkers.Remove(id);
        }
    }
    public void OnEditToggleClick()
    {
        if (Workertoggle.isOn)
        {
            GameObject.Find("EditWorkerSelectPanel").GetComponent<WorkerSelect>().selectedWorkers.Add(id);
        }
        else if (!Workertoggle.isOn)
        {
            GameObject.Find("EditWorkerSelectPanel").GetComponent<WorkerSelect>().selectedWorkers.Remove(id);
        }
    }
}
