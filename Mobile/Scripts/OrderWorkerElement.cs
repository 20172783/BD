using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OrderWorkerElement : MonoBehaviour
{
    public string fullname;
    public string firmid;
    public string workerid;
    public List<string> workersIds = new List<string>();
    public List<DateTime> BusyDates = new List<DateTime>();
    public List<string> shedule = new List<string>();
    public TMP_Text Name;

    public void NewElement(List<string> _workersIds, string _name, string _firmid) //for all (bet kuris)
    {
        fullname = _name;
        workersIds = _workersIds;
        workerid = "";
        firmid = _firmid;

        Name.text = fullname;
    }
    public void NewElement(string _workerid, string _name, string _firmid) // for worker element
    {
        fullname = _name;
        workerid = _workerid;
        firmid = _firmid;

        Name.text = fullname;
    }

    public void SelectWorker()
    {
        GameObject OrderPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().OrderPanel;
        OrderPanel.SetActive(true);
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().GetWorkerDates(this.gameObject);
    }

    public void sendDataToTimeSelect()
    {
        GameObject TimeSelectPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().TimeSelectPanel;
        TimeSelectPanel.GetComponent<OrderDays>().shedule = shedule;
        TimeSelectPanel.GetComponent<OrderDays>().BusyDates = BusyDates;
        TimeSelectPanel.GetComponent<OrderDays>().workerid = workerid;
        TimeSelectPanel.GetComponent<OrderDays>().spawnDays();
        TimeSelectPanel.GetComponent<OrderDays>().spawnHours(DateTime.Now, workerid);

        GameObject.Find("OrderSelectedDateText").GetComponent<TMP_Text>().text = DateTime.Now.ToString("yyyy-MM-dd");
        GameObject.Find("OrderSelectedHourText").GetComponent<TMP_Text>().text = "";
        GameObject.Find("ScrollViewDay").GetComponent<OrderDays>().Orderbutton.SetActive(false);

        GameObject FinalOrderPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().FinalOrderPanel;
        FinalOrderPanel.GetComponent<OrderComplete>().workerId = workerid;
        FinalOrderPanel.GetComponent<OrderComplete>().workerName= fullname;
    }
}
