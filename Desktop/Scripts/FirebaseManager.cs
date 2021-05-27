using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using Firebase.Storage;
using Firebase.Extensions;
using System;

public class FirebaseManager : MonoBehaviour
{
    [Header("DB")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;
    FirebaseStorage storage;
    StorageReference storage_ref;

    [Header("Others")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_InputField phoneNumberField;
    public TMP_InputField asm_Kodo_Field;
    public TMP_InputField name_Field;
    public TMP_Text warningRegisterText;

    public GameObject searchWorkerElement;
    public Transform searchWorkerListContent;

    public GameObject FirmWorkerElement;
    public Transform FirmWorkerListContent;

    public TMP_InputField NewLocationCity;
    public TMP_InputField NewLocationAdress;

    public TMP_Text NewLocationDebugText;
    public GameObject NewLocationCreationPanel;

    public GameObject FirmLocationElement;
    public Transform FirmLocationListContent;

    public GameObject NotWorkingElement;
    public GameObject BookedElement;

    public Transform WorkerSelectListContent;
    public GameObject SelectWorkerPrefab;
    public Transform WorkerSelectListContentEdit;
    public GameObject EditSelectWorkerPrefab;

    public Transform LocationSelectContent;
    public Transform LocationSelectContentEdit;

    public Transform FirmServicesListContent;
    public GameObject FirmServicesPrefab;

    public GameObject scoreElement;
    public Transform scoreboardContent;

    public List<List<string>> SheduleList = new List<List<string>>();
    public List<string> booked = new List<string>();
    public List<string> NotWorking = new List<string>();

    public GameObject requestMessage;

    //workernotifications
    public GameObject NotificationElement;
    public Transform NotificationListContent;

    public GameObject EditLocationPanel;

    public GameObject EditServicePanel;
    public GameObject PreviewServicePanel;

    public GameObject ProfilePanel;

    public GameObject OrderElement;
    public Transform OrderListContent;
    public GameObject OrderPanel;

    [Header("Calendar")]
    public GameObject FlatCalendar;

    public string timeNow;
    private bool FirebaseInitializeDone = false;
    void Awake()
    {
        FirebaseInitializeDone = false;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) InitializeFirebase();
            else Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }).ContinueWith(task => { FirebaseInitializeDone = true; });
    }

    void FillSheduleList()
    {
        for (int i = 0; i < 7; i++)
        {
            List<string> List = new List<string>();
            SheduleList.Add(List);
        }
    }
    void Start()
    {
        TrilleonAutomation.AutomationMaster.Initialize();
        UIManager.instance.LoadingScreen();
        FlatCalendar.SetActive(true);//paleisti kalendoriu kad spetu issijungt
        StartCoroutine(WaitForFirebaseInitialize());
        FillSheduleList(); // create lists list bcs be sito out of bounds
        timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
    }

    IEnumerator WaitForFirebaseInitialize()
    {
        Debug.Log("Waiting For Firebase To Initialize...");
        yield return new WaitUntil(() => FirebaseInitializeDone == true);
        Debug.Log("Firebase Successfully Initialized!");
        CheckIfLoggedIn();
    }
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth...");
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        storage = FirebaseStorage.DefaultInstance;
        storage_ref = storage.GetReferenceFromUrl("gs://kirpyklos-is.appspot.com");
    }

    private IEnumerator CheckProfileType()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("type").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            int type = int.Parse(snapshot.Value.ToString());

            if (type == 2)
            {
                warningLoginText.text = "";
                Debug.LogFormat("User signed in successfully as FIRM: {0} ({1})", User.DisplayName, User.Email);
                confirmLoginText.text = "Logged In";
                UIManager.instance.FirmMainScreen();
                StartCoroutine(LoadUserData());
            }
            else if (type == 3)
            {
                warningLoginText.text = "";
                Debug.LogFormat("User signed in successfully as ADMIN: {0} ({1})", User.DisplayName, User.Email);
                confirmLoginText.text = "Logged In";
                UIManager.instance.AdminMainScreen();
                StartCoroutine(LoadUserData());
            }
            else
            {
                warningLoginText.text = "Wrong Log In Data";
                confirmLoginText.text = "";
                auth.SignOut();
                UIManager.instance.LoginScreen();
            }
        }
    }
    private void CheckIfLoggedIn()
    {
        if (auth.CurrentUser != null)
        {
            User = auth.CurrentUser;
            warningLoginText.text = "";
            confirmLoginText.text = "";

            StartCoroutine(CheckProfileType());
            confirmLoginText.text = "";
            warningLoginText.text = "";
            ClearLoginFeilds();
            ClearRegisterFeilds();
        }
        else
        {
            Debug.LogFormat("User Not Signed In");
            UIManager.instance.LoginScreen();
        }
    }

    public void ClearLoginFeilds()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }
    public void ClearRegisterFeilds()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }
    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    public void RegisterButton()
    {
        StartCoroutine(Register_Firm(emailRegisterField.text, name_Field.text, passwordRegisterField.text, usernameRegisterField.text, phoneNumberField.text, asm_Kodo_Field.text));
    }
    public void AddLocation()
    {
        StartCoroutine(AddNewLocation(User.UserId, NewLocationCity.text, NewLocationAdress.text));
    }
    public void LoadFirmServiceList()
    {
        StartCoroutine(LoadFirmServiceData());
    }
    public void SaveWorkerTimesButton(string workerid, List<List<string>> SheduleList2)
    {
        StartCoroutine(SaveWorkerWorkTimes(workerid, SheduleList2));
    }
    public void CreateInactiveWorkerTimeButton(string workerId, string date, string hour, GameObject panel)
    {
        StartCoroutine(CreateInactiveWorkerTime(workerId, date, hour, panel));
    }
    public void LoadInactiveWorkerTimeButton(string workerId, Transform BookedNotWorkingListContent)
    {
        StartCoroutine(LoadInactiveWorkerTime(workerId, BookedNotWorkingListContent));
    }
    public void RemoveInactiveWorkerTimeButton(string workerId, string elementId)
    {
        StartCoroutine(RemoveInactiveWorkerTime(workerId, elementId));
    }
    public void CreateServiceButton(string title, string desc, string location_id, string price, string duration, string public_status, List<string> workerids, List<string> types, string ImgUrl, string ImgFormat)
    {
        StartCoroutine(CreateService(title, desc, location_id, price, duration, public_status, workerids, types, ImgUrl, ImgFormat));
    }
    public void SignOutButton()
    {
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFeilds();
        ClearLoginFeilds();
    }
    private void CreateFirmData(string username, string name, string email, string phone, string asm_kodas)
    {
        StartCoroutine(UpdateUsernameDatabase(username));
        StartCoroutine(UpdateEmail(email));
        StartCoroutine(UpdatePhone(phone));
        StartCoroutine(UpdateAsmKodas(asm_kodas));
        StartCoroutine(UpdateFirstName(name));

        //default
        StartCoroutine(UpdateRating(0));
        StartCoroutine(UpdateVerified(0));
        StartCoroutine(UpdateAccepted(0));
        StartCoroutine(UpdateStatus(0));
        StartCoroutine(UpdateType(2)); //1 - worker 0 - client, 2 - firm

        //default lists
        List<string> empty = new List<string>();
        //  empty.Add("empty");
        StartCoroutine(UpdateServicesList(empty));
        StartCoroutine(UpdateWorkerList(empty));
        StartCoroutine(UpdateLocationList(empty));
    }
    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }
    public void LoadFirmLocationDataButton()
    {
        StartCoroutine(LoadFirmLocationData());
    }
    public void LoadFirmLocationSelectDataButton()
    {
        StartCoroutine(LoadFirmLocationToSelect());
    }
    public void SearchWorkerButton()
    {
        StartCoroutine(LoadSearchWorkersData());
    }
    public void FirmWorkersButton()
    {
        StartCoroutine(LoadFirmWorkersData());
        StartCoroutine(LoadWorkerWorkTimes());
    }
    public void RequestButton(string id)
    {
        StartCoroutine(SendRequestToWorker(id));
    }
    public void LoadFirmWorkersToSelectButton()
    {
        StartCoroutine(LoadFirmWorkersToSelect());
    }
    private IEnumerator Login(string _email, string _password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            User = LoginTask.Result;
            warningLoginText.text = "";
            confirmLoginText.text = "";
            StartCoroutine(CheckProfileType()); // pagal prisijungta acc turetu ijungt screen
            yield return new WaitForSeconds(2);

            confirmLoginText.text = "";
            warningLoginText.text = "";
            ClearLoginFeilds();
            ClearRegisterFeilds();

            LoadFirmServiceList();
            LoadFirmLocationSelectDataButton();
        }
    }

    private IEnumerator Register_Firm(string _email, string _name, string _password, string _username, string _phone, string _asm_kodas)
    {
        if (_username == "")
        {
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                User = RegisterTask.Result;

                if (User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        CreateFirmData(_username, _name, _email, _phone, _asm_kodas);
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                        ClearRegisterFeilds();
                        ClearLoginFeilds();
                    }
                }
            }
        }
    }
    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdateEmail(string email)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("email").SetValueAsync(email);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdatePhone(string phone)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("phone").SetValueAsync(phone);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdateAsmKodas(string asm_kodas)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("asm_kodas").SetValueAsync(asm_kodas);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdateFirstName(string name)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("name").SetValueAsync(name);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }
    private IEnumerator UpdateRating(int rating)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("rating").SetValueAsync(rating);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdateVerified(int verified)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("verified").SetValueAsync(verified);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdateAccepted(int accepted)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("accepted").SetValueAsync(accepted);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdateStatus(int status)
    {
        //Set the currently logged in user Email
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("status").SetValueAsync(status);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdateType(int type)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("type").SetValueAsync(type);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    private IEnumerator AddNewLocation(string firmid, string city, string adress)
    {
        string key = DBreference.Child("users").Child(firmid).Child("LocationList").Push().Key;
        var DBTask = DBreference.Child("users").Child(firmid).Child("LocationList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot snapshot = DBTask.Result;
        bool ValueExist = false;
        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
        {
            if (childSnapshot.Child("City").Value.ToString() == city && childSnapshot.Child("Adress").Value.ToString() == adress)
            {
                ValueExist = true;
                break;
            }
        }

        if (!ValueExist)
        {
            var DBTask1 = DBreference.Child("users").Child(firmid).Child("LocationList").RunTransaction(Data =>
            {
                Data.Child(key).Child("City").Value = city;
                Data.Child(key).Child("Adress").Value = adress;
                return TransactionResult.Success(Data);
            });
            yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);
            NewLocationCreationPanel.SetActive(false); // isjungti new Location create paneli
        }
        else
        {
            NewLocationDebugText.text = "City with this Adress already in the list";
            Debug.Log("City with this Adress already in the list");
        }
    }

    private IEnumerator UpdateServicesList(List<string> services)
    {
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("ServicesList").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        if (DBTask1.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to Delete task with {DBTask1.Exception}");
        }

        int number = 0;
        foreach (string service in services)
        {
            var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("ServicesList").Child("" + number).SetValueAsync(service);
            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

            if (DBTask2.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to Delete task with {DBTask2.Exception}");
                break;
            }
            else number++;
        }
    }

    private IEnumerator UpdateFirmList(List<string> firms)
    {
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("FirmList").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        if (DBTask1.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to Delete task with {DBTask1.Exception}");
        }

        int number = 0;
        foreach (string firm in firms)
        {
            var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("FirmList").Child("" + number).SetValueAsync(firm);
            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

            if (DBTask2.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to Delete task with {DBTask2.Exception}");
                break;
            }
            else number++;
        }
    }

    private IEnumerator UpdateWorkerList(List<string> workers)
    {
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        if (DBTask1.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to Delete task with {DBTask1.Exception}");
        }

        int number = 0;
        foreach (string worker in workers)
        {
            var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child("" + number).SetValueAsync(worker);
            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

            if (DBTask2.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to Delete task with {DBTask2.Exception}");
                break;
            }
            else number++;
        }
    }
    private IEnumerator CreateInactiveWorkerTime(string workerId, string date, string hour, GameObject panel)
    {
        string key = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(workerId).Child("NotWorking").Push().Key;
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(workerId).Child("NotWorking").RunTransaction(Data =>
        {
            Data.Child(key).Child("Date").Value = date;
            Data.Child(key).Child("Hour").Value = hour;
            return TransactionResult.Success(Data);
        });
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        panel.GetComponent<BookedNotWorking>().LoadListElements();
    }

    private IEnumerator LoadInactiveWorkerTime(string workerId, Transform BookedNotWorkingListContent)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(workerId).Child("NotWorking").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot snapshot = DBTask.Result;
        foreach (Transform child in BookedNotWorkingListContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
        {
            NotWorking.Add(childSnapshot.Key);
            string date = childSnapshot.Child("Date").Value.ToString();
            string hour = childSnapshot.Child("Hour").Value.ToString();
            string id = childSnapshot.Key.ToString();

            GameObject NotWorkingListElement = Instantiate(NotWorkingElement, BookedNotWorkingListContent);
            NotWorkingListElement.GetComponent<InactiveElement>().NewElement(id, workerId, date, hour);
        }
    }
    private IEnumerator RemoveInactiveWorkerTime(string workerId, string elementId)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(workerId).Child("NotWorking").Child(elementId).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    private IEnumerator LoadWorkerWorkTimes()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("WorkerList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        for (int i = 0; i < 7; i++)
        {
            SheduleList[i].Clear();
        }
        booked.Clear();
        NotWorking.Clear();

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to Delete task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (DataSnapshot childSnapshot1 in snapshot.Children.Reverse<DataSnapshot>()) // user
            {
                //    Debug.Log("User:" + childSnapshot1.Key);
                foreach (DataSnapshot childSnapshot2 in childSnapshot1.Child("Shedule").Children.Reverse<DataSnapshot>()) //pirm antr trec ....
                {
                    int i = int.Parse(childSnapshot2.Key);
                    //     Debug.Log("     Day:" + childSnapshot2.Key);
                    foreach (DataSnapshot childSnapshot3 in childSnapshot2.Children.Reverse<DataSnapshot>()) //hours
                    {
                        //        Debug.Log("         Time:" + childSnapshot3.Key);
                        SheduleList[i].Add(childSnapshot3.Key);
                    }

                }

                foreach (DataSnapshot childSnapshot2 in childSnapshot1.Child("Booked").Children.Reverse<DataSnapshot>())
                {
                    booked.Add(childSnapshot2.Key);
                    string name = childSnapshot2.Child("name").Value.ToString();
                    string lastname = childSnapshot2.Child("lastname").Value.ToString();
                    int rating = int.Parse(childSnapshot2.Child("rating").Value.ToString());
                    string id = childSnapshot2.Key.ToString();

                    int i = int.Parse(childSnapshot2.Key);
                    SheduleList[i].Add(childSnapshot2.Key);
                    /*
                      GameObject BookedListElement = Instantiate(BookedElement, BookedNotWorkingListContent);
                      BookedListElement.GetComponent<e>().NewElement(name, lastname, rating, id);
                    */
                }

                 foreach (DataSnapshot childSnapshot2 in childSnapshot1.Child("NotWorking").Children.Reverse<DataSnapshot>())
                 {
                    NotWorking.Add(childSnapshot2.Key);
                    string date = childSnapshot2.Child("date1").Value.ToString();
                    string hour = childSnapshot2.Child("hour").Value.ToString();
                    string id = childSnapshot2.Key.ToString();

                    int i = int.Parse(childSnapshot2.Key);
                    SheduleList[i].Add(childSnapshot2.Key);
                   /*
                     GameObject NotWorkingListElement = Instantiate(NotWorkingElement, BookedNotWorkingListContent);
                     NotWorkingListElement.GetComponent<InactiveElement>().NewElement(id, date, hour);
                   */
                 }
            }
        }
    }

    private IEnumerator SaveWorkerWorkTimes(string workerid, List<List<string>> SheduleList)
    {
        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(workerid).Child("Shedule").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        var DBTask3 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(workerid).Child("Booked").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);

        var DBTask4 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(workerid).Child("NotWorking").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);

        var DBTask = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(workerid).RunTransaction(Data =>
        {
            for (int i = 0; i < 7; i++)
            {
                foreach (string child in SheduleList[i])
                {
                    Data.Child("Shedule").Child(i + "").Child(child).Value = "";
                }
            }
            return TransactionResult.Success(Data);
        });
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to Delete task with {DBTask.Exception}");
        }
    }

    private IEnumerator UpdateLocationList(List<string> locations)
    {
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("LocationList").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        if (DBTask1.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to Delete task with {DBTask1.Exception}");
        }

        int number = 0;
        foreach (string location in locations)
        {
            var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("LocationList").Child("" + number).SetValueAsync(location);
            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

            if (DBTask2.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to Delete task with {DBTask2.Exception}");
                break;
            }
            else number++;
        }
    }

    private IEnumerator SendRequestToWorker(string workerId)
    {
        if (User != null)
        {
            string FirmName = "";
            var DBTask3 = DBreference.Child("users").Child(User.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);
            DataSnapshot snapshot3 = DBTask3.Result;
            FirmName = snapshot3.Child("name").Value.ToString();
            //Debug.Log(" " + FirmName);

            var DBTask2 = DBreference.Child("users").Child(workerId).GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
            DataSnapshot snapshot2 = DBTask2.Result;
            string WorkerName = snapshot2.Child("name").Value.ToString();
            //Debug.Log(" " + WorkerName);

            var DBTask = DBreference.Child("requests").OrderByChild("FromFirm_ToWorker").EqualTo(User.UserId + "_" + workerId).GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value != null)
            {
                Debug.Log("request already exists " + DBTask.Result.Value);
                //request already exists
            }
            else
            {
                string key = DBreference.Child("requests").Push().Key;

                var DBTask1 = DBreference.Child("requests").RunTransaction(Data =>
                {
                    Data.Child(key).Child("ToWorker").Value = workerId;
                    Data.Child(key).Child("Worker").Value = WorkerName;
                    Data.Child(key).Child("FromFirm").Value = User.UserId;
                    Data.Child(key).Child("Firm").Value = FirmName;
                    Data.Child(key).Child("Status").Value = 0;
                    Data.Child(key).Child("DateSent").Value = 0;
                    Data.Child(key).Child("FromFirm_ToWorker").Value = User.UserId + "_" + workerId;
                    Data.Child(key).Child("FromWorker_ToFirm").Value = "";
                    return TransactionResult.Success(Data);
                });
            }
        }
    }
    private IEnumerator LoadUserData()
    {
        if (User != null)
        {
            var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                //No data exists
            }
            else
            {
                DataSnapshot snapshot = DBTask.Result;

                string name = snapshot.Child("name").Value.ToString();
                string phone = snapshot.Child("phone").Value.ToString();
                string rating = snapshot.Child("rating").Value.ToString();
                string username = snapshot.Child("username").Value.ToString();
                string accepted = snapshot.Child("accepted").Value.ToString();
                string verified = snapshot.Child("verified").Value.ToString();
                string status = snapshot.Child("status").Value.ToString();
                string asm_kodas = snapshot.Child("asm_kodas").Value.ToString();
                string email = snapshot.Child("email").Value.ToString();

                ProfilePanel.GetComponent<ProfileInfo>().fillData(name, phone, rating, username, accepted, verified, status, asm_kodas, email);
            }
        }
    }

    private IEnumerator LoadSearchWorkersData()
    {
        var DBTask = DBreference.Child("users").OrderByChild("type").EqualTo(1).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        DataSnapshot snapshot2 = DBTask2.Result;

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in searchWorkerListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (!snapshot2.Child(childSnapshot.Key).Exists)
                {
                    string name = childSnapshot.Child("name").Value.ToString();
                    string lastname = childSnapshot.Child("lastname").Value.ToString();
                    int rating = int.Parse(childSnapshot.Child("rating").Value.ToString());
                    string id = childSnapshot.Key.ToString();

                    GameObject WorkerListElement = Instantiate(searchWorkerElement, searchWorkerListContent);
                    WorkerListElement.GetComponent<SearchWorkerElements>().NewElement(name, lastname, rating, id);
                }
            }
        }
    }

    private IEnumerator UploadFileToFirebase(string localUrl)
    {
        StorageReference NewRef = storage_ref.Child("file/filename.file");

        // Upload the file to the path "images/img.jpg"
        var DBTask = NewRef.PutFileAsync(localUrl).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                StorageMetadata metadata = task.Result;
                //   string md5Hash = metadata.Md5Hash;
                Debug.Log("Finished uploading...");
                // Debug.Log("md5 hash = " + md5Hash);
                //  Debug.Log("link? = " + metadata);
            }
        });
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    private IEnumerator CreateService(string title, string desc, string location_id, string price, string duration, string public_status, List<string> workerids, List<string> types, string ImgUrl, string ImgFormat)
    {
        string key = DBreference.Child("services").Push().Key;

        var DBTask = DBreference.Child("services").Child(key).RunTransaction(Data =>
        {
            Data.Child("FirmId").Value = User.UserId;
            Data.Child("Title").Value = title;
            Data.Child("Desc").Value = desc;
            Data.Child("LocationId").Value = location_id;
            Data.Child("Price").Value = price;
            Data.Child("Duration").Value = duration;
            Data.Child("PublicStatus").Value = public_status;
            DBreference.Child("users").Child(User.UserId).Child("ServicesList").Child(key).SetValueAsync("");

            foreach (string workerid in workerids)
            {
                Data.Child("Workers").Child(workerid).Value = "";
                DBreference.Child("users").Child(workerid).Child("ServicesList").Child(key).SetValueAsync("");
            }

            foreach (string type in types)
            {
                Data.Child("Types").Child(type).Value = "";
            }

            Data.Child("Rating").Value = 0;
            Data.Child("Reviews").Value = 0;
            Data.Child("Status").Value = 0;

            return TransactionResult.Success(Data);
        });
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        StorageReference NewRef = storage_ref.Child("ServicesImages/" + key + ".jpg");

        var DBTask1 = NewRef.PutFileAsync(ImgUrl).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                StorageMetadata metadata = task.Result;
                string md5Hash = metadata.Md5Hash;
                Debug.Log("Image uploaded");
            }
        });
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    public void EditServiceButton(string id, string title, string desc, string location_id, string price, string duration, string public_status, List<string> workerids, List<string> types, string ImgUrl, string ImgFormat)
    {
        StartCoroutine(EditService(id, title, desc, location_id, price, duration, public_status, workerids, types, ImgUrl, ImgFormat));
    }
    private IEnumerator EditService(string id, string title, string desc, string location_id, string price, string duration, string public_status, List<string> workerids, List<string> types, string ImgUrl, string ImgFormat)
    {
        List<string> allFirmWorkers = new List<string>();
        var DBTask4 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);

        DataSnapshot snapshot = DBTask4.Result;
        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
        {
            allFirmWorkers.Add(childSnapshot.Key);
        }

        foreach (string worker in allFirmWorkers)
        {
            var DBTask6 = DBreference.Child("users").Child("ServicesList").Child(id).RemoveValueAsync();
            yield return new WaitUntil(predicate: () => DBTask6.IsCompleted);
        }

        var DBTask2 = DBreference.Child("services").Child(id).Child("Workers").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        var DBTask3 = DBreference.Child("services").Child(id).Child("Types").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);

        var DBTask = DBreference.Child("services").Child(id).RunTransaction(Data =>
        {

            Data.Child("FirmId").Value = User.UserId;
            Data.Child("Title").Value = title;
            Data.Child("Desc").Value = desc;
            Data.Child("LocationId").Value = location_id;
            Data.Child("Price").Value = price;
            Data.Child("Duration").Value = duration;
            Data.Child("PublicStatus").Value = public_status;

            foreach (string workerid in workerids)
            {
                Data.Child("Workers").Child(workerid).Value = "";
                DBreference.Child("users").Child(workerid).Child("ServicesList").Child(id).SetValueAsync("");
            }

            foreach (string type in types)
            {
                Data.Child("Types").Child(type).Value = "";
            }

            return TransactionResult.Success(Data);
        });
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        StorageReference NewRef = storage_ref.Child("ServicesImages/" + id + ".jpg");

        var DBTask1 = NewRef.PutFileAsync(ImgUrl).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                StorageMetadata metadata = task.Result;
                string md5Hash = metadata.Md5Hash;
                Debug.Log("Image uploaded");
            }
        });
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    private IEnumerator LoadFirmWorkersData()
    {
        var DBTask = DBreference.Child("users").OrderByChild("type").EqualTo(1).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        DataSnapshot snapshot2 = DBTask2.Result;

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in FirmWorkerListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (snapshot2.Child(childSnapshot.Key).Exists)
                {
                    string name = childSnapshot.Child("name").Value.ToString();
                    string lastname = childSnapshot.Child("lastname").Value.ToString();
                    int rating = int.Parse(childSnapshot.Child("rating").Value.ToString());
                    string id = childSnapshot.Key.ToString();

                    GameObject FirmWorkerListElement = Instantiate(FirmWorkerElement, FirmWorkerListContent);
                    FirmWorkerListElement.GetComponent<WorkerElement>().NewElement(name, lastname, rating, id);
                }
            }
        }
    }

    private IEnumerator LoadFirmWorkersToSelect()
    {
        var DBTask = DBreference.Child("users").OrderByChild("type").EqualTo(1).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        DataSnapshot snapshot2 = DBTask2.Result;

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in WorkerSelectListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in WorkerSelectListContentEdit.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (snapshot2.Child(childSnapshot.Key).Exists)
                {
                    string name = childSnapshot.Child("name").Value.ToString();
                    string lastname = childSnapshot.Child("lastname").Value.ToString();
                    int rating = int.Parse(childSnapshot.Child("rating").Value.ToString());
                    string id = childSnapshot.Key.ToString();

                    GameObject FirmWorkerSelectListElement = Instantiate(SelectWorkerPrefab, WorkerSelectListContent);
                    FirmWorkerSelectListElement.GetComponent<WorkerSelectElement>().NewElement(id, name, lastname, rating);

                    GameObject FirmWorkerSelectListElementEdit = Instantiate(EditSelectWorkerPrefab, WorkerSelectListContentEdit);
                    FirmWorkerSelectListElementEdit.GetComponent<WorkerSelectElement>().NewElement(id, name, lastname, rating);
                }
            }
        }
    }

    public void FindLocationById(string locationid, GameObject edit)
    {
        StartCoroutine(LocationById(locationid, edit));
    }

    private IEnumerator LocationById(string locationid, GameObject edit)
    {
        var DBTask4 = DBreference.Child("users").Child(User.UserId).Child("LocationList").Child(locationid).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);

        if (DBTask4.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask4.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask4.Result;
            string adress = snapshot.Child("Adress").Value.ToString();
            string city = snapshot.Child("City").Value.ToString();
            string location = city + ", " + adress;

            edit.GetComponent<FirmServiceElement>().location = location;
        }
    }
    private IEnumerator LoadFirmServiceData()
    {
        List<string> ServicesId = new List<string>();
        List<string> workersid = new List<string>();

        string workers = "";
        string location = "";

        var DBTask = DBreference.Child("users").Child(User.UserId).Child("ServicesList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                ServicesId.Add(childSnapshot.Key);
            }
        }

        var DBTask2 = DBreference.Child("services").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        if (DBTask2.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask2.Exception}");
        }

        else
        {
            DataSnapshot snapshot = DBTask2.Result;
            foreach (Transform child in FirmServicesListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                int status = int.Parse(childSnapshot.Child("Status").Value.ToString());
                if (ServicesId.Contains(childSnapshot.Key) && status == 0)
                {
                    foreach (DataSnapshot childSnapshot3 in childSnapshot.Child("Workers").Children.Reverse<DataSnapshot>())
                    {
                        if (!workersid.Contains(childSnapshot3.Key)) workersid.Add(childSnapshot3.Key);
                    }
                }
            }
        }

        var DBTask3 = DBreference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);

        if (DBTask3.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask3.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask3.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (workersid.Contains(childSnapshot.Key))
                {
                    string name = childSnapshot.Child("name").Value.ToString();
                    string lastname = childSnapshot.Child("lastname").Value.ToString();
                    string fullname = name + " " + lastname + ", ";
                    workers += fullname;
                }
            }
        }

        var DBTask5 = DBreference.Child("services").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask5.IsCompleted);

        if (DBTask5.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask5.Exception}");
        }

        else
        {
            DataSnapshot snapshot = DBTask5.Result;
            foreach (Transform child in FirmServicesListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                int status = int.Parse(childSnapshot.Child("Status").Value.ToString());
                if (ServicesId.Contains(childSnapshot.Key) && status == 0)
                {
                    string id = childSnapshot.Key.ToString();
                    string title = childSnapshot.Child("Title").Value.ToString();
                    int publicStatus = int.Parse(childSnapshot.Child("PublicStatus").Value.ToString());
                    string desc = childSnapshot.Child("Desc").Value.ToString();
                    string locationid = childSnapshot.Child("LocationId").Value.ToString();
                    float price = float.Parse(childSnapshot.Child("Price").Value.ToString());
                    int duration = int.Parse(childSnapshot.Child("Duration").Value.ToString());
                    int reviews = int.Parse(childSnapshot.Child("Reviews").Value.ToString());
                    float rating = int.Parse(childSnapshot.Child("Rating").Value.ToString());
                    List<string> types = new List<string>();
                    Sprite sprite = null;

                    foreach (DataSnapshot childSnapshot2 in childSnapshot.Child("Types").Children.Reverse<DataSnapshot>())
                    {
                        if (!types.Contains(childSnapshot2.Key)) types.Add(childSnapshot2.Key);
                    }
                    foreach (DataSnapshot childSnapshot3 in childSnapshot.Child("Workers").Children.Reverse<DataSnapshot>())
                    {
                        if (!workersid.Contains(childSnapshot3.Key)) workersid.Add(childSnapshot3.Key);
                    }

                    storage_ref.Child("ServicesImages").Child(childSnapshot.Key + ".jpg").GetBytesAsync(1024 * 1024).ContinueWithOnMainThread(task =>
                    {
                        if (task.IsCompleted)
                        {
                            var texture = new Texture2D(2, 2);
                            byte[] fileContent = task.Result;

                            texture.LoadImage(fileContent);
                            var newRect = new Rect(0.0f, 0.0f, texture.width, texture.height);
                            sprite = Sprite.Create(texture, newRect, Vector2.zero);

                            Debug.Log("FINISH DOWNLOAD");
                        }
                        else
                        {
                            print("DOWNLOAD WRONG");
                        }

                        workers = workers.Substring(0, workers.Length - 1);

                        GameObject FirmServiceListElement = Instantiate(FirmServicesPrefab, FirmServicesListContent);
                        FirmServiceListElement.GetComponent<FirmServiceElement>().NewElement(id, title, publicStatus, desc, locationid, location, price, duration, reviews, rating, types, workersid, workers, sprite);
                    });
                }
            }
        }
    }

    private IEnumerator LoadFirmLocationData()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("LocationList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in FirmLocationListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string city = childSnapshot.Child("City").Value.ToString();
                string address = childSnapshot.Child("Adress").Value.ToString();
                string id = childSnapshot.Key.ToString();

                GameObject FirmLocationListElement = Instantiate(FirmLocationElement, FirmLocationListContent);
                FirmLocationListElement.GetComponent<LocationElement>().NewElement(city, address, id);
            }
        }
    }

    private IEnumerator LoadFirmLocationToSelect()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("LocationList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in LocationSelectContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in LocationSelectContentEdit.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string city = childSnapshot.Child("City").Value.ToString();
                string address = childSnapshot.Child("Adress").Value.ToString();
                string id = childSnapshot.Key.ToString();

                GameObject FirmLocationListElement = Instantiate(FirmLocationElement, LocationSelectContent);
                FirmLocationListElement.GetComponent<LocationElement>().NewElement(city, address, id);

                GameObject FirmLocationListElementEdit = Instantiate(FirmLocationElement, LocationSelectContentEdit);
                FirmLocationListElementEdit.GetComponent<LocationElement>().NewElement(city, address, id);
            }
        }
    }
    private IEnumerator LoadScoreboardData()
    {
        var DBTask = DBreference.Child("users").OrderByChild("kills").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int kills = int.Parse(childSnapshot.Child("kills").Value.ToString());
                int deaths = int.Parse(childSnapshot.Child("deaths").Value.ToString());
                int xp = int.Parse(childSnapshot.Child("xp").Value.ToString());

                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, deaths, xp);
            }
            UIManager.instance.ScoreboardScreen();
        }
    }
    public void LoadNotifications()
    {
        StartCoroutine(LoadFirmNotifications());
    }

    private IEnumerator LoadFirmNotifications()
    {
        string user_id = User.UserId.ToString();
        string worker_id = "";
        string worker_name = "";
        string request_id = "";

        var DBTask4 = DBreference.Child("requests").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);

        if (DBTask4.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask4.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask4.Result;

            foreach (Transform child in NotificationListContent.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("Fields Cleared...");

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (childSnapshot.Child("ToFirm").Value.ToString() == User.UserId.ToString())
                {
                    Debug.Log("Adding Element...");

                    worker_id = childSnapshot.Child("FromWorker").Value.ToString();

                    worker_name = childSnapshot.Child("Worker").Value.ToString();

                    request_id = childSnapshot.Key as String;
                    // Debug.Log("ky " + key);
                    GameObject NotifyElement = Instantiate(NotificationElement, NotificationListContent);
                    NotifyElement.GetComponent<FirmNotificationElement>().NewElement(worker_name, worker_id, user_id, request_id);
                }
            }
        }
    }
    public void AcceptReqestButton(string firmid, string workerid, string requestid)
    {
        StartCoroutine(AcceptJobRequest(firmid, workerid, requestid));
    }
    private IEnumerator AcceptJobRequest(string firmid, string workerid, string requestid)
    {
        var DBTask1 = DBreference.Child("users").Child(firmid).Child("WorkerList").Child(workerid).SetValueAsync("");
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);
        //Debug.Log("Added to WorkerList...");

        var DBTask2 = DBreference.Child("users").Child(workerid).Child("FirmList").Child(firmid).SetValueAsync("");
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        //Debug.Log("Added to FirmList...");

        var DBTask3 = DBreference.Child("requests").Child(requestid).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);
        //Debug.Log("Removes Request...");
    }

    public void DeclineReqestButton(string requestid)
    {
        StartCoroutine(DeclineJobRequest(requestid));
    }

    private IEnumerator DeclineJobRequest(string requestid)
    {
        var DBTask3 = DBreference.Child("requests").Child(requestid).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);
        //Debug.Log("Removes Request...");
    }

    public void EditLocation(string loc_id, string city, string adress)
    {
        StartCoroutine(EditFirmLocation(loc_id, city, adress));
    }

    private IEnumerator EditFirmLocation(string loc_id, string city, string adress)
    {
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("LocationList").Child(loc_id).RunTransaction(Data =>
        {
            Data.Child("City").Value = city;
            Data.Child("Adress").Value = adress;
            return TransactionResult.Success(Data);
        });
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        EditLocationPanel.SetActive(false);
        LoadFirmLocationDataButton();
    }

    public void DeleteLocation(string loc_id)
    {
        StartCoroutine(DeleteFirmLocation(loc_id));
    }

    private IEnumerator DeleteFirmLocation(string loc_id)
    {
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("LocationList").Child(loc_id).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        LoadFirmLocationDataButton();
    }

    public void DeleteService(string service_id)
    {
        StartCoroutine(DeleteFirmService(service_id));
    }

    private IEnumerator DeleteFirmService(string service_id)
    {
        var DBTask1 = DBreference.Child("services").Child(service_id).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        LoadFirmLocationDataButton();
    }

    public void DeleteWorkerFromFirm(string worker_id)
    {
        StartCoroutine(DeleteWorkerFromById(worker_id));
    }
    private IEnumerator DeleteWorkerFromById(string worker_id)
    {
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("WorkerList").Child(worker_id).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        var DBTask2 = DBreference.Child("users").Child(worker_id).Child("FirmList").Child(User.UserId).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        LoadFirmWorkersData();
    }

    public void LoadOrders()
    {
        StartCoroutine(LoadFirmOrders());
    }

    private IEnumerator LoadFirmOrders()
    {
        var DBTask4 = DBreference.Child("services").OrderByChild("Status").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);

        if (DBTask4.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask4.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask4.Result;

            foreach (Transform child in OrderListContent.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("Fields Cleared...");

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string statusString = childSnapshot.Child("Status").Value.ToString();
                int statusInt = int.Parse(statusString);

                if (statusInt > 0)
                {
                    string adress = childSnapshot.Child("Adress").Value.ToString();
                    string client_id = childSnapshot.Child("ClientId").Value.ToString();
                    string client_phone = childSnapshot.Child("ClientPhone").Value.ToString();
                    string Date = childSnapshot.Child("Date").Value.ToString();
                    string desc = childSnapshot.Child("Desc").Value.ToString();
                    string duration = childSnapshot.Child("Duration").Value.ToString();
                    string firm_id = childSnapshot.Child("FirmId").Value.ToString();
                    string loc_id = childSnapshot.Child("LocationId").Value.ToString();
                    string OrderDate = childSnapshot.Child("OrderDate").Value.ToString();
                    string paymentStatus = childSnapshot.Child("PaymentStatus").Value.ToString();
                    string paymentType = childSnapshot.Child("PaymentType").Value.ToString();
                    string price = childSnapshot.Child("Price").Value.ToString();
                    string status = childSnapshot.Child("Status").Value.ToString();
                    string title = childSnapshot.Child("Title").Value.ToString();
                    string worker_id = childSnapshot.Child("WorkerId").Value.ToString();
                    string workerName = childSnapshot.Child("workerName").Value.ToString();
                    string id = childSnapshot.Key;
                    Debug.Log("Adding Element...");

                    // Debug.Log("ky " + key);
                    GameObject FirmOrderElement = Instantiate(OrderElement, OrderListContent);
                    FirmOrderElement.GetComponent<FirmOrderElement>().NewElement(id, adress, client_id, client_phone, Date, desc, duration, firm_id, loc_id, OrderDate,
                        paymentStatus, paymentType, price, status, title, worker_id, workerName);
                }
            }
        }
    }

    public void ChangeOrderStatus(string order_id, string status)
    {
        StartCoroutine(ChangeOrderStatusById(order_id, status));
    }

    private IEnumerator ChangeOrderStatusById(string worker_id, string status)
    {
        var DBTask1 = DBreference.Child("services").Child(worker_id).Child("Status").SetValueAsync(status);
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);
        LoadOrders();
    }
}
