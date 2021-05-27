using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlatCalendar_Demo : MonoBehaviour {

	FlatCalendar flatCalendar;

	void Start () 
	{
		flatCalendar = GameObject.Find("FlatCalendar").GetComponent<FlatCalendar>();
		flatCalendar.initFlatCalendar();
		flatCalendar.installDemoData();

		flatCalendar.setCallback_OnDaySelected(dayUpdated);
		flatCalendar.setCallback_OnMonthChanged(monthUpdated);
		flatCalendar.setCallback_OnEventSelected(eventsDiscovered);
		flatCalendar.setCallback_OnNowday(backHome);

		flatCalendar.setUIStyle(0);
	}
	public void dayUpdated(FlatCalendar.TimeObj time)
	{
		Debug.Log("Day has changed");
		time.print();
	}

	public void monthUpdated(FlatCalendar.TimeObj time)
	{
		Debug.Log("Month has changed");
		time.print();
	}
	public void eventsDiscovered(FlatCalendar.TimeObj time, List<FlatCalendar.EventObj> list)
	{
		Debug.Log("You have selected a day with: "+list.Count+ "events");
		for(int i = 0; i < list.Count; i++)
			Debug.Log("Event: " + i + " ==> " + "Name: " + list[i].name + " Description: " + list[i].description);
	}
	public void backHome(FlatCalendar.TimeObj time)
	{
		Debug.Log("You have come back at home");
		time.print();
	}
}
