using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PublicCamera : MonoBehaviour
{

    Rect rect = new Rect(0, 0, 1, 1);

    bool sizeChanging = false;

    Camera publicCamera;
    SizeChanger sizeChanger;

    float offset = 0f;
    bool isRevers = false;
    int judgCount = 0;
    float pos_at_revers = 0f;
    Camera prevCamera;
    GameObject prevCameraObj;
    Coroutine prevCoroutine;
    Coroutine publicCoroutine;

    [SerializeField] Transform tracePoint;
    [SerializeField] float maxSize = 10f;
    [SerializeField] float minSize = 2f;
    [SerializeField] float variation = 0.3f;
    
    // Start is called before the first frame update


    void Start()
    {


        publicCamera = GetComponent<Camera>();
        publicCamera.orthographicSize = minSize;
        //publicCamera.aspect = publicCamera.rect.width / publicCamera.rect.height;
        prevCameraObj = new GameObject();
        prevCamera = prevCameraObj.AddComponent<Camera>();
        prevCamera.CopyFrom(publicCamera);
        prevCamera.orthographicSize = prevCamera.orthographicSize - variation;
        prevCamera.depth = prevCamera.depth - 1f;

        sizeChanger = GetComponent<SizeChanger>();
        sizeChanger.Init(publicCamera);
        publicCoroutine = StartCoroutine(monitorPublicCamera());
        prevCoroutine = StartCoroutine(monitorPrevCamera());


    }

    void Update()
    // Update is called once per frame
    {
       
    }

    IEnumerator monitorPublicCamera()
    {
        Vector3 viewportPos = publicCamera.WorldToViewportPoint(tracePoint.position);

        while (rect.Contains(viewportPos)||sizeChanger.processing)
        {
            viewportPos = publicCamera.WorldToViewportPoint(tracePoint.position);

            yield return null;

        }

       
        if (publicCamera.orthographicSize <= maxSize)
        {
            StopCoroutine(prevCoroutine);
            prevCamera.orthographicSize = publicCamera.orthographicSize;
            sizeChanger.ChangeSize(publicCamera.orthographicSize + variation);
            prevCoroutine = StartCoroutine(monitorPrevCamera());
            publicCoroutine = StartCoroutine(monitorPublicCamera());
        }
    }


    IEnumerator monitorPrevCamera()
    {
        Vector3 viewportPosInPrev;
        float wdt = 0f;
        float timeOut = 15f;

        while (true)
        {
            viewportPosInPrev = prevCamera.WorldToViewportPoint(tracePoint.position);
            if (rect.Contains(viewportPosInPrev))
            {
                wdt += Time.deltaTime;
            }
            else
            {
                wdt = 0f;
            }

            if (wdt > timeOut)
            {
                while (sizeChanger.processing)
                {
                    yield return null;
                }
                if (prevCamera.orthographicSize >= minSize)
                {
                    StopCoroutine(publicCoroutine);
                    sizeChanger.ChangeSize(prevCamera.orthographicSize);
                    prevCamera.orthographicSize -= variation;
                    prevCoroutine = StartCoroutine(monitorPrevCamera());
                    publicCoroutine = StartCoroutine(monitorPublicCamera());
                }
                yield break;
            }

            yield return null;
        }
    }




}





