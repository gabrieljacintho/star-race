using System;
using UnityEngine;

public class WallControl : MonoBehaviour
{
    [NonSerialized]
    public int speed = 2;
    private bool left = false;
    private bool wallSpawned = false;

    private void Start()
    {
        left = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;
    }

    private void Update()
    {
        if (left)
        {
            if (transform.position.x >= -SpawnManager.instance.wallMaxPositionX)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            else
            {
                left = false;
            }
        }
        else
        {
            if (transform.position.x <= SpawnManager.instance.wallMaxPositionX)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else
            {
                left = true;
            }
        }

        transform.position += Vector3.down * GameManager.instance.gravity * Time.deltaTime;

        if (!wallSpawned && transform.position.y < (6 - GameManager.instance.wallDistance) && GameManager.instance.gameInProgress)
        {
            SpawnManager.instance.SpawnWall();
            wallSpawned = true;
        }
    }

    private void OnBecameInvisible()
    {
        if (transform.position.y < 0)
        {
            wallSpawned = false;
            left = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;
            gameObject.SetActive(false);
        }
    }
}