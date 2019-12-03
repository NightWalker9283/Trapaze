using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnHighScore : MonoBehaviour
{
    [SerializeField] GameObject wndBackGround, wndHighScore;
    // Start is called before the first frame update
    void Start()
    {
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            wndBackGround.SetActive(true);
            wndHighScore.SetActive(true);
        }); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
