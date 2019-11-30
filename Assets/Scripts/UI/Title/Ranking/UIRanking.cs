using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityScript.Scripting.Pipeline;

public class UIRanking : MonoBehaviour
{
    public static UIRanking uIRanking;
    [SerializeField] ToggleGroup tggMode, tggDirection, tggCategory;
    [SerializeField] Toggle tglModeOrigin;
    [SerializeField] Text txtName;
    [SerializeField] GameObject objRankingRecord;
    [SerializeField] GameObject ugRankingRecords;
    List<GameMode> gameModes;
    List<GameObject> lstRankingRecords;
    RankingManager rankingManager;
    // Start is called before the first frame update
    void Start()
    {
        uIRanking = this;
        gameModes = GameMaster.gameMaster.gameModes;
        lstRankingRecords = new List<GameObject>();
        rankingManager = GameMaster.rankingManager;
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
            tglModeElementObj.transform.GetComponent<ModeElementForRanking>().id = modeElement.id;

            tglModeElementObj.transform.GetComponentInChildren<Text>().text = (modeElement.name != null ? modeElement.name : "");
            tglModeElementObj.transform.parent = tggMode.transform;
            tglModeElementObj.transform.localScale = tglModeOrigin.transform.localScale;
            //tglModeElementObj.isOn = false;
            tglModeElementObj.GetComponent<Toggle>().onValueChanged.AddListener(tglModeElementObj.GetComponent<ModeElementForRanking>().OnChangeModeElement);
        }
        Destroy(tglModeOrigin.gameObject);
        tggMode.transform.GetComponentInChildren<Button>().transform.SetAsLastSibling();
        //tggMode.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        Debug.Log(tggMode.transform.GetChild(0).GetComponent<Toggle>().isOn);
        Debug.Log(tggMode.transform.GetChild(1).GetComponent<Toggle>().isOn);
        //CreateRankingViews();

        tggMode.GetComponentInChildren<Button>().transform.SetAsLastSibling();
    }

    

    void CreateRankingRecords(List<RankingRecord> lst)
    {
        if (lst.Count>0) 
        {
            txtName.text += "(RANK:" + lst.Count;
        }
        else
        {
            txtName.text += "(RANK: - )";
        }
        for (int i = 0; i < lst.Count; i++)
        {
            var objRecord = lst[i];
            CreateRankingRecord(objRecord.rank, objRecord.name, objRecord.distance, objRecord.timeSpan);
        }
    }

   

    public void CreateRankingViews()
    {
        ClearRankingRecords();
        var playerName = GameMaster.gameMaster.settings.name;
        var id = tggMode.ActiveToggles().FirstOrDefault().GetComponent<ModeElementForRanking>().id;
        var direction = tggDirection.ActiveToggles().FirstOrDefault().GetComponent<DirectionElementForRanking>().directionType;
        var category = tggCategory.ActiveToggles().FirstOrDefault().GetComponent<CategoryElementForRanking>().category;
        
        txtName.text = "Your Name:"+playerName;
        //rankingManager.fetchRank(playerName, id, direction,SetPlayerRank);

        switch (category)
        {
            case CategoryElementForRanking.categorys.TOP:
                rankingManager.getRankingTop(id, direction,CreateRankingRecords);
                break;
            case CategoryElementForRanking.categorys.RIVAL:
                rankingManager.getRankingNeighbors(playerName,id, direction,CreateRankingRecords);
                break;

            default:
                
                break;
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
        rankingManager.IsNameExistInRanking(playerName, id, direction, (bool isNameExist) =>
           {
               if (isNameExist)
               {
                   tglCatNeighbors.interactable = true;
               }
               else
               {
                   tglCatNeighbors.interactable = false;
               }
           });
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
