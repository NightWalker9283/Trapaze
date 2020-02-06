using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//プレイ画面。設定（歯車）ボタン
public class btnSettings : MonoBehaviour
{


    [SerializeField] Image wndSettings;
    [SerializeField] Transform wndBackGround;
    [SerializeField] PlayingManager playingManager;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenSettings);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenSettings()
    {

        wndBackGround.gameObject.SetActive(true);
        wndSettings.gameObject.SetActive(true);


        playingManager.SwitchPause(true);

    }
}
