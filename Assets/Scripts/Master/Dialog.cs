using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Dialog
{

    string title = "アプリの更新があります";
    string message = "更新しますか？";
    string yes = "はい";
    string no = "いいえ";

    public Dialog() { }
    public Dialog(string title,string message,string yes, string no)
    {
        this.title = title;
        this.message = message;
        this.yes = yes;
        this.no = no;

    }

}
