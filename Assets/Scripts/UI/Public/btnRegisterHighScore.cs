using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnRegisterHighScore : MonoBehaviour
{
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
        GameMaster.rankingManager.SaveRanking(selectRecordData, save_Ranking_Item);
    }
}
