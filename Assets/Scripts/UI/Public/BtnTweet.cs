using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TweetWithScreenShot;
using System.Linq;
using UnityEditor.Experimental.TerrainAPI;

public class BtnTweet : MonoBehaviour
{
    string cr = "\n";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
	{
        string strTweet;
        strTweet = "種目:" + PlayingManager.gameMaster.gameMode.name;
        strTweet += cr;
        strTweet += "記録:" + PlayingManager.playingManager.resultDistance.ToString("F2") + "m";
        strTweet += cr;
        var acquiredTitles = PlayingManager.playingManager.titleMonitor.acquiredTitles;
        var strTitle = "";
        var idTitle = acquiredTitles[acquiredTitles.Count - 1];
        if (acquiredTitles.Count > 0)
        {
            strTitle = PlayingManager.gameMaster.titles.allTitles.FirstOrDefault(t => t.id == idTitle).name;
            strTweet += "称号:" + strTitle;
            strTweet += cr;
        }
        StartCoroutine(TweetManager.TweetWithScreenShot(strTweet));
	}
}
