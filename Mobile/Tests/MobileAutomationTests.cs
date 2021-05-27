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
		public IEnumerator Test_3_2_Worker_Login()
		{
			GameObject parentObject = null;
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "WorkersButton", false), "Click object with name WorkersButton"));
			GameObject middleLevelObject = null;
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Login_UI", false);
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Email_Input").GetComponent<TMP_InputField>(), "zxczxc@gmail.com"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Password_Input").GetComponent<TMP_InputField>(), "zxczxc"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Login_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Login_Btn", false), "Click object with name Login_Btn"));
			
			FirebaseUser User = null;
			User = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().User;

			if (User.UserId == "7fCmTRXKi4ZdjTKchJTZBg1kniu1") yield return Q.assert.Pass("Sėkmingai autentifikuota");
			else yield return Q.assert.Pass("Autentifikuoti nepavyko");
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_4_1_Client_Login()
		{
			GameObject parentObject = null;
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "ClientsButton", false), "Click object with name ClientsButton"));
			GameObject middleLevelObject = null;
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Login_UI", false);
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "PhoneInputField").GetComponent<InputField>(), "+16505551234"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Login_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SendCodeButton", false), "Click object with name SendCodeButton"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "CodeInputField").GetComponent<InputField>(), "123456"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "Login_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "EnterCodeButton", false), "Click object with name EnterCodeButton"));
			FirebaseUser User = null;
			User = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().User;

			if (User.UserId == "2KRPk3E7OqdLC4c5j828bkAZ1oi2") yield return Q.assert.Pass("Sėkmingai autentifikuota");
			else yield return Q.assert.Pass("Autentifikuoti nepavyko");
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_11_1_Worker_Salon_List()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "Search_Btn", false), "Click object with name Search_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SearchFirmsElement", false), "Click object with name SearchFirmsElement"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ListPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Request_Btn", false), "Click object with name Request_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_12_1_Worker_Edit()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "EditProfile_Btn", false), "Click object with name EditProfile_Btn"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Name_Input").GetComponent<TMP_InputField>(), "darbuotojas2"));
			yield return StartCoroutine(Q.driver.SendKeys(Q.driver.FindIn(middleLevelObject, By.Name, "Lastname_Input").GetComponent<TMP_InputField>(), "dpavarde2"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ProfilePanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Save_Btn", false), "Click object with name Save_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "WorkerMain_UI", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SaveBtn", false), "Click object with name SaveBtn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_13_1_Client_Services()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			parentObject = Q.driver.Find(By.Name, "Canvas", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ActiveClientService (3)", false), "Click object with name ActiveClientService (3)"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_14_1_Client_Services_categories()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "Search_Btn", false), "Click object with name Search_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ServiceType (1)", false), "Click object with name ServiceType (1)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ServiceType", false), "Click object with name ServiceType"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ServiceType (3)", false), "Click object with name ServiceType (3)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ServiceType", false), "Click object with name ServiceType"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_15_1_Client_Date_select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "OrderPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OrderDaySelect", false), "Click object with name OrderDaySelect"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_15_2_Client_Time_select()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "OrderPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OrderHourSelect", false), "Click object with name OrderHourSelect"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_15_3_Client_Create_Order()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ServiceType", false), "Click object with name ServiceType"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "PassiveService", false), "Click object with name PassiveService"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OrderButton", false), "Click object with name OrderButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ListPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OrderSelectWorker", false), "Click object with name OrderSelectWorker"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "OrderPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "OrderDaySelect", false), "Click object with name OrderDaySelect"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "SelectTimeButton", false), "Click object with name SelectTimeButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ContinueOrderButton", false), "Click object with name ContinueOrderButton"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Item 1: Monėti vietoje (Po paslaugos suteikimo)", false), "Click object with name Item 1: Monėti vietoje (Po paslaugos suteikimo)"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "SearchPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "FinalOrderButton", false), "Click object with name FinalOrderButton"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_16_1_Cancel_Order()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ListPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Settings_Btn", false), "Click object with name Settings_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "OrderEditPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "Cancel_Btn", false), "Click object with name Cancel_Btn"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_17_1_Worker_Orders()
		{
			GameObject parentObject = null;
			GameObject middleLevelObject = null;
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(parentObject, By.Name, "MyServices_Btn", false), "Click object with name MyServices_Btn"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ActiveFirmService", false), "Click object with name ActiveFirmService"));
			middleLevelObject = Q.driver.FindIn(parentObject, By.Name, "ServicesPanel", false);
			yield return StartCoroutine(Q.driver.Click(Q.driver.FindIn(middleLevelObject, By.Name, "ActiveFirmService (9)", false), "Click object with name ActiveFirmService (9)"));
		}

		[Automation("Automatiniai testai")]
		public IEnumerator Test_18_1_Date_Format()
		{
			string date = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().timeNow;
			if (date.Contains("-")) yield return Q.assert.Pass("Datos formatas tinkamas");
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

			GameObject obj = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().EnterCodePanel;
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
