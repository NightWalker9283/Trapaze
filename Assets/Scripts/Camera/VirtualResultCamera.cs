﻿using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualResultCamera : MonoBehaviour
{
    CinemachineVirtualCamera vcamera;
    CinemachineTransposer body;
    float t = 0f;
    float waitTimer = 0f;
    [SerializeField]
    float Radius = 0.5f,Period = 6f,Wait=2f;
    float time2Euler;

    void Start()
    {
        vcamera = GetComponent<CinemachineVirtualCamera>();
        body = vcamera.GetCinemachineComponent<CinemachineTransposer>();
        time2Euler = 360f / Period;

    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer > Wait)
        {
            Vector3 pos = new Vector3();

            pos.x = Mathf.Sin(t * time2Euler * Mathf.Deg2Rad)*Radius;
            pos.z = Mathf.Cos(t * time2Euler * Mathf.Deg2Rad)*Radius;

            pos.y = 1f;
            body.m_FollowOffset = pos;
            t += Time.deltaTime;
            if (t >= Period) t -= Period;
        }
        else
        {
            waitTimer += Time.deltaTime;
        }
    }
}
