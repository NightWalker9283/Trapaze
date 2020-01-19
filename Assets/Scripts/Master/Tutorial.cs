using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Text txtSub;
    [SerializeField] Slider sldJUMP;
    [SerializeField] GameObject stage;
    [SerializeField] GameObject btnCommentRush, btnParachute, btnComment, btnCloseSub;
    [SerializeField] GameObject parentImgDark, joystick;
    [SerializeField] RectTransform rectImgUnMask;
    [SerializeField] Rigidbody rbTrapeze;
    [SerializeField] GameObject txtDistance;
    // Start is called before the first frame update

    void Start()
    {
        stage.SetActive(false);
        sldJUMP.interactable = false;
        txtDistance.SetActive(false);
        StartCoroutine(StepTutorial());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StepTutorial()
    {
        var isFinishStep = false;
        yield return new WaitForSeconds(1f);
        ShowMessage("ようこそチュートリアルへ！\nそれではブランコをこぐ練習から始めましょう。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("右画面のコントローラーをタッチし、\nそのまま好きな方向にドラッグしてみてください。", joystick);
        StartCoroutine(MonitorDraging(20f, () =>
         {
             isFinishStep = true;
         }));
        while (!isFinishStep) { yield return null; }
        ShowMessage("コントローラーの動きに合わせてお尻の位置が移動しましたね。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("それでは早速ブランコを漕いでみましょう。\n少しお尻を動かしてみてください。");
        rbTrapeze.isKinematic = false;
        isFinishStep = false;
        StartCoroutine(MonitorDraging(10f, () =>
         {
             isFinishStep = true;
         }));
        while (!isFinishStep) { yield return null; }

        ShowMessage("重心が動くことでブランコが揺れ始めましたね。\n本番では最初から少しブランコが揺れています。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("揺れの周期に合わせて、正しいタイミングでお尻の位置を\n上下に動かすことでブランコの揺れは大きくなります。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("漕ぎ方のコツは、ブランコが最下点に進むまではお尻を低く、\n再下点を過ぎ減速が始まったらお尻を高くすることです。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("タイミングが非常に重要です。上手く漕げない場合は\nお尻を上下させるタイミングを色々ずらしてみて、ベストなタイミングを見つけて下さい。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("また、上手に漕げるようになったら前後方向の位置も意識してみましょう。\n重心の位置を意識すると更に効率的に漕ぐことができます。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("それでは少しの間、自由に漕いでみましょう。");
        yield return new WaitForSeconds(30f);

        ShowMessage("うまく漕ぐことはできましたか？");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("次に弾幕コメントボタンの説明です。", btnCommentRush);
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("マヨネーズを持っていると、左下の弾幕コメントボタンが有効になります。", btnCommentRush);
        while (Time.timeScale == 0f) { yield return null; }
        yield return new WaitForSeconds(0.5f);
        PlayingManager.playingManager.mayoCount++;
        yield return null;

        ShowMessage("マヨネーズをひとつプレゼントします。\n弾幕コメントボタンを押してみましょう", btnCommentRush);
        isFinishStep = false;

        btnCommentRush.transform.Find("btnCommentRush").GetComponent<Button>().onClick.AddListener(() =>
        {
            isFinishStep = true;
            var btn = btnCommentRush.transform.Find("btnCommentRush").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(btn.GetComponent<btnCommentRush>().ShotComments);

        });
        while (!isFinishStep) { yield return null; }
        yield return new WaitForSeconds(10f);

        ShowMessage("マヨネーズでカロリーを補給し、うんこちゃんが大声を出すことで\n弾幕コメントが流れてきました。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("コメントがうんこちゃんの体に当たると少し加速します。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("実際のプレイでは時々空からマヨネーズが降ってきます。\nマヨネーズにタッチすると獲得することができます(広告が再生されます)");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("マヨネーズがあれば弾幕コメントはいつでも使うことができます。\n使い方で結果が大きく変わるので、色々研究してみてください。");
        while (Time.timeScale == 0f) { yield return null; }

        ShowMessage("では最後に、ジャンプの練習です。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("右下のスライダーを左にスライドすると、うんこちゃんが\nジャンプ待機状態に入ります。", sldJUMP.gameObject);
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("それではスライドしてみてください。", sldJUMP.gameObject);
        sldJUMP.GetComponent<Slider>().interactable = true;
        isFinishStep = false;
        StartCoroutine(MonitorSlidingSldJUMP(() =>
        {
            isFinishStep = true;
        }));
        while (!isFinishStep) { yield return null; }
        yield return new WaitForSeconds(0.5f);

        ShowMessage("待機状態に入りました。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("この状態で画面上を素早くドラッグすると、ドラッグした方向に\nうんこちゃんがジャンプします（右画面のうんこちゃんが向いている向きが基準）");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("良いタイミングでドラッグしてみましょう。");
        while (PlayingManager.playingManager.Stat!=PlayingManager.Stat_global.fly) { yield return null; }
        yield return new WaitForSeconds(2f);
        PlayingManager.playingManager.playerControlPoint.drag = 10f;
        
        ShowMessage("うんこちゃんがジャンプしました！");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("ジャンプ中は左下のパラシュートボタンでパラシュートを開けます。\nうんこちゃんが下降中のみ押すことができます。",btnParachute);

        while (!btnComment.activeSelf) { yield return null; }
        yield return new WaitForSeconds(1.5f);
        ShowMessage("パラシュートが開きました。パラシュートは下降を遅くすることが\nできますが、横方向にもブレーキがかかります。");
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("実際のプレイでパラシュートを開くタイミングによって飛距離に\nどのように影響するか色々試してみてください。");
        while (Time.timeScale == 0f) { yield return null; }

        ShowMessage("パラシュートを開くとパラシュートボタンがコメントボタンに変化します。",btnComment);
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("コメントボタンを押すたびにひとつコメントが流れます。\n弾幕コメントと同じように、うんこちゃんの体に当たると加速します。", btnComment);
        while (Time.timeScale == 0f) { yield return null; }
        ShowMessage("沢山コメントを流して飛距離を稼ぎましょう！", btnComment);
        while (Time.timeScale == 0f) { yield return null; }
        isFinishStep = false;
        btnComment.GetComponent<Button>().onClick.AddListener(() =>
        {
            isFinishStep = true;            
        });
        while (!isFinishStep) { yield return null; }
        yield return new WaitForSeconds(10f);
        ShowMessage("お疲れ様でした。これでチュートリアルは終了です。\n沢山練習してランキング上位を目指しましょう！");
        while (Time.timeScale == 0f) { yield return null; }
        PlayingManager.gameMaster.Title();
        Destroy(FindObjectOfType<GameMaster>().gameObject);
        Destroy(FindObjectOfType<PlayingManager>().gameObject);

    }

    IEnumerator MonitorSlidingSldJUMP(System.Action callback)
    {
        var sld = sldJUMP.GetComponent<Slider>();
        while (sldJUMP.value < 1f) { yield return null; }
        callback();
    }

    IEnumerator MonitorDraging(float distance, System.Action callback)
    {
        var controller = joystick.GetComponent<SimpleTouchController>();
        float totalDistance = 0f;
        Vector2 posOldTouch = controller.GetTouchPosition;
        while (totalDistance < distance)
        {
            totalDistance += (controller.GetTouchPosition - posOldTouch).magnitude;
            posOldTouch = controller.GetTouchPosition;
            yield return null;
        }
        callback();
    }

    IEnumerator MonitorDragingToBack(float wait, System.Action callback)
    {
        var controller = joystick.GetComponent<SimpleTouchController>();

        while (controller.GetTouchPosition.x <= 0.8f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(wait);
        callback();
    }
    IEnumerator MonitorDragingToCenter(float wait, System.Action callback)
    {
        var controller = joystick.GetComponent<SimpleTouchController>();

        while (controller.GetTouchPosition.x <= 0.8f)
        {
            yield return null;
        }
        while (controller.GetTouchPosition.x >= 0f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(wait);
        callback();
    }
    IEnumerator MonitorRowing()
    {
        var rbControllPoint = PlayingManager.playingManager.playerControlPoint;
        yield return null;

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
        rectImgUnMask.sizeDelta = sizeHighLight * ratio * rectHighLight.localScale;
        if (parent.name == "Canvas_player")
        {
            posTopViewPort = new Vector3(0.7f + posLocalViewPort.x * 0.3f, posLocalViewPort.y);
        }
        else if (parent.name == "Canvas_public")
        {
            posTopViewPort = new Vector3(posLocalViewPort.x * PlayingManager.playingManager.cmrPublic.rect.width, posLocalViewPort.y);
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
