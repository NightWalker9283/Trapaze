using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityScript.Scripting.Pipeline;

public class UIRanking : MonoBehaviour
{
    [SerializeField] ToggleGroup tggMode, tggDirection, tggCategory;
    [SerializeField] Toggle tglModeOrigin;
    [SerializeField] Text txtName;
    [SerializeField] GameObject objRankingRecord;
    [SerializeField] GameObject ugRankingRecords;
    List<GameMode> gameModes;
    List<GameObject> lstRankingRecords;
    RankingManager rankingManager= GameMaster.rankingManager;
    // Start is called before the first frame update
    void Start()
    {
        gameModes = GameMaster.gameMaster.gameModes;
        lstRankingRecords = new List<GameObject>();
        CreateRankingUI();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CreateRankingUI()
    {


        for (int i = 0; i < gameModes.Count; i++)
        {

            var tglModeElementObj = Instantiate(tglModeOrigin);
            var modeElement = gameModes[i];
            tglModeElementObj.transform.GetComponent<ModeElementForRecords>().id = modeElement.id;

            tglModeElementObj.transform.GetComponentInChildren<Text>().text = (modeElement.name != null ? modeElement.name : "");
            tglModeElementObj.transform.parent = tggMode.transform;
            tglModeElementObj.transform.localScale = tglModeOrigin.transform.localScale;
            tglModeElementObj.isOn = false;
            tglModeElementObj.GetComponent<Toggle>().onValueChanged.AddListener(tglModeElementObj.GetComponent<ModeElementForRanking>().OnChangeModeElement);
        }
        tglModeOrigin.GetComponent<Toggle>().onValueChanged.AddListener(tglModeOrigin.GetComponent<ModeElementForRanking>().OnChangeModeElement);
        tglModeOrigin.isOn = true;
        JudgmentTggCategory();
        CreateRankingRecords();

        tggMode.GetComponentInChildren<Button>().transform.SetAsLastSibling();
    }

    public void CreateRankingRecords()
    {
        ClearRankingRecords();
        var playerName = GameMaster.gameMaster.settings.name;
        var id = tggMode.ActiveToggles().FirstOrDefault().GetComponent<ModeElementForRanking>().id;
        var direction = tggDirection.ActiveToggles().FirstOrDefault().GetComponent<DirectionElementForRanking>().directionType;
        var category = tggCategory.ActiveToggles().FirstOrDefault().GetComponent<CategoryElementForRanking>().category;
        List<RankingRecord> list=null;
        txtName.text = playerName;
        if (rankingManager.isNameExistInRanking(playerName, id, direction)) 
        {
            txtName.text += "(RANK:" + rankingManager.fetchRank(playerName, id, direction).ToString();
        }
        else
        {
            txtName.text += "(RANK: - )";
        }

        switch (category)
        {
            case CategoryElementForRanking.categorys.TOP:
                list = rankingManager.getRankingTop(id, direction);
                break;
            case CategoryElementForRanking.categorys.RIVAL:
                list = rankingManager.getRankingNeighbors(id, direction);
                break;

            default:
                
                break;
        }

        for (int i=0;i<list.Count;i++)
        {
            var objRecord = list[i];
            CreateRankingRecord(objRecord.rank, objRecord.name,objRecord.distance, objRecord.timeSpan);
        }


    }
    private void ClearRankingRecords()
    {
        lstRankingRecords = new List<GameObject>();
    }

    public void ResetToggles()
    {

        tggDirection.transform.Find("tglDirectionMax").GetComponent<Toggle>().isOn = true;
        tggCategory.transform.Find("tglCatTop").GetComponent<Toggle>().isOn = true;
    }

    public void JudgmentTggCategory()
    {
        var tglCatNeighbors = tggCategory.transform.Find("tglCatNeighbors").GetComponent<Toggle>();
        var playerName = GameMaster.gameMaster.settings.name;
        var id = tggMode.ActiveToggles().FirstOrDefault().GetComponent<ModeElementForRanking>().id;
        var direction = tggDirection.ActiveToggles().FirstOrDefault().GetComponent<DirectionElementForRanking>().directionType;
        if (rankingManager.isNameExistInRanking(playerName,id,direction))
        {
            tglCatNeighbors.interactable = true;
        }
        else
        {
            tglCatNeighbors.interactable = false;
        }
    }

    private void CreateRankingRecord(int rank,string rankerName,float distance,float timeSpan)
    {
        var objRecord = Instantiate(tglModeOrigin);
        objRecord.transform.Find("txtRank").GetComponent<Text>().text = rank.ToString();
        objRecord.transform.Find("txtRankerName").GetComponent<Text>().text = rankerName;
        objRecord.transform.Find("txtDistance").GetComponent<Text>().text = distance.ToString("F1") + "m";
        objRecord.transform.Find("txtTimeSpan").GetComponent<Text>().text= (int)(timeSpan / 60) + ":" + ((int)(timeSpan % 60)).ToString("D2") + ")";
        lstRankingRecords.Add(objRecord.gameObject);
    }
}
