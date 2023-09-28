using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{

    public static AdsManager Instance;
    public List<ManageAdsHeirarchy> manageAdsHeirarchies;
    public Dictionary<string, int> sceneVisitCounter;

    [HideInInspector]
    public AdsBannerPositions bannerPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sceneVisitCounter = new Dictionary<string, int>();
        foreach (var adScene in Enum.GetNames(typeof(AdScenes)))
        {
            sceneVisitCounter[adScene] = 0;
        }
    }

    public void SetBannerAd(string currentScene)
    {
        StartCoroutine(SetBannerAdCoroutine(currentScene));
    }

    IEnumerator SetBannerAdCoroutine(string currentScene)
    {
        AdsType type = AdsType.Banner;
        ManageAdsHeirarchy heirarchy = manageAdsHeirarchies.Find(heirarchy => heirarchy.adScenes.ToString().Equals(currentScene));

        foreach (AdListing ad in heirarchy.adListing)
        {
            if (type == ad.firstCallAdType)
            {
                bannerPosition = heirarchy.bannerPosition;

                int i = 0;

                while (i < ad.adCall.Count)
                {
                    PlayerPrefs.SetInt("BannerAdCompleted", 0);
                    PlayerPrefs.SetInt("BannerAdFailure", 0);
                    SendMessage("Show" + ad.firstCallAdType.ToString() + ad.adCall[i].firstCall.ToString(), bannerPosition);

                    while (PlayerPrefs.GetInt("BannerAdCompleted") == 0 && PlayerPrefs.GetInt("BannerAdFailure") == 0)
                    {
                        yield return null;
                    }

                    if (PlayerPrefs.GetInt("BannerAdFailure") == 1)
                    {
                        i++;
                    }

                    else if (PlayerPrefs.GetInt("BannerAdCompleted") == 1)
                    {
                        yield break;
                    }
                }
            }
        }
    }

    public void ShowAd(string currentScene)
    {
        StartCoroutine(ShowAdCoroutine(currentScene));
    }

    IEnumerator ShowAdCoroutine(string currentScene)
    {
        sceneVisitCounter[currentScene]++;
        ManageAdsHeirarchy heirarchy = manageAdsHeirarchies.Find(heirarchy => heirarchy.adScenes.ToString().Equals(currentScene));

        foreach (AdListing ad in heirarchy.adListing)
        {
            if (sceneVisitCounter[currentScene].Equals((int)ad.adsCount))
            {
                int i = 0;

                while (i < ad.adCall.Count)
                {
                    PlayerPrefs.SetInt("adCompleted", 0);
                    PlayerPrefs.SetInt("adFailure", 0);
                    SendMessage("Show" + ad.firstCallAdType.ToString() + ad.adCall[i].firstCall.ToString());

                    while (PlayerPrefs.GetInt("adCompleted") == 0 && PlayerPrefs.GetInt("adFailure") == 0)
                    {
                        yield return null;
                    }

                    if (PlayerPrefs.GetInt("adFailure") == 1)
                    {
                        i++;
                    }

                    else if (PlayerPrefs.GetInt("adCompleted") == 1)
                    {
                        int j = 0;

                        while (j < ad.adCall[i].secondCall.Count)
                        {
                            PlayerPrefs.SetInt("adCompleted", 0);
                            PlayerPrefs.SetInt("adFailure", 0);

                            SendMessage("Show" + ad.adCall[i].secondCallAdType.ToString() + ad.adCall[i].secondCall[j].ToString());

                            while (PlayerPrefs.GetInt("adCompleted") == 0 && PlayerPrefs.GetInt("adFailure") == 0)
                            {
                                yield return null;
                            }

                            if (PlayerPrefs.GetInt("adFailure") == 1)
                            {
                                j++;
                            }

                            else if (PlayerPrefs.GetInt("adCompleted") == 1)
                            {
                                if (sceneVisitCounter[currentScene.ToString()] >= (int)heirarchy.adListing[heirarchy.adListing.Count - 1].adsCount)
                                {
                                    sceneVisitCounter[currentScene.ToString()] = 0;
                                }
                                yield break;
                            }
                        }
                        if (sceneVisitCounter[currentScene.ToString()] >= (int)heirarchy.adListing[heirarchy.adListing.Count - 1].adsCount)
                        {
                            sceneVisitCounter[currentScene.ToString()] = 0;
                        }
                        yield break;
                    }
                }
            }
        }
        if (sceneVisitCounter[currentScene.ToString()] >= (int)heirarchy.adListing[heirarchy.adListing.Count - 1].adsCount)
        {
            sceneVisitCounter[currentScene.ToString()] = 0;
        }
    }

    #region INTERSTITIAL

    public void ShowInterstitialAdmobAd()
    {
        AdMobAds.instance.ShowAdmobinterstitialAd();
    }

    public void ShowInterstitialUnityAd()
    {
        UnityAds.instance.ShowUnityInterstitialAd();
    }

    public void ShowInterstitialFacebookAd()
    {
        FAN.instance.ShowInterstitial();
    }

    #endregion

    #region REWARDED

    public void ShowRewardedAdmobAd()
    {
        AdMobAds.instance.ShowAdmobRewardedAd();
    }

    public void ShowRewardedUnityAd()
    {
        UnityAds.instance.ShowUnityRewardedAd();
    }

    public void ShowRewardedFacebookAd()
    {
        FAN.instance.ShowRewardedVideo();
    }

    #endregion

    #region BANNER

    public void ShowBannerAdmobAd(AdsBannerPositions position)
    {
        if (position.Equals(AdsBannerPositions.TOP))
        {
            AdMobAds.instance.ShowAdmobBannerAd(GoogleMobileAds.Api.AdPosition.Top);
        }
        else if (position.Equals(AdsBannerPositions.BOTTOM))
        {
            AdMobAds.instance.ShowAdmobBannerAd(GoogleMobileAds.Api.AdPosition.Bottom);
        }
    }

    public void HideBannerAdmobAd()
    {
        AdMobAds.instance.HideAdmobBannerAd();
    }

    public void ShowBannerUnityAd(AdsBannerPositions position)
    {
        if (position.Equals(AdsBannerPositions.TOP))
        {
            UnityAds.instance.ShowUnityBannerAd(BannerPosition.TOP_CENTER);
        }
        else if (position.Equals(AdsBannerPositions.BOTTOM))
        {
            UnityAds.instance.ShowUnityBannerAd(BannerPosition.BOTTOM_CENTER);
        }
    }

    public void HideBannerUnityAd()
    {
        UnityAds.instance.HideUnityBannerAd();
    }

    public void ShowBannerFacebookAd(AdsBannerPositions position)
    {
        if (position.Equals(AdsBannerPositions.TOP))
        {
            FAN.instance.ShowBanner(AudienceNetwork.AdPosition.TOP);
        }
        else if (position.Equals(AdsBannerPositions.BOTTOM))
        {
            FAN.instance.ShowBanner(AudienceNetwork.AdPosition.BOTTOM);
        }
    }

    public void HideBannerFacebookAd()
    {
        FAN.instance.HideBanner();
    }

    #endregion

}

[Serializable]
public enum AdsBannerPositions
{
    TOP,
    BOTTOM
}

[Serializable]
public struct ManageAdsHeirarchy
{
    public AdScenes adScenes;
    public AdsBannerPositions bannerPosition;
    public List<AdListing> adListing; 
}

[Serializable]
public enum AdsHeirarchy
{
    NoAd,
    UnityAd,
    AdmobAd,
    FacebookAd
}

[Serializable]
public struct AdCallHeirarchy
{
    public AdsHeirarchy firstCall;
    public AdsType secondCallAdType;
    public List<AdsHeirarchy> secondCall;
}

[Serializable]
public enum AdScenes
{
    LevelFailed,
    LevelComplete,
    RewardAdButton

}

[Serializable]
public enum AdsType
{
    Rewarded,
    Interstitial,
    Banner
}

[Serializable]
public enum AdsCount
{
    zero,one, two, three, four, five, six, seven, eight,nine,ten,none
}

[Serializable]
public struct AdsHeirarchyListing
{
    public List<AdListing> adListing;
}

[Serializable]
public struct AdListing
{
    public AdsCount adsCount;
    public AdsType firstCallAdType;
    public List<AdCallHeirarchy> adCall;
}



