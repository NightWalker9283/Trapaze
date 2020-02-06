using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//タイトル画面用Ver.表示
public class TxtVer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "Ver" + Application.version;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
