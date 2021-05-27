using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ServiceType : MonoBehaviour
{
    string title;
    public TMP_Text Title;
    public GameObject ServicesListWindow;
    public GameObject TypesListWindow;
    
    public void OpenType()
    {
        //TypesListWindow = GameObject.Find("Scroll View Categories").gameObject;
       // ServicesListWindow = GameObject.Find("Scroll View Services").gameObject;
        title = Title.text;

        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().GetServiceByCat(title);
        ServicesListWindow.SetActive(true);
        TypesListWindow.SetActive(false);
    }
}
