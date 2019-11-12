[System.Serializable]
public class RecordData
{
    public int game_mode_id;
    public float max_distance;
    public float timespan_maxdistance;
    public float min_distance;
    public float timespan_mindistance;
    public int play_count;
    public float total_time;

    public RecordData(int game_mode_id,
        float max_distance,
        float timespan_maxdistance,
        float min_distance,
        float timespan_mindistance,
        int play_count,
        float total_time)
    {
        this.game_mode_id = game_mode_id;
        this.max_distance = max_distance;
        this.timespan_maxdistance = timespan_maxdistance;
        this.min_distance = min_distance;
        this.timespan_mindistance = timespan_mindistance;
        this.play_count = play_count;
        this.total_time = total_time;
    }


}
