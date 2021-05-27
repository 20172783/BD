using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkerSelect : MonoBehaviour
{
    public Transform ListContent;
    public TMP_InputField WorkerInput;
    public GameObject SelectWorkerPrefab;
    public GameObject CreateNewServiceObject;
    //public List<string> workers = new List<string>();
    public List<string> selectedWorkers = new List<string>();

    public void LoadAllWorkers()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().LoadFirmWorkersToSelectButton();
    }
    public void SelectAllSelectedTypes()
    {
        foreach (string worker in selectedWorkers)
        {
            foreach (Transform child in ListContent.transform)
            {
                Debug.Log(child.gameObject.GetComponent<WorkerSelectElement>().id + " " + worker);
                if (child.gameObject.GetComponent<WorkerSelectElement>().id == worker)
                {
                    child.gameObject.GetComponent<WorkerSelectElement>().Workertoggle.SetIsOnWithoutNotify(true);
                }
            }
        }
    }

    public void OpenSelectTypeButton()
    {
        this.gameObject.SetActive(true);
        //LoadAllWorkers(); // paleistas ant Create New Service Button
        SelectAllSelectedTypes();
    }

    public void SaveTypeSelect()
    {
        WorkerInput.text = "";
        CreateNewServiceObject.GetComponent<CreateNewService>().selectedWorkerIds.Clear();
        foreach (string worker in selectedWorkers)
        {
            foreach (Transform child in ListContent.transform)
            {
                if (child.gameObject.GetComponent<WorkerSelectElement>().id == worker)
                {
                    string workerName = child.gameObject.GetComponent<WorkerSelectElement>().Name.text;
                    string workerLastname = child.gameObject.GetComponent<WorkerSelectElement>().Lastname.text;
                    WorkerInput.text = WorkerInput.text + " " + workerName + " " + workerLastname + ",";

                    CreateNewServiceObject.GetComponent<CreateNewService>().selectedWorkerIds.Add(child.gameObject.GetComponent<WorkerSelectElement>().id);
                }
            }
        }
        this.gameObject.SetActive(false);
    }
    public void EditSaveTypeSelect()
    {
        WorkerInput.text = "";
       // CreateNewServiceObject.GetComponent<EditService>().selectedWorkerIds.Clear();
        foreach (string worker in selectedWorkers)
        {
            foreach (Transform child in ListContent.transform)
            {
                if (child.gameObject.GetComponent<WorkerSelectElement>().id == worker)
                {
                    string workerName = child.gameObject.GetComponent<WorkerSelectElement>().Name.text;
                    string workerLastname = child.gameObject.GetComponent<WorkerSelectElement>().Lastname.text;
                    WorkerInput.text = WorkerInput.text + " " + workerName + " " + workerLastname + ",";

               //     CreateNewServiceObject.GetComponent<EditService>().selectedWorkerIds.Add(child.gameObject.GetComponent<WorkerSelectElement>().id);
                }
            }
        }
        this.gameObject.SetActive(false);
    }

    public void LoadOnInputField()
    {
        WorkerInput.text = "";

        foreach (string type in selectedWorkers)
        {
            WorkerInput.text = WorkerInput.text + " " + type + ",";
        }
        // this.gameObject.SetActive(false);
    }

    public List<string> GetSelectedTypes()
    {
        if (selectedWorkers.Count < 1)
        {
            selectedWorkers.Add("Nėra");
        }
        return selectedWorkers;
    }

}
