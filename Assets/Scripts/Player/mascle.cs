using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーの各パーツの筋肉をシミュレート
public class mascle : MonoBehaviour
{


    public float angle { get; protected set; }
    public Vector3 parentDirection { get; protected set; }
    public Vector3 direction { get; protected set; }
    public Rigidbody rb { get; protected set; }

    CharacterJoint cj;
    Rigidbody rb_parent;

    Vector3 center;
    Vector3 parentCenter;
    
    Coroutine holdCoroutine;
    float oldAngle, oldDeltaAngle;
    bool oldHd;
    [SerializeField] float BendMp = 5000f; //係数
    [SerializeField] float StretchMp = 5000f;


   
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
        angle = Vector3.Angle(direction, parentDirection);

    }
    //脱力
    public void Free()
    {
        if (holdCoroutine != null) StopCoroutine(holdCoroutine);
        holdCoroutine = null;
    }
    //指定した角度を保持
    public void Hold(float holdAngle)
    {
        Free();
        holdCoroutine = StartCoroutine(HoldProc(holdAngle));
        
    }

    IEnumerator HoldProc(float holdAngle)
    {
        

        while (true)
        {
           
            float deffAngle = holdAngle - angle;
            if (angle >= holdAngle) Bend(BendMp*Mathf.Sin(-deffAngle*Mathf.Deg2Rad));
            if (angle < holdAngle) Stretch(StretchMp*Mathf.Sin(deffAngle*Mathf.Deg2Rad));
            //Debug.Log(name);
            yield return new WaitForFixedUpdate();
        }
    }
    //伸ばす
    void Stretch(float mp)
    {


        var force = (center - parentCenter) * mp;

        rb.AddForceAtPosition(force, center);
        rb_parent.AddForceAtPosition(-force, parentCenter);

    }
    //曲げる
    void Bend(float mp)
    {
        var force = (parentCenter - center) * mp;
        rb.AddForceAtPosition(force, center);
        rb_parent.AddForceAtPosition(-force, parentCenter);

        //    //DrawPoint(rb_parent.centerOfMass);

     }

    //デバッグ用
        void DrawPoint(Vector3 point)
    {
        Vector3 point2 = new Vector3(point.x, point.y, point.z + 0.1f);
        Debug.DrawLine(point, point2, Color.red, 1f);
    }

}
