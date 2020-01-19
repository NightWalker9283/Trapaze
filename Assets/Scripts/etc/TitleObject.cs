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

    public TitleObject ttlHyperOmoshiroi = new TitleObject(5000, "ハイパー面白いくん", "ジャンプ時に特定ボイス再生", "パイパーネオDQN\nほうきで野球をして職員室に呼び出されたうんこちゃんを救う為に立ち上がる");
    public TitleObject ttlTrapezeMachine = new TitleObject(5010, "ブランコを漕ぐ機械", "弾幕を使用せず一周", "おまえはこれからブランコを漕ぐ機械として生きてくれ");
    public Titles()
    {
        plusDistanceTitles.Add(new StandardTitle(10000, "本当のgm", "飛距離10m未満", "一歩も踏み出せない本当のgm",0f));
        plusDistanceTitles.Add(new StandardTitle(10, "BRAR", "飛距離10m以上", "ベリアル",10f));
        plusDistanceTitles.Add(new StandardTitle(20, "無", "飛距離20m以上", "お前の信用は地に落ちた、お前は無！", 20f));
        plusDistanceTitles.Add(new StandardTitle(30, "ねもうす鳥", "飛距離30m以上", "一日に一度「ぴょおおおおおおお」と鳴く\n鳴かない日は何かが起きる", 30f));
        plusDistanceTitles.Add(new StandardTitle(40, "もこうのバーター", "飛距離40m以上", "ひとりではNG", 40f));
        plusDistanceTitles.Add(new StandardTitle(50, "イーサン", "飛距離50m以上", "元有名建築家。自分がわからない。\nブーメランの扱いに長けている。",50f));
        plusDistanceTitles.Add(new StandardTitle(60, "ハラマキ", "飛距離60m以上", "寝たきり状態になりながらもうんこちゃんを運んでくれる友達。\nアラマキという東大生の弟がいる\n弟とよく比べられ、親せきには馬鹿にされ、同級生には憎まれていた。\n「純ちゃんをいじめるな！」と庇ったり、お互いによく助け合っていた。", 60f));
        plusDistanceTitles.Add(new StandardTitle(70, "笑顔のおっさん", "飛距離70m以上", "見るものを幸せにする",70f));
        plusDistanceTitles.Add(new StandardTitle(80, "つらぬき丸", "飛距離80m以上", "いかなる障害も突き破る存在",80f));
        plusDistanceTitles.Add(new StandardTitle(90, "ねもうすちゃんねる", "飛距離90m以上", "ネ申ちゃんねる",90f));
        plusDistanceTitles.Add(new StandardTitle(100, "104点", "飛距離100m以上", "加藤純一から授かる最高の賛辞" ,100f));
        plusDistanceTitles.Add(new StandardTitle(120, "やるねェ", "飛距離120m以上", "コメントの砲を全身で受けてロケット噴射みたいに加速をつけたのか… やるねェ",120f));
        plusDistanceTitles.Add(new StandardTitle(140, "大卒プレイ", "飛距離140m以上", "さすが慶○医学部",140f));
        plusDistanceTitles.Add(new StandardTitle(160, "ゴルベーザ四天王登場！", "飛距離160m以上", "全てはここからはじまった", 160f));
        plusDistanceTitles.Add(new StandardTitle(180, "オクレイマン", "飛距離180m以上", "力の象徴",180f));
        plusDistanceTitles.Add(new StandardTitle(200, "大地讃頌", "飛距離200m以上", "大地への限りない感謝と賛美。", 200f));
        plusDistanceTitles.Add(new StandardTitle(250, "ニコ生チャンピオン", "飛距離250m以上", "加藤純一最強！！加藤純一最強！！",250f));
        plusDistanceTitles.Add(new StandardTitle(300, "多分、純", "飛距離300m以上", "遠すぎて視認できない。けど多分、純",300f));
        plusDistanceTitles.Add(new StandardTitle(350, "ゾーン", "飛距離350m以上", "集中力の境地。RTA勢張りの動きを見せる。",350f));
        plusDistanceTitles.Add(new StandardTitle(397, "397", "飛距離397ｍ(小数点以下切り捨て)", "いつも397", 397f));
        plusDistanceTitles.Add(new StandardTitle(398, "インターネットヒーロー", "飛距離398m以上", "ニコ生に納まらない男\nライバルの出現を座して待つ",398f));
      


        minusDistanceTitles.Add(new StandardTitle(1010, "ひん", "飛距離-10m以下", "終わりの予感。Fin", -10f));
        minusDistanceTitles.Add(new StandardTitle(1020, "ゴミゴギブリちゃねら", "飛距離-20m以下", "集中力を乱すgm", -20f));
        minusDistanceTitles.Add(new StandardTitle(1030, "大脳摘出", "飛距離-30m以下", "信者衛門の理想の姿", -30f));
        minusDistanceTitles.Add(new StandardTitle(1040, "インターネットガイジ", "飛距離-40m以下", "敵は画面の向こうだけじゃない", -40f));
        minusDistanceTitles.Add(new StandardTitle(1050, "絵畜生", "飛距離-30m以下", "極めてなにか生命に対する侮辱を感じます", -50f));
        minusDistanceTitles.Add(new StandardTitle(1060, "ベーシックインカマー", "飛距離-60m以下", "人の心を持たぬタラコの化身", -60f));
        minusDistanceTitles.Add(new StandardTitle(1070, "Summer Field", "飛距離-70m以下", "バカすぎワロタ", -70f));
        minusDistanceTitles.Add(new StandardTitle(1080, "歩くクレヨン", "飛距離-80m以下", "正体はシカとされる。あまり深入りしてはいけない", -80f));
        minusDistanceTitles.Add(new StandardTitle(1090, "シン", "飛距離-90m以下", "正体はジェクト", -90f));
        minusDistanceTitles.Add(new StandardTitle(1100, "良い方のgm", "飛距離-100m以下", "gmだけどgmじゃない", -100f));
        minusDistanceTitles.Add(new StandardTitle(1120, "カトロイド", "飛距離-120m以下", "それに触れてはいけない", -120f));
        minusDistanceTitles.Add(new StandardTitle(1140, "鬼の子", "飛距離-140m以下", "反抗期の再来。わらぁ鬼の子だぁ", -140f));
        minusDistanceTitles.Add(new StandardTitle(1160, "◯ooking.com", "飛距離-160m以下", "あなたは休暇に行くのではない…\n冒険へと出発するのだ！自分の旗を打ち立てよ！\n冒険に最高の拠点を予約しよう！", -160f));
        minusDistanceTitles.Add(new StandardTitle(1180, "ユダ", "飛距離-180m以下", "嘘は良くない", -180f));
        minusDistanceTitles.Add(new StandardTitle(1200, "真性ホギー", "飛距離-200m以下", "若気の至り。\n仮性ホギーや広東ホギーもいる。", -200f));
        minusDistanceTitles.Add(new StandardTitle(1250, "人は日々うんこをする", "飛距離-250m以下", "どんな人間でも糞はする。", -250f));

       

        allTitles.AddRange(plusDistanceTitles);
        allTitles.AddRange(minusDistanceTitles);
        allTitles.Add(ttlHyperOmoshiroi);
        allTitles.Add(ttlTrapezeMachine);
       

    }

    
    

}