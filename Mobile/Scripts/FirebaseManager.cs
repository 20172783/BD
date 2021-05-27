using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class FirebaseManager : MonoBehaviour
{
    [Header("DB")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    [Header("Others")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    public GameObject SendCodePanel;
    public GameObject EnterCodePanel;

    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_InputField phoneNumberField;
    public TMP_InputField asm_Kodo_Field;
    public TMP_InputField name_Field;
    public TMP_InputField lastname_Field;
    public TMP_Text warningRegisterText;

    public TMP_InputField emailRegField;
    public InputField phoneField;
    public TMP_InputField names_Field;
    public TMP_InputField lastnames_Field;
    public TMP_Text warningRegText;

    public TMP_InputField usernameField;
    public TMP_InputField xpField;
    public TMP_InputField killsField;
    public TMP_InputField deathsField;
    public GameObject scoreElement;
    public Transform scoreboardContent;

    public GameObject NotificationElement;
    public Transform NotificationListContent;

    public GameObject PassiveServiceElement;
    public Transform PassiveServiceListContent;

    public GameObject searchFirmElement;
    public Transform searchFirmListContent;
    public GameObject requestMessage;

    [SerializeField] InputField phoneNumber;
    [SerializeField] InputField CountryCode;
    private uint phoneAuthTimeoutMs = 60 * 1000;
    PhoneAuthProvider provider;
    private string VerificationId;[SerializeField] Text debug;
    [SerializeField] InputField otp;
    private bool FirebaseInitializeDone = false;
    public string timeNow;
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

    void Start()
    {
        TrilleonAutomation.AutomationMaster.Initialize();
        UIManager.instance.LoadingScreen();
        StartCoroutine(WaitForFirebaseInitialize());
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

            if (type == 1)
            {
                warningLoginText.text = "";
                Debug.LogFormat("User signed in successfully as WORKER: {0} ({1})", User.DisplayName, User.Email);
                confirmLoginText.text = "Logged In";
                UIManager.instance.WorkerMainScreen();
                StartCoroutine(LoadUserData());
            }
            else if (type == 0)
            {
                warningLoginText.text = "";
                Debug.LogFormat("User signed in successfully as CLIENT: {0} ({1})", User.DisplayName, User.Email);
                confirmLoginText.text = "Logged In";
                UIManager.instance.ClientMainScreen();
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
            //  Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "";
            StartCoroutine(CheckProfileType());
            usernameField.text = User.DisplayName;
            confirmLoginText.text = "";
            warningLoginText.text = "";
            ClearLoginFeilds();
            ClearRegisterFeilds();
        }
        else
        {
            Debug.LogFormat("User Not Signed In");
            SendCodePanel.SetActive(true);
            EnterCodePanel.SetActive(false);
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

    public void PhoneLogin()
    {
        StartCoroutine(verify_otp());
    }
    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    public void RegisterButton()
    {
        StartCoroutine(Register_Worker(emailRegisterField.text, name_Field.text, lastname_Field.text, passwordRegisterField.text, usernameRegisterField.text, phoneNumberField.text, asm_Kodo_Field.text));
    }
    public void ClientRegisterButton()
    {
        StartCoroutine(Register_Client(emailRegField.text, names_Field.text, lastnames_Field.text, phoneField.text));
    }

    public void SignOutButton()
    {
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFeilds();
        ClearLoginFeilds();
    }
    public void SaveDataButton()
    {
        StartCoroutine(UpdateUsernameAuth(usernameField.text));
        StartCoroutine(UpdateUsernameDatabase(usernameField.text));
    }

    private void CreateWorkerData(string username, string name, string lastname, string email, string phone, string asm_kodas, string gender, string birthdate)
    {
        //StartCoroutine(UpdateUsernameAuth(usernameField.text));
        StartCoroutine(UpdateUsernameDatabase(username));
        StartCoroutine(UpdateEmail(email));
        StartCoroutine(UpdatePhone(phone));
        StartCoroutine(UpdateAsmKodas(asm_kodas));
        StartCoroutine(UpdateFirstName(name));
        StartCoroutine(UpdateLastName(lastname));
        StartCoroutine(UpdateGender(gender));
        UpdateBirthdate(birthdate);

        //default
        StartCoroutine(UpdateRating(0));
        StartCoroutine(UpdateVerified(0));
        StartCoroutine(UpdateAccepted(0));
        StartCoroutine(UpdateStatus(0));
        StartCoroutine(UpdateType(1)); //1 - worker 0 - client, 2 - firm

        //default lists
        List<string> empty = new List<string>();
        //  empty.Add("empty");
        StartCoroutine(UpdateServicesList(empty));
        StartCoroutine(UpdateFirmList(empty));
    }

    private void CreateClientData(string email, string name, string lastname, string phone)
    {
        //StartCoroutine(UpdateUsernameAuth(usernameField.text));
        StartCoroutine(UpdateEmail(email));
        StartCoroutine(UpdatePhone(phone));
        StartCoroutine(UpdateFirstName(name));
        StartCoroutine(UpdateLastName(lastname));

        //default
        StartCoroutine(UpdateRating(0));
        StartCoroutine(UpdateType(0)); //0 - client, 1 - worker, 2 - firm

        //default lists
        List<string> empty = new List<string>();
        // empty.Add("empty");
        StartCoroutine(UpdateServicesList(empty));
    }
    public void LoadNotifications()
    {
        StartCoroutine(LoadWorkerNotifications());
    }
    public void AcceptReqestButton(string firmid, string workerid, string requestid)
    {
        StartCoroutine(AcceptJobRequest(firmid, workerid, requestid));
    }
    public void DeclineReqestButton(string requestid)
    {
        StartCoroutine(DeclineJobRequest(requestid));
    }

    public void phone_login()
    {
        //Debug.Log("INPUT: "+ CountryCode.text + " " + phoneNumber.text + ". ");
        provider = PhoneAuthProvider.GetInstance(auth);
        provider.VerifyPhoneNumber(phoneNumber.text, phoneAuthTimeoutMs, null,
          verificationCompleted: (credential) =>
          {
              // Auto-sms-retrieval or instant validation has succeeded (Android only).
              // There is no need to input the verification code.
              // `credential` can be used instead of calling GetCredential().
          },
          verificationFailed: (error) =>
          {
              // The verification code was not sent.
              // `error` contains a human readable explanation of the problem.
          },
          codeSent: (id, token) =>
          {
              VerificationId = id;
              debug.text = "Kodas išsiųstas";

              SendCodePanel.SetActive(false);
              EnterCodePanel.SetActive(true);
              // Verification code was successfully sent via SMS.
              // `id` contains the verification id that will need to passed in with
              // the code from the user when calling GetCredential().
              // `token` can be used if the user requests the code be sent again, to
              // tie the two requests together.
          },
          codeAutoRetrievalTimeOut: (id) =>
          {
              // Called when the auto-sms-retrieval has timed out, based on the given
              // timeout parameter.
              // `id` contains the verification id of the request that timed out.
          });
    }

    private IEnumerator verify_otp()
    {
        Credential credential = provider.GetCredential(VerificationId, otp.text);
        var PhoneLoginTask = auth.SignInWithCredentialAsync(credential);
        yield return new WaitUntil(predicate: () => PhoneLoginTask.IsCompleted);

        User = PhoneLoginTask.Result;
        debug.text = ("Sėkmingai prisijungta");
        debug.text = ("Tel nr: " + User.PhoneNumber);
        debug.text = ("Numerio teikėjas: " + User.ProviderId);

        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        else if (DBTask.Result.Value == null) UIManager.instance.ClientRegisterScreen();
        else
        {
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Prisijungėte";

            StartCoroutine(LoadUserData());

            usernameField.text = User.DisplayName;
            UIManager.instance.ClientMainScreen(); // Change to user data UI
            confirmLoginText.text = "";
            ClearLoginFeilds();
            ClearRegisterFeilds();

            SendCodePanel.SetActive(true);
            EnterCodePanel.SetActive(false);
        }
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

            string message = "Nepavyko prisijungti";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Trūksta pašto";
                    break;
                case AuthError.MissingPassword:
                    message = "Trūksta slaptažodžio";
                    break;
                case AuthError.WrongPassword:
                    message = "Neteisingas slaptažodis";
                    break;
                case AuthError.InvalidEmail:
                    message = "Neteisingas paštas";
                    break;
                case AuthError.UserNotFound:
                    message = "Profiliu su tokiu paštu nėra";
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

            usernameField.text = User.DisplayName;
            warningLoginText.text = "";
            confirmLoginText.text = "";
            ClearLoginFeilds();
            ClearRegisterFeilds();
        }
    }

    private IEnumerator Register_Worker(string _email, string _name, string _lastname, string _password, string _username, string _phone, string _asm_kodas)
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
                        //GenderDetection
                        string _gender = "";
                        string _age = ""; //amzius pvz (XX, XXI)
                        int firstNumber = int.Parse(_asm_kodas.Substring(0, 1));
                        if (firstNumber == 1)
                        {
                            _gender = "M";
                            _age = "18";
                        }
                        else if (firstNumber == 2)
                        {
                            _gender = "F";
                            _age = "18";
                        }
                        else if (firstNumber == 3)
                        {
                            _gender = "M";
                            _age = "19";
                        }
                        else if (firstNumber == 4)
                        {
                            _gender = "F";
                            _age = "19";
                        }
                        else if (firstNumber == 5)
                        {
                            _gender = "M"; //Male
                            _age = "20";
                        }
                        else if (firstNumber == 6)
                        {
                            _gender = "F"; //Female
                            _age = "20";
                        }

                        //BirthDateDetection
                        //Year
                        string _birthdate_year = _age + _asm_kodas.Substring(1, 2);
                        //Month
                        string _birthdate_month = _asm_kodas.Substring(3, 2);
                        //Day
                        string _birthdate_day = _asm_kodas.Substring(5, 2);
                        //Full
                        string _birthdate = _birthdate_year + "-" + _birthdate_month + "-" + _birthdate_day;

                        //CreateDatabaseStuff
                        CreateWorkerData(_username, _name, _lastname, _email, _phone, _asm_kodas, _gender, _birthdate);
                        //Now return to login screen
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                        ClearRegisterFeilds();
                        ClearLoginFeilds();
                    }
                }
            }
        }
    }

    private IEnumerator Register_Client(string _email, string _name, string _lastname, string _phone)
    {
        if (_name == "")
        {
            warningRegisterText.text = "Missing Name";
        }
        else
        {
            if (User != null)
            {
                UserProfile profile = new UserProfile { DisplayName = _name };
                var ProfileTask = User.UpdateUserProfileAsync(profile);
                yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                if (ProfileTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                    warningRegisterText.text = "Username Set Failed!";
                }
                else
                {
                    CreateClientData(_email, _name, _lastname, _phone);
                    Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
                    warningLoginText.text = "";
                    confirmLoginText.text = "Logged In";

                    StartCoroutine(LoadUserData());

                    usernameField.text = User.DisplayName;
                    UIManager.instance.ClientMainScreen(); // Change to user data UI
                    confirmLoginText.text = "";
                    warningLoginText.text = "";
                    ClearLoginFeilds();
                    ClearRegisterFeilds();
                }
            }
        }
    }
    private IEnumerator UpdateUsernameAuth(string _username)
    {
        UserProfile profile = new UserProfile { DisplayName = _username };
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
    }

    private IEnumerator UpdateEmailAuth(string _email)
    {
        var ProfileTask = User.UpdateEmailAsync(_email);
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
    }

    private IEnumerator UpdatePassAuth(string _pass)
    {
        var ProfileTask = User.UpdatePasswordAsync(_pass);
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
    }

    private IEnumerator UpdateBirthdate(string _birthdate)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("birthdate").SetValueAsync(_birthdate);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }
    private IEnumerator UpdateGender(string _gender)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("gender").SetValueAsync(_gender);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }
    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateEmail(string email)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("email").SetValueAsync(email);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdatePhone(string phone)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("phone").SetValueAsync(phone);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateAsmKodas(string asm_kodas)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("asm_kodas").SetValueAsync(asm_kodas);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateFirstName(string name)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("name").SetValueAsync(name);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateLastName(string lastname)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("lastname").SetValueAsync(lastname);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateRating(int rating)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("rating").SetValueAsync(rating);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateVerified(int verified)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("verified").SetValueAsync(verified);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateAccepted(int accepted)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("accepted").SetValueAsync(accepted);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateStatus(int status)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("status").SetValueAsync(status);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateType(int type)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("type").SetValueAsync(type);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    }

    private IEnumerator UpdateServicesList(List<string> services)
    {
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("ServicesList").RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        if (DBTask1.Exception != null) Debug.LogWarning(message: $"Failed to Delete task with {DBTask1.Exception}");

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

        if (DBTask1.Exception != null) Debug.LogWarning(message: $"Failed to Delete task with {DBTask1.Exception}");

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
    private IEnumerator LoadUserData()
    {
        if (User != null)
        {
            var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            else if (DBTask.Result.Value == null)
            {
                //No data exists yet
            }
            else
            {
                DataSnapshot snapshot = DBTask.Result;
            }
        }
    }

    private IEnumerator AcceptJobRequest(string firmid, string workerid, string requestid)
    {
        var DBTask1 = DBreference.Child("users").Child(firmid).Child("WorkerList").Child(workerid).SetValueAsync("");
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        var DBTask2 = DBreference.Child("users").Child(workerid).Child("FirmList").Child(firmid).SetValueAsync("");
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        var DBTask3 = DBreference.Child("requests").Child(requestid).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);
    }
    private IEnumerator DeclineJobRequest(string requestid)
    {
        var DBTask3 = DBreference.Child("requests").Child(requestid).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);
        Debug.Log("Removes Request...");
    }
    private IEnumerator LoadWorkerNotifications()
    {
        string user_id = User.UserId.ToString();
        string firm_id = "";
        string fname = "";
        string request_id = "";

        var DBTask4 = DBreference.Child("requests").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);

        if (DBTask4.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask4.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask4.Result;

            foreach (Transform child in NotificationListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (childSnapshot.Child("ToWorker").Value.ToString() == User.UserId.ToString())
                {
                    firm_id = childSnapshot.Child("FromFirm").Value.ToString();
                    fname = childSnapshot.Child("Firm").Value.ToString();
                    request_id = childSnapshot.Key as String;
                    // Debug.Log("ky " + key);
                    GameObject NotifyElement = Instantiate(NotificationElement, NotificationListContent);
                    NotifyElement.GetComponent<WorkNotificationElement>().NewElement(fname, firm_id, user_id, request_id);
                }
            }
        }
    }

    List<string> Categories = new List<string>();
    private IEnumerator GetCategoriesFromDB()
    {
        var DBTask = DBreference.Child("categories").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask.Exception}");
        else
        {
            Categories.Clear();
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                Categories.Add(childSnapshot.Key);
            }
        }
    }

    List<string> Subategories = new List<string>();
    private IEnumerator GetSubCategoriesFromDB(string Cagetory)
    {
        var DBTask = DBreference.Child("categories").Child(Cagetory).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask.Exception}");
        else
        {
            Subategories.Clear();
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                Subategories.Add(childSnapshot.Key);
            }
        }
    }

    public void GetFirmWorkersNameById(GameObject serviceInfo, List<string>  workerids)
    {
        StartCoroutine(GetFirmWorkersNameByIds(serviceInfo, workerids));
    }
    private IEnumerator GetFirmWorkersNameByIds(GameObject serviceInfo, List<string> workerids)
    {
        List<string> workerNames = new List<string>();
        List<string> workerId = new List<string>();
        var DBTask = DBreference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if(workerids.Contains(childSnapshot.Key))
                {
                    string name = childSnapshot.Child("name").Value.ToString();
                    string lastname = childSnapshot.Child("lastname").Value.ToString();
                    string fullname = name + " " + lastname;
                    workerNames.Add(fullname);
                }
            }
            serviceInfo.GetComponent<OrderServiceInfo>().workersName = workerNames;
            serviceInfo.GetComponent<OrderServiceInfo>().updateWorkersNames();
        }
    }
    public void GetFirmNameById(GameObject serviceInfo, string FirmId)
    {
        StartCoroutine(GetFirmNameByFirmId(serviceInfo, FirmId));
    }
    private IEnumerator GetFirmNameByFirmId(GameObject serviceInfo, string FirmId)
    {
        var DBTask = DBreference.Child("users").Child(FirmId).Child("username").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            serviceInfo.GetComponent<OrderServiceInfo>().firmName = snapshot.Value.ToString();
            serviceInfo.GetComponent<OrderServiceInfo>().updateFirmName();
        }
    }
    public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
    {
        for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            yield return day;
    }
    public void GetWorkerDates(GameObject workerSelect)
    {
        StartCoroutine(GetWorkerDatesById(workerSelect));
    }
    private IEnumerator GetWorkerDatesById(GameObject workerSelect)
    {
        List<string> shedule = new List<string>();
        List<DateTime> BusyDates = new List<DateTime>();
        string FirmId = workerSelect.GetComponent<OrderWorkerElement>().firmid;
        string workerids = workerSelect.GetComponent<OrderWorkerElement>().workerid;

        var DBTask = DBreference.Child("users").Child(FirmId).Child("WorkerList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (workerids.Contains(childSnapshot.Key))
                {
                    foreach (DataSnapshot childSnapshot2 in childSnapshot.Child("NotWorking").Children.Reverse<DataSnapshot>())
                    {
                        string firstDate = childSnapshot2.Child("Date").Value.ToString().Replace("-", "/");
                        string lastDate="";
                        string hour="";
                        DateTime firstdate, lastdate;
                        if (childSnapshot2.Child("Hour").Value.ToString().Contains("-")) //Data
                        {
                            lastDate = childSnapshot2.Child("Hour").Value.ToString().Replace("-", "/");
                            firstdate = Convert.ToDateTime(firstDate);
                            lastdate = Convert.ToDateTime(lastDate);

                            foreach (DateTime day in EachDay(firstdate, lastdate))
                            {
                                BusyDates.Add(day);
                            }
                        }
                        else //valanda
                        {
                            hour = childSnapshot2.Child("Hour").Value.ToString();
                            firstdate = Convert.ToDateTime(firstDate);
                            firstdate.AddHours(double.Parse(hour));
                            BusyDates.Add(firstdate);
                        }
                    }

                    foreach (DataSnapshot childSnapshot2 in childSnapshot.Child("Shedule").Children.Reverse<DataSnapshot>())
                    {
                        // "0,13,14,15,.."
                        string sheduleDay = childSnapshot2.Key;
                        foreach (DataSnapshot childSnapshot3 in childSnapshot2.Children.Reverse<DataSnapshot>())
                        {
                            if (childSnapshot3.Key[0].ToString() == "0") sheduleDay += "," + childSnapshot3.Key[1].ToString(); // panaikinam 09 -> 9
                            else sheduleDay += "," + childSnapshot3.Key;
                        }
                        shedule.Add(sheduleDay);
                    }
                }
            }
            workerSelect.GetComponent<OrderWorkerElement>().BusyDates = BusyDates;
            workerSelect.GetComponent<OrderWorkerElement>().shedule = shedule;
            workerSelect.GetComponent<OrderWorkerElement>().sendDataToTimeSelect();
        }
    }
    public void GetWorkerShedule(GameObject Shedule)
    {
        StartCoroutine(GetWorkerSheduleById(Shedule));
    }
    private IEnumerator GetWorkerSheduleById(GameObject Shedule)
    {
        List<string> shedule = new List<string>();
        List<DateTime> BusyDates = new List<DateTime>();
        List<string> FirmIds = new List<string>();

        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                foreach (DataSnapshot childSnapshot2 in childSnapshot.Child("FirmList").Children.Reverse<DataSnapshot>())
                {
                    FirmIds.Add(childSnapshot2.Key);
                }
            }
        }

        foreach (string firmid in FirmIds)
        {
            var DBTask1 = DBreference.Child("users").Child(firmid).Child("WorkerList").GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

            if (DBTask1.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask1.Exception}");
            else
            {
                DataSnapshot snapshot = DBTask1.Result;
                foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                {
                    if (User.UserId == childSnapshot.Key)
                    {
                        foreach (DataSnapshot childSnapshot2 in childSnapshot.Child("NotWorking").Children.Reverse<DataSnapshot>())
                        {
                            string firstDate = childSnapshot2.Child("Date").Value.ToString().Replace("-", "/");
                            string lastDate = "";
                            string hour = "";
                            DateTime firstdate, lastdate;
                            if (childSnapshot2.Child("Hour").Value.ToString().Contains("-")) //Data
                            {
                                lastDate = childSnapshot2.Child("Hour").Value.ToString().Replace("-", "/");
                                firstdate = Convert.ToDateTime(firstDate);
                                lastdate = Convert.ToDateTime(lastDate);

                                foreach (DateTime day in EachDay(firstdate, lastdate))
                                {
                                    if(!BusyDates.Contains(day)) BusyDates.Add(day);
                                }
                            }
                            else //valanda
                            {
                                hour = childSnapshot2.Child("Hour").Value.ToString();
                                firstdate = Convert.ToDateTime(firstDate);
                                firstdate.AddHours(double.Parse(hour));
                                if (!BusyDates.Contains(firstdate)) BusyDates.Add(firstdate);
                            }
                        }

                        foreach (DataSnapshot childSnapshot2 in childSnapshot.Child("Shedule").Children.Reverse<DataSnapshot>())
                        {
                            // "0,13,14,15,.."
                            string sheduleDay = childSnapshot2.Key;
                            foreach (DataSnapshot childSnapshot3 in childSnapshot2.Children.Reverse<DataSnapshot>())
                            {
                                if (childSnapshot3.Key[0].ToString() == "0") sheduleDay += "," + childSnapshot3.Key[1].ToString(); // panaikinam 09 -> 9
                                else sheduleDay += "," + childSnapshot3.Key;
                            }
                            if (!shedule.Contains(sheduleDay)) shedule.Add(sheduleDay);
                        }
                    }
                }
            }
        }
        Shedule.GetComponent<ShedulePanel>().BusyDates = BusyDates;
        Shedule.GetComponent<ShedulePanel>().shedule = shedule;
        Shedule.GetComponent<ShedulePanel>().spawnDays();
    }

    public void CreateClientOrder(string title, string desc, string adress, string price, string duration, string firmId, string workerId,
        string serviceId, DateTime date, int paymentType, int paymentStatus, string locationId, string discoutCode, string OrderForClientName, string clientPhone, string workerName)
    {
        StartCoroutine(CreateOrder(title, desc, adress, price, duration, firmId, workerId, serviceId, date, paymentType,
            paymentStatus, locationId, discoutCode, OrderForClientName, clientPhone, workerName));
    }

    private IEnumerator CreateOrder(string title, string desc, string adress, string price, string duration, string firmId, string workerId,
        string serviceId, DateTime date, int paymentType, int paymentStatus, string locationId, string discoutCode, string OrderForClientName, string clientPhone, string workerName)
    {
        string key = DBreference.Child("services").Push().Key;

        var DBTask = DBreference.Child("services").Child(key).RunTransaction(Data =>
        {
            Data.Child("ClientId").Value = User.UserId;
            Data.Child("FirmId").Value = firmId;
            Data.Child("WorkerId").Value = workerId;
            Data.Child("Title").Value = title;
            Data.Child("Desc").Value = desc;
            Data.Child("Adress").Value = adress;
            Data.Child("LocationId").Value = locationId;
            Data.Child("Price").Value = price;
            Data.Child("Duration").Value = duration;
            Data.Child("Date").Value = date.ToString("yyyy-MM-dd HH:mm");
            Data.Child("PaymentType").Value = paymentType.ToString();
            Data.Child("PaymentStatus").Value = paymentStatus.ToString();
            Data.Child("OrderDate").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            Data.Child("DiscoutCode").Value = discoutCode;
            Data.Child("OrderForClientName").Value = OrderForClientName;
            Data.Child("ClientPhone").Value = clientPhone;
            Data.Child("workerName").Value = workerName;
            DBreference.Child("users").Child(User.UserId).Child("ServicesList").Child(key).SetValueAsync("");
            DBreference.Child("users").Child(firmId).Child("ServicesList").Child(key).SetValueAsync("");
            DBreference.Child("users").Child(workerId).Child("ServicesList").Child(key).SetValueAsync("");

            Data.Child("Status").Value = 1;
            return TransactionResult.Success(Data);
        });
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    public void GetClientDataForOrder()
    {
        StartCoroutine(GetClientOrderData());
    }

    private IEnumerator GetClientOrderData()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            string name = snapshot.Child("name").Value.ToString();
            string phone = snapshot.Child("phone").Value.ToString();

            GameObject OrderInfoPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().OrderInfoPanel;
            OrderInfoPanel.GetComponent<OrderProfileInfo>().name = name;
            OrderInfoPanel.GetComponent<OrderProfileInfo>().phone = phone;
            OrderInfoPanel.GetComponent<OrderProfileInfo>().fillFields();
        }
    }

    public void GetLocById(GameObject service)
    {
        StartCoroutine(GetLocationById(service));
    }

    private IEnumerator GetLocationById(GameObject service)
    {
        string FirmId = service.GetComponent<ClientServiceElement>().firmid;
        string LocId = service.GetComponent<ClientServiceElement>().locationid;

        var DBTask = DBreference.Child("users").Child(FirmId).Child("LocationList").Child(LocId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        string city="";
        string adress="";

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to get task with {DBTask.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (childSnapshot.Key == "City") city = childSnapshot.Value.ToString();
                else adress = childSnapshot.Value.ToString();
            }

            service.GetComponent<ClientServiceElement>().adress += adress + ", " + city;
            service.GetComponent<ClientServiceElement>().Adress.text = service.GetComponent<ClientServiceElement>().adress;
        }
    }

    public void GetServiceByCat(string Cat)
    {
        StartCoroutine(LoadServicesByCategory(Cat));
    }

    private IEnumerator LoadServicesByCategory(string Cat)
    {
        var DBTask = DBreference.Child("services").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (Transform child in PassiveServiceListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                List<string> types = new List<string>();
                foreach (DataSnapshot childSnapshot2 in childSnapshot.Child("Types").Children.Reverse<DataSnapshot>())
                {
                    types.Add(childSnapshot2.Key);
                }
                int status = int.Parse(childSnapshot.Child("Status").Value.ToString());

                if (types.Contains(Cat) && status == 0)
                {
                    string id = childSnapshot.Key.ToString();
                    string title = childSnapshot.Child("Title").Value.ToString();
                    int PublicStatus = int.Parse(childSnapshot.Child("PublicStatus").Value.ToString());
                    string desc = childSnapshot.Child("Desc").Value.ToString();
                    string locationid = childSnapshot.Child("LocationId").Value.ToString();
                    string firmid = childSnapshot.Child("FirmId").Value.ToString();
                    float price = float.Parse(childSnapshot.Child("Price").Value.ToString());
                    int duration = int.Parse(childSnapshot.Child("Duration").Value.ToString());
                    int reviews = int.Parse(childSnapshot.Child("Reviews").Value.ToString());
                    float rating = int.Parse(childSnapshot.Child("Rating").Value.ToString());
                    
                    List<string> workersid = new List<string>();

                    foreach (DataSnapshot childSnapshot3 in childSnapshot.Child("Workers").Children.Reverse<DataSnapshot>())
                    {
                        workersid.Add(childSnapshot3.Key);
                    }

                    GameObject PassiveServiceListElement = Instantiate(PassiveServiceElement, PassiveServiceListContent);
                    PassiveServiceListElement.GetComponent<ClientServiceElement>().NewElement(id, title, PublicStatus, desc, locationid, firmid, price, duration, reviews, rating, types, workersid);
                }
            }
        }
    }

    public void SearchFirmsButton()
    {
        StartCoroutine(LoadSearchFirmsData());
    }
    private IEnumerator LoadSearchFirmsData()
    {
        var DBTask = DBreference.Child("users").OrderByChild("type").EqualTo(2).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("FirmList").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        DataSnapshot snapshot2 = DBTask2.Result;

        if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in searchFirmListContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (!snapshot2.Child(childSnapshot.Key).Exists)
                {
                    string name = childSnapshot.Child("name").Value.ToString();
                    int rating = int.Parse(childSnapshot.Child("rating").Value.ToString());
                    string id = childSnapshot.Key.ToString();

                    //Instantiate new scoreboard elements
                    GameObject FirmListElement = Instantiate(searchFirmElement, searchFirmListContent);
                    FirmListElement.GetComponent<SearchFirmElement>().NewElement(name, rating, id);
                }
            }
        }
    }

    public void RequestButton(string id)
    {
        StartCoroutine(SendRequestToFirm(id));
    }
    private IEnumerator SendRequestToFirm(string firmId)
    {
        if (User != null)
        {
            var DBTask3 = DBreference.Child("users").Child(User.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);
            DataSnapshot snapshot3 = DBTask3.Result;
            string FullName = snapshot3.Child("name").Value.ToString() + " " + snapshot3.Child("lastname").Value.ToString();
            //Debug.Log(" " + FullName);

            var DBTask2 = DBreference.Child("users").Child(firmId).GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
            DataSnapshot snapshot2 = DBTask2.Result;
            string FirmName = snapshot2.Child("name").Value.ToString();
            //Debug.Log(" " + FirmName);

            var DBTask = DBreference.Child("requests").OrderByChild("FromWorker_ToFirm").EqualTo(User.UserId + "_" + firmId).GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            else if (DBTask.Result.Value != null) Debug.Log("request already exists " + DBTask.Result.Value);
            else
            {
                string key = DBreference.Child("requests").Push().Key;

                var DBTask1 = DBreference.Child("requests").RunTransaction(Data =>
                {
                    Data.Child(key).Child("ToFirm").Value = firmId;
                    Data.Child(key).Child("Worker").Value = FullName;
                    Data.Child(key).Child("FromWorker").Value = User.UserId;
                    Data.Child(key).Child("Firm").Value = FirmName;
                    Data.Child(key).Child("Status").Value = 0;
                    Data.Child(key).Child("DateSent").Value = 0;
                    Data.Child(key).Child("FromWorker_ToFirm").Value = User.UserId + "_" + firmId;
                    Data.Child(key).Child("FromFirm_ToWorker").Value = "";
                    return TransactionResult.Success(Data);
                });
            }
        }
    }
}
