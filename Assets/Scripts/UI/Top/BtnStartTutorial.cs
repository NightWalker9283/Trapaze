using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
//トレーニングモード。チュートリアル開始ボタン
public class BtnStartTutorial : MonoBehaviour
{
    Button btn;
    bool beStat;
   

    // Start is called before the first frame update
    void Start()
    {
        beStat = !PlayingManager.gameMaster.isTutorial;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        PlayingManager.gameMaster.isTutorial = beStat;
        PlayingManager.gameMaster.Play();
        Destroy(FindObjectOfType<PlayingManager>().gameObject);
    }

    
}
