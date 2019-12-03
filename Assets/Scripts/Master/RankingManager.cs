using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using NCMB;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class RankingManager : MonoBehaviour
{
    public bool isBusy, isNameExist;
    float TIME_OUT = 3000f;
    string rankingClassName = "RankingData";
    string usersClassName = "Users";

    public List<RecordData> recordDatas
    {
        get
        {
            return GameMaster.gameMaster.recordDatas;
        }
    }

    Settings settings
    {
        get
        {
            return GameMaster.gameMaster.settings;
        }
    }

    public enum Save_ranking_item { SAVE_RANKING_HIGH, SAVE_RANKING_LOW }
    bool isNameExistFetched = false;
    bool isScoreFetched = false;
    bool isRankFetched = false;
    public delegate void CallbackVoid();
    public delegate void CallbackBool(bool flg);
    public delegate void CallbackInt(int num);
    public delegate void CallbackRecordsList(List<RankingRecord> lst);
    int currentRank;

    private void Start()
    {

    }

    private void Update()
    {

    }
    public void SaveNewUser(string name, CallbackBool callback)
    {
        isNameExist = false;
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(usersClassName);
        query.WhereEqualTo("name", name); // 古い名前でデータを絞る
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            { //検索成功したら
                if (objList.Count > 0)
                { // 1個以上あれば
                    isNameExist = true;
                }
                else
                {
                    NCMBObject obj = new NCMBObject(usersClassName);
                    obj["name"] = name; // 新しい名前にする
                    obj.SaveAsync((NCMBException e2) =>
                    {
                        if (e2 == null)
                        {
                            GameMaster.gameMaster.Save();
                        }
                    });
                }
            }
            callback(isNameExist);
        });
        return;

    }

    public void RenameUser(string previousName, string newName, CallbackBool callback)
    {
        isNameExist = false;
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(usersClassName);
        query.WhereEqualTo("name", previousName); // 古い名前でデータを絞る
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            { //検索成功したら
                if (objList.Count > 0)
                { // 1個以上あれば
                    for (int i = 0; i < objList.Count; i++)
                    {
                        objList[i]["name"] = newName; // 新しい名前にする
                        objList[i].SaveAsync((NCMBException e2) =>
                        {
                            if (e2 == null)
                            {
                                GameMaster.gameMaster.Save();
                                isNameExist = true;
                            }
                        });
                    }
                }
            }
            callback(isNameExist);
        });

        query = new NCMBQuery<NCMBObject>(rankingClassName);
        query.WhereEqualTo("name", previousName); // 古い名前でデータを絞る
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            { //検索成功したら
                if (objList.Count > 0)
                { // 1個以上あれば
                    for (int i = 0; i < objList.Count; i++)
                    {
                        objList[i]["name"] = newName; // 新しい名前にする
                        objList[i].SaveAsync((NCMBException e2) =>
                        {
                            if (e2 == null)
                            {

                            }
                        });
                    }
                }
            }
        });
        return;
    }

    public void SaveRankingAll(CallbackVoid callback)
    {
        int cntProc = 0;
        int cntFinishProc = 0;
        foreach (var recordData in recordDatas)
        {
            cntProc = cntProc + 2;
            SaveRanking(recordData, Save_ranking_item.SAVE_RANKING_HIGH, () => { cntFinishProc++; });
            SaveRanking(recordData, Save_ranking_item.SAVE_RANKING_LOW, () => { cntFinishProc++; });
        }
        StartCoroutine(MonitorSaveProcs());


        return;

        IEnumerator MonitorSaveProcs()
        {
            while (cntProc > cntFinishProc)
            {

                yield return null;
            }
            callback();
        }


    }

    public void SaveRanking(RecordData recordData, Save_ranking_item save_Ranking_Item, CallbackVoid callback)
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);

        query.WhereEqualTo("name", settings.name); // プレイヤー名でデータを絞る
        query.WhereEqualTo("gameModeId", recordData.game_mode_id); // 種目でデータを絞る
        query.WhereEqualTo("type", (int)save_Ranking_Item); // MAX・MINでデータを絞る

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            { // データの検索が成功したら、

                if (objList.Count == 0)
                { // ハイスコアが未登録の場合
                    NCMBObject cloudObj = new NCMBObject(rankingClassName);
                    cloudObj["gameModeId"] = recordData.game_mode_id;
                    cloudObj["type"] = (int)save_Ranking_Item;
                    cloudObj["name"] = settings.name;



                    switch (save_Ranking_Item)
                    {
                        case Save_ranking_item.SAVE_RANKING_HIGH:
                            cloudObj["distance"] = recordData.max_distance;
                            cloudObj["timeSpan"] = recordData.timespan_maxdistance;
                            break;
                        case Save_ranking_item.SAVE_RANKING_LOW:
                            cloudObj["distance"] = recordData.min_distance;
                            cloudObj["timeSpan"] = recordData.timespan_mindistance;
                            break;
                        default:
                            break;
                    }
                    cloudObj.SaveAsync(); // セーブ
                }
                else
                { // ハイスコアが登録済みの場合
                    NCMBObject cloudObj = objList[0]; // クラウド上のレコードデータを取得
                    float distance = System.Convert.ToSingle(cloudObj["distance"]);
                    float timeSpan = System.Convert.ToSingle(cloudObj["timeSpan"]);
                    switch (save_Ranking_Item)
                    {
                        case Save_ranking_item.SAVE_RANKING_HIGH:
                            if (distance < recordData.max_distance || (distance == recordData.max_distance && timeSpan > recordData.timespan_maxdistance))
                            {
                                cloudObj["distance"] = recordData.max_distance;
                                cloudObj["timeSpan"] = recordData.timespan_maxdistance;
                                cloudObj.SaveAsync();
                            }
                            break;
                        case Save_ranking_item.SAVE_RANKING_LOW:
                            if (distance > recordData.min_distance || (distance == recordData.min_distance && timeSpan > recordData.timespan_mindistance))
                            {
                                cloudObj["distance"] = recordData.min_distance;
                                cloudObj["timeSpan"] = recordData.timespan_mindistance;
                                cloudObj.SaveAsync();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            callback();
        });
    }

    public void fetchRank(string name, int gameModeId, Save_ranking_item save_Ranking_Item, CallbackInt callback) //ランキングに自身の名前が見つからなければ-1を返す。
    {
        currentRank = 0;
        RankingRecord myRecord = null;
        // データスコアの「HighScore」から検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);
        query.WhereEqualTo("name", name);
        query.WhereEqualTo("gameModeId", gameModeId);
        query.WhereEqualTo("type", (int)save_Ranking_Item);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            { //検索成功したら

                string _name = System.Convert.ToString(objList[0]["name"]); // 名前を取得
                float _distance = System.Convert.ToSingle(objList[0]["distance"]); // スコアを取得
                float _timeSpan = System.Convert.ToSingle(objList[0]["timeSpan"]);
                myRecord = new RankingRecord(0, _name, _distance, _timeSpan, save_Ranking_Item);

                var query2 = new NCMBQuery<NCMBObject>(rankingClassName);
                query2.WhereEqualTo("gameModeId", gameModeId);
                query2.WhereEqualTo("type", (int)save_Ranking_Item);
                switch (save_Ranking_Item)
                {
                    case Save_ranking_item.SAVE_RANKING_HIGH:
                        query2.WhereGreaterThanOrEqualTo("distance", myRecord.distance);
                        break;
                    case Save_ranking_item.SAVE_RANKING_LOW:
                        query2.WhereLessThanOrEqualTo("distance", myRecord.distance);
                        break;
                    default:
                        break;
                }

                query2.CountAsync((int count, NCMBException e2) =>
                {

                    if (e2 != null)
                    {
                        //件数取得失敗
                    }
                    else
                    {
                        //件数取得成功
                        currentRank = count + 1; // 自分よりスコアが上の人がn人いたら自分はn+1位
                    }
                    callback(currentRank);
                });
            }

            return;
        
        });

    }


