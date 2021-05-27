#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class FlatCalendar : MonoBehaviour {

	public GameObject CalledGameObject;
	public static readonly int max_day_slots = 37;
	public Sprite[] sprites;
	public int current_UiStyle;

	public struct EventObj
	{
		public string name;
		public string description;
		public EventObj(string _name, string _description)
		{
			name 		= _name;
			description = _description;
		}
		public void print()
		{
			Debug.Log("Name Event: " + name + " Description Event: " + description);
		}
	}

	public struct TimeObj
	{
		public int    year;
		public int    month;
		public int    day;
		public int    totalDays;
		public string dayOfWeek;
		public int    dayOffset;
		public TimeObj(int _year,int _month,int _day, int _totalDays, string _dayOfWeek, int _dayOffset)
		{
			year      = _year;
			month     = _month;
			day       = _day;
			totalDays = _totalDays;
			dayOffset = _dayOffset;
			dayOfWeek = _dayOfWeek;
		}

		public void print()
		{
			//Debug.Log("Year:"+year+" Month:"+month+" Day:"+day+" Day of Week:"+dayOfWeek);
			Debug.Log(year + "-" + month + "-" + day);
			//2018-12-16T23:03:53Z FORMATAS
		}
		public string getSelectedData()
        {
			string data = year + "-" + month + "-" + day;
			return data;
		}
	}
	public void SaveButton()
    {
		CalledGameObject.GetComponent<DateSelect>().GetSelectedDate();
	}
	GameObject btn_nextMonth;
	GameObject btn_prevMonth;
	GameObject btn_calendar;
	GameObject label_year;
	GameObject label_month;
	GameObject label_dayOfWeek;
	GameObject label_dayNumber;
	GameObject label_numberEvents;
	public TimeObj currentTime;
	public static Dictionary<int,Dictionary<int,Dictionary<int,List<EventObj>>>> events_list; // <Year,<Month,<Day,Number of Events>>>
	public delegate void Delegate_OnDaySelected(TimeObj time);
	public delegate void Delegate_OnEventSelected(TimeObj time, List<EventObj> evs);
	public delegate void Delegate_OnMonthChanged(TimeObj time);
	public delegate void Delegate_OnNowDay(TimeObj time);
	public Delegate_OnDaySelected   delegate_ondayselected;
	public Delegate_OnEventSelected delegate_oneventselected;
	public Delegate_OnMonthChanged  delegate_onmonthchanged;
	public Delegate_OnNowDay		 delegate_onnowday;

	public void initFlatCalendar()
	{
		btn_nextMonth      = GameObject.Find("Right_btn");
		btn_prevMonth      = GameObject.Find("Left_btn");
		btn_calendar       = GameObject.Find("Calendar_Btn");
		label_year         = GameObject.Find("Year");
		label_month        = GameObject.Find("Month");
		label_dayOfWeek    = GameObject.Find("Day_Title1");
		label_dayNumber    = GameObject.Find("Day_Title2");
		label_numberEvents = GameObject.Find("NumberEvents");

		addEventsListener();
		FlatCalendarStyle.changeUIStyle(current_UiStyle);
		setCurrentTime();
		events_list = new Dictionary<int, Dictionary<int, Dictionary<int,List<EventObj>>>>();
		updateCalendar(currentTime.month,currentTime.year);
		markSelectionDay(currentTime.day);
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		this.transform.GetChild(0).gameObject.SetActive(false); // isjungia calendar child po initializingo
	}
	public void updateCalendar(int month_number, int year)
	{
		populateAllSlot(month_number,year);
		label_year.GetComponent<Text>().text      = "" + currentTime.year;
		label_month.GetComponent<Text>().text     = getMonthStringFromNumber(currentTime.month);
	}

	public void refreshCalendar()
	{
		populateAllSlot(currentTime.month,currentTime.year);
	}

	string getMonthStringFromNumber(int month_number)
	{
		string month = "";

		if(month_number == 1) month = "Sausis";
		if(month_number == 2) month = "Vasaris";
		if(month_number == 3) month = "Kovas";
		if(month_number == 4) month = "Balandis";
		if(month_number == 5) month = "Gegužė";
		if(month_number == 6) month = "Birželis";
		if(month_number == 7) month = "Liepa";
		if(month_number == 8) month = "Rugpjūtis";
		if(month_number == 9) month = "Rugsėjis";
		if(month_number == 10) month = "Spalis";
		if(month_number == 11) month = "Lapkritis";
		if(month_number == 12) month = "Gruodis";

		return month;
	}
	string getDayOfWeek(int year, int month, int day)
	{
		System.DateTime dateValue = new System.DateTime(year,month,day);

		return dateValue.DayOfWeek.ToString();
	}

	int getIndexOfFirstSlotInMonth(int year, int month)
	{
		int indexOfFirstSlot = 0;
		System.DateTime dateValue = new System.DateTime(year,month,1);
		string dayOfWeek          = dateValue.DayOfWeek.ToString();

		if(dayOfWeek == "Pirmadienis")    indexOfFirstSlot = 0;
		if(dayOfWeek == "Antradienis")   indexOfFirstSlot = 1;
		if(dayOfWeek == "Trečiadienis") indexOfFirstSlot = 2;
		if(dayOfWeek == "Ketvirtadienis")  indexOfFirstSlot = 3;
		if(dayOfWeek == "Penktadienis")    indexOfFirstSlot = 4;
		if(dayOfWeek == "Šeštadienis")  indexOfFirstSlot = 5;
		if(dayOfWeek == "Sekmadienis")    indexOfFirstSlot = 6;

		return indexOfFirstSlot;
	}

	void disableAllSlot()
	{
		for(int i = 0; i < max_day_slots; i++)
			disableSlot(i+1);
	}

	void disableSlot(int numSlot)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = false;
		day_slot.GetComponent<Image>().enabled  = false;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = false;
	}

	void setNormalSlot(int numSlot)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = true;
		day_slot.GetComponent<Image>().enabled  = false;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = true;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().color = FlatCalendarStyle.color_dayTextNormal;
	}
	void setEventSlot(int numSlot)
	{
		Sprite sprite       = Resources.Load<Sprite>("img/circle_filled");
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = true;
		day_slot.GetComponent<Image>().enabled  = true;
		day_slot.GetComponent<Image>().sprite   = sprite;
		day_slot.GetComponent<Image>().color    = FlatCalendarStyle.color_bubbleEvent;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = true;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().color = FlatCalendarStyle.color_dayTextEvent;
	}
	void populateAllSlot(int monthNumber, int year)
	{
		disableAllSlot();

		for (int i = 0; i < currentTime.totalDays; i++)
		{	
			changeTextSlot(i+currentTime.dayOffset+1,""+(i+1));
			if(checkEventExist(currentTime.year,currentTime.month,(i+1)))
				setEventSlot(i+currentTime.dayOffset+1);
			else
				setNormalSlot(i+currentTime.dayOffset+1);
		}
	}

	void changeTextSlot(int numSlot, string text)
	{
		GameObject day_slot = GameObject.Find("Slot_"+numSlot);
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().text = text;
	}

	int getDayInSlot(int numSlot)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		string txt = day_slot.GetComponentInChildren<Text>().text;
		return int.Parse(txt);
	}
	public void markSelectionDay(int day)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (day+currentTime.dayOffset));

		if(!checkEventExist(currentTime.year,currentTime.month,day))
		{
			Sprite sprite       = Resources.Load<Sprite>("img/circle_unfilled");
			day_slot.GetComponent<Image>().sprite   = sprite;
			day_slot.GetComponent<Image>().enabled  = true;
			day_slot.GetComponent<Image>().color    = FlatCalendarStyle.color_bubbleSelectionMarker;
			day_slot.GetComponent<Button>().GetComponentInChildren<Text>().color = FlatCalendarStyle.color_dayTextNormal;
		}
		label_dayOfWeek.GetComponent<Text>().text = currentTime.dayOfWeek;
		label_dayNumber.GetComponent<Text>().text = "" + currentTime.day;
	}

	void unmarkSelctionDay(int day)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (day+currentTime.dayOffset));
		if(!checkEventExist(currentTime.year,currentTime.month,day))
		{
			setNormalSlot(day+currentTime.dayOffset);
		}
	}

	public static bool checkEventExist(int year, int month, int day)
	{
		if(events_list == null)
			return false;

		if(!events_list.ContainsKey(year))
			return false;

		if(!events_list[year].ContainsKey(month))
			return false;

		if(!events_list[year][month].ContainsKey(day))
			return false;

		if(events_list[year][month][day] == null)
			return false;

		if(events_list[year][month][day].Count == 0)
			return false;

		return true;
	}
	void addEventsListener()
	{
		btn_nextMonth.GetComponent<Button>().onClick.AddListener(() => evtListener_NextMonth());
		btn_prevMonth.GetComponent<Button>().onClick.AddListener(() => evtListener_PreviousMonth());
		btn_calendar.GetComponent<Button>().onClick.AddListener(()   => evtListener_GoToNowday());
		for(int i = 0; i < max_day_slots; i++)
			GameObject.Find("Slot_"+(i+1)).GetComponent<Button>().onClick.AddListener(() => evtListener_DaySelected());
	}

	public void setCurrentTime()
	{
		currentTime.year      = System.DateTime.Now.Year;
		currentTime.month     = System.DateTime.Now.Month;
		currentTime.day       = System.DateTime.Now.Day;
		currentTime.dayOfWeek = System.DateTime.Now.DayOfWeek.ToString();
		currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year,currentTime.month);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
	}
	void setCurrentTime(FlatCalendar.TimeObj obj)
	{
		obj.year      = System.DateTime.Now.Year;
		obj.month     = System.DateTime.Now.Month;
		obj.day       = System.DateTime.Now.Day;
		obj.dayOfWeek = System.DateTime.Now.DayOfWeek.ToString();
		obj.totalDays = System.DateTime.DaysInMonth(obj.year,obj.month);
		obj.dayOffset = getIndexOfFirstSlotInMonth(obj.year,obj.month);
	}
	public void installDemoData()
	{
		addEvent(2016,3,7,  new EventObj("Event","Description"));
		addEvent(2016,3,7,  new EventObj("Event","Description"));
		addEvent(2016,3,10, new EventObj("Event","Description"));
		addEvent(2016,3,22, new EventObj("Event","Description"));
		addEvent(2016,4,5,  new EventObj("Event","Description"));
		addEvent(2016,4,5,  new EventObj("Event","Description"));
		addEvent(2016,4,5,  new EventObj("Event","Description"));
		addEvent(2016,4,15, new EventObj("Event","Description"));
		addEvent(2016,4,22, new EventObj("Event","Description"));
		addEvent(2016,5,1,  new EventObj("Event","Description"));
		addEvent(2016,5,2,  new EventObj("Event","Description"));
		addEvent(2016,5,3,  new EventObj("Event","Description"));
		addEvent(2016,5,15, new EventObj("Event","Description"));
		addEvent(2016,6,2,  new EventObj("Event","Description"));
		addEvent(2016,6,3,  new EventObj("Event","Description"));
		addEvent(2016,6,4,  new EventObj("Event","Description"));
		addEvent(2016,6,22, new EventObj("Event","Description"));
	}

	public void setUIStyle(int style)
	{
		current_UiStyle = style;
		FlatCalendarStyle.changeUIStyle(current_UiStyle);
	}

	public void addEvent(int year, int month, int day, EventObj ev)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		events_list[year][month][day].Add(ev);
	}

	public void removeEvent(int year, int month, int day, EventObj ev)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		if(events_list[year][month][day].Contains(ev))
			events_list[year][month][day].Remove(ev);
	}
	public void removeAllEventOfDay(int year, int month, int day)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		events_list[year][month][day].Clear();
	}

	public void removeAllCalendarEvents()
	{
		events_list.Clear();
	}
	public static List<EventObj> getEventList(int year, int month, int day)
	{
		List<EventObj> list = new List<EventObj>();

		if(!events_list.ContainsKey(year))
			return list;

		if(!events_list[year].ContainsKey(month))
			return list;

		if(!events_list[year][month].ContainsKey(day))
			return list;

		return events_list[year][month][day];
	}

	void updateUiLabelEvents(int year, int month, int day)
	{
		label_numberEvents.GetComponent<Text>().text = "" + getEventList(year,month,day).Count;
	}

	void evtListener_NextMonth()
	{
		unmarkSelctionDay(currentTime.day);

		currentTime.month = (currentTime.month+1) % 13;
		if(currentTime.month == 0)
		{
			currentTime.year++;
			currentTime.month = 1;
		}

		currentTime.day       = 1;
        currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
        currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year, currentTime.month);
  
		updateCalendar(currentTime.month,currentTime.year);
		markSelectionDay(currentTime.day);
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		if(delegate_onmonthchanged != null)
			delegate_onmonthchanged(currentTime);
	}

	void evtListener_PreviousMonth()
	{
		unmarkSelctionDay(currentTime.day);
		currentTime.month = (currentTime.month-1) % 13;
		if(currentTime.month == 0)
		{
			currentTime.year--;
			currentTime.month = 12;
		}

		currentTime.day   = 1;
		currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
        currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year, currentTime.month);

		updateCalendar(currentTime.month,currentTime.year);
		markSelectionDay(currentTime.day);
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		if(delegate_onmonthchanged != null)
			delegate_onmonthchanged(currentTime);
	}

	void evtListener_DaySelected()
	{
		unmarkSelctionDay(currentTime.day);

		string slot_name             = EventSystem.current.currentSelectedGameObject.name;
		int    slot_position         = int.Parse(slot_name.Substring(5,(slot_name.Length-5)));
		 	   currentTime.day       = getDayInSlot(slot_position);
			   currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);

		markSelectionDay(currentTime.day);
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		if(delegate_ondayselected != null)
			delegate_ondayselected(currentTime);

		if(getEventList(currentTime.year,currentTime.month,currentTime.day).Count > 0)
			if(delegate_oneventselected != null)
				delegate_oneventselected(currentTime,getEventList(currentTime.year,currentTime.month,currentTime.day));
	}

	void evtListener_GoToNowday()
	{
		unmarkSelctionDay(currentTime.day);
		setCurrentTime();
		updateCalendar(currentTime.month,currentTime.year);
		markSelectionDay(currentTime.day);
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		if(delegate_onnowday != null)
			delegate_onnowday(currentTime);
	}

	public void setCallback_OnDaySelected(Delegate_OnDaySelected func)
	{
		delegate_ondayselected = func;
	}

	public void setCallback_OnEventSelected(Delegate_OnEventSelected func)
	{
		delegate_oneventselected = func;
	}

	public void setCallback_OnMonthChanged(Delegate_OnMonthChanged func)
	{
		delegate_onmonthchanged = func;
	}
	public void setCallback_OnNowday(Delegate_OnNowDay func)
	{
		delegate_onnowday = func;
	}
}
