using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class btnComment : MonoBehaviour
{
    
    
    
    [SerializeField] GameObject PrefComment;
    
    Button btn;
  


    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
       

        btn.onClick.AddListener(OneShotComment);

    }

    // Update is called once per frame
    void OneShotComment()
    {
        Instantiate(PrefComment, PlayingManager.playingManager.cvsPublic.transform);
       

    }

   
}


