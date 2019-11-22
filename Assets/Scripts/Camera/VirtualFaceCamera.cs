using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;



public class VirtualFaceCamera : MonoBehaviour
{
    CinemachineVirtualCamera vcamera;
    CinemachineTransposer body;
    CinemachineComposer aim;
    Vector3 startPos = new Vector3(), endPos = new Vector3();

    void Start()
    {
        vcamera = GetComponent<CinemachineVirtualCamera>();
        body = vcamera.GetCinemachineComponent<CinemachineTransposer>();
        aim = vcamera.GetCinemachineComponent<CinemachineComposer>();

        aim.m_DeadZoneWidth = Random.Range(0f, 0.3f);
        aim.m_SoftZoneWidth = Random.Range(aim.m_DeadZoneWidth, 0.5f);
        aim.m_DeadZoneHeight = Random.Range(0f, 0.3f);
        aim.m_SoftZoneHeight = Random.Range(aim.m_DeadZoneHeight, 0.5f);
        startPos.x = (float)(Random.Range(-3, 4)) / 2;
        startPos.y = (float)(Random.Range(-1, 2)) / 2;
        startPos.z = (float)(Random.Range(1, 3)) / 2;
        endPos.x = -startPos.x;//(float)(Random.Range(-2, 3)) / 2;
        endPos.y = (float)(Random.Range(-1, 2)) / 2;
        endPos.z = (float)(Random.Range(1, 3)) / 2;
        StartCoroutine(RotateAroundTarget());
    }

    IEnumerator RotateAroundTarget()
    {
        float t = 0f;
        Vector3 pos = new Vector3();
        while (t < 3f)
        {
            pos.x = Mathf.SmoothStep(startPos.x, endPos.x, t / 3f);
            pos.y = Mathf.SmoothStep(startPos.y, endPos.y, t / 3f);
            pos.z = Mathf.SmoothStep(startPos.z, endPos.z, t / 3f);
            body.m_FollowOffset = pos;
            t += Time.deltaTime;
            yield return null;  
        }

    }


}
