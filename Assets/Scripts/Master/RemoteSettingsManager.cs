using UnityEngine;
using System.Collections;

//最新Ver.やランキング参照先を通知するためにRemoteSettingsを利用
//ただし現状ランキング参照先はNCMBのクラステーブルから取得しておりこちらは未使用。
public class RemoteSettingsManager : MonoBehaviour
{
    static public RemoteSettingsManager rSM;
    public string latestVer;　//ストアで公開されているアプリの最新Ver.
    public string rankingName;　//ランキング参照先のNCMBクラス名
    public string rankingPeriod;　//現在のランキングの集計期間。将来的にランキング定期リセット実装時に使用予定

    public bool busy = false;

    void Awake()
    {
        rSM = this;

        // Fetch configuration setting from the remote service: 
    }
    private void Start()
    {
        RemoteSettings.Completed += ApplyRemoteSettings;
        FetchSettings();

    }

    public void FetchSettings()
    {
        busy = true;

        RemoteSettings.ForceUpdate();　//RemoteSettingsから取得
    }

    // Create a function to set your variables to their keyed values:
    void ApplyRemoteSettings(bool wasUpdatedFromServer, bool settingsChanged, int serverResponse)
    {

        Debug.Log("New settings loaded this session; update values accordingly.");

        //RemoteSettings側でプラットフォームの判別が機能しない場合があるため端末側で切り分け
#if UNITY_IOS
        latestVer = RemoteSettings.GetString("LatestVer_iOS");
#elif UNITY_ANDROID
        latestVer = RemoteSettings.GetString("LatestVer_Android");
#endif

        rankingName = RemoteSettings.GetString("RankingName");
        rankingPeriod = RemoteSettings.GetString("RankingPeriod");
        
        busy = false;
    }
}