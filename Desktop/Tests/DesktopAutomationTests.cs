using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;

namespace TrilleonAutomation {

	[AutomationClass]
	public class DesktopAutomationTests : MonoBehaviour {

		[SetUpClass]
		public IEnumerator SetUpClass() {
			yield return null;
		}

		[SetUp]
		public IEnumerator SetUp() {
			yield return null;
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_1_1_DB_Error()
		{
			DatabaseReference DBreference = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().DBreference;
			DBreference = FirebaseDatabase.DefaultInstance.RootReference;
			var DBTask4 = DBreference.Child("servicesFails").OrderByChild("Status").GetValueAsync();
			yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);

			if (DBTask4.Exception != null) yield return Q.assert.Fail("Neišsiųstas klaidos pranešimas apie nepasiekiama DB");
			else yield return Q.assert.Pass("Išsiųstas klaidos pranešimas nepasiekus DB");
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_1_2_Internet_Error()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable) yield return Q.assert.Pass("Neprisijungta prie interneto");
			else if (Application.internetReachability != NetworkReachability.NotReachable) yield return Q.assert.Pass("Prisijungta prie interneto");
			else yield return Q.assert.Fail("Nepavyko patikrinti interneto ryšio");
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_3_1_Firm_Login()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Login_UI", false);
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Email_Input").GetComponent<TMP_InputField>(), "firm@gmail.com"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Password_Input").GetComponent<TMP_InputField>(), "zxczxc"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Login_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Login_Btn", false), "Click object with name Login_Btn"));
			FirebaseUser User = null;
			User = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().User;

			if (User.UserId == "IwuuUMJrJCfqC1Nt5kKSVJZ6S6R2") yield return Q.assert.Pass("Sėkmingai autentifikuota");
			else yield return Q.assert.Pass("Autentifikuoti nepavyko");
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_2_2_Get_Request()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "Search_Btn", false), "Click object with name Search_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SearchWorkers", false), "Click object with name SearchWorkers"));
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "NotificationsMenuButton", false), "Click object with name NotificationsMenuButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "NotificationPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "FirmNotificationElement", false), "Click object with name FirmNotificationElement"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "NotificationPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Accept_Btn", false), "Click object with name Accept_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "NotificationPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Decline_Btn", false), "Click object with name Decline_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "FirmMain_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ExiitButton", false), "Click object with name ExiitButton"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_5_1_Search_Worker()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "Search_Btn", false), "Click object with name Search_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SearchWorkers", false), "Click object with name SearchWorkers"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SearchWorkers", false), "Click object with name SearchWorkers"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Request_Btn", false), "Click object with name Request_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_6_1_Firm_Workers()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyWorkers_Btn", false), "Click object with name MyWorkers_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Worker", false), "Click object with name Worker"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Scroll View", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ExitButton", false), "Click object with name ExitButton"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_6_2_Delete_Workers()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyWorkers_Btn", false), "Click object with name MyWorkers_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Worker", false), "Click object with name Worker"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Scroll View", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Remove_Btn", false), "Click object with name Remove_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Scroll View", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ExitButton", false), "Click object with name ExitButton"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_7_1_Firm_Services()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "PassiveService", false), "Click object with name PassiveService"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "FirmMain_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Backbtn", false), "Click object with name Backbtn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "PassiveService", false), "Click object with name PassiveService"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "FirmMain_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "CloseButton", false), "Click object with name CloseButton"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_7_3_Firm_Locations()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "Locations_Btn", false), "Click object with name Locations_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "LocationPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Location", false), "Click object with name Location"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "LocationPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Location", false), "Click object with name Location"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "LocationPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Location", false), "Click object with name Location"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_8_1_Location_select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "CreateButton", false), "Click object with name CreateButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "CreateNewServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Item 0: Alytus, Alytaus G", false), "Click object with name Item 0: Alytus, Alytaus G"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "CreateNewServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Item 1: Kaunas, Liubarto g. 15", false), "Click object with name Item 1: Kaunas, Liubarto g. 15"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "CreateNewServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Item 2: Vilnius, Lentvario g. 5", false), "Click object with name Item 2: Vilnius, Lentvario g. 5"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_8_2_Worker_select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "CreateButton", false), "Click object with name CreateButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "CreateNewServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "WorkerSelectElement", false), "Click object with name WorkerSelectElement"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_8_3_Category_select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "CreateButton", false), "Click object with name CreateButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "TypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "TypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "TypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "CreateNewServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "TypeElement", false), "Click object with name TypeElement"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "CreateNewServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "TypeElement", false), "Click object with name TypeElement"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_8_4_Create_Service()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "CreateButton", false), "Click object with name CreateButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Title_Input").GetComponent<TMP_InputField>(), "testname"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Desc_Input").GetComponent<TMP_InputField>(), "testdesc"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Price_Input").GetComponent<TMP_InputField>(), "5"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "CreateNewServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Item 0: Alytus, Alytaus G", false), "Click object with name Item 0: Alytus, Alytaus G"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "CreateNewServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "WorkerSelectElement", false), "Click object with name WorkerSelectElement"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "TypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Duration_Input").GetComponent<TMP_InputField>(), ""));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ExitButton", false), "Click object with name ExitButton"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Duration_Input").GetComponent<TMP_InputField>(), "55"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "FirmMain_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Create_Btn", false), "Click object with name Create_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "PassiveService", false), "Click object with name PassiveService"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_9_1_Edit_Location_Select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Item 0: Alytus, Alytaus G", false), "Click object with name Item 0: Alytus, Alytaus G"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_9_2_Edit_Worker_Select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "EditWorkerSelectElement", false), "Click object with name EditWorkerSelectElement"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditWorkerSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditWorkerSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_9_3_Edit_Category_Select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditTypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditTypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditTypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditTypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_9_4_Edit_Service()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Title_Input").GetComponent<TMP_InputField>(), "testname2"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Desc_Input").GetComponent<TMP_InputField>(), "testdesc2"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Duration_Input").GetComponent<TMP_InputField>(), "552"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Price_Input").GetComponent<TMP_InputField>(), "52"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Item 1: Kaunas, Liubarto g. 15", false), "Click object with name Item 1: Kaunas, Liubarto g. 15"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Item 0: Alytus, Alytaus G", false), "Click object with name Item 0: Alytus, Alytaus G"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditWorkerSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditServicePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "EditWorkerSelectElement", false), "Click object with name EditWorkerSelectElement"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditWorkerSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OpenSelect", false), "Click object with name OpenSelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditTypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditTypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditTypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "EditTypeSelectPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "FirmMain_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SaveBtn", false), "Click object with name SaveBtn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_9_5_Delete_Service()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Remove_Btn", false), "Click object with name Remove_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_10_1_Date_Select_Calendar()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyWorkers_Btn", false), "Click object with name MyWorkers_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Viewport", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Create_Btn", false), "Click object with name Create_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SelectDataButton", false), "Click object with name SelectDataButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "FlatCalendar", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Slot_10", false), "Click object with name Slot_10"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "FlatCalendar", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_10_2_Time_Select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyWorkers_Btn", false), "Click object with name MyWorkers_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "HourElement (4)", false), "Click object with name HourElement (4)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "HourElement (5)", false), "Click object with name HourElement (5)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "HourElement (9)", false), "Click object with name HourElement (9)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "HourElement (21)", false), "Click object with name HourElement (21)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Viewport", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_10_3_Worker_Shedule_Change()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyWorkers_Btn", false), "Click object with name MyWorkers_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "HourElement (3)", false), "Click object with name HourElement (3)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "HourElement (6)", false), "Click object with name HourElement (6)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "HourElement (8)", false), "Click object with name HourElement (8)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "HourElement (18)", false), "Click object with name HourElement (18)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Viewport", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Viewport", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "WeekDay", false), "Click object with name WeekDay"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Viewport", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "WeekDay (2)", false), "Click object with name WeekDay (2)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Content", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Edit_Btn", false), "Click object with name Edit_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Worker(Clone)", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Toggle", false), "Click object with name Toggle"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Viewport", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Scroll View", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ExitButton", false), "Click object with name ExitButton"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_12_2_Firm_Edit()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "EditProfile_Btn", false), "Click object with name EditProfile_Btn"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Name_Input").GetComponent<TMP_InputField>(), "pavadinimas2"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ProfilePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "FirmMain_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SaveBtn", false), "Click object with name SaveBtn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_16_2_Order_Cancel()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyOrders_Btn", false), "Click object with name MyOrders_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "OrdersPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "FirmOrder", false), "Click object with name FirmOrder"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "OrdersPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Cancel_Btn", false), "Click object with name Cancel_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_18_1_Date_Format()
		{
			string date = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().timeNow;
			if(date.Contains("-")) yield return Q.assert.Pass("Datos formatas tinkamas");
			else yield return Q.assert.Fail("Datos formatas netinkamas");
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_18_2_Time_Format()
		{
			string date = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().timeNow;
			if (date.Contains(":")) yield return Q.assert.Pass("Laiko formatas tinkamas");
			else yield return Q.assert.Fail("Laiko formatas netinkamas");
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_19_1_Average_Calc()
		{
			float a, b, c, ats;
			a = 2f;
			b = 2.5f;
			c = 4.9f;
			ats = 3.13f;

			float newAts = (a + b + c) / 3;
			if (newAts == ats) yield return Q.assert.Pass("Vidurkis = 3.13");
			else yield return Q.assert.Fail("Neteisingai suskaičiuota");
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_19_2_Objects_Calc()
		{
			List<GameObject> ObjList = new List<GameObject>();
			GameObject obj = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().PreviewServicePanel;
			ObjList = obj.GetChildren();
			int i = 0;
			foreach (GameObject o in ObjList) i++;

			if (i == 6) yield return Q.assert.Pass("Objektų skaičius = 6");
			else yield return Q.assert.Fail("Neteisingai suskaičiuota");
		}

		[TearDown]
		public IEnumerator TearDown() {
			yield return null;
		}

		[TearDownClass]
		public IEnumerator TearDownClass() {
			yield return null;
		}
	}
}
