using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Ump;

using System;
using System.Collections;
using UnityEngine.Advertisements;
using TMPro;
using UnityEngine.UI;
using GoogleMobileAds.Ump.Api;
using System.Collections.Generic;


public class AdMobAds : MonoBehaviour
{
    /*    [Serializable]
        public struct NativeADComp
        {
            public GameObject nativeADGameObject;
            public Image icon;
            public Image adChoices;
            public RawImage rating;
            public TextMeshProUGUI headline;
            public TextMeshProUGUI advertiser;
            public GameObject callToActionButton;
            public Text callToAction;
            public bool dataTransfered;
        }*/

    public static AdMobAds instance;
    public bool testAds;

    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private BannerView bannerView;

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

    #region GOOGLE ADMOB ADS VARIABLES

    [Header("Google Admob ADs")]
    [Header("Android")]
    public string androidAppOpenAd;
    public string androidBannerAd;
    public string androidInterstitialAd;
    public string androidRewardedAd;
    public string androidRewardedInterstitialAd;
    public string androidNativeAd;

    [Header("IOS")]
    public string IOSAppOpenAd;
    public string IOSBannerAd;
    public string IOSInterstitialAd;
    public string IOSRewardedAd;
    public string IOSRewardedInterstitialAd;
    public string IOSNativeAd;

    [Header("Android Test ID's")]
    public string _androidAppOpenAdId = "ca-app-pub-3940256099942544/5662855259";
    public string _androidBannerAdId = "ca-app-pub-3940256099942544/2934735716";
    public string _androidInterstitialAdId = "ca-app-pub-3940256099942544/4411468910";
    public string _androidRewardedAdId = "ca-app-pub-3940256099942544/1712485313";
    public string _androidRewardedInterstitialAdId = "ca-app-pub-3940256099942544/5354046379";
    public string _androidNativeAdId = "ca-app-pub-3940256099942544/3986624511";

    [Header("IOS Test ID's")]
    public string _IOSAppOpenAdId = "ca-app-pub-3940256099942544/5662855259";
    public string _IOSBannerAdId = "ca-app-pub-3940256099942544/2934735716";
    public string _IOSInterstitialAdId = "ca-app-pub-3940256099942544/4411468910";
    public string _IOSRewardedAdId = "ca-app-pub-3940256099942544/1712485313";
    public string _IOSRewardedInterstitialAdId = "ca-app-pub-3940256099942544/6978759866";
    public string _IOSNativeAdId = "ca-app-pub-3940256099942544/3986624511";

    [Header("Banner AD")]
    public AdPosition bannerAdInitialPosition;

    [Header("App Open AD")]
    //   public ScreenOrientation appOpenAddOrientation;




    [Header("Native AD")]
    // public NativeADComp tempNativeAdObject;


    string androidAppOpenAdId;
    string androidBannerAdId;
    string androidInterstitialAdId;
    string androidRewardedAdId;
    string androidRewardedInterstitialAdId;
    string androidNativeAdId;

    string iosAppOpenAdId;
    string iosBannerAdId;
    string iosInterstitialAdId;
    string iosRewardedAdId;
    string iosRewardedInterstitialAdId;
    string iosNativeAdId;

    string AppOpenAdId;
    string BannerAdId;
    string InterstitialAdId;
    string RewardedAdId;
    string RewardedInterstitialAdId;
    string NativeAdId;

    // AppOpenAd appOpenAd;
    BannerView _bannerView;
    RewardedInterstitialAd rewardedInterstitialAd;

    // bool nativeAdLoaded;
    // public NativeAd nativeAd;
    int mainMenuVisited;
    int playButtonClicked;

    #endregion

    public void Start()
    {
        //  AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

        CheckIfTestAdsMode();

#if UNITY_ANDROID

        AppOpenAdId = androidAppOpenAdId;
        BannerAdId = androidBannerAdId;
        InterstitialAdId = androidInterstitialAdId;
        RewardedAdId = androidRewardedAdId;
        RewardedInterstitialAdId = androidRewardedInterstitialAdId;
        NativeAdId = androidNativeAdId;

#elif UNITY_IPHONE
        
        AppOpenAdId = iosAppOpenAdId;
        BannerAdId = iosBannerAdId;
        InterstitialAdId=iosInterstitialAdId;
        RewardedAdId = iosRewardedAdId;
        RewardedInterstitialAdId = iosRewardedInterstitialAdId;
        NativeAdId = iosNativeAdId;

#else

        AppOpenAdId = "unused";
        BannerAdId = "unused";
        InterstitialAdId="unused";
        RewardedAdId = "unused";
        RewardedInterstitialAdId = "unused";
        NativeAdId = "unused";

#endif

        MobileAds.Initialize((InitializationStatus initStatus) =>
        {

        });

        LoadAdmobBannerAd();
        LoadAdmobInterstitialAd();
        LoadAdmobRewardedAd();
        //LoadAdmobAppOpenAd();
    }

