using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uiElapseTime : MonoBehaviour
{

    TextMeshProUGUI textElapsedTime;
    float nowTime, timeLimit;


    // Start is called before the first frame update
    void Start()
    {
        textElapsedTime = GetComponent<TextMeshProUGUI>();
        timeLimit = PlayingManager.gameMaster.gameMode.timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        nowTime = PlayingManager.playingManager.elapseTime;
        textElapsedTime.text="<mspace=0.6em>";
        if (timeLimit > 0f)
        {
            if (nowTime <= timeLimit)
            {
                textElapsedTime.text += "<color=black>";
            }
            else
            {
                textElapsedTime.text += "<color=red>";
            }
        }
        else
        {
            textElapsedTime.text += "<color=black>";
        }
        textElapsedTime.text += ((int)(nowTime / 60f)).ToString("D2") + ":" + ((int)(nowTime % 60f)).ToString("D2");
        textElapsedTime.text += "</color><color=#cc5500> / ";
        if (timeLimit > 0f)
        {
            textElapsedTime.text +=  ((int)(timeLimit / 60f)).ToString("D2") + ":" + ((int)(timeLimit % 60f)).ToString("D2");
        }
        else
        {
            textElapsedTime.text += "∞";
        }
    }
}
