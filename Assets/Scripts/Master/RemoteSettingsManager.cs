using UnityEngine;
using System.Collections;

public class RemoteSettingsManager : MonoBehaviour
{
    static public RemoteSettingsManager rSM;
    public string latestVer;
    public string rankingName;
    public string rankingPeriod;

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

        RemoteSettings.ForceUpdate();
    }
    // Create a function to set your variables to their keyed values:
    void ApplyRemoteSettings(bool wasUpdatedFromServer, bool settingsChanged, int serverResponse)
    {

        Debug.Log("New settings loaded this session; update values accordingly.");
        latestVer = RemoteSettings.GetString("LatestVer");
        rankingName = RemoteSettings.GetString("RankingName");
        rankingPeriod = RemoteSettings.GetString("RankingPeriod");
        
        busy = false;
    }
}