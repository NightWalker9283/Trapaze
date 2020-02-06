using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//プレイ画面。称号取得通知メッセージ表示
public class WndGetTitle : MonoBehaviour
{
    public static WndGetTitle wndGetTitle;
    [SerializeField] Text txtGetTitle;
    Animator anm;
    Coroutine crtn;

    private void Awake()
    {
        wndGetTitle = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        anm = GetComponent<Animator>();
        anm.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowMessage(string message)
    {
        txtGetTitle.text = "称号:" + message;
        anm.Play("WndGetTitle",0,0f);
        anm.enabled = true;
        if (crtn != null) StopCoroutine(crtn);
        crtn=StartCoroutine(ShowMessageProc());
    }

    IEnumerator ShowMessageProc()
    {
        yield return new WaitForSeconds(6.5f);
        anm.enabled = false;
    }
}
