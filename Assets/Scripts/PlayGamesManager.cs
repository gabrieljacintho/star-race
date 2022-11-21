using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

public class PlayGamesManager : MonoBehaviour
{
    public static PlayGamesManager instance;
    public static PlayGamesPlatform platform;

    [NonSerialized]
    public bool aboutTheSeaCompleted = false;
    [NonSerialized]
    public bool landInSightCompleted = false;
    [NonSerialized]
    public bool inTheCloudsCompleted = false;
    [NonSerialized]
    public bool gettingStartedCompleted = false;
    [NonSerialized]
    public bool takingOffCompleted = false;
    [NonSerialized]
    public bool professionalCompleted = false;
    [NonSerialized]
    public bool wowAmazingCompleted = false;

    private bool showLeaderboardUI = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }

        SignIn();
    }

    public void SignIn()
    {
        if (showLeaderboardUI)
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
            {
                if (result == SignInStatus.Success) ShowLeaderboardUI();
            });
        }
        else
        {
            Social.Active.localUser.Authenticate(success => { });
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) => { });
            showLeaderboardUI = true;
        }

        if (Social.Active.localUser.authenticated && PlayerPrefs.GetInt("HighScoreUpdated", 0) == 0)
        {
            ReportScore(PlayerPrefs.GetInt("HighScore", 0));
            PlayerPrefs.SetInt("HighScoreUpdated", 1);
        }
    }

    public void UnlockAchievement(string achievementID, string playerprefsKey)
    {
        if (Social.Active.localUser.authenticated && PlayerPrefs.GetInt(playerprefsKey, 0) == 0)
        {
            Social.Active.ReportProgress(achievementID, 100.0f, (bool success) =>
            {
                if (success)
                {
                    PlayerPrefs.SetInt(playerprefsKey, 1);
                    PlayerPrefs.Save();
                }
            });
        }
    }

    public void ReportScore(int highScore)
    {
        if (Social.Active.localUser.authenticated)
        {
            Social.Active.ReportScore(highScore, GPGSIds.leaderboard_scoreboard, null);
        }
    }

    public void ShowLeaderboardUI()
    {
        if (Social.Active.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_scoreboard);
        }
        else
        {
            SignIn();
        }
    }
}
