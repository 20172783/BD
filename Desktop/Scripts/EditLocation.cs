using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditLocation : MonoBehaviour
{
    public string loc_id;
    public string city;
    public string adress;

    public TMP_InputField cityInput;
    public TMP_InputField adressInput;

    public void loadEdit(string _loc_id, string _city, string _adress)
    {
        loc_id = _loc_id;
        city = _city;
        adress = _adress;

        adressInput.text = adress;
        cityInput.text = city;
    }
    public void Edit()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().EditLocation(loc_id, cityInput.text, adressInput.text);
    }
}
