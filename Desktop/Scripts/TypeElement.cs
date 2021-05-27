using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeElement : MonoBehaviour
{
    public TMP_Text Name;
    public Toggle toggle;
    public void NewElement(string _name)
    {
        Name.text = _name;
    }
    public void OnToggleClick()
    {
        if(toggle.isOn)
        {
            GameObject.Find("TypeSelectPanel").GetComponent<TypeSelect>().selectedTypes.Add(Name.text);
        }
        else if(!toggle.isOn)
        {
            GameObject.Find("TypeSelectPanel").GetComponent<TypeSelect>().selectedTypes.Remove(Name.text);
        }
    }

    public void OnEditToggleClick()
    {
        if (toggle.isOn)
        {
            GameObject.Find("EditTypeSelectPanel").GetComponent<TypeSelect>().selectedTypes.Add(Name.text);
        }
        else if (!toggle.isOn)
        {
            GameObject.Find("EditTypeSelectPanel").GetComponent<TypeSelect>().selectedTypes.Remove(Name.text);
        }
    }
}
