using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SpawnPoint : MonoBehaviour
{
    public Collider2D coll;

    public bool isGround = true;
    public bool isWall = false;
    public bool isPossibleSpawn = false;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (isGround && !isWall)
        {
            isPossibleSpawn = true;
        }
        else
        {
            isPossibleSpawn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = true;
        }

        if (collision.CompareTag("Wall"))
        {
            isWall = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = false;
        }

        if (collision.CompareTag("Wall"))
        {
            isWall = false;
        }
    }
}
