using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uiElapseTime : MonoBehaviour
{
	float nowTime;
    TextMeshProUGUI textElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        textElapsedTime = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;
        textElapsedTime.text = ((int)(nowTime / 60f)).ToString("D2") + ":" + ((int)(nowTime % 60f)).ToString("D2");
    }   
}
