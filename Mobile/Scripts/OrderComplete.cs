using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrderComplete : MonoBehaviour
{
    string title;
    string desc;
    string adress;
    string price;
    string duration;
    string firmId;
    public string workerId;
    public string workerName;
    string serviceId;
    public DateTime date;
    int paymentType;
    int paymentStatus;
    string locationId;
    string discoutCode;
    string OrderForClientName;
    public string clientPhone;

    public TMP_Dropdown PaymentTypeDropdown;
    public TMP_Text TitleText;
    public TMP_Text WorkerText;
    public TMP_Text AdressText;
    public TMP_Text DateText;
    public TMP_Text PriceText;
    public void FillDataFromServiceInfo(string _title, string _desc, string _adress, string _price, string _duration, string _firmid, string _serviceId, string _locationId)
    {
        title = _title;
        desc = _desc;
        adress = _adress;
        price = _price;
        duration = _duration;
        firmId = _firmid;
        serviceId = _serviceId;
        locationId = _locationId;
    }
    public void fillPanelData()
    {
        TitleText.text = title;
        WorkerText.text = workerName;
        AdressText.text = adress;
        DateText.text = date.ToString();
        PriceText.text = price + " eur.";
    }
    public void CreateOrderButton()
    {
        if(PaymentTypeDropdown.value == 1)
        {
            paymentType = 1;
            paymentStatus = 0;
        }
        else
        {
            paymentType = 0;
            paymentStatus = 0;
        }
        
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().CreateClientOrder(title, desc, adress, price, duration, firmId, workerId, serviceId,
            date, paymentType, paymentStatus, locationId, discoutCode, OrderForClientName, clientPhone, workerName);
    }
}
