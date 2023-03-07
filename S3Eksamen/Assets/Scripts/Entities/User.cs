using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System;

[FirestoreData]
public class User
{
    #region Variables

    [FirestoreProperty]
    public string UserId
    {
        get;
        set;
    }

    [FirestoreProperty]
    public string Username
    {
        get;
        set;
    }

    [FirestoreProperty]
    public string Email
    {
        get;
        set;
    }

    #endregion
}