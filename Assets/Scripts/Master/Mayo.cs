using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイ中に降ってくるマヨネーズの制御
public class Mayo : MonoBehaviour
{
    Button btn; //マヨの位置をキャンバスに転写したボタンオブジェクト
    [SerializeField] RectTransform virtualMayo;　//マヨの位置をキャンバスに転写したボタンオブジェクトのRectTransform
    [SerializeField] GameObject particles; //キラキラ演出用パーティクル
    [SerializeField] float deadTime = 20f; //次のマヨのドロップ判定が開始するまでの時間（deadTimeが0になると一定時間ごとに確率でマヨドロップ）
    Coroutine crtnJudge = null, crtnDrop = null;
    Camera cmrPublic, cmrUI;
    Canvas cvsPublic;
    Rect rect = new Rect(0, -0.5f, 1, 1.8f); //マヨが画面外に出たことを判定するためのRect

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayingManager.gameMaster.gameMode.enableDropMayo)
        {
            gameObject.SetActive(false);
        }
        cmrPublic = PlayingManager.playingManager.cmrPublic;
        cmrUI = PlayingManager.playingManager.cmrUI;
        cvsPublic = PlayingManager.playingManager.cvsPublic;
        btn = virtualMayo.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (crtnDrop == null &&
            PlayingManager.playingManager.mayoCount < 3 &&
            PlayingManager.playingManager.Stat == PlayingManager.Stat_global.play &&
            Time.timeScale > 0)
        {


            if (deadTime > 0f)
            {
                deadTime -= Time.deltaTime;
            }
            else
            {
                if (crtnJudge == null) crtnJudge = StartCoroutine(JudgeStartMayo());
            }

        }
        if (crtnJudge != null && PlayingManager.playingManager.Stat != PlayingManager.Stat_global.play)
        {
            StopCoroutine(crtnJudge);
            crtnJudge = null;
        }
    }

    //一定間隔ごとに一定確率でマヨドロップを開始
    IEnumerator JudgeStartMayo()
    {
        bool isStart = false;
        while (!isStart)
        {
            if (Random.Range(0, 6) == 0)
            {
                crtnDrop = StartCoroutine(DropMayo());

                isStart = true;
            }
            yield return new WaitForSeconds(5);
        }
        crtnJudge = null;
    }

    //マヨドロップ処理
    IEnumerator DropMayo()
    {
        GetComponent<MeshRenderer>().enabled = true;
        btn.interactable = true;
        particles.SetActive(true);
        GetComponent<AudioSource>().enabled = true;

        Vector3 posViewportPublic;
        var posWorldMayo = cmrPublic.ViewportToWorldPoint(new Vector3(0.2f, 1.05f));
        var isIgnore = false;
        var direction = new Vector3(0, -0.01f, 0);
        var canvasRectsizeDeta = cvsPublic.GetComponent<RectTransform>().sizeDelta;
        transform.position = new Vector3(0f, posWorldMayo.y, posWorldMayo.z);
        while (!isIgnore)
        {
            if (Time.timeScale > 0)
            {
                //画面上のマヨの位置からキャンバス上の見えないボタンの位置を計算
                transform.position += direction;
                posViewportPublic = cmrPublic.WorldToViewportPoint(transform.position);
               
                virtualMayo.localPosition = new Vector2(
                   posViewportPublic.x*canvasRectsizeDeta.x-canvasRectsizeDeta.x/2f,
                   posViewportPublic.y*canvasRectsizeDeta.y-canvasRectsizeDeta.y/2f
                   );

                if (!rect.Contains(posViewportPublic)) isIgnore = true;
            }

            yield return null;
        }
        deadTime = 180f; //取得、スルー後にデッドタイムを設ける
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
        btn.interactable = false;
        particles.SetActive(false);
        if (crtnDrop != null) crtnDrop = null;
    }

    //マヨ取得後やスルー後の後処理
    public void Finish()
    {
        if (crtnDrop != null)
        {
            StopCoroutine(crtnDrop);
            crtnDrop = null;
        }
        GetComponent<MeshRenderer>().enabled = false;
        btn.interactable = false;
        particles.SetActive(false);
        GetComponent<AudioSource>().enabled = false;
        deadTime = 180f;
    }
}
