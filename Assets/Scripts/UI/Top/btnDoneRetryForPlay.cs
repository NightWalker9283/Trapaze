using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//プレイ画面設定ウィンドウから呼び出すリトライウィンドウのDONEボタン
public class btnDoneRetryForPlay : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Done);
    }

    // Update is called once per frame
    void Done()
    {
        PlayingManager.gameMaster.Play();
        Destroy(FindObjectOfType<PlayingManager>().gameObject);
    }
}
