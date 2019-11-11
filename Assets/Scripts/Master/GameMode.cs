[System.Serializable]
public class GameMode 
{
    public int id;
    public string name;
    public float trapezeLength;
    public float timeLimit;
    public bool eventFlag; //trueでプレイ中にイベントありのモードを表す
    public string detail; //モード解説用の文章。タイトル画面で使用。

    public GameMode(int id,string name,float trapezeLength,float timeLimit,bool eventFlag,string detail)
    {
        this.id = id;
        this.name = name;
        this.trapezeLength = trapezeLength;
        this.timeLimit = timeLimit;
        this.eventFlag = eventFlag;
        this.detail = detail;

    }

}
