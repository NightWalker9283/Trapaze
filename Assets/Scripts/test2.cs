using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    [SerializeField] Rigidbody rb_Trapaze;
    [SerializeField] float Multiplier = 10f;
    
    // PRIVATE

    Rigidbody rb;
    Vector3 anchor_pos;


   
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anchor_pos= rb_Trapaze.transform.TransformPoint(GetComponent<ConfigurableJoint>().connectedAnchor);
    }


    void Update()
    {

        
    }

    private void FixedUpdate()
    {
        Vector3 force = new Vector3(0, -10, 0);//destination - rb.position; //ワールド座標差分ベクトル
        Vector3 force_pos = anchor_pos*2 - rb.position;
       
        //force_pos.z = 2f;
        //Vector3 force = new Vector3(0,diff_pos.y * Mathf.Sin(rb_Trapaze.rotation.x),diff_pos.z*Mathf.Cos(rb_Trapaze.rotation.x));    //ワールド座標に変換
        rb.AddForceAtPosition(force*Multiplier, rb.position);
        rb_Trapaze.AddForceAtPosition(force*Multiplier,force_pos);


    }
}

