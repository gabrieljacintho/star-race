using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    private InterstitialAd interstitial;

    private string adUnitId;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus => { });

#if UNITY_ANDROID || UNITY_IOS
        adUnitId = "ca-app-pub-6939957165401361/1699330600"; // Teste: ca-app-pub-3940256099942544/1033173712
#else
        adUnitId = "unexpected_platform";
#endif

        RequestInterstitial();
    }

    private void RequestInterstitial()
    {
        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public void ShowAd()
    {
        if (interstitial.IsLoaded()) interstitial.Show();
    }

    private void OnAdOpening()
    {
        SongsManager.instance.audioSource.Stop();
    }

    private void OnAdClosed()
    {
        SongsManager.instance.audioSource.Play();
        interstitial.Destroy();
        RequestInterstitial();
    }
}