public void getRankingTop(int gameModeId, Save_ranking_item save_Ranking_Item, CallbackRecordsList callback)
{
    List<RankingRecord> rankingRecords = new List<RankingRecord>();
    NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);
    query.WhereEqualTo("gameModeId", gameModeId);
    query.WhereEqualTo("type", (int)save_Ranking_Item);
    query.Limit = 10; // 上位10件のみ取得
    switch (save_Ranking_Item)
    {
        case Save_ranking_item.SAVE_RANKING_HIGH:
            query.OrderByDescending("distance");
            query.AddAscendingOrder("timeSpan");
            break;
        case Save_ranking_item.SAVE_RANKING_LOW:
            query.OrderByAscending("distance");
            query.AddAscendingOrder("timeSpan");
            break;
        default:
            break;
    }
    query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
    {
        if (e == null)
        { //検索成功したら

            for (int i = 0; i < objList.Count; i++)
            {

                string _name = System.Convert.ToString(objList[i]["name"]); // 名前を取得
                float _distance = System.Convert.ToSingle(objList[i]["distance"]); // スコアを取得
                float _timeSpan = System.Convert.ToSingle(objList[i]["timeSpan"]);
                RankingRecord rankingRecord = new RankingRecord(i + 1, _name, _distance, _timeSpan, save_Ranking_Item);
                rankingRecords.Add(rankingRecord);
            }
            callback(rankingRecords);
        }

    });
    return;
}




