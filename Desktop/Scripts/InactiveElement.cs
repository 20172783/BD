using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InactiveElement : MonoBehaviour
{
    public string id;
    public string workerid;
    public TMP_Text Date;
    public TMP_Text Hour;
    public void NewElement(string _id, string _workerid, string _date, string _hour)
    {
        Date.text = _date;
        Hour.text = _hour + ":00";
        id = _id;
        workerid = _workerid;
    }
    public void RemoveButton()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().RemoveInactiveWorkerTimeButton(workerid, id);
        Destroy(this.gameObject);
    }
}
