﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trank : MonoBehaviour
{
    float angle;
    Vector3 parentDirection;
    Vector3 direction;
    Vector3 TrapezeDirection;
    Rigidbody rb;

    CharacterJoint cj;
    Rigidbody rb_parent;

    Vector3 center;
    Vector3 parentCenter;

    IEnumerator holdCoroutine;
    float oldAngle, oldDeltaAngle;
    bool oldHd;
    [SerializeField] float BendMp = 5000f; //係数
    [SerializeField] float StretchMp = 5000f;
    [SerializeField] GameObject objTrapeze;
    [SerializeField] mascle mslL_Leg, mslR_Leg, mslL_Foot, mslR_Foot;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cj = GetComponent<CharacterJoint>();
        rb_parent = cj.connectedBody;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        center = rb.transform.TransformPoint(rb.centerOfMass);
        parentCenter = rb_parent.transform.TransformPoint(rb_parent.centerOfMass);
        //parentCenter.Set(center.x, parentCenter.y, parentCenter.z);
        direction = center - rb.position;
        parentDirection = parentCenter - rb.position;
        TrapezeDirection = objTrapeze.GetComponent<ConfigurableJoint>().anchor - rb.position;
        angle = Vector3.Angle(direction, parentDirection);

        Hold(Vector3.Angle(direction, TrapezeDirection));

    }

    public void Free()
    {
        if (holdCoroutine != null) StopCoroutine(holdCoroutine);
        holdCoroutine = null;
    }
    public void Hold(float holdAngle)
    {
        Free();
        holdCoroutine = HoldProc(holdAngle);
        StartCoroutine(holdCoroutine);
    }

    IEnumerator HoldProc(float holdAngle)
    {


        while (true)
        {

            float deffAngle = holdAngle - angle;
            if (angle >= holdAngle) Bend(BendMp * Mathf.Sin(-deffAngle * Mathf.Deg2Rad));
            if (angle < holdAngle) Stretch(StretchMp * Mathf.Sin(deffAngle * Mathf.Deg2Rad));
            //Debug.Log(name);
            yield return new WaitForFixedUpdate();
        }
    }

    void Stretch(float mp)
    {

        if (mslL_Foot.angle > 10f || mslR_Foot.angle > 10f &&
           mslL_Leg.angle > 100f || mslR_Leg.angle > 100f)
        {
            return;
        }
        var force = (center - parentCenter) * mp;

        rb.AddForceAtPosition(force, center);

        rb_parent.AddForceAtPosition(-force, parentCenter);

    }

    void Bend(float mp)
    {
        var force = (parentCenter - center) * mp;
        rb.AddForceAtPosition(force, center);
        rb_parent.AddForceAtPosition(-force, parentCenter);

        //    //DrawPoint(rb_parent.centerOfMass);

    }


}
