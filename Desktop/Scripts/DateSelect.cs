using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DateSelect : MonoBehaviour
{
    public GameObject FlatCalendar;
    public GameObject FlatCalendarChild;
    public GameObject TimeSelectPanel;
    public InputField DateInput;
    public InputField HourInput;
    public string SelectedDate="";

    public void openCalendar()
    {
        FlatCalendar = GameObject.Find("FlatCalendar");
        FlatCalendarChild = FlatCalendar.transform.GetChild(0).gameObject; //FlatCalendar Child "Calendar"
        FlatCalendar.GetComponent<FlatCalendar>().CalledGameObject = this.gameObject;
        FlatCalendarChild.SetActive(true);
    }
    public void GetSelectedDate()
    {
        FlatCalendarChild.SetActive(false);
        SelectedDate = FlatCalendar.GetComponent<FlatCalendar>().currentTime.getSelectedData();
        DateInput.text = SelectedDate;
    }
    public void GetSelectedHour()
    {
        TMP_Text hour = this.transform.GetChild(0).GetComponent<TMP_Text>();
        HourInput.text = hour.text;
        TimeSelectPanel.SetActive(false);
    }
}
