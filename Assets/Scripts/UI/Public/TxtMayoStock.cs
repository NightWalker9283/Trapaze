using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//マヨ残数表示UI
public class TxtMayoStock : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "x " + PlayingManager.playingManager.mayoCount + "/3";
    }
}
