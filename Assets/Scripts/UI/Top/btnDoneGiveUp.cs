using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnDoneGiveUp: MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Done);
    }

    // Update is called once per frame
    void Done()
    {
       PlayingManager.gameMaster.Title();
        Destroy(FindObjectOfType<GameMaster>().gameObject);
    }
}
