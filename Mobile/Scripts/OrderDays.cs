using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderDays : MonoBehaviour
{
    public List<DateTime> BusyDates = new List<DateTime>();
    public List<string> shedule = new List<string>();

    public GameObject OrderDayPrefab;
    public Transform OrderDayContent;

    public GameObject OrderHourPrefab;
    public Transform OrderHourContent;

    public GameObject Orderbutton;
    public string workerid;
    public DateTime SelectedDate;

    DateTime StartDate;
    DateTime EndDate;
    public void spawnDays()
    {
        foreach (Transform child in OrderDayContent.transform)
        {
            Destroy(child.gameObject);
        }

        StartDate = DateTime.Now;
        //EndDate = new DateTime(2021, 12, 01, 0, 00, 00);
        EndDate = StartDate.AddMonths(3);
        foreach (DateTime day in EachDay(StartDate, EndDate))
        {
            bool busy = false;

            foreach(DateTime busyday in BusyDates)
            {
                if(busyday == day)
                {
                    busy = true;
                    break;
                }
            }

            string weekdayId = ConvertWeekDayToId(day.DayOfWeek.ToString());
            foreach (string weekday in shedule)
            {
                string[] splitWeekday = weekday.Split(char.Parse(","));
                if (weekdayId == splitWeekday[0] && (splitWeekday[1] == "Nėra" || splitWeekday[1] =="" || splitWeekday[1] == null))
                {
                    busy = true;
                    break;
                }
            }

            if (!busy)
            {
                GameObject OrderDayListElement = Instantiate(OrderDayPrefab, OrderDayContent);
                OrderDayListElement.GetComponent<OrderDayElement>().NewElement(day.DayOfWeek.ToString(), day.Month, day.Day, day, workerid);
            }
        }
    }

    public void spawnHours(DateTime Date, string workerid)
    {
        foreach (Transform child in OrderHourContent.transform)
        {
            Destroy(child.gameObject);
        }

        string weekdayId = ConvertWeekDayToId(Date.DayOfWeek.ToString());
        List<string> hours = new List<string>();
        
        foreach(string weekday in shedule)
        {
            string[] splitWeekday = weekday.Split(char.Parse(","));
            if(weekdayId == splitWeekday[0])
            {
                for(int i= splitWeekday.Length-1; i>0; i--)
                {
                    if (splitWeekday[i] != "Nėra")
                    {
                        hours.Add(splitWeekday[i]);
                    }
                }
            }
        }

        foreach (string hour in hours)
        {
            GameObject OrderHourListElement = Instantiate(OrderHourPrefab, OrderHourContent);
            OrderHourListElement.GetComponent<OrderHourElement>().NewElement(hour, workerid);
        }
    }

    public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
    {
        for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            yield return day;
    }

    public string ConvertWeekDayToId(string weekday)
    {
        string id = "Nėra";

        if (weekday == "Monday") id = "0";
        else if (weekday == "Tuesday") id = "1";
        else if (weekday == "Wednesday") id = "2";
        else if (weekday == "Thursday") id = "3";
        else if (weekday == "Friday") id = "4";
        else if (weekday == "Saturday") id = "5";
        else if (weekday == "Sunday") id = "6";

        return id;
    }

    public void SelectedTimeButton()
    {
        GameObject OrderInfoPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().OrderInfoPanel;
        OrderInfoPanel.GetComponent<OrderProfileInfo>().getInfo();
        OrderInfoPanel.SetActive(true);
        GameObject.Find("ListPanel").GetComponent<OrderListPanel>().TimeSelectPanel.SetActive(false);

        GameObject FinalOrderPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().FinalOrderPanel;
        FinalOrderPanel.GetComponent<OrderComplete>().date = SelectedDate;
    }
}
