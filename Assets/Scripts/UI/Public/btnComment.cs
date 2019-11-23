using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class btnComment : MonoBehaviour
{
    [SerializeField] Transform PlayerControlPoint;
    [SerializeField] Transform cvsPublic;
    [SerializeField] Camera cmrPublic;
    [SerializeField] GameObject PrefComment;
    Button btn;
    List<CommentsData> AllComments;


    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        AllComments = new List<CommentsData>();
        AllComments.Add(Resources.Load<CommentsData>("Comments/CommentsData0"));
        AllComments.Add(Resources.Load<CommentsData>("Comments/CommentsData1"));
        AllComments.Add(Resources.Load<CommentsData>("Comments/CommentsData2"));
        AllComments.Add(Resources.Load<CommentsData>("Comments/CommentsData3"));
        AllComments.Add(Resources.Load<CommentsData>("Comments/CommentsData4"));

        btn.onClick.AddListener(OneShotComment);

    }

    // Update is called once per frame
    void OneShotComment()
    {
        var objComment = Instantiate(PrefComment, cvsPublic);
        objComment.GetComponent<Text>().text = PicUpComment();

    }

    string PicUpComment()
    {
        var posPlayer = PlayerControlPoint.position;
        Ray ray = new Ray(posPlayer, Vector3.down);
        int layerMask = 1 << 10;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,300f, layerMask))
        {
            
            var distance = Mathf.Abs(posPlayer.z);
            var height = hit.distance;
            for (int i = 0; i < AllComments.Count; i++)
            {
                if (distance > AllComments[i].DistanceL && distance <= AllComments[i].DistanceU &&
                    height > AllComments[i].HeightL && height <= AllComments[i].HeightU)
                {
                    return AllComments[i].Comments[Random.Range(0, AllComments[i].Comments.Count)].Value;

                }
            }
        }
        return "ｗｗｗ";
    }
}


