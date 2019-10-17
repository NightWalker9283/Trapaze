using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_parent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Parent;
    
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        var rb_parent = Parent.GetComponent<Rigidbody>();
        rb.transform.SetParent(rb_parent.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
