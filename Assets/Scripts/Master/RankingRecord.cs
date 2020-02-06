//ランキング用レコード構造
public class RankingRecord
{
    public int rank; //ランク
    public string name; //ユーザー名
    public float distance; //飛距離
    public float timeSpan; //１ゲームのプレイ時間[s]
    public RankingManager.Save_ranking_item direction; //飛距離の正負判定用
    
    public RankingRecord() { }
    public RankingRecord(int rank, string name,float distance,float timeSpan,RankingManager.Save_ranking_item direction)
    {
        this.rank = rank;
        this.name = name;
        this.distance = distance;
        this.timeSpan = timeSpan;
        this.direction = direction;
    }


}