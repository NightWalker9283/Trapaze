using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PublicCamera : MonoBehaviour
{
    public enum stat_publicCamera
    {
        play, jump
    }

    public stat_publicCamera stat = stat_publicCamera.play;
    stat_publicCamera oldStat;

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
    [SerializeField] float rotationDamping = 1f;
    [SerializeField] Rigidbody Player;

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
        if (stat != oldStat)
        {
            if (stat == stat_publicCamera.jump)
            {
                StopCoroutine(prevCoroutine);
                StopCoroutine(publicCoroutine);
                StartCoroutine(tracePlayer());
            }
        }

        oldStat = stat;
    }

    IEnumerator monitorPublicCamera()
    {
        Vector3 viewportPos = publicCamera.WorldToViewportPoint(tracePoint.position);

        while (rect.Contains(viewportPos) || sizeChanger.processing)
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

    IEnumerator tracePlayer()
    {
        var offset_z = transform.position.z - Player.position.z;
        while (transform.position.y < Player.position.y + Player.velocity.y * Time.deltaTime +20f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Player.position.z + offset_z);
            transform.LookAt(Player.transform);
            yield return new WaitForEndOfFrame();
        }
        var offset_y = transform.position.y - Player.position.y;
        sizeChanger.ChangeSize(maxSize);

        while (stat == stat_publicCamera.jump)
        {
            transform.position = new Vector3(transform.position.x, Player.position.y + offset_y, Player.position.z + offset_z);
            transform.LookAt(Player.transform);

            yield return new WaitForEndOfFrame();
        }


    }

}





