using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    private float a = 1000f;

    [SerializeField]
    private Vector3 Direction;

   

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var force = (Direction - rb.position)*Vector3.Distance(Direction,rb.position);
        rb.velocity = force;
    }
}
