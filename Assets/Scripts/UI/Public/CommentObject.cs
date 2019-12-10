using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentObject : MonoBehaviour
{

    RectTransform rectObjComment, rectCvsPublic;
    bool isChangedColliderRect = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = GetComment();
        rectCvsPublic = transform.parent.GetComponent<RectTransform>();
        rectObjComment = GetComponent<RectTransform>();
        var pos = new Vector3(Random.Range(-5f, 0f) + rectCvsPublic.rect.width / 2f,
            (int)(Random.Range(80f, rectCvsPublic.rect.height - rectObjComment.rect.height - 25f) / rectObjComment.rect.height) * rectObjComment.rect.height - rectCvsPublic.rect.height / 2f - rectObjComment.rect.height / 2f,
            0f);

        rectObjComment.localPosition = pos;



    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangedColliderRect)
        {
            var rect = rectObjComment.rect;
            var collider = GetComponent<BoxCollider>();
            collider.center = new Vector3(rect.width / 2f, rect.height / 2f, 0f);
            collider.size = new Vector3(rect.width, rect.height, 5f);
            isChangedColliderRect = true;
        }
        rectObjComment.localPosition = new Vector3(rectObjComment.localPosition.x - Time.deltaTime * 150f, rectObjComment.localPosition.y, 0f);
        var isOut = (rectCvsPublic.rect.width + (rectObjComment.localPosition.x + rectObjComment.rect.width - rectCvsPublic.rect.width / 2f)) < 0f;

        if (isOut) Destroy(gameObject);
    }


    string GetComment()
    {
        var posPlayer = PlayingManager.playingManager.playerControlPoint.position;
        Ray ray = new Ray(posPlayer, Vector3.down);
        int layerMask = 1 << 10;
        RaycastHit hit;
        var AllComments = PlayingManager.playingManager.allComments;
        if (PlayingManager.playingManager.Stat == PlayingManager.Stat_global.play)
        {
            return AllComments[0].Comments[Random.Range(0, AllComments[0].Comments.Count)].Value;
        }
        else
        {
            if (Physics.Raycast(ray, out hit, 300f, layerMask))
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
        }
        return "ｗｗｗ";
    }
}
