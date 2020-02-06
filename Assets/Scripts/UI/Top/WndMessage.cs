using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//プレイ画面。汎用メッセージ通知ダイアログ
public class WndMessage : MonoBehaviour
{
    public static WndMessage wndMessage;
    Text txtMessage;
    Button btnOK;
    RectTransform rectWnd;
    // Start is called before the first frame update
    void Awake()
    {
        wndMessage = this;
        txtMessage = transform.Find("txtMessage").GetComponent<Text>();
        btnOK = transform.Find("btnOK").GetComponent<Button>();
        rectWnd = GetComponent<RectTransform>();
        Close();

    }
    
    public void ShowMessage(string message, UnityAction callback)
    {

        rectWnd.sizeDelta = new Vector2(600, 250);
        txtMessage.text = message;
        btnOK.onClick.RemoveAllListeners();
        btnOK.onClick.AddListener(callback);

    }

    public void Close()
    {
        rectWnd.sizeDelta = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