    public void CheckIfTestAdsMode()
    {
        if (testAds)
        {
            androidAppOpenAdId = _androidAppOpenAdId;
            androidBannerAdId = _androidBannerAdId;
            androidInterstitialAdId = _androidInterstitialAdId;
            androidRewardedAdId = _androidRewardedAdId;
            androidRewardedInterstitialAdId = _androidRewardedInterstitialAdId;
            androidNativeAdId = _androidNativeAdId;

            iosAppOpenAdId = _IOSAppOpenAdId;
            iosBannerAdId = _IOSBannerAdId;
            iosInterstitialAdId = _IOSInterstitialAdId;
            iosRewardedAdId = _IOSRewardedAdId;
            iosRewardedInterstitialAdId = _IOSRewardedInterstitialAdId;
            iosNativeAdId = _IOSNativeAdId;
        }
        else
        {
            androidAppOpenAdId = androidAppOpenAd;
            androidBannerAdId = androidBannerAd;
            androidInterstitialAdId = androidInterstitialAd;
            androidRewardedAdId = androidRewardedAd;
            androidRewardedInterstitialAdId = androidRewardedInterstitialAd;
            androidNativeAdId = androidNativeAd;

            iosAppOpenAdId = IOSAppOpenAd;
            iosBannerAdId = IOSBannerAd;
            iosInterstitialAdId = IOSInterstitialAd;
            iosRewardedAdId = IOSRewardedAd;
            iosRewardedInterstitialAdId = IOSRewardedInterstitialAd;
            iosNativeAdId = IOSNativeAd;
        }
    }

    #region AdmobBannerAds

    public void CreateBannerView()
    {
        if (bannerView != null)
        {
            DestroyAd();
        }

        bannerView = new BannerView(BannerAdId, AdSize.Banner, bannerAdInitialPosition);
    }

    public void LoadAdmobBannerAd()
    {
        if (bannerView == null)
        {
            CreateBannerView();
            ListenToAdEvents();
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        bannerView.LoadAd(adRequest);
    }

    public void SetBannerPosition(AdPosition adPosition)
    {
        if (bannerView != null)
        {
            bannerView.SetPosition(adPosition);
        }
    }

    public void ShowAdmobBannerAd(AdPosition position)
    {
        if (bannerView != null)
        {
            Debug.Log("Banner View Available");
            PlayerPrefs.SetInt("BannerAdCompleted", 1);
            SetBannerPosition(position);
            bannerView.Show();
        }
        else
        {
            PlayerPrefs.SetInt("BannerAdFailure", 1);
        }
    }

    public void HideAdmobBannerAd()
    {
        bannerView.Hide();
    }

    private void ListenToAdEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            bannerView.Hide();
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) => { };

        bannerView.OnAdPaid += (AdValue adValue) => { };

        bannerView.OnAdImpressionRecorded += () => { };

        bannerView.OnAdClicked += () => { };

        bannerView.OnAdFullScreenContentOpened += () => { };

