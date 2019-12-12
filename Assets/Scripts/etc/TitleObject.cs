using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleObject
{
    public int id;
    public string name;
    public string condition;
    public string description;
    public TitleObject(int id, string name, string condition, string description)
    {
        this.id = id;
        this.name = name;
        this.condition = condition;
        this.description = description;
    }
}

public class StandardTitle : TitleObject
{
    public float distance;
    public StandardTitle(int id, string name, string condition, string description, float distance)
        : base(id, name, condition, description)
    {
        this.distance = distance;
    }

}

public class Titles
{
    public List<TitleObject> allTitles = new List<TitleObject>();
    public List<StandardTitle> plusDistanceTitles = new List<StandardTitle>(),
                                minusDistanceTitles = new List<StandardTitle>();

    public Titles()
    {
        plusDistanceTitles.Add(new StandardTitle(10000, "本当のgm", "飛距離10m未満", "一歩も踏み出せない本当のgm",0f));
        plusDistanceTitles.Add(new StandardTitle(10, "本当のgm", "飛距離10m以上", "一歩も踏み出せない本当のgm",10f));
        plusDistanceTitles.Add(new StandardTitle(30, "本当のgm", "飛距離30m以上", "一歩も踏み出せない本当のgm",30f));
        plusDistanceTitles.Add(new StandardTitle(40, "本当のgm", "飛距離40m以上", "一歩も踏み出せない本当のgm",40f));
        plusDistanceTitles.Add(new StandardTitle(50, "本当のgm", "飛距離50m以上", "一歩も踏み出せない本当のgm",50f));
        plusDistanceTitles.Add(new StandardTitle(60, "本当のgm", "飛距離60m以上", "一歩も踏み出せない本当のgm",60f));
        plusDistanceTitles.Add(new StandardTitle(70, "本当のgm", "飛距離70m以上", "一歩も踏み出せない本当のgm",70f));
        plusDistanceTitles.Add(new StandardTitle(80, "本当のgm", "飛距離80m以上", "一歩も踏み出せない本当のgm",80f));
        plusDistanceTitles.Add(new StandardTitle(90, "本当のgm", "飛距離90m以上", "一歩も踏み出せない本当のgm",90f));
        plusDistanceTitles.Add(new StandardTitle(100, "104点", "飛距離100m以上", "加藤純一から授かる最高の賛辞" ,100f));
        plusDistanceTitles.Add(new StandardTitle(120, "本当のgm", "飛距離120m以上", "一歩も踏み出せない本当のgm",120f));
        plusDistanceTitles.Add(new StandardTitle(140, "本当のgm", "飛距離140m以上", "一歩も踏み出せない本当のgm",140f));
        plusDistanceTitles.Add(new StandardTitle(160, "本当のgm", "飛距離160m以上", "一歩も踏み出せない本当のgm",160f));
        plusDistanceTitles.Add(new StandardTitle(180, "本当のgm", "飛距離180m以上", "一歩も踏み出せない本当のgm",180f));
        plusDistanceTitles.Add(new StandardTitle(200, "本当のgm", "飛距離200m以上", "一歩も踏み出せない本当のgm",200f));
        plusDistanceTitles.Add(new StandardTitle(250, "インターネットチャンピオン", "飛距離250m以上", "加藤純一最強！！加藤純一最強！！",250f));
        plusDistanceTitles.Add(new StandardTitle(300, "本当のgm", "飛距離300m以上", "一歩も踏み出せない本当のgm",300f));
        plusDistanceTitles.Add(new StandardTitle(350, "本当のgm", "飛距離350m以上", "一歩も踏み出せない本当のgm",350f));
        plusDistanceTitles.Add(new StandardTitle(397, "397", "飛距離397ｍ(小数点以下切り捨て)", "いつも397", 397f));
        plusDistanceTitles.Add(new StandardTitle(398, "本当のgm", "飛距離398m以上", "一歩も踏み出せない本当のgm", 398f));
        plusDistanceTitles.Add(new StandardTitle(450, "本当のgm", "飛距離450m以上", "一歩も踏み出せない本当のgm",450f));
        plusDistanceTitles.Add(new StandardTitle(500, "本当のgm", "飛距離500m以上", "一歩も踏み出せない本当のgm",500f));


        minusDistanceTitles.Add(new StandardTitle(1010, "本当のgm", "飛距離-10m以下", "一歩も踏み出せない本当のgm", -10f));
        minusDistanceTitles.Add(new StandardTitle(1030, "本当のgm", "飛距離-30m以下", "一歩も踏み出せない本当のgm", -30f));
        minusDistanceTitles.Add(new StandardTitle(1040, "本当のgm", "飛距離-40m以下", "一歩も踏み出せない本当のgm", -40f));
        minusDistanceTitles.Add(new StandardTitle(1050, "本当のgm", "飛距離-50m以下", "一歩も踏み出せない本当のgm", -50f));
        minusDistanceTitles.Add(new StandardTitle(1060, "本当のgm", "飛距離-60m以下", "一歩も踏み出せない本当のgm", -60f));
        minusDistanceTitles.Add(new StandardTitle(1070, "本当のgm", "飛距離-70m以下", "一歩も踏み出せない本当のgm", -70f));
        minusDistanceTitles.Add(new StandardTitle(1080, "本当のgm", "飛距離-80m以下", "一歩も踏み出せない本当のgm", -80f));
        minusDistanceTitles.Add(new StandardTitle(1090, "本当のgm", "飛距離-90m以下", "一歩も踏み出せない本当のgm", -90f));
        minusDistanceTitles.Add(new StandardTitle(1100, "本当のgm", "飛距離-100m以下", "一歩も踏み出せない本当のgm", -100f));
        minusDistanceTitles.Add(new StandardTitle(1120, "本当のgm", "飛距離-120m以下", "一歩も踏み出せない本当のgm", -120f));
        minusDistanceTitles.Add(new StandardTitle(1140, "本当のgm", "飛距離-140m以下", "一歩も踏み出せない本当のgm", -140f));
        minusDistanceTitles.Add(new StandardTitle(1160, "本当のgm", "飛距離-160m以下", "一歩も踏み出せない本当のgm", -160f));
        minusDistanceTitles.Add(new StandardTitle(1180, "本当のgm", "飛距離-180m以下", "一歩も踏み出せない本当のgm", -180f));
        minusDistanceTitles.Add(new StandardTitle(1200, "本当のgm", "飛距離-200m以下", "一歩も踏み出せない本当のgm", -200f));
        minusDistanceTitles.Add(new StandardTitle(1250, "本当のgm", "飛距離-250m以下", "一歩も踏み出せない本当のgm", -250f));
        minusDistanceTitles.Add(new StandardTitle(1300, "本当のgm", "飛距離-300m以下", "一歩も踏み出せない本当のgm", -300f));
        minusDistanceTitles.Add(new StandardTitle(1350, "本当のgm", "飛距離-350m以下", "一歩も踏み出せない本当のgm", -350f));
        minusDistanceTitles.Add(new StandardTitle(1400, "本当のgm", "飛距離-400m以下", "一歩も踏み出せない本当のgm", -400f));
        minusDistanceTitles.Add(new StandardTitle(1450, "本当のgm", "飛距離-450m以下", "一歩も踏み出せない本当のgm", -450f));
        minusDistanceTitles.Add(new StandardTitle(1500, "本当のgm", "飛距離-500m以下", "一歩も踏み出せない本当のgm", -500f));

        allTitles.AddRange(plusDistanceTitles);
        allTitles.AddRange(minusDistanceTitles);
       

    }

}