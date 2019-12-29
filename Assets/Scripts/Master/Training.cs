using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Training : MonoBehaviour
{
    [SerializeField] Text txtBtnTutorial, txtTutorial;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayingManager.playingManager.isTutorial)
        {
            txtBtnTutorial.text = "Tutorial";
            txtTutorial.text = "Tutorial";
        }
        else
        {
            txtBtnTutorial.text = "Training";
            txtTutorial.text = "Training";

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
