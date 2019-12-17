[System.Serializable]
public class GameMode
{
    public int id;
    public string name;
    public int initialMayoCnt;
    public bool enableDropMayo;
    public bool enableEvents;
    public float trapezeLength;
    public float timeLimit;
    public string detail; //モード解説用の文章。タイトル画面で使用。

    public GameMode(
        int id,
        string name,
        int initialMayoCnt,
        bool enableDropMayo,
        bool enableEvents,
        float trapezeLength,
        float timeLimit,
        string detail
        )
    {
        this.id = id;
        this.name = name;
        this.initialMayoCnt = initialMayoCnt;
        this.enableDropMayo = enableDropMayo;
        this.enableEvents = enableEvents;
        this.trapezeLength = trapezeLength;
        this.timeLimit = timeLimit;
        this.detail = detail;

    }

}
