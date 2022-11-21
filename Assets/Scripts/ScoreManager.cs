using GooglePlayGames;
using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField]
    private AudioClip scoreExceededAudio = null;

    [NonSerialized]
    public int score;
    [NonSerialized]
    public bool scoreExceeded = false;

    private bool wowAmazingCalled = false;
    private bool professionalCalled = false;
    private bool takingOffCalled = false;
    private bool gettingStartedCalled = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (GameManager.instance.gameInProgress)
        {
            score = Mathf.RoundToInt((GameManager.instance.gravity - GameManager.instance.initialGravity) * 100);

            if (score == 1100 && !wowAmazingCalled)
            {
                PlayGamesManager.instance.UnlockAchievement(GPGSIds.achievement_wow_amazing, "wowAmazingCompleted");
                wowAmazingCalled = true;
            }
            else if (score == 800 && !professionalCalled)
            {
                PlayGamesManager.instance.UnlockAchievement(GPGSIds.achievement_professional, "professionalCompleted");
                professionalCalled = true;
            }
            else if (score == 500 && !takingOffCalled)
            {
                PlayGamesManager.instance.UnlockAchievement(GPGSIds.achievement_taking_off, "takingOffCompleted");
                takingOffCalled = true;
            }
            else if (score == 100 && !gettingStartedCalled)
            {
                PlayGamesManager.instance.UnlockAchievement(GPGSIds.achievement_getting_started, "gettingStartedCompleted");
                gettingStartedCalled = true;
            }

            if (scoreExceeded || score > PlayerPrefs.GetInt("HighScore", 0))
            {
                if (!scoreExceeded && PlayerPrefs.GetInt("HighScore", 0) > 0)
                {
                    scoreExceeded = true;
                    UIManager.instance.ActivateHighScoreTextAnimation();
                    SongsManager.instance.audioSource.PlayOneShot(scoreExceededAudio, 0.8f);
                }
                UIManager.instance.UpdateHighScoreToEqualsScore();
            }
            if (UIManager.instance.scoreText.gameObject.activeSelf) UIManager.instance.UpdateScoreText();
        }
    }

    public void UpdateHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            if (Social.localUser.authenticated) PlayGamesManager.instance.ReportScore(score);
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            UIManager.instance.UpdateHighScoreText();
        }
    }

    public void ResetCalls()
    {
        wowAmazingCalled = false;
        professionalCalled = false;
        takingOffCalled = false;
        gettingStartedCalled = false;
    }
}