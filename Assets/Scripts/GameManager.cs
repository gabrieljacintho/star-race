using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Range(1, 20)]
    public float gravity;
    [NonSerialized]
    public float initialGravity = 2.0f;

    [NonSerialized]
    public bool gameInProgress = false;
    [NonSerialized]
    public bool canStart = true;
    private int compositionLevel = 0;
    [NonSerialized]
    public int wallDistance = 5;

    private Vector2 initialClickPosition;
    private bool start = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gravity = initialGravity;
    }

    private void Update()
    {
        if (!gameInProgress && canStart)
        {

#if UNITY_EDITOR || UNITY_STANDALONE

            if (Input.GetMouseButtonDown(0))
            {
                initialClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y - initialClickPosition.y >= 2)
                {
                    start = true;
                }
            }

#elif UNITY_ANDROID || UNITY_IOS

            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    initialClickPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y - initialClickPosition.y >= 2)
                    {
                        start = true;
                    }
                }
            }
#endif

            if (start)
            {
                StartGame();
            }
        }
        else if (gameInProgress)
        {
            gravity += 0.075f * Time.deltaTime;

            if (gravity >= 6 && compositionLevel < 2)
            {
                if (gravity >= 12)
                {
                    if (compositionLevel != 2)
                    {
                        compositionLevel = 2;
                        BackgroundManager.instance.PutNextBackground();
                        PlayGamesManager.instance.UnlockAchievement(GPGSIds.achievement_in_the_clouds, "inTheCloudsCompleted");
                    }
                }
                else if (compositionLevel != 1)
                {
                    compositionLevel = 1;
                    BackgroundManager.instance.PutNextBackground();
                    PlayGamesManager.instance.UnlockAchievement(GPGSIds.achievement_land_in_sight, "landInSightCompleted");
                }
            }
        }
    }

    private void StartGame()
    {
        gravity = initialGravity;
        compositionLevel = 0;
        gameInProgress = true;
        canStart = false;
        start = false;
        initialClickPosition = Vector2.up * 5;
        ScoreManager.instance.score = 0;
        ScoreManager.instance.scoreExceeded = false;
        SpawnManager.instance.RemoveWalls();
        SpawnManager.instance.SpawnPlayer();
        SpawnManager.instance.SpawnWall();
        UIManager.instance.RemoveMainMenuObjs();
        UIManager.instance.ActivateScoreText();
        UIManager.instance.ActivateHighScoreTextAnimation();
        PlayGamesManager.instance.UnlockAchievement(GPGSIds.achievement_about_the_sea, "aboutTheSeaCompleted");
    }

    public void GameOver()
    {
        gameInProgress = false;
        canStart = true;
        if (BackgroundManager.instance.currentBackground > 0) BackgroundManager.instance.PutNextBackground();
        ScoreManager.instance.UpdateHighScore();
        UIManager.instance.AddMainMenuObjs();
        UIManager.instance.ActivateScoreText();
        UIManager.instance.ActivateHighScoreTextAnimation();
        AdsManager.instance.ShowAd();
        ScoreManager.instance.ResetCalls();
    }
}