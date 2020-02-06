using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
//ロード画面用テキスト
public class TxtLoading : MonoBehaviour
{

    Text txt;
    // Start is called before the first frame update
    void Awake()
    {
        txt = GetComponent<Text>();
        
    }
    private void OnEnable()
    {
        StartCoroutine(UpdateText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdateText()
    {
        float time = 0;
        var strLoading = "Now Loading";
        var strAdd = ".";
        while (txt!=null && gameObject.activeInHierarchy)
        {
            txt.text = strLoading + strAdd;
            if ((int)(time + Time.deltaTime) != (int)time)
            {
                strAdd += ".";
            }
            time += Time.deltaTime;
            if (strAdd.Length > 3) strAdd = ".";
            yield return null;
        }
    }
}
