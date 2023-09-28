using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudienceNetwork;

public class FAN : MonoBehaviour
{
    public static FAN instance;

    [Header("Test Ads")]
    public bool testAds;

    [Header("Interstitial Ad")]
    public string interstitialAndroid;

    [Header("Rewarded Ad")]
    public string rewardedAndroid;

    [Header("Banner Ad")]
    public string bannerAndroid;

    private InterstitialAd interstitialAd;
    private bool isInterstitialLoaded;
    private bool didInterstitialClose;

    private RewardedVideoAd rewardedVideoAd;
    private bool isRewardedLoaded;
    private bool didRewardedClose;

    private AdView adView;
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

        AudienceNetworkAds.Initialize();
    }

    private void Start()
    {
        isBannerLoaded = false;

        LoadInterstitial();
        LoadRewardedVideo();
        LoadBanner();
    }

    #region INTERSTITIAL

    public void LoadInterstitial()
    {

        if (testAds)
        {
            this.interstitialAd = new InterstitialAd($"VID_HD_16_9_46S_APP_INSTALL#{interstitialAndroid}");
        }
        else
        {
            this.interstitialAd = new InterstitialAd($"{interstitialAndroid}");
        }

        interstitialAd.Register(gameObject);

        interstitialAd.InterstitialAdDidLoad = delegate ()
        {
            isInterstitialLoaded = true;
            didInterstitialClose = false;
            string isAdValid = interstitialAd.IsValid() ? "valid" : "invalid";
        };

        interstitialAd.InterstitialAdDidFailWithError = delegate (string error)
        {
            
        };

        interstitialAd.InterstitialAdWillLogImpression = delegate ()
        {
            
        };

        interstitialAd.InterstitialAdDidClick = delegate ()
        {
           
        };

        interstitialAd.InterstitialAdDidClose = delegate ()
        {
            PlayerPrefs.SetInt("adCompleted", 1);
            didInterstitialClose = true;
            if (interstitialAd != null)
            {
                interstitialAd.Dispose();
            }
            LoadInterstitial();
        };

#if UNITY_ANDROID
        /*
         * Only relevant to Android.
         * This callback will only be triggered if the Interstitial activity has
         * been destroyed without being properly closed. This can happen if an
         * app with launchMode:singleTask (such as a Unity game) goes to
         * background and is then relaunched by tapping the icon.
         */
        interstitialAd.interstitialAdActivityDestroyed = delegate ()
        {
            if (!didInterstitialClose)
            {

            }
        };
#endif

        interstitialAd.LoadAd();
    }


    public void ShowInterstitial()
    {
        if (isInterstitialLoaded)
        {
            interstitialAd.Show();
            isInterstitialLoaded = false;
        }
        else
        {
            PlayerPrefs.SetInt("adFailure", 1);
            LoadInterstitial();
        }
    }

    #endregion

    #region REWARDED VIDEO

    public void LoadRewardedVideo()
    {

        if (testAds)
        {
            this.rewardedVideoAd = new RewardedVideoAd($"VID_HD_16_9_46S_APP_INSTALL#{rewardedAndroid}");
        }
        else
        {
            this.rewardedVideoAd = new RewardedVideoAd($"{rewardedAndroid}");
        }

        rewardedVideoAd.Register(gameObject);

        rewardedVideoAd.RewardedVideoAdDidLoad = delegate ()
        {
            isRewardedLoaded = true;
            didRewardedClose = false;
            string isAdValid = rewardedVideoAd.IsValid() ? "valid" : "invalid";
        };

        rewardedVideoAd.RewardedVideoAdDidFailWithError = delegate (string error)
        {

        };

        rewardedVideoAd.RewardedVideoAdWillLogImpression = delegate ()
        {
            
        };

        rewardedVideoAd.RewardedVideoAdDidClick = delegate ()
        {
            
        };

        rewardedVideoAd.RewardedVideoAdDidSucceed = delegate ()
        {
            
        };

        rewardedVideoAd.RewardedVideoAdDidFail = delegate ()
        {
            
        };

        rewardedVideoAd.RewardedVideoAdDidClose = delegate ()
        {
            didRewardedClose = true;
            PlayerPrefs.SetInt("adCompleted", 1);
            if (rewardedVideoAd != null)
            {
                rewardedVideoAd.Dispose();
            }

            if (GameManger.instance.rewardType.Equals(RewardTypes.SlideHint))
            {
                PlayerPrefs.SetInt("puzzleHint", 2);

            }
            else if (GameManger.instance.rewardType.Equals(RewardTypes.QmHint))
            {
                PlayerPrefs.SetInt("questionMarkHint", 2);

            }


            LoadRewardedVideo();
        };

#if UNITY_ANDROID
        /*
         * Only relevant to Android.
         * This callback will only be triggered if the Rewarded Video activity
         * has been destroyed without being properly closed. This can happen if
         * an app with launchMode:singleTask (such as a Unity game) goes to
         * background and is then relaunched by tapping the icon.
         */
        rewardedVideoAd.RewardedVideoAdActivityDestroyed = delegate ()
        {
            if (!didRewardedClose)
            {

            }
        };
#endif

        rewardedVideoAd.LoadAd();

    }

    public void ShowRewardedVideo()
    {
        if (isRewardedLoaded)
        {
            rewardedVideoAd.Show();
            isRewardedLoaded = false;
        }
        else
        {
            PlayerPrefs.SetInt("adFailure", 1);
            LoadRewardedVideo();
        }
    }

    #endregion

    #region BANNER

    public void LoadBanner()
    {
        if (this.adView)
        {
            this.adView.Dispose();
        }

        if (testAds)
        {
            this.adView = new AdView($"IMG_16_9_APP_INSTALL#{bannerAndroid}", AdSize.BANNER_HEIGHT_50);
        }
        else
        {
            this.adView = new AdView($"{bannerAndroid}", AdSize.BANNER_HEIGHT_50);
        }
        
        this.adView.Register(this.gameObject);

        this.adView.AdViewDidLoad = (delegate ()
        {
            isBannerLoaded = true;
        });
        adView.AdViewDidFailWithError = (delegate (string error) { });

        adView.AdViewWillLogImpression = (delegate () { });

        adView.AdViewDidClick = (delegate () { });

        adView.LoadAd();
    }

    public void ShowBanner(AdPosition position)
    {
        if (isBannerLoaded)
        {
            PlayerPrefs.SetInt("BannerAdCompleted", 1);
            adView.Show(position);
        }
        else
        {
            PlayerPrefs.SetInt("BannerAdFailure", 1);
        }
    }

    public void HideBanner()
    {
        adView.Show(0.1);

    }

    #endregion

}
