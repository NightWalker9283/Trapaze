using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class push : MonoBehaviour
{

    [SerializeField] bool isPush=false;
    [SerializeField] float multiplier = 300f;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPush)
        {
            isPush = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, 0f,0.5f) * multiplier,ForceMode.Impulse);
        }
    }

}
