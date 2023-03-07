using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FirebaseData
{
    public static DependencyStatus ThisDependecyStatus;

    public static FirebaseAuth ThisFirebaseAuth;

    public static FirebaseUser ThisFirebaseUser;

    public static FirebaseManager Instance;

    public static void SetFirebaseData()
    {
        FirebaseManager.Instance = Instance;
        FirebaseManager.Instance.dependencyStatus = ThisDependecyStatus;
        FirebaseManager.Instance.firebaseUser = ThisFirebaseUser;
        FirebaseManager.Instance.auth = ThisFirebaseAuth;
    }
}