using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiGameMode : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (PlayingManager.playingManager.isTutorial)
        {
            GetComponentInChildren<Text>().text = "チュートリアル";
        }
        else
        {
            GetComponentInChildren<Text>().text = PlayingManager.gameMaster.gameMode.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
