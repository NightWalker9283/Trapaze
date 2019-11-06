using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SizeChanger : MonoBehaviour
{
    public bool processing { get; protected set; }
    public Camera targetCamera { get; protected set; }
    public float nextSize { get; protected set; }
    public float prevSize { get; protected set; }
    public float now { get; protected set; }
    float span = 1f; //s



    IEnumerator corutine;

    public void Init(Camera c)
    {
        this.targetCamera = c;
        this.processing = false;
        this.prevSize = targetCamera.orthographicSize;
        this.nextSize = targetCamera.orthographicSize;
        corutine = null;
        this.now = 0f;
    }

    public void ChangeSize(float size)
    {
        this.processing = true;
        this.prevSize = targetCamera.orthographicSize;
        this.nextSize = size;
        this.now = 0f;
        corutine = null;
        corutine = ChangingSize(2f);
        //corutine.MoveNext();
        this.StartCoroutine(corutine);

    }
    public void ChangeSize(float size, float damp)
    {
        this.processing = true;
        this.prevSize = targetCamera.orthographicSize;
        this.nextSize = size;
        this.now = 0f;
        corutine = null;
        corutine = ChangingSize(damp);
        //corutine.MoveNext();
        this.StartCoroutine(corutine);

    }



    IEnumerator ChangingSize(float damp)
    {
        while (this.now < this.span)
        {
            targetCamera.orthographicSize = Mathf.Lerp(this.prevSize, this.nextSize, this.now);
            this.now = Mathf.Clamp01(this.now + Time.deltaTime / damp);
            yield return null;
        }
        this.processing = false;
        this.now = 0f;
        Debug.Log("finish change size");
        yield break;

    }


}
