using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnPause : MonoBehaviour
{
    
    [SerializeField] Image image;
    [SerializeField] Transform wndSettings;
    [SerializeField] Sprite sprRun, sprPause;
    [SerializeField] PlayingManager playingManager;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SwitchImage);
    }

    // Update is called once per frame
    void SwitchImage()
    {
        if (PlayingManager.playingManager.stat != PlayingManager.stat_global.pause)
        {
            image.sprite = sprRun;
            wndSettings.gameObject.SetActive(true);
            
            
        }
        else if (PlayingManager.playingManager.stat == PlayingManager.stat_global.pause)
        {
            image.sprite = sprPause;
            wndSettings.gameObject.SetActive(false);
           
        }
        playingManager.SwitchPause();
    }

}