        bannerView.OnAdFullScreenContentClosed += () => { };
    }

    public void DestroyAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }

    #endregion

    #region AdmobInterstitialAds

    public void LoadAdmobInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(InterstitialAdId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    return;
                }

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }

    public void ShowAdmobinterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            LoadAdmobInterstitialAd();
        }
        else
        {
            PlayerPrefs.SetInt("adFailure", 1);
            LoadAdmobInterstitialAd();
        }
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) => { };

        ad.OnAdImpressionRecorded += () => { };

        ad.OnAdClicked += () => { };

        ad.OnAdFullScreenContentOpened += () => { };

        ad.OnAdFullScreenContentClosed += () =>
        {
            PlayerPrefs.SetInt("adCompleted", 1);
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            PlayerPrefs.SetInt("adFailure", 1);
        };
    }

    #endregion

    #region AdmobRewardedAds

    public void LoadAdmobRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(RewardedAdId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    return;
                }

                rewardedAd = ad;

                RegisterEventHandlers(rewardedAd);
            });
    }

    public void ShowAdmobRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                if (GameManger.instance.rewardType.Equals(RewardTypes.SlideHint) )
                {
                    PlayerPrefs.SetInt("puzzleHint", 2);

                }
                else if (GameManger.instance.rewardType.Equals(RewardTypes.QmHint))
                {
                    PlayerPrefs.SetInt("questionMarkHint", 2);

                }

            });

            LoadAdmobRewardedAd();
        }
        else
        {
            PlayerPrefs.SetInt("adFailure", 1);
            LoadAdmobRewardedAd();
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) => { };

        ad.OnAdImpressionRecorded += () => { };

        ad.OnAdClicked += () => { };

        ad.OnAdFullScreenContentOpened += () => { };

        ad.OnAdFullScreenContentClosed += () =>
        {
            PlayerPrefs.SetInt("adCompleted", 1);
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            PlayerPrefs.SetInt("adFailure", 1);
        };
    }

    #endregion

    /*  #region AdmobNATIVEADS

      private void RequestNativeAd()
      {
          AdLoader adLoader = new AdLoader.Builder(NativeAdId)
              .ForNativeAd()
              .Build();

          adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
          adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
          adLoader.LoadAd(new AdRequest.Builder().Build());
      }

      private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
      {
          //Debug.Log("Native ad failed to load: " + args.LoadAdError.GetMessage());
      }

      private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
      {
          //Debug.Log("Native ad loaded.");
          this.nativeAd = args.nativeAd;


          //tempNativeAdObject.dataTransfered = false;

          //tempNativeAdObject.icon.gameObject.GetComponent<BoxCollider>().size = tempNativeAdObject.icon.GetComponent<RectTransform>().sizeDelta;
          //tempNativeAdObject.headline.gameObject.GetComponent<BoxCollider>().size = tempNativeAdObject.headline.GetComponent<RectTransform>().sizeDelta;
          //tempNativeAdObject.advertiser.gameObject.GetComponent<BoxCollider>().size = tempNativeAdObject.advertiser.GetComponent<RectTransform>().sizeDelta;
          //tempNativeAdObject.callToAction.gameObject.GetComponent<BoxCollider>().size = tempNativeAdObject.callToAction.GetComponent<RectTransform>().sizeDelta;


          // Get Texture2D for the icon asset of native ad.


          tempNativeAdObject.dataTransfered = true;




      }

      public NativeAd SetNativeAd(NativeADComp nativeAdSample)
      {
          //tempNativeAdObject = nativeAdSample;
          RequestNativeAd();
          return nativeAd;
      }


      #endregion*/


    #region AdmobRewardedInterstitial

    public void LoadAdmobRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedInterstitialAd.Load(RewardedInterstitialAdId, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    return;
                }

                rewardedInterstitialAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    public void ShowRewardedInterstitialAd()
    {
        const string rewardMsg =
            "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {

            });
        }
    }

    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) => { };

        ad.OnAdImpressionRecorded += () => { };

        ad.OnAdClicked += () => { };

        ad.OnAdFullScreenContentOpened += () => { };

        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadAdmobRewardedInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadAdmobRewardedInterstitialAd();
        };
    }

    #endregion

    /*   #region AdmobAppOpenAd




       /// Loads the app open ad.
       /// </summary>
       public void LoadAdmobAppOpenAd()
       {
           // Clean up the old ad before loading a new one.
           if (appOpenAd != null)
           {
               appOpenAd.Destroy();
               appOpenAd = null;
           }

           Debug.Log("Loading the app open ad.");

           // Create our request used to load the ad.
           var adRequest = new AdRequest();

           // send the request to load the ad.
           AppOpenAd.Load(AppOpenAdId, appOpenAddOrientation, adRequest,
               (AppOpenAd ad, LoadAdError error) =>
               {
                   // if error is not null, the load request failed.
                   if (error != null || ad == null)
                   {
                       Debug.LogError("app open ad failed to load an ad " +
                                      "with error : " + error);
                       return;
                   }

                   Debug.Log("App open ad loaded with response : "
                             + ad.GetResponseInfo());

                   appOpenAd = ad;


                   RegisterEventHandlers(ad);
               });
       }

       private void RegisterEventHandlers(AppOpenAd ad)
       {
           // Raised when the ad is estimated to have earned money.
           ad.OnAdPaid += (AdValue adValue) =>
           {
               Debug.Log(String.Format("App open ad paid {0} {1}.",
                   adValue.Value,
                   adValue.CurrencyCode));
           };
           // Raised when an impression is recorded for an ad.
           ad.OnAdImpressionRecorded += () =>
           {
               Debug.Log("App open ad recorded an impression.");
           };
           // Raised when a click is recorded for an ad.
           ad.OnAdClicked += () =>
           {
               Debug.Log("App open ad was clicked.");
           };
           // Raised when an ad opened full screen content.
           ad.OnAdFullScreenContentOpened += () =>
           {
               Debug.Log("App open ad full screen content opened.");
           };
           // Raised when the ad closed full screen content.
           ad.OnAdFullScreenContentClosed += () =>
           {
               Debug.Log("App open ad full screen content closed.");
               LoadAdmobAppOpenAd();
           };
           // Raised when the ad failed to open full screen content.
           ad.OnAdFullScreenContentFailed += (AdError error) =>
           {
               Debug.LogError("App open ad failed to open full screen content " +
                              "with error : " + error);
               LoadAdmobAppOpenAd();
           };
       }



       private void OnDestroy()
       {
           // Always unlisten to events when complete.
           AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
       }

       private void OnAppStateChanged(AppState state)
       {
           Debug.Log("App State changed to : " + state);

           // if the app is Foregrounded and the ad is available, show it.
           if (state == AppState.Foreground)
           {

               StartCoroutine(ShowAdWithDelay());

           }
       }

       IEnumerator ShowAdWithDelay()
       {
           yield return new WaitForSeconds(0.2f);
           ShowAppOpenAd();
       }
       public void ShowAppOpenAd()
       {
           if (appOpenAd != null && appOpenAd.CanShowAd())
           {
               Debug.Log("Showing app open ad.");
               appOpenAd.Show();
           }
           else
           {
               Debug.LogError("App open ad is not ready yet.");
           }
       }
       #endregion
   */
}


