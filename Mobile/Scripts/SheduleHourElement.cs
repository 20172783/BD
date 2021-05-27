using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SheduleHourElement : MonoBehaviour
{
    public string hour;
    public string orderid;

    public TMP_Text Hour;
    public void NewElement(string _hour, string _orderid)
    {
        hour = _hour;
        orderid = _orderid;

        Hour.text = hour + ":00";
    }

}
