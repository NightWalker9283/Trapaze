using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class setRotation : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] float m = 300f;
    [SerializeField] bool debug = false;
    
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        var q = Quaternion.Euler(new Vector3(angle, 0, 0));
   //     Vector3 local_angle_zero = transform.InverseTransformPoint
        Vector3  vec_angle = q * Vector3.forward;
        Vector3 forward = transform.forward;
        forward.x = 0;
        Vector3 torque = Vector3.Cross(forward, vec_angle);
        rb.AddTorque(torque * m);
        if (debug) Debug.Log(torque*m);
        //rb.transform.forward
        //rb.AddTorque(angle);



    }
}
