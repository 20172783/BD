using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrderServiceInfo : MonoBehaviour
{
    public GameObject SelectedService = null;

    public string id;
    public string title;
    public int status;
    public string desc;
    public string locationid;
    public string adress;
    public string firmid;
    public string firmName;
    public float price;
    public int duration;
    public float rating = 0;
    public int reviews = 0;
    public List<string> types = new List<string>();
    public List<string> workersid = new List<string>();
    public List<string> workersName = new List<string>();

    public TMP_Text Title;
    public TMP_Text Rating;
    public TMP_Text Review;
    public TMP_Text Desc;
    public TMP_Text Price;
    public TMP_Text Duration;
    public TMP_Text Adress;
    public TMP_Text Firm;
    public TMP_Text Workers;

    public GameObject WorkerSelectionElement;
    public Transform WorkerSelectionListContent;
    public void NewElement(string _id, string _title, int _status, string _desc, string _locationid, string _adress, string _firmid, float _price, int _duration, int _reviews, float _rating, List<string> _types, List<string> _workersid)
    {
        id = _id;
        title = _title;
        status = _status;
        desc = _desc;
        locationid = _locationid;
        firmid = _firmid;
        price = _price;
        duration = _duration;
        types = _types;
        workersid = _workersid;
        rating = _rating;
        reviews = _reviews;
        adress = _adress;

        Title.text = title;
        Rating.text = rating.ToString();
        Review.text = reviews.ToString();
        Desc.text = desc;
        Price.text = price + " eur.";
        Duration.text = duration + " min.";
        Adress.text = adress;
        Firm.text = firmName;
    }

    public void orderButton()
    {
        GameObject FinalOrderPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().FinalOrderPanel;
        FinalOrderPanel.GetComponent<OrderComplete>().FillDataFromServiceInfo(title, desc, adress, price.ToString(), duration.ToString(), firmid, id, locationid);
    }

    public void updateFirmName()
    {
        Firm.text = firmName;
    }

    public void updateWorkersNames()
    {
        string workers = "";
        GameObject WorkerSelectionListElement = null;

        foreach (Transform child in WorkerSelectionListContent.transform)
        {
            Destroy(child.gameObject);

        }
        //ANY worker
        WorkerSelectionListElement = Instantiate(WorkerSelectionElement, WorkerSelectionListContent);
        WorkerSelectionListElement.GetComponent<OrderWorkerElement>().NewElement(workersid, "Bet kuris darbuotojas", firmid);

        for (int i=0; i<workersName.Count;i++)
        {
            workers += workersName[i] + "\n";

            WorkerSelectionListElement = Instantiate(WorkerSelectionElement, WorkerSelectionListContent);
            WorkerSelectionListElement.GetComponent<OrderWorkerElement>().NewElement(workersid[i], workersName[i], firmid);
        }
        Workers.text = workers;
    }

}
