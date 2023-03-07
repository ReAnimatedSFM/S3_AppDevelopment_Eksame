using Firebase.Auth;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Firestore;
using System;

public class LoginManager : MonoBehaviour
{

    public static LoginManager Instance;

    /// <summary>
    /// Login GameObject
    /// </summary>
    [SerializeField] private GameObject loginPanel;

    /// <summary>
    /// Register GameObject
    /// </summary>
    [SerializeField] private GameObject registerPanel;

    [Space]
    [Header("Login Fields")]
    public InputField EmailLoginField;
    public InputField PasswordLoginField;

    [Space]
    [Header("Register Fields")]
    public InputField EmailRegisterField;
    public InputField UsernameRegisterField;
    public InputField PasswordRegisterField;
    public InputField ConfirmPasswordRegisterField;


    private string userId;


    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OpenLoginPanel();
    }

    #region Panels

    public void OpenLoginPanel()
    {

        loginPanel.SetActive(true);
        registerPanel.SetActive(false);

    }

    public void RegisterLoginPanel()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    #endregion

    #region Login

    /// <summary>
    /// Runs Coroutine LoginAsync after pressing login button
    /// </summary>
    public void Login()
    {
        StartCoroutine(LoginAsync(EmailLoginField.text, PasswordLoginField.text));
    }

    /// <summary>
    /// Runs Coroutine LoginAsync with a parameter for email and password. Mainly used to login after registering
    /// </summary>
    /// <param name="email">Email to login with</param>
    /// <param name="password">Password to login with</param>
    private void Login(string email, string password)
    {
        StartCoroutine(LoginAsync(email, password));
    }

    /// <summary>
    /// Logs in with firebase auth
    /// </summary>
    /// <param name="email">Email to login with</param>
    /// <param name="password">Password to login with</param>
    /// <returns></returns>
    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = FirebaseManager.Instance.auth.SignInWithEmailAndPasswordAsync(email, password);

        var userRepository = FirebaseManager.userRepository;

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;
            string failedMessage = "Login Failed! Because ";

            // Sets failedMessage depending on case
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }

            Debug.Log(failedMessage);
        }
        else
        {
            FirebaseManager.Instance.firebaseUser = loginTask.Result;

            CoroutineWithData cd = new CoroutineWithData(this, userRepository.GetEntityList("Users"));

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            yield return cd.coroutine;

            list = (List<Dictionary<string, object>>)cd.result;
            Debug.Log(list.Count.ToString());

            string id = userRepository.GetId(FirebaseManager.Instance.firebaseUser.UserId, list);

            if (id == FirebaseManager.Instance.firebaseUser.UserId)
            {
                StartCoroutine(FirebaseManager.Instance.SetUserData());

                FirebaseManager.Instance.SetFirebaseData();

                SceneManagement.Instance.GoToScene("Main", LoadSceneMode.Single);
                Debug.LogFormat("{0} You are successfully logged in", FirebaseManager.Instance.firebaseUser.DisplayName);
            }
        }
    }

    #endregion

    #region Registration

    /// <summary>
    /// Starts Coroutine RegisterAsync with parameters from register input fields
    /// </summary>
    public void Register()
    {
        StartCoroutine(RegisterAsync(UsernameRegisterField.text, EmailRegisterField.text, PasswordRegisterField.text,
            ConfirmPasswordRegisterField.text));
    }

    /// <summary>
    /// Adds user to Firestore using parameters for collection and document(UserId), and User entity. If successful, logs the user in using Email and Password
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="userId"></param>
    /// <param name="userToAdd"></param>
    /// <returns></returns>
    IEnumerator AddUserToDb(string collection, string userId, User userToAdd)
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = firestore.Collection(collection).Document(userId);

        var setTask = docRef.SetAsync(userToAdd);


        yield return new WaitUntil(() => setTask.IsCompleted);

        if (setTask.IsFaulted)
        {
            Debug.Log(setTask.IsFaulted);

            if (setTask.Exception != null)
                Debug.Log(setTask.Exception);
        }

        else
        {
            Login(userToAdd.Email, PasswordRegisterField.text);
        }
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(email))
        {
            Debug.LogError("Username or email or contains spaces is empty");
        }

        else if (password != confirmPassword)
        {
            Debug.LogError("Password does not match");
        }

        else
        {
            var registerTask = FirebaseManager.Instance.auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);
                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;
                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }
                Debug.Log(failedMessage);
            }

            else
            {
                // Get The User After Registration Success
                FirebaseManager.Instance.firebaseUser = registerTask.Result;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = FirebaseManager.Instance.firebaseUser.UpdateUserProfileAsync(userProfile);
                yield return new WaitUntil(() => updateProfileTask.IsCompleted);
                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    FirebaseManager.Instance.firebaseUser.DeleteAsync();
                    Debug.LogError(updateProfileTask.Exception);
                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;
                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }
                    Debug.Log(failedMessage);
                }
                else
                {

                    try
                    {

                        userId = FirebaseManager.Instance.firebaseUser.UserId;

                        User newUser = new User()
                        {
                            UserId = userId,
                            Username = name,
                            Email = email
                        };

                        // To be tested
                        StartCoroutine(AddUserToDb("Users", userId, newUser));

                        Debug.Log("Registration Sucessful Welcome " + FirebaseManager.Instance.firebaseUser.DisplayName);

                        Debug.Log($"Added user: {userId}");
                    }
                    catch (UnityException e)
                    {
                        Debug.Log(e.Message);
                    }
                }
            }
        }
    }

    #endregion

}
