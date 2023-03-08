using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public class Fruit
{
    [FirestoreProperty]
    public string Name
    {
        get;
        set;
    }

    [FirestoreProperty]
    public float Points
    {
        get;
        set;
    }
}
