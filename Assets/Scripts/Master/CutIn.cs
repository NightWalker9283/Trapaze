using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutIn : MonoBehaviour
{
    public bool busy = false;
    public static CutIn cutIn;
    RectTransform rectTransform;
    float witdh;
    float startPosX;
    float InPosX;
    float OutPosX;
    // Start is called before the first frame update
    private void Awake()
    {
        cutIn = this;
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        witdh = rectTransform.rect.width;
        startPosX = rectTransform.localPosition.x;
        InPosX = startPosX - witdh;
        OutPosX = InPosX - witdh;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveIn()
    {
        busy = true;
        StartCoroutine(MoveIn());
        IEnumerator MoveIn()
        {
            float now = 0;
            while (rectTransform.localPosition.x > InPosX)
            {
                now += Time.unscaledDeltaTime / 0.15f;
                rectTransform.localPosition=new Vector3(Mathf.Lerp(startPosX, InPosX, now), 0f, 0f);
                yield return null;
            }
            busy = false;
        }

    }

    public void MoveOut(Action callback)
    {
        busy = true;
        StartCoroutine(MoveOut());

        
        IEnumerator MoveOut()
        {
            float now = 0;
            while (rectTransform.localPosition.x > OutPosX)
            {
                Debug.Log(rectTransform.localPosition.x+":"+OutPosX);
                now += Time.unscaledDeltaTime / 0.15f;
                rectTransform.localPosition=new Vector3(Mathf.Lerp(InPosX, OutPosX, now), 0f, 0f);
                yield return null;
            }
            Debug.Log("Finish MoveOut");
            busy = false;
            callback();
            gameObject.SetActive(false);
        }
    }
}
