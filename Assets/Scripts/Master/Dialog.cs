using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Dialog
{
    public delegate void OnCompleteHandler(DialogResult result);
    public event OnCompleteHandler OnComplete;
    public enum DialogResult
    {
        YES, NO
    }
    Canvas cvsDialog;
    string title = "アプリの更新があります";
    string message = "更新しますか？";
    string yes = "はい";
    string no = "いいえ";

    public Dialog()
    {
        CreateDialog();
    }
    public Dialog(string title,string message,string yes, string no)
    {
        this.title = title;
        this.message = message;
        this.yes = yes;
        this.no = no;
        CreateDialog();
    }
    void CreateDialog()
    {
        cvsDialog= Object.Instantiate(Resources.Load<Canvas>("CanvasDialog"),GameMaster.gameMaster.transform.parent).GetComponent<Canvas>();
        var wndDialog = cvsDialog.transform.Find("wndDialog");
        var txtMessage = wndDialog.Find("txtMessage").GetComponent<Text>();
        var btnYes = wndDialog.Find("btnYes").GetComponent<Button>();
        var btnNo = wndDialog.Find("btnNo").GetComponent<Button>();
        var objTitle = wndDialog.Find("title");

        objTitle.Find("txtTitle").GetComponent<Text>().text = title;
        txtMessage.text = message;
        btnYes.transform.Find("txtYes").GetComponent<Text>().text = yes;
        btnNo.transform.Find("txtNo").GetComponent<Text>().text = no;
        
        btnYes.onClick.AddListener(()=>
        {
            OnComplete(DialogResult.YES);
            Object.Destroy(cvsDialog.gameObject);
        });
        btnNo.onClick.AddListener(() =>
        {
            OnComplete(DialogResult.NO);
            Object.Destroy(cvsDialog.gameObject);
        });
    }
}
