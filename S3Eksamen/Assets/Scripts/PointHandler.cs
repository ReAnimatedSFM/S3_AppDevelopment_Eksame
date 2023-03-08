using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHandler : MonoBehaviour
{
    public static PointHandler Instance;

    public float Score = 0;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    public void AddToScore(float points)
    {
        Score += points;

        Debug.Log(Score);
    }
}
