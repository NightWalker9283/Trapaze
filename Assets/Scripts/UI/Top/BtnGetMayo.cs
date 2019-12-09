using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnGetMayo : MonoBehaviour
{
    [SerializeField] GameObject wndBackGround, wndMayo;
    [SerializeField] Mayo mayo;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(GetMayo);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetMayo()
    {
        wndMayo.SetActive(false);
        wndBackGround.SetActive(false);
        var audioSource= GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        PlayingManager.playingManager.mayoCount++;
        mayo.Finish();
        PlayingManager.playingManager.SwitchPause(false);

    }
}
