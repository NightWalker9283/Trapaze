using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//ハイスコア登録ウィンドウのDONEボタン
public class btnRegisterHighScore : MonoBehaviour
{
    [SerializeField] Transform cnvsRanking,wndBackGround, wndRegisterHighScore, wndWaitMessage;
    [SerializeField] Button btnHighScore;
    public static RankingManager.Save_ranking_item save_Ranking_Item { get; set; }
    RecordData selectRecordData;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Done);
        selectRecordData = GameMaster.gameMaster.recordDatas.Find(x => x.game_mode_id == GameMaster.gameMaster.gameMode.id);
    }

    // Update is called once per frame
    void Done()
    {
        wndRegisterHighScore.gameObject.SetActive(false);
        wndWaitMessage.gameObject.SetActive(true);
        GameMaster.rankingManager.SaveRanking(selectRecordData,save_Ranking_Item,() => {
            wndWaitMessage.gameObject.SetActive(false);
            wndBackGround.gameObject.SetActive(false);
            cnvsRanking.gameObject.SetActive(true);
            btnHighScore.onClick.RemoveAllListeners();
            btnHighScore.onClick.AddListener(() =>
            {
                cnvsRanking.gameObject.SetActive(true);
            });

        });
    }

}
