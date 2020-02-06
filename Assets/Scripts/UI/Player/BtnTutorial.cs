using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//トレーニングモード。チュートリアル開始ボタン
public class BtnTutorial : MonoBehaviour
{


   
    [SerializeField] Transform wndBackGround,wndTutorial;
   

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenWndTutorial);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenWndTutorial()
    {

        wndBackGround.gameObject.SetActive(true);
        wndTutorial.gameObject.SetActive(true);


        PlayingManager.playingManager.SwitchPause(true);

    }
}
