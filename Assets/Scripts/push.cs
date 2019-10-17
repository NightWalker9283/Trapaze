using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class push : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("delay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(3);
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, 0f,0.5f) * 3000f,ForceMode.Impulse);
       
    } 
}
