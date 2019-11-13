using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uiElapseTime : MonoBehaviour
{
	
    TextMeshProUGUI textElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        textElapsedTime = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var nowTime=PlayingManager.playingManager.elapseTime;
        textElapsedTime.text = ((int)(nowTime / 60f)).ToString("D2") + ":" + ((int)(nowTime % 60f)).ToString("D2");
    }   
}
