using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ClientServiceElement : MonoBehaviour
{
    public string id;
    public string title;
    public int status;
    public string desc;
    public string locationid;
    public string adress = "";
    public string firmid;
    public float price;
    public int duration;
    public float rating = 0;
    public int reviews = 0;
    public List<string> types = new List<string>();
    public List<string> workersid = new List<string>();

    public TMP_Text Title;
    public TMP_Text Rating;
    public TMP_Text Review;
    public TMP_Text Adress;
    public void NewElement(string _id, string _title, int _status, string _desc, string _locationid, string _firmid, float _price, int _duration, int _reviews, float _rating, List<string> _types, List<string> _workersid)
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

        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().GetLocById(this.gameObject);

        Title.text = title;
        Rating.text = rating.ToString();
        Review.text = reviews.ToString();
        //Adress.text = adress.ToString();
    }

    public void SelectService()
    {
        GameObject ServiceInfoPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().ServiceInfo;
        GameObject ServicesPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().Services;
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().GetFirmWorkersNameById(ServiceInfoPanel, workersid);

        ServiceInfoPanel.GetComponent<OrderServiceInfo>().NewElement(id, title, status, desc, locationid, Adress.text, firmid, price, duration, reviews, rating, types, workersid);
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().GetFirmNameById(ServiceInfoPanel, firmid);
        
        ServiceInfoPanel.SetActive(true);
        ServicesPanel.SetActive(false);
    }
}
