using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addfc_hp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float a = 1000f;

    [SerializeField]
    private float x, y, z;

    [SerializeField]
    private Rigidbody PartnerRb;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.AddForceAtPosition(new Vector3(x, y, z) * a,rb.position);
        PartnerRb.AddForceAtPosition(new Vector3(-x, -y, -z) * a,rb.position);
        Debug.Log(rb.position);
    } 
}
