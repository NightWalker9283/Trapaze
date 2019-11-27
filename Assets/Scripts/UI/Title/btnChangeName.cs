using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;

public class btnChangeName : MonoBehaviour
{
    [SerializeField] InputField inptName;
    [SerializeField] Text txtWarning;

    Button btn;
    Coroutine showWarningCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();


        btn.onClick.AddListener(ChangeName);

    }

    // Update is called once per frame
    void ChangeName()
    {
        if (inptName.text.Length <= 0) return;
        Settings settings = GameMaster.gameMaster.settings;
        bool isPassInputRules=false;
        if (Encoding.GetEncoding("Shift_JIS").GetByteCount(inptName.text)<=64){
            isPassInputRules = true;
        }
        var isNameExist = GameMaster.rankingManager.isNameExistInRankingAll(inptName.text);
        if (isNameExist || !isPassInputRules)
        {
            StopCoroutine(showWarningCoroutine);
            showWarningCoroutine= StartCoroutine(showWarning());
        }
        else
        {
            if (GameMaster.rankingManager.renameforRanking(settings.name, inptName.text))
            {
                settings.name = inptName.text;
            }
            else
            {
                StopCoroutine(showWarningCoroutine);
                showWarningCoroutine = StartCoroutine(showWarning());
            }
        }


    }
    IEnumerator showWarning()
    {
        txtWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        txtWarning.gameObject.SetActive(false);
    }
}
