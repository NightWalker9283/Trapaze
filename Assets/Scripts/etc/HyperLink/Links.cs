using System.Collections;
using Hypertext;
using UnityEngine;
using UnityEngine.UI;

public class Links : MonoBehaviour
{
    
    const string RegexURL = "http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?";
    Color32 linkColor = new Color32(215, 10, 10, 255);
    void Start()
    {
        CreateLinks();
    }
    //メール
    private const string MAIL_ADRESS = "nightwalker.9283@gmail.com";
    private const string NEW_LINE_STRING = "\n";
    private const string CAUTION_STATEMENT = "---------以下の内容はそのままで---------" + NEW_LINE_STRING;

    /// <summary>
    /// メーラーを起動する
    /// </summary>
    public void OpenMailer()
    {

        //タイトルはアプリ名
        string subject = "うんこちゃんが巨大ブランコでジャンプするゲーム";

        //本文は端末名、OS、アプリバージョン、言語
        string deviceName = SystemInfo.deviceModel;
#if UNITY_IOS && !UNITY_EDITOR
    deviceName = UnityEngine.iOS.Device.generation.ToString();
#endif

        string body = NEW_LINE_STRING + NEW_LINE_STRING + CAUTION_STATEMENT + NEW_LINE_STRING;
        body += "Device   : " + deviceName + NEW_LINE_STRING;
        body += "OS       : " + SystemInfo.operatingSystem + NEW_LINE_STRING;
        body += "Ver      : " + Application.version + NEW_LINE_STRING;
        body += "Language : " + Application.systemLanguage.ToString() + NEW_LINE_STRING;

        //エスケープ処理
        body = System.Uri.EscapeDataString(body);
        subject = System.Uri.EscapeDataString(subject);

        Application.OpenURL("mailto:" + MAIL_ADRESS + "?subject=" + subject + "&body=" + body);
    }
    public void CreateLinks()
    {
        GetComponent<RegexHypertext>().OnClick("nightwalker.9283@gmail.com", linkColor, url => OpenMailer());
        GetComponent<RegexHypertext>().OnClick(RegexURL, linkColor, url => OpenBrowser(url));
        GetComponent<RegexHypertext>().OnClick("ねもうすお母さん@nemousuokasan", linkColor, url => OpenBrowser("https://twitter.com/nemousuokasan"));
        GetComponent<RegexHypertext>().OnClick("NightWalker@NightWa43733459", linkColor, url => OpenBrowser("https://twitter.com/NightWa43733459"));
    }

    public void OpenBrowser(string url)
    {
        Application.OpenURL(url);
    }

}