using System;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundControl : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;
    public RawImage rawImage;

    [NonSerialized]
    public float uvRectY;
    [SerializeField]
    private float speedScale = 1;

    [SerializeField]
    public GameObject backgroundIntro = null;

    private void Update()
    {
        if (transform.position.y == 0)
        {
            uvRectY += GameManager.instance.gravity * speedScale * rawImage.uvRect.height / (10 * transform.localScale.y) * Time.deltaTime;
            rawImage.uvRect = new Rect(rawImage.uvRect.x, uvRectY, rawImage.uvRect.width, rawImage.uvRect.height);
        }
        else
        {
            transform.position += Vector3.down * GameManager.instance.gravity * Time.deltaTime;

            if (transform.position.y <= 0)
            {
                if (backgroundIntro != null) backgroundIntro.SetActive(false);

                if (BackgroundManager.instance.currentBackground == 3)
                {
                    BackgroundManager.instance.PutNextBackground();
                }
                else
                {
                    rectTransform.anchoredPosition = Vector3.zero;
                    BackgroundManager.instance.RemovePreviousBackgrounds();
                }
            }
        }
    }
}
