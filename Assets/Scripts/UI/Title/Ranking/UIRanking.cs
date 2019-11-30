using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

        foreach (Transform item in ugRankingRecords.transform)
        {
            lstRankingRecords.Add(item.gameObject);
        }


        for (int i = 0; i < gameModes.Count-1; i++)
        {

            var tglModeElementObj = Instantiate(tglModeOrigin);
            tglModeElementObj.transform.parent = tggMode.transform;
            tglModeElementObj.transform.localScale = tglModeOrigin.transform.localScale;
         
        }
        tggMode.GetComponentInChildren<Button>().transform.SetAsLastSibling();
        for (int i = 0; i < gameModes.Count; i++)
        {

            var tglModeElementObj = tggMode.transform.GetChild(i);
            var modeElement = gameModes[i];
            tglModeElementObj.GetComponent<ModeElementForRanking>().id = modeElement.id;

            tglModeElementObj.GetComponentInChildren<Text>().text = (modeElement.name != null ? modeElement.name : "");
            //tglModeElementObj.isOn = false;
            tglModeElementObj.GetComponent<Toggle>().onValueChanged.AddListener(tglModeElementObj.GetComponent<ModeElementForRanking>().OnChangeModeElement);
        }
        
        tggMode.transform.GetComponentInChildren<Button>().transform.SetAsLastSibling();
        tggMode.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        
     
       
        //CreateRankingViews();

    }

    

    void CreateRankingRecords(List<RankingRecord> lst)
    {
        int rank=0;
        foreach (var item in lst)
        {
            if (item.name == GameMaster.gameMaster.settings.name) rank = item.rank;
        }
        if (rank>0) 
        {
            txtName.text += "(RANK:" + rank+")";
        }
        else
        {
            txtName.text += "(RANK: - )";
        }
        for (int i = 0; i < lst.Count; i++)
        {
            var objRecord = lst[i];
            CreateRankingRecord(lstRankingRecords[i],objRecord.rank, objRecord.name, objRecord.distance, objRecord.timeSpan);
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
        foreach (var item in lstRankingRecords)
        {
            item.transform.Find("txtRank").GetComponent<Text>().text = "";
            item.transform.Find("txtRankerName").GetComponent<Text>().text = "";
            item.transform.Find("txtDistance").GetComponent<Text>().text = "";
            item.transform.Find("txtTimeSpan").GetComponent<Text>().text = "";
        }
        
    }

    public void ResetAllToggles()
    {
        var tgl = tggDirection.transform.Find("tglDirectionMax").GetComponent<Toggle>();
        tgl.enabled=false;
        tgl.isOn = true;
        tgl.enabled = true;

        tgl = tggCategory.transform.Find("tglCatTop").GetComponent<Toggle>();
        tgl.enabled = false;
        tgl.isOn = true;
        tgl.enabled = true;
    }

    public void ResetCategoryToggles()
    {
       

        var tgl = tggCategory.transform.Find("tglCatTop").GetComponent<Toggle>();
        tgl.enabled = false;
        tgl.isOn = true;
        tgl.enabled = true;
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

    private void CreateRankingRecord(GameObject objRecord, int rank,string rankerName,float distance,float timeSpan)
    {
        
        
        objRecord.transform.Find("txtRank").GetComponent<Text>().text = rank.ToString();
        objRecord.transform.Find("txtRankerName").GetComponent<Text>().text = rankerName;
        objRecord.transform.Find("txtDistance").GetComponent<Text>().text = distance.ToString("F1") + "m";
        objRecord.transform.Find("txtTimeSpan").GetComponent<Text>().text= (int)(timeSpan / 60) + ":" + ((int)(timeSpan % 60)).ToString("D2");
        lstRankingRecords.Add(objRecord.gameObject);
    }
}
