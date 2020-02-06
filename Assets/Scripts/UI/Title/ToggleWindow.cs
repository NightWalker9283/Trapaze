using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//汎用。トグル動作で特定のウィンドウをON・OFFするボタンにアタッチ
public class ToggleWindow : MonoBehaviour
{
    [SerializeField] GameObject targetWindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ToggleWindowProc()
    {
        targetWindow.SetActive(!targetWindow.activeSelf);
    }
}
