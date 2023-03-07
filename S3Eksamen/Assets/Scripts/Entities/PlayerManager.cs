using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health;

    public static Player Instance;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
            Health = 5;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
