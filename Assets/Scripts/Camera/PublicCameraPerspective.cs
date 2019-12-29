using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PublicCameraPerspective : MonoBehaviour
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
    SizeChangerPerspective sizeChanger;

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
        publicCamera.fieldOfView = minSize;
        //publicCamera.aspect = publicCamera.rect.width / publicCamera.rect.height;
        prevCameraObj = new GameObject();
        prevCamera = prevCameraObj.AddComponent<Camera>();
        prevCamera.CopyFrom(publicCamera);
        prevCamera.fieldOfView = prevCamera.fieldOfView - variation;
        prevCamera.depth = prevCamera.depth - 1f;

        sizeChanger = GetComponent<SizeChangerPerspective>();
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


        if (publicCamera.fieldOfView <= maxSize)
        {
            StopCoroutine(prevCoroutine);
            prevCamera.fieldOfView = publicCamera.fieldOfView;
            sizeChanger.ChangeSize(publicCamera.fieldOfView + variation);
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
                if (prevCamera.fieldOfView >= minSize)
                {
                    StopCoroutine(publicCoroutine);
                    sizeChanger.ChangeSize(prevCamera.fieldOfView);
                    prevCamera.fieldOfView -= variation;
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
        sizeChanger.ChangeSize(maxSize, 0.2f);
        var offset_z = transform.position.z - Player.position.z;
        while (transform.position.y < Player.position.y + Player.velocity.y * Time.deltaTime)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Player.position.z + offset_z);
            //transform.LookAt(Player.transform);
            yield return new WaitForFixedUpdate();
        }

        while (transform.position.y < Player.position.y + Player.velocity.y * Time.deltaTime + 5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Player.position.z + offset_z);
            transform.LookAt(Player.transform);
            yield return new WaitForFixedUpdate();
        }


        var offset_y = transform.position.y - Player.position.y;

        while (true)
        {
            var _y = Player.position.y + offset_y;
            
            transform.position = new Vector3(transform.position.x, _y, Player.position.z + offset_z);
            if (transform.eulerAngles.x > 1.4f) transform.LookAt(Player.transform);

            yield return new WaitForFixedUpdate();
        }



    }

}





