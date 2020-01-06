﻿using UnityEngine;
using System;
using System.Collections;
using System.Xml;

using UnityEngine.Networking;


public class VersionChecker : MonoBehaviour
{
    string _storeUrl = "";


    void Start()
    {
        CurrentVersionCheck();

        if (LoadInvalidVersionUpCheck())
        {
            return;
        }
#if UNITY_IOS
        VersionCheckIOS();
#elif UNITY_ANDROID
        VersionCheckAndroid();
#endif
    }

    const string INVALID_VERSION_UP_CHECK_KEY = "InvalidVersionUpCheck";

    bool LoadInvalidVersionUpCheck()
    {
        return PlayerPrefs.GetInt(INVALID_VERSION_UP_CHECK_KEY, 0) != 0;
    }

    void SaveInvalidVersionUpCheck(bool invalid)
    {
        PlayerPrefs.SetInt(INVALID_VERSION_UP_CHECK_KEY, invalid ? 1 : 0);
    }

    const string CURRENT_VERSION_CHECK_KEY = "CurrentVersionCheck";

    void CurrentVersionCheck()
    {
        var version = PlayerPrefs.GetString(CURRENT_VERSION_CHECK_KEY, "");
        if (version != Application.version)
        {
            PlayerPrefs.SetString(CURRENT_VERSION_CHECK_KEY, Application.version);
            SaveInvalidVersionUpCheck(false);
        }
    }

    void VersionCheckIOS()
    {
        var url = string.Format("https://itunes.apple.com/lookup?bundleId={0}", Application.identifier);
        GameMaster.rankingManager.GetAppLatestVer((string ver) =>
        {

            if (ver != "")
            {
                if (VersionComparative(ver))
                {
                    ShowUpdatePopup(url);
                }
            }

        });
    }

    void VersionCheckAndroid()
    {
        var url = string.Format("https://play.google.com/store/apps/details?id={0}", Application.identifier);

        GameMaster.rankingManager.GetAppLatestVer((string ver) =>
        {
            
            if (ver != "")
            {
                if (VersionComparative(ver))
                {
                    ShowUpdatePopup(url);
                }
            }

        });
    }

    bool VersionComparative(string storeVersionText)
    {
        if (string.IsNullOrEmpty(storeVersionText))
        {
            return false;
        }
        try
        {
            var storeVersion = new System.Version(storeVersionText);
            var currentVersion = new System.Version(Application.version);

            if (storeVersion.CompareTo(currentVersion) > 0)
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("{0} VersionComparative Exception caught.", e);
        }

        return false;
    }

    void ShowUpdatePopup(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        _storeUrl = url;
#if !UNITY_EDITOR
		if (Application.systemLanguage == SystemLanguage.Japanese)
		{
			string title = "アプリの更新があります";
			string message = "更新しますか？";
			string yes = "はい";
			string no = "いいえ";
			Dialog dialog = new Dialog(title, message, yes, no);
			dialog.OnComplete += OnPopUpClose;
		}
		else
		{
			string title = "There is an update of the application";
			string message = "Do you want to update the application?";
			string yes = "Yes";
			string no = "No";
			Dialog dialog = new Dialog(title, message, yes, no);

			dialog.OnComplete += OnPopUpClose;
		}
#endif
    }


    private void OnPopUpClose(Dialog.DialogResult result)
    {
        switch (result)
        {
            case Dialog.DialogResult.YES:
                Application.OpenURL(_storeUrl);
                break;
            case Dialog.DialogResult.NO:
                SaveInvalidVersionUpCheck(true);
                break;
        }
    }
}


[Serializable]
public class AppLookupData
{
    public int resultCount;
    public AppLookupResult[] results;

}

[Serializable]
public class AppLookupResult
{
    public string version;
    public string trackViewUrl;
}