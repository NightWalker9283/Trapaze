using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addfc : MonoBehaviour
{
    // Start is called before the first frame update
    public float a=1000f;
    public int sw=0;
    private Rigidbody rb;
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 right = transform.right;
        Vector3 up = transform.up;
        Vector3 forward = transform.forward;
        Vector3 restoringTorque = new Vector3(forward.y - up.z, right.z - forward.x, up.x - right.y) * a;
        Vector3 trq;
        switch(sw){
            case 0:
                trq = Vector3.forward;
                break;
            case 1:
                trq = Vector3.up;
                break;
            case 2:
                trq = Vector3.down;
                break;
            default:
                trq = Vector3.forward;
                break;
    }
        rb.AddTorque(restoringTorque);
        
    }
}
