using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;

public class push2 : MonoBehaviour
{

        [SerializeField] float multiplier = 50f;
 
    // Start is called before the first frame update
    void Start()
    {
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, 0f,0.5f) * multiplier);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

}
