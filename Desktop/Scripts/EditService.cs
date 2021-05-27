using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditService : MonoBehaviour
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
    public Image Img;
    public string loc;
    public GameObject workerSelect;
    public GameObject typeSelect;
    public string id;

    public void FillEdit(string _id, string title, string desc, string location, string price, string duration, List<string> workersids, List<string> types, Image img, int status)
    {
        CleanEdit();

        id = _id;
        Title.text = title;
        Desc.text = desc;
        Price.text = price;
        Duration.text = duration;
        Img = img;
        selectedWorkerIds = workersids;
        selectedTypes = types;
        loc = location;
        if (status == 1) Public.SetIsOnWithoutNotify(true);
        else Public.SetIsOnWithoutNotify(false);

        FillLocationDropdow(loc);
        // SelectWorkers();
        //SelectTypes();

        foreach (string type in types) Types.text += type + ", ";

        typeSelect.GetComponent<TypeSelect>().selectedTypes = selectedTypes;
        workerSelect.GetComponent<WorkerSelect>().selectedWorkers = selectedWorkerIds;
        workerSelect.GetComponent<WorkerSelect>().EditSaveTypeSelect();
    }

    public void SelectTypes()
    {
        foreach (Transform child in workerSelect.transform)
        {
            string type = child.GetComponent<TypeElement>().Name.text;
            if (selectedTypes.Contains(type))
            {
                child.Find("Toggle").GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            }
            else child.Find("Toggle").GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        }
    }

    public void SelectWorkers()
    {
        foreach (Transform child in workerSelect.transform)
        {
            string id = child.GetComponent<WorkerSelectElement>().id;
            if(selectedWorkerIds.Contains(id))
            {
                child.Find("Toggle").GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            }
            else child.Find("Toggle").GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        }
    }
    public void FillLocationDropdow(string location)
    {
        Location.ClearOptions();
        Location.options.Clear();
        //Location.options.Add(new TMP_Dropdown.OptionData() { text = "Pasirinkti lokaciją" });
        foreach (Transform child in LocationList.transform)
        {
            string city = child.gameObject.GetComponent<LocationElement>().CityText.text;
            string adress = child.gameObject.GetComponent<LocationElement>().AdressText.text;

            Location.options.Add(new TMP_Dropdown.OptionData() { text = city + ", " + adress });
        }
        Debug.Log("" + location);
        Location.value = Location.options.FindIndex(option => option.text == location);
    }

    public void CleanEdit()
    {
        Workers.text = "";
        Types.text = "";
        selectedWorkerIds.Clear();
        selectedTypes.Clear();
    }
    public void EditServiceButton()
    {
        string title = Title.text;
        string desc = Desc.text;
        string price = Price.text;
        string duration = Duration.text;
        string selectedLocationId = "";
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

        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().EditServiceButton(id, title, desc, selectedLocationId, price, duration, publicStatus, selectedWorkerIds, selectedTypes, ImgUrl, ImgFormat);

        this.gameObject.SetActive(false);
        Workers.text = "";
        Types.text = "";
    }
}
