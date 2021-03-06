﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//成績画面。モード切り替え用トグルグループのトグルオブジェクトにアタッチ。ゲームモードIDをそれぞれが保持
public class ModeElementForRecords : MonoBehaviour
{
    [SerializeField] ToggleGroup tggModes;
    [SerializeField] GameObject ModeRecords, GeneralRecords;

    [SerializeField]
    Text txtMaxDisatnce, txtTimeSpanMx,
         txtMaxDisatnceBest, txtTimeSpanMxBest,
         txtMinDistance, txtTimeSpanMn,
         txtMinDistanceBest, txtTimeSpanMnBest,
         txtPlayCount, txtTotalTime, txtTotalDistance;

    [SerializeField]
    Text txtGeneralTotalCount, txtGeneralTotalTime, txtGeneralTotalDistance;



    public int id = 0;
    public void OnChangeModeElement(bool isOn)
    {
        if (isOn)
        {
            Debug.Log(id);
            if (id == 0)
            {

                GeneralRecords.SetActive(true);
                ModeRecords.SetActive(false);
            }
            else
            {
                SetModeRecords();
                GeneralRecords.SetActive(false);
                ModeRecords.SetActive(true);
            }
        }
    }


    public void SetGeneralRecords()
    {
        int totalCount = 0;
        float totalDistance = 0f;
        float totalTime = 0f;
        foreach (var item in GameMaster.gameMaster.recordDatas)
        {
            totalCount += item.play_count;
            totalTime += item.total_time;
            totalDistance += item.total_distance;
        }


        txtGeneralTotalCount.text = totalCount.ToString() + "回";
        txtGeneralTotalTime.text = (int)(totalTime / 3600) + "時間 " + (int)((totalTime % 3600) / 60) + "分";
        txtGeneralTotalDistance.text = (totalDistance / 1000).ToString("F1") + "km";


    }

    void SetModeRecords()
    {
        var rds = GameMaster.gameMaster.recordDatas;
        var rd = rds.Find(x => x.game_mode_id == id);
        txtMaxDisatnce.text = rd.max_distance.ToString("F1") + "m";
        txtTimeSpanMx.text = (int)(rd.timespan_maxdistance / 60) + ":" + ((int)(rd.timespan_maxdistance % 60)).ToString("D2") + ")";
        txtMaxDisatnceBest.text = rd.max_distance_best.ToString("F1") + "m";
        txtTimeSpanMxBest.text = (int)(rd.timespan_maxdistance_best / 60) + ":" + ((int)(rd.timespan_maxdistance_best % 60)).ToString("D2") + ")";

        txtMinDistance.text = rd.min_distance.ToString("F1") + "m";
        txtTimeSpanMn.text = (int)(rd.timespan_mindistance / 60) + ":" + ((int)(rd.timespan_mindistance % 60)).ToString("D2") + ")";
        txtMinDistanceBest.text = rd.min_distance_best.ToString("F1") + "m";
        txtTimeSpanMnBest.text = (int)(rd.timespan_mindistance_best / 60) + ":" + ((int)(rd.timespan_mindistance_best % 60)).ToString("D2") + ")";

        txtPlayCount.text = rd.play_count + "回";
        txtTotalTime.text = (int)(rd.total_time / 3600) + "時間" + ((int)((rd.total_time % 3600) / 60)).ToString("D2") + "分";
        txtTotalDistance.text = (rd.total_distance / 1000).ToString("F1") + "km";
    }


}