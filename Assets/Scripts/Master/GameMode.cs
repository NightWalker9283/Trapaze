
public struct GameMode 
{
    public string name;
    public float trapezeLength;
    public float timeLimit;
    public bool eventFlag; //trueでプレイ中にイベントありのモードを表す
    public string detail; //モード解説用の文章。タイトル画面で使用。

    public GameMode(string name,float trapezeLength,float timeLimit,bool eventFlag,string detail)
    {
        this.name = name;
        this.trapezeLength = trapezeLength;
        this.timeLimit = timeLimit;
        this.eventFlag = eventFlag;
        this.detail = detail;

    }

}
