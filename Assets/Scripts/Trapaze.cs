using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapaze : MonoBehaviour
{
    [SerializeField] float ConnectedHeight = 15f;
    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {
        var cj = GetComponent<ConfigurableJoint>();
        cj.anchor=new Vector3(cj.anchor.x, ConnectedHeight, cj.anchor.z);
        cj.connectedAnchor=new Vector3(cj.anchor.x, ConnectedHeight+transform.position.y, cj.anchor.z);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
