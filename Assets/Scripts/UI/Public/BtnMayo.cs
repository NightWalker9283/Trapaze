using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnMayo : MonoBehaviour
{
    [SerializeField] GameObject wndBackGround,wndMayo;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenWndMayo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenWndMayo()
    {
        wndBackGround.SetActive(true);
        wndMayo.SetActive(true);
        PlayingManager.playingManager.SwitchPause(true);

    }
}
