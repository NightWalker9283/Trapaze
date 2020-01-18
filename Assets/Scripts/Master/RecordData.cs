[System.Serializable]
public class RecordData
{
    public int game_mode_id;
    public float max_distance;
    public float timespan_maxdistance;
    public float max_distance_best;
    public float timespan_maxdistance_best;
    public float min_distance;
    public float timespan_mindistance;
    public float min_distance_best;
    public float timespan_mindistance_best;
    public int play_count;
    public float total_time;
    public float total_distance;

    public RecordData() { }
    public RecordData(int game_mode_id,
        float max_distance,
        float timespan_maxdistance,
        float max_distance_best,
        float timespan_maxdistance_best,
        float min_distance,
        float timespan_mindistance,
        float min_distance_best,
        float timespan_mindistance_best,
        int play_count,
        float total_time,
        float total_distance)
    {
        this.game_mode_id = game_mode_id;
        this.max_distance = max_distance;
        this.timespan_maxdistance = timespan_maxdistance;
        this.max_distance = max_distance_best;
        this.timespan_maxdistance = timespan_maxdistance_best;
        this.min_distance = min_distance;
        this.timespan_mindistance = timespan_mindistance;
        this.min_distance = min_distance_best;
        this.timespan_mindistance = timespan_mindistance_best;
        this.play_count = play_count;
        this.total_time = total_time;
        this.total_distance = total_distance;
    }


}
