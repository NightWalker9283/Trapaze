using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;
using System.CodeDom;
using System;

public class btnChangeName : MonoBehaviour
{
    [SerializeField] Canvas cvsInputName;
    [SerializeField] InputField inptName;
    [SerializeField] Text txtWarning;
    string msgAlredy = "The name is already in use";
    string msgLengthZero = "Give your name more than 1 charactor";
    string msgTooLong = "Too many characters in the name";
    string msgFaild = "Failed to save name";
    Settings settings;
    Button btn;
    Coroutine showWarningCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        settings = GameMaster.gameMaster.settings;

        btn.onClick.AddListener(ChangeName);

    }

    // Update is called once per frame
    void ChangeName()
    {
        if (inptName.text == settings.name) return;
        if (inptName.text.Length <= 0)
        {
            HideWarningMessage();
            StartCoroutine(showWarning(msgLengthZero));
            return;
        }
        if (Encoding.GetEncoding("Shift_JIS").GetByteCount(inptName.text) > 64)
        {
            HideWarningMessage();
            StartCoroutine(showWarning(msgTooLong));
            return;
        }
        GameMaster.rankingManager.RenameUser(settings.name,inptName.text, Callback);
        btn.interactable = false;
    }

    void HideWarningMessage()
    {
        if (showWarningCoroutine != null) StopCoroutine(showWarningCoroutine);
        txtWarning.gameObject.SetActive(false);
    }

    void Callback(bool isNameExist)
    {
        
        if (isNameExist)
        {
            HideWarningMessage();
            showWarningCoroutine = StartCoroutine(showWarning(msgAlredy));
        }
        else
        {
            settings.name = inptName.text;
           
        }
        btn.interactable = true;
    }

    IEnumerator showWarning(string message)
    {
        txtWarning.text = message;
        txtWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        txtWarning.gameObject.SetActive(false);
    }
}
