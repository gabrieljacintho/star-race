using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text scoreText = null;
    [SerializeField]
    private Text highScoreText = null;
    [SerializeField]
    private Animator highScoreTextAnimator = null;

    [SerializeField]
    private GameObject mouseObj = null;
    [SerializeField]
    private Animator mouseAnimator = null;

    [SerializeField]
    private Image pauseButtonImage = null;
    [SerializeField]
    private GameObject playButtonObject = null;
    [SerializeField]
    private GameObject pausePanelObject = null;

    [SerializeField]
    private Image muteButtonImage = null;
    [SerializeField]
    private Sprite withAudioSprite = null;
    [SerializeField]
    private Sprite withoutAudioSprite = null;

    [SerializeField]
    private GameObject[] mainMenuObjects = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Mouse", 0) >= 2)
        {
            Destroy(mouseObj);
        }

        highScoreTextAnimator.enabled = false;
        highScoreText.transform.localScale = new Vector3(1.25f, 1.25f, 1);
        if (PlayerPrefs.GetInt("HighScore", 0) > 0)
        {
            UpdateHighScoreText();
            highScoreText.rectTransform.anchoredPosition = new Vector2(0, -250);
            highScoreText.gameObject.SetActive(true);
        }
        else
        {
            highScoreText.rectTransform.anchoredPosition = new Vector2(0, -115);
        }
    }

    public void AddMainMenuObjs()
    {
        foreach (GameObject mainMenuObject in mainMenuObjects)
        {
            mainMenuObject.gameObject.SetActive(true);
        }

        if (mouseObj != null)
        {
            mouseObj.SetActive(true);
        }

        pauseButtonImage.gameObject.SetActive(false);
        pauseButtonImage.raycastTarget = false;
    }

    public void RemoveMainMenuObjs()
    {
        foreach (GameObject mainMenuObject in mainMenuObjects)
        {
            mainMenuObject.gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Mouse", 0) < 2)
        {
            mouseAnimator.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Mouse", PlayerPrefs.GetInt("Mouse", 0) + 1);
            PlayerPrefs.Save();
        }
        else if (mouseObj != null)
        {
            Destroy(mouseObj);
        }

        pauseButtonImage.gameObject.SetActive(true);
        pauseButtonImage.raycastTarget = true;
    }

    public void UpdateScoreText()
    {
         scoreText.text = ScoreManager.instance.score.ToString();
    }

    public void UpdateHighScoreToEqualsScore()
    {
        highScoreText.text = ScoreManager.instance.score.ToString();
    }

    public void UpdateHighScoreText()
    {
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void ActivateScoreText()
    {
        if ( GameManager.instance.gameInProgress && PlayerPrefs.GetInt("HighScore", 0) > 0)
        {
            scoreText.gameObject.SetActive(true);
        }
        else if (scoreText.gameObject.activeSelf)
        {
            scoreText.gameObject.SetActive(false);
        }
    }

    public void ActivateHighScoreTextAnimation()
    {
        if (highScoreTextAnimator.enabled == false) highScoreTextAnimator.enabled = true;

        if (!highScoreTextAnimator.gameObject.activeSelf)
        {
            highScoreText.gameObject.SetActive(true);
        }
        else if (GameManager.instance.gameInProgress)
        {
            if (ScoreManager.instance.scoreExceeded)
            {
                highScoreTextAnimator.SetTrigger("HighScoreToScore");
                StartCoroutine(RemoveScoreText());
            }
            else
            {
                highScoreTextAnimator.SetTrigger("MainMenuToHighScore");
            }
        }
        else
        {
            if (highScoreText.rectTransform.localScale == Vector3.one)
            {
                highScoreTextAnimator.SetTrigger("HighScoreToMainMenu");
            }
            else
            {
                highScoreTextAnimator.SetTrigger("ScoreToMainMenu");
            }
        }
    }

    public void MuteButton()
    {
        SongsManager.instance.audioSource.mute = SongsManager.instance.audioSource.mute ? false : true;
        if (SongsManager.instance.audioSource.mute)
        {
            muteButtonImage.sprite = withoutAudioSprite;
        }
        else
        {
            muteButtonImage.sprite = withAudioSprite;
        }
    }

    public void PauseButton()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pausePanelObject.SetActive(false);

            pauseButtonImage.gameObject.SetActive(true);
            pauseButtonImage.raycastTarget = true;

            playButtonObject.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pausePanelObject.gameObject.SetActive(true);

            pauseButtonImage.gameObject.SetActive(false);
            pauseButtonImage.raycastTarget = false;

            playButtonObject.gameObject.SetActive(true);
        }
    }

    private IEnumerator RemoveScoreText()
    {
        yield return new WaitForSeconds(1);
        scoreText.gameObject.SetActive(false);
    }
}