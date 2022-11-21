using System;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    [SerializeField]
    public GameObject[] backgroundObjects = new GameObject[4];
    [SerializeField]
    private RectTransform[] backgroundRectTransforms = new RectTransform[4];
    [SerializeField]
    private BackgroundControl[] backgroundControls = new BackgroundControl[4];

    [NonSerialized]
    public int currentBackground = 0;
    private int nextBackground = 1;

    private void Awake()
    {
        instance = this;
    }

    public void PutNextBackground()
    {
        if (GameManager.instance.gameInProgress)
        {
            backgroundObjects[nextBackground].SetActive(true);
            currentBackground = nextBackground;
            nextBackground = currentBackground + 1;
        }
        else if (currentBackground == backgroundObjects.Length - 1)
        {
            backgroundObjects[nextBackground].SetActive(true);
            currentBackground = nextBackground;
            nextBackground = currentBackground + 1;
            RemovePreviousBackgrounds();
        }
        else
        {
            backgroundObjects[backgroundObjects.Length - 1].SetActive(true);
            currentBackground = backgroundObjects.Length - 1;
            nextBackground = 0;
        }
    }

    public void RemovePreviousBackgrounds()
    {
        for (int i = 0; i < backgroundObjects.Length; i++)
        {
            if (backgroundObjects[i] != backgroundObjects[currentBackground] && backgroundObjects[i].activeSelf)
            {
                if (i != 0)
                {
                    backgroundRectTransforms[i].anchoredPosition = new Vector3(0, 1900, 0);
                    backgroundControls[i].backgroundIntro.SetActive(true);
                }

                backgroundControls[i].uvRectY = 0;
                backgroundControls[i].rawImage.uvRect = new Rect(backgroundControls[i].rawImage.uvRect.x, backgroundControls[i].uvRectY, backgroundControls[i].rawImage.uvRect.width, backgroundControls[i].rawImage.uvRect.height);

                backgroundObjects[i].SetActive(false);
            }
        }
    }
}
