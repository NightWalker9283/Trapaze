using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//現在時刻表示UI
public class uiTime : MonoBehaviour
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
        
        textElapsedTime.text = DateTime.Now.ToString("HH:mm");
    }
}