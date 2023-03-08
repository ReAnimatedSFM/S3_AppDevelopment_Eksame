using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitObject : MonoBehaviour
{
    [HideInInspector] public Fruit thisFruit;

    [HideInInspector] public Renderer thisRenderer;

    [HideInInspector] public Material[] Materials;

    [HideInInspector] public bool isDead = false;

    private bool readyToRespawn;

    private float fallSpeed = 0f;

    private void Awake()
    {
        readyToRespawn = false;
    }

    private void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        Materials = thisRenderer.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToRespawn)
        {
            Fall();
            SetFallSpeed();
        }
        else
            fallSpeed = 0f;

        if (isDead)
        {
            fallSpeed = 0;
            readyToRespawn = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathBoundary"))
        {
            isDead = true;
            readyToRespawn = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PointHandler.Instance.AddToScore(thisFruit.Points);
            Invoke(nameof(SetReadyToRespawn), 2f * Time.fixedDeltaTime);
        }
    }

    private void SetFallSpeed()
    {
        if (!isDead)
            fallSpeed -= 2.5f * Time.fixedDeltaTime;
        else
            fallSpeed = 0f;
    }

    private void SetReadyToRespawn()
    {
        readyToRespawn = true;
    }

    private void Fall()
    {
        transform.position = new Vector3(transform.position.x, fallSpeed, transform.position.z);
    }

}
