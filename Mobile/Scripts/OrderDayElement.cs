using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OrderDayElement : MonoBehaviour
{
    string weekday;
    int month;
    int day;
    public string workerid;
    public TMP_Text WeekDayName;
    public TMP_Text DayNr;
    public TMP_Text Month;
    public DateTime Date;

    public void NewElement(string _weekday, int _month, int _day, DateTime _Date, string _workerid)
    {
        weekday = _weekday;
        month = _month;
        day = _day;
        Date = _Date;
        workerid = _workerid;

        DoWeekDayName(weekday);
        DoMontName(month);

        DayNr.text = day.ToString();
    }

    public void DoWeekDayName(string weekday)
    {
        string lt_weekday ="";

        if (weekday == "Monday") lt_weekday = "Pr";
        else if (weekday == "Tuesday") lt_weekday = "An";
        else if (weekday == "Wednesday") lt_weekday = "Tr";
        else if (weekday == "Thursday") lt_weekday = "Ke";
        else if (weekday == "Friday") lt_weekday = "Pn";
        else if (weekday == "Saturday") lt_weekday = "Št";
        else if (weekday == "Sunday") lt_weekday = "Sk";

        WeekDayName.text = lt_weekday;
    }
    public void DoMontName(int month)
    {
        string lt_month = "";

        if (month == 1) lt_month = "Sausis";
        else if (month == 2) lt_month = "Vasaris";
        else if (month == 3) lt_month = "Kovas";
        else if (month == 4) lt_month = "Balandis";
        else if (month == 5) lt_month = "Gegužė";
        else if (month == 6) lt_month = "Birželis";
        else if (month == 7) lt_month = "Liepa";
        else if (month == 8) lt_month = "Rūgpjūtis";
        else if (month == 9) lt_month = "Rugsėjis";
        else if (month == 10) lt_month = "Spalis";
        else if (month == 11) lt_month = "Lapkritis";
        else if (month == 12) lt_month = "Gruodis";

        Month.text = lt_month;
    }

    public void OnSelectedDate()
    {
        GameObject.Find("OrderSelectedDateText").GetComponent<TMP_Text>().text = Date.ToString("yyyy-MM-dd");
        GameObject.Find("OrderSelectedHourText").GetComponent<TMP_Text>().text = "";
        GameObject.Find("ScrollViewDay").GetComponent<OrderDays>().Orderbutton.SetActive(false);
        GameObject.Find("ScrollViewDay").GetComponent<OrderDays>().spawnHours(Date, workerid);
        GameObject.Find("ScrollViewDay").GetComponent<OrderDays>().SelectedDate = Date;
    }
}
