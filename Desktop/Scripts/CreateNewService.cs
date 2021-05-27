using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateNewService : MonoBehaviour
{
    public TMP_InputField Title;
    public TMP_InputField Desc;
    public TMP_Dropdown Location;
    public TMP_InputField Price;
    public TMP_InputField Duration;
    public TMP_InputField Workers;
    public TMP_InputField Types;
    public Toggle Public;
    public Transform LocationList;
    public List<string> selectedWorkerIds = new List<string>();
    public List<string> selectedTypes = new List<string>();
    public string ImgUrl;
    public string ImgFormat;

    public void SpawnFirmLocations()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().LoadFirmLocationSelectDataButton();
    }
    public void FillLocationDropdow()
    {
        Location.ClearOptions();
        Location.options.Clear();
        foreach (Transform child in LocationList.transform)
        {
            string city = child.gameObject.GetComponent<LocationElement>().CityText.text;
            string adress = child.gameObject.GetComponent<LocationElement>().AdressText.text;

            Location.options.Add(new TMP_Dropdown.OptionData() { text = city + ", " + adress });
        }
    }

    public void createServiceButton()
    {
        string title = Title.text;
        string desc = Desc.text;
        string price = Price.text;
        string duration = Duration.text;
        string selectedLocationId="";
        string publicStatus;
        if (Public.isOn) publicStatus = "1";
        else publicStatus = "0";
        int LocationValue = Location.value;
        string selectedLocation = Location.options[LocationValue].text;
        foreach (Transform child in LocationList.transform)
        {
            string city = child.gameObject.GetComponent<LocationElement>().CityText.text;
            string adress = child.gameObject.GetComponent<LocationElement>().AdressText.text;

            if (selectedLocation == city + ", " + adress)
            {
                selectedLocationId = child.gameObject.GetComponent<LocationElement>().location_id;
            }
        }

        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().CreateServiceButton(title, desc, selectedLocationId, price, duration, publicStatus, selectedWorkerIds, selectedTypes, ImgUrl, ImgFormat);
        this.gameObject.SetActive(false);
    }
}
