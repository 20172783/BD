using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrderHourElement : MonoBehaviour
{
    public string hour;
    public string workerid;

    public TMP_Text Hour;
    public void NewElement(string _hour, string _workerid)
    {
        hour = _hour;
        workerid = _workerid;

        Hour.text = hour + ":00";
    }

    public void OnSelectedHour()
    {
        GameObject.Find("OrderSelectedHourText").GetComponent<TMP_Text>().text = hour + ":00";
        GameObject.Find("ScrollViewDay").GetComponent<OrderDays>().Orderbutton.SetActive(true);
        GameObject.Find("ScrollViewDay").GetComponent<OrderDays>().SelectedDate.AddHours(double.Parse(hour));
    }
}
