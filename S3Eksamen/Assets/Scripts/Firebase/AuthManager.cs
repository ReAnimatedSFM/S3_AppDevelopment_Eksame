using Firebase.Auth;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthManager : MonoBehaviour
{

    /// <summary>
    /// Firebase DependencyStatus instance
    /// </summary>
    public DependencyStatus dependencyStatus;

    /// <summary>
    /// Firebase authentication variable
    /// </summary>
    public FirebaseAuth auth;

    /// <summary>
    /// User for firebase
    /// </summary>
    public FirebaseUser firebaseUser;

    /// <summary>
    /// Checks and fixes dependencies asynchronously. If dependecyStatus is available then run InitializeFirebase(), otherwise
    /// log error to console
    /// </summary>
    private void CheckAndHandleDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }

            else
            {
                Debug.LogError("There was an error with dependecy status: " + dependencyStatus);
            }
        });
    }

    /// <summary>
    /// Initializes firebase authentication settings
    /// </summary>
    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void InitializeComponents()
    {
        CheckAndHandleDependencies();
    }


    /// <summary>
    /// On any change to Authentication status, either signs user in or out depending on task
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != firebaseUser)
        {
            bool signedIn = firebaseUser != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && firebaseUser != null)
            {
                Debug.Log("Signed out " + firebaseUser.UserId);
            }
            firebaseUser = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + firebaseUser.UserId);
            }
        }
    }

    public void SignOut()
    {
        if (auth.CurrentUser != null)
            auth.SignOut();
    }

}
