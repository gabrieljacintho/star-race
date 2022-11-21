using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField]
    private GameObject player = null;
    private const float distanceFromTheStarMaxPositionX = 0.453f;
    [NonSerialized]
    public float starMaxPositionX;

    [SerializeField]
    private GameObject[] walls = new GameObject[3];
    private int currentWall = 0;
    private const float distanceFromTheWallMaxPositionX = 0.9775f;
    [NonSerialized]
    public float wallMaxPositionX;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        wallMaxPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, 0)).x - distanceFromTheWallMaxPositionX;
        starMaxPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, 0)).x - distanceFromTheStarMaxPositionX;
    }

    public void SpawnPlayer()
    {
        player.transform.position = Vector2.down * 6;
        player.transform.rotation = Quaternion.identity;
        player.SetActive(true);
    }

    public void SpawnWall()
    {
        walls[currentWall].transform.position = new Vector2(UnityEngine.Random.Range(-1.7f, 1.7f), 6);
        walls[currentWall].SetActive(true);
        currentWall++;
        if (currentWall == walls.Length)
        {
            currentWall = 0;
        }
    }

    public void RemoveWalls()
    {
        foreach (GameObject wall in walls)
        {
            wall.SetActive(false);
            if (wall.transform.childCount > 0)
            {
                wall.transform.GetChild(0).gameObject.SetActive(false);
                wall.transform.DetachChildren();
            }
        }
    }
}