

using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UIElements;

public class UnityAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static UnityAds instance;

    [Header("Test Ads")]
    public bool testAds;

    [Header("Unity ADs")]
    [Header("Game IDs")]
    public string androidGameID;
    public string IOSGameID;

    [Header("Android")]
    public string unityAndroidInterstitialAdId = "Interstitial_Android";
    public string unityAndroidRewardedAdId = "Rewarded_Android";
    public string unityAndroidBannerAdId = "Banner_Android";

    [Header("IOS")]
    public string unityIOSInterstitialAdId = "Interstitial_iOS";
    public string unityIOSRewardedAdId = "Rewarded_iOS";
    public string unityIOSBannerAdId = "Banner_iOS";


    private string _gameId;
    string unityInterstitialAdId;
    string unityRewardedAdId;
    string unityBannerAdId;

    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    private bool isBannerLoaded;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
#if UNITY_ANDROID

        _gameId = androidGameID;
        unityInterstitialAdId = unityAndroidInterstitialAdId;
        unityRewardedAdId = unityAndroidRewardedAdId;
        unityBannerAdId = unityAndroidBannerAdId;

#elif UNITY_IPHONE
        
        _gameId = IOSGameID;
        unityInterstitialAdId = unityIOSInterstitialAdId;
        unityRewardedAdId = unityIOSRewardedAdId;
        unityBannerAdId = unityIOSBannerAdId;

#else

        _gameId = androidGameID;
        unityInterstitialAdId = unityAndroidInterstitialAdId;
        unityRewardedAdId = unityAndroidRewardedAdId;

#endif

        isBannerLoaded = false;

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, testAds, this);
        }

        LoadUnityInterstitialAd();
        LoadUnityRewardedAd();
        LoadUnityBanner();
    }

    #region UNITY ADS

    public void OnInitializationComplete() { }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { }

    public void OnUnityAdsAdLoaded(string adUnitId) { }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        if (_adUnitId.Equals(unityInterstitialAdId))
        {
            LoadUnityInterstitialAd();
        }
        if (_adUnitId.Equals(unityRewardedAdId))
        {
            LoadUnityRewardedAd();
        }
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        if (_adUnitId.Equals(unityInterstitialAdId))
        {
            PlayerPrefs.SetInt("adFailure", 1);
            LoadUnityInterstitialAd();
        }
        if (_adUnitId.Equals(unityRewardedAdId))
        {
            PlayerPrefs.SetInt("adFailure", 1);
            LoadUnityRewardedAd();
        }
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }

    public void OnUnityAdsShowClick(string _adUnitId) { }

    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (_adUnitId.Equals(unityRewardedAdId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            PlayerPrefs.SetInt("adCompleted", 1);
            LoadUnityRewardedAd();
        }

        if (_adUnitId.Equals(unityInterstitialAdId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            PlayerPrefs.SetInt("adCompleted", 1);

            if (GameManger.instance.rewardType.Equals(RewardTypes.SlideHint))
            {
                PlayerPrefs.SetInt("puzzleHint", 2);

            }
            else if (GameManger.instance.rewardType.Equals(RewardTypes.QmHint))
            {
                PlayerPrefs.SetInt("questionMarkHint", 2);

            }


            LoadUnityInterstitialAd();
        }

        else if (_adUnitId.Equals(unityInterstitialAdId) && showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
        {
            PlayerPrefs.SetInt("adCompleted", 1);
            LoadUnityInterstitialAd();
        }
    }

    #region UNITY BANNER ADS

    void OnBannerLoaded()
    {
        isBannerLoaded = true;
        Debug.Log("Unity Banner Loaded");
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        Advertisement.Banner.Show(unityBannerAdId, options);
    }

    void OnBannerError(string message) { }

    void OnBannerClicked() { }

    void OnBannerShown() { }

    void OnBannerHidden() { }

    public void LoadUnityBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(unityBannerAdId, options);
    }

    public void ShowUnityBannerAd(BannerPosition bannerPosition)
    {
        if (isBannerLoaded)
        {
            PlayerPrefs.SetInt("BannerAdCompleted", 1);

            Advertisement.Banner.Hide(true);

            Advertisement.Banner.SetPosition(bannerPosition);
            LoadUnityBanner();

        }
        else
        {
            PlayerPrefs.SetInt("BannerAdFailure", 1);
        }
    }

    public void ShowUnityBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        Advertisement.Banner.Show(unityBannerAdId, options);
    }

    public void HideUnityBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    #endregion

    #region UNITY INTERSTITIAL ADS

    public void LoadUnityInterstitialAd()
    {
        Advertisement.Load(unityInterstitialAdId, this);
    }

    public void ShowUnityInterstitialAd()
    {
        Advertisement.Show(unityInterstitialAdId, this);
    }

    #endregion

    #region UNITY REWRDED ADS

    public void LoadUnityRewardedAd()
    {
        Advertisement.Load(unityRewardedAdId, this);
    }

    public void ShowUnityRewardedAd()
    {
        Advertisement.Show(unityRewardedAdId, this);
    }

    #endregion

    #endregion
}
