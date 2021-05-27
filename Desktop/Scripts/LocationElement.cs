using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationElement : MonoBehaviour
{
    public TMP_Text CityText;
    public TMP_Text AdressText;
    public string location_id;

    public void NewElement(string _city, string _adress, string _location_id)
    {
        CityText.text = _city;
        AdressText.text = _adress;
        location_id = _location_id;
    }

    public void Delete()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().DeleteLocation(location_id);
    }

    public void Edit()
    {
        GameObject editPanel = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().EditLocationPanel;
        editPanel.GetComponent<EditLocation>().loadEdit(location_id, CityText.text, AdressText.text);
        editPanel.SetActive(true);
    }

}