using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class btnCommentRush : MonoBehaviour
{


    
    [SerializeField] GameObject PrefComment;
    
    Button btn;



    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();


        btn.onClick.AddListener(ShotComments);

    }

    // Update is called once per frame
    void ShotComments()
    {
        StartCoroutine(CommentRush());
    }
    IEnumerator CommentRush()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(PrefComment, PlayingManager.playingManager.cvsPublic.transform);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        }

    }
}


