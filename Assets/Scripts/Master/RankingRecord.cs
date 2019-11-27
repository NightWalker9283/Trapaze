public class RankingRecord
{
    public int rank;
    public string name;
    public float distance;
    public float timeSpan;
    public RankingManager.Save_ranking_item direction;
    
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