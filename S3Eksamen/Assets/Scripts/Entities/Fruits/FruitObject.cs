using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitObject : MonoBehaviour
{
    public Fruit thisFruit;

    public bool isDead;

    private bool readyToRespawn;

    private float fallSpeed = 0f;

    private void Awake()
    {
        isDead = false;
        readyToRespawn = false;
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
