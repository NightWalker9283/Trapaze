[System.Serializable]
public class GameMode
{
    public int id; //ゲームモードID
    public string name; //モード名
    public int initialMayoCnt; //ゲーム開始時のマヨ所持数
    public bool enableDropMayo; //trueでマヨドロップ有効
    public bool enableEvents; //trueでイベント有効（現在未使用)
    public float trapezeLength; //ブランコ長
    public float timeLimit; //制限時間
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
