using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Text txtSub;
    [SerializeField] Slider sldJUMP;
    [SerializeField] GameObject btnCommentRush, btnParachute, btnComment, btnCloseSub;
    [SerializeField] GameObject parentImgDark, joystick;
    [SerializeField] RectTransform rectImgUnMask;
    // Start is called before the first frame update

    void Start()
    {

        sldJUMP.interactable = false;

        StartCoroutine(StepTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(btnCommentRush.GetComponent<RectTransform>().localPosition);
    }

    IEnumerator StepTutorial()
    {
        yield return new WaitForSeconds(1f);
        ShowMessage("ようこそチュートリアルへ！\nそれではブランコをこぐ練習から始めましょう。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("右画面のコントローラーをタッチし、そのまま好きな方向にドラッグしてみてください。", joystick);
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("十分に勢いがついたらスライダーを左にスライドしてください。\nジャンプ待機状態に入ります。", sldJUMP.gameObject);
    }
    void ShowMessage(string message)
    {
        parentImgDark.SetActive(true);
        PlayingManager.playingManager.SwitchPause(true);
        StartCoroutine(delayShowMessage());
        IEnumerator delayShowMessage()
        {
            yield return new WaitForSecondsRealtime(0.4f);
            txtSub.gameObject.SetActive(true);
            txtSub.text = message;
            float alpha = 0f;
            while (alpha <= 1f)
            {
                txtSub.color = new Color(txtSub.color.r, txtSub.color.g, txtSub.color.b, Mathf.Clamp01(alpha));
                alpha += 0.1f;
                yield return null;
            }
            btnCloseSub.gameObject.SetActive(true);
        }
    }
    void ShowMessage(string message, GameObject highLight)
    {
        var parent = highLight.transform.parent;
        while (parent.name != "Canvas_player" && parent.name != "Canvas_public")
        {
            parent = parent.parent;
        }
        Vector2 sizeParentCanvas = parent.GetComponent<RectTransform>().sizeDelta;
        RectTransform rectHighLight = highLight.GetComponent<RectTransform>();
        Vector2 sizeHighLight = rectHighLight.sizeDelta;
        Vector2 pivotHighLight = rectHighLight.pivot;
        Vector3 posHighLight = rectHighLight.localPosition;



        Vector3 posLocalViewPort = new Vector3((posHighLight.x - (pivotHighLight.x - 0.5f) * sizeHighLight.x + sizeParentCanvas.x / 2) / sizeParentCanvas.x,
                                               (posHighLight.y - (pivotHighLight.y - 0.5f) * sizeHighLight.y + sizeParentCanvas.y / 2) / sizeParentCanvas.y
                                               );
        Vector3 posTopViewPort = new Vector3();
        
        var ratio = PlayingManager.playingManager.cvsTop.GetComponent<RectTransform>().sizeDelta.y / sizeParentCanvas.y;
//        var ratioVector2 = PlayingManager.playingManager.cvsTop.GetComponent<RectTransform>().sizeDelta / (sizeParentCanvas * ratio);
        rectImgUnMask.sizeDelta = sizeHighLight*ratio*rectHighLight.localScale;
        if (parent.name == "Canvas_player")
        {
            posTopViewPort = new Vector3(0.7f + posLocalViewPort.x * 0.3f, posLocalViewPort.y);
        }
        else if (parent.name == "Canvas_public")
        {
            posTopViewPort = new Vector3(posLocalViewPort.x * 0.7f, posLocalViewPort.y);
        }
        Vector2 sizeCvsTop = PlayingManager.playingManager.cvsTop.GetComponent<RectTransform>().sizeDelta;
        Vector2 sizeImgUnMask = rectImgUnMask.sizeDelta;
        Vector2 pivotImgUnMask = rectImgUnMask.pivot;


        rectImgUnMask.localPosition = new Vector3(posTopViewPort.x * sizeCvsTop.x - sizeCvsTop.x / 2 + (pivotImgUnMask.x - 0.5f) * sizeImgUnMask.x,
                                                  posTopViewPort.y * sizeCvsTop.y - sizeCvsTop.y / 2 + (pivotImgUnMask.y - 0.5f) * sizeImgUnMask.y
                                                  );
        rectImgUnMask.gameObject.SetActive(true);
        ShowMessage(message);
    }
}