public void getRankingNeighbors(string name, int gameModeId, Save_ranking_item save_Ranking_Item, CallbackRecordsList callback)
{
    fetchRank(name, gameModeId, save_Ranking_Item, (int currentRank) =>
    {
        int numSkip = currentRank - 3;
        if (numSkip < 0) numSkip = 0;

        List<RankingRecord> rankingRecords = new List<RankingRecord>();
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);
        query.WhereEqualTo("gameModeId", gameModeId);
        query.WhereEqualTo("type", (int)save_Ranking_Item);
        query.Skip = numSkip;
        query.Limit = 5;
        switch (save_Ranking_Item)
        {
            case Save_ranking_item.SAVE_RANKING_HIGH:
                query.OrderByDescending("distance");
                query.AddAscendingOrder("timeSpan");
                break;
            case Save_ranking_item.SAVE_RANKING_LOW:
                query.OrderByAscending("distance");
                query.AddAscendingOrder("timeSpan");
                break;
            default:
                break;
        }
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            { //検索成功したら

                for (int i = 0; i < objList.Count; i++)
                {
                    string _name = System.Convert.ToString(objList[i]["name"]); // 名前を取得
                    float _distance = System.Convert.ToSingle(objList[i]["distance"]); // スコアを取得
                    float _timeSpan = System.Convert.ToSingle(objList[i]["timeSpan"]);
                    RankingRecord rankingRecord = new RankingRecord(numSkip + i + 1, _name, _distance, _timeSpan, save_Ranking_Item);
                    rankingRecords.Add(rankingRecord);
                }
                callback(rankingRecords);
            }
        });
    });
    return;
}

public void IsNameExistAll(string name, CallbackBool callback)
{
    isBusy = true;
    isNameExist = false;

    NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);
    query.WhereEqualTo("name", name);
    query.CountAsync((int count, NCMBException e) =>
    {

        if (e == null)
        {
            if (count == 0)
            { // 0個なら名前は登録されていない
                Debug.Log("false");
                isNameExist = false;
            }
            else
            { // 0個じゃなかったらすでに名前が登録されている
                Debug.Log("true");
                isNameExist = true;
            }
        }
        callback(isNameExist);
        isBusy = false;
    });


}




public void IsNameExistInRanking(string name, int gameModeId, Save_ranking_item save_Ranking_Item, CallbackBool callback)
{

    isNameExist = false;

    NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);
    query.WhereEqualTo("name", name);
    query.WhereEqualTo("gameModeId", gameModeId);
    query.WhereEqualTo("type", (int)save_Ranking_Item);
    query.CountAsync((int count, NCMBException e) =>
    { // 1つ上のコードで絞られたデータが何個あるかかぞえる 
        if (e == null)
        {
            if (count == 0)
            { // 0個なら名前は登録されていない
                isNameExist = false;
            }
            else
            { // 0個じゃなかったらすでに名前が登録されている
                isNameExist = true;
            }
        }
        callback(isNameExist);
    });

    return;
}




}

