using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//リザルト画面。取得称号一覧ウィンドウのタイトルバー。タッチするたびに一覧表示部がトグル動作で表示・非表示
public class TitleBar : MonoBehaviour
{
    [SerializeField] Mask mask;
    [SerializeField] CanvasGroup cvsgtitleElements;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        if (mask.showMaskGraphic)
        {
            mask.showMaskGraphic = false;
            cvsgtitleElements.alpha=0f;
        }
        else
        {
            mask.showMaskGraphic = true;
            cvsgtitleElements.alpha=1f;
        }

    }

}
