using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FirmServiceElement : MonoBehaviour
{
    public string id;
    public string title;
    public int status;
    public string desc;
    public string locationid;
    public string location;
    public float price;
    public int duration;
    public float rating =0;
    public int reviews = 0;
    public List<string> types = new List<string>();
    public List<string> workersid = new List<string>();
    public string workers;
    public Sprite sprite;
    public Image image;

    public TMP_Text Title;
    public TMP_Text Rating;
    public TMP_Text Review;
    public void NewElement(string _id, string _title, int _status, string _desc, string _locationid, string _location, float _price, int _duration, int _reviews, float _rating, List<string> _types, List<string> _workersid, string _workers, Sprite _sprite)
    {
        id = _id;
        title= _title;
        status = _status;
        desc = _desc;
        locationid = _locationid;
        location = _location;
        price = _price;
        duration = _duration;
        types = _types;
        workersid = _workersid;
        workers = _workers;
        rating = _rating;
        reviews = _reviews;
        sprite = _sprite;

        image.sprite = sprite;
        Title.text = title;
        Rating.text = rating.ToString();
        Review.text = reviews.ToString();

        LoadLocation(locationid);
    }

    public void LoadLocation(string p_id)
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().FindLocationById(p_id, this.gameObject);
    }

    public void Delete()
    {
       GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().DeleteService(id);
    }

    public void Edit()
    {
        GameObject EditServicePanel = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().EditServicePanel;
        EditServicePanel.GetComponent<EditService>().FillEdit(id, title, desc, location, price.ToString(), duration.ToString(), workersid, types, image, status);
        EditServicePanel.SetActive(true);
    }

    public void Preview()
    {
        string allTypes = "";
        foreach (string type in types) allTypes += type + ", ";
        allTypes = allTypes.Substring(0, allTypes.Length - 1);
        GameObject PreviewPanel = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().PreviewServicePanel;
        PreviewPanel.GetComponent<FirmServicePreview>().FillPreview(title, desc, location, price.ToString(), duration.ToString(), workers, allTypes, image);
        PreviewPanel.SetActive(true);
    }
}
