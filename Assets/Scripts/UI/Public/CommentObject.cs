using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentObject : MonoBehaviour
{

    public RectTransform rectObjComment, rectCvsPublic;
    Rect rectCvsZeroPoint;
    // Start is called before the first frame update
    void Start()
    {
        rectCvsPublic = transform.parent.GetComponent<RectTransform>();
        rectObjComment = GetComponent<RectTransform>();
        rectObjComment.localPosition=new Vector3(rectObjComment.localPosition.x, Random.Range(0f, rectCvsPublic.rect.height/2 - rectObjComment.rect.height), 0f);
        rectCvsZeroPoint = new Rect(new Vector2(0f, 0f), rectCvsPublic.rect.size);
    }

    // Update is called once per frame
    void Update()
    {
        rectObjComment.localPosition=new Vector3(rectObjComment.localPosition.x - Time.deltaTime * 100f, rectObjComment.localPosition.y, 0f);

        //var isOut = !rectCvsZeroPoint.Contains(new Vector2(-rectCvsPublic.rect.width + rectObjComment.localPosition.x + rectObjComment.rect.width,
        //    rectObjComment.localPosition.y));

        //if (isOut) Destroy(gameObject);
    }
}
