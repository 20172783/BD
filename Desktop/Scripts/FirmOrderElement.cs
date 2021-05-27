using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirmOrderElement : MonoBehaviour
{
    public string adress;
    public string client_id;
    public string client_phone;
    public string Date;
    public string desc;
    public string duration;
    public string firm_id;
    public string loc_id;
    public string OrderDate;
    public string paymentStatus;
    public string paymentType;
    public string price;
    public string status;
    public string title;
    public string worker_id;
    public string workerName;
    public TMP_Text statusText;
    public TMP_Text workerText;
    public TMP_Text dateText;
    public TMP_Text titleText;
    public GameObject CancelButton;
    public string id;

    public void NewElement(string _id, string _adress, string _client_id, string _client_phone, string _Date, string _desc, string _duration, string _firm_id, string _loc_id,
        string _OrderDate, string _paymentStatus, string _paymentType, string _price, string _status, string _title, string _worker_id, string _workerName)
    {
        id = _id;
        adress = _adress;
        client_id = _client_id;
        client_phone = _client_phone;
        Date = _Date;
        desc = _desc;
        duration = _duration;
        firm_id = _firm_id;
        loc_id = _loc_id;
        OrderDate = _OrderDate;
        paymentStatus = _paymentStatus;
        paymentType = _paymentType;
        price = _price;
        status = _status;
        title = _title;
        worker_id = _worker_id;
        workerName = _workerName;

        if (int.Parse(status) == 1)
        {
            statusText.text = "Būsimas";
            CancelButton.SetActive(true);
        }
        else if (int.Parse(status) == 2)
        {
            statusText.text = "Įvykdytas";
            CancelButton.SetActive(false);
        }
        else if (int.Parse(status) == 3)
        {
            statusText.text = "Atšauktas";
            CancelButton.SetActive(false);
        }
        workerText.text = workerName;
        dateText.text = Date;
        titleText.text = title;
    }
    public void cancelOrder()
    {
        string newStatus = "3";
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().ChangeOrderStatus(id, newStatus);
    }
}
