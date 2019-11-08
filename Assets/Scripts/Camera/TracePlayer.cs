using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePlayer : MonoBehaviour
{
	private Vector3 pos_offset;
    
    [SerializeField] Transform target;
    [SerializeField] Transform trapaze;

    void Start()
    {
        pos_offset = transform.position - target.position;
    }

    private void Awake()
    {
        
    }

    void Update()
	{
		Vector3 targetCamPos = target.position + pos_offset;
        Quaternion trapaze_angle = trapaze.rotation;
        Quaternion right90deg = Quaternion.Euler(0f, 90f, 0f);

        transform.position = targetCamPos;
        transform.rotation = trapaze_angle*right90deg;


        //Debug.Log(trapaze.rotation.eulerAngles + "|" + trapaze.localEulerAngles);
        
    }
}