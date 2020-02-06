using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
//コメントボタン
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

    //コメント発射
    void OneShotComment()
    {
        Instantiate(PrefComment, PlayingManager.playingManager.cvsPublic.transform);
       

    }

   
}


