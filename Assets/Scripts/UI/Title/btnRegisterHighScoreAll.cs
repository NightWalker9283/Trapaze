using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class btnRegisterHighScoreAll : MonoBehaviour
{
    [SerializeField] Transform canvasRegisterHiscoreAll,wndRegisterHighScoreAll,wndWaitMessage;
    
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
        wndRegisterHighScoreAll.gameObject.SetActive(false);
        wndWaitMessage.gameObject.SetActive(true);
		GameMaster.rankingManager.SaveRankingAll(()=> {
            wndWaitMessage.gameObject.SetActive(false);
            canvasRegisterHiscoreAll.gameObject.SetActive(false);
        });
	}
}
