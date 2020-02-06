using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//タイトル画面。プレイヤー名入力InputField
public class inptName : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetNameString()
    {
        GetComponent<InputField>().text = GameMaster.gameMaster.settings.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
