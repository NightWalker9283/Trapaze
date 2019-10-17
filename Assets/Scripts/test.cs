using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject obj_fixed;

    private Rigidbody rb;
    private Rigidbody rb_fixed;
    private float distance;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb_fixed = obj_fixed.GetComponent<Rigidbody>();
        var pos = rb.transform.position;
        var pos_fixed = rb_fixed.transform.position;
        var pos_2d = new Vector2(pos.z, pos.y);
        var pos_fixed_2d = new Vector2(pos_fixed.z, pos_fixed.y);

        distance = Vector2.Distance(pos_2d, pos_fixed_2d);
    }

    // Update is called once per frame
    void Update()
    {
        var pos = rb.transform.position;
        var pos_fixed = rb_fixed.transform.position;
        var pos_2d = new Vector2(pos.z, pos.y);
        var pos_fixed_2d = new Vector2(pos_fixed.z, pos_fixed.y);

        var ydistance = (rb_fixed.transform.position.y - rb.transform.position.y);
        var zdistance = (rb.transform.position.z - rb_fixed.transform.position.z);

        var angle = Mathf.Acos(Mathf.Clamp(ydistance / distance,-1f,1f));
        //var angle = Mathf.Acos(-1.1f);
        /* Debug.Log(angle + "    " +
            ydistance + " " +
            zdistance + "   " +
            (Mathf.Abs(zdistance) / zdistance * (Mathf.Sin(angle))) + " " +
            (Mathf.Cos(angle))
            + "  t.u= " +transform.up
            );

        */
        Debug.Log(
                Vector3.forward
                + "  |   " + transform.forward

            );


        rb.AddForce(new Vector3(0f, Mathf.Abs(zdistance)/zdistance*(Mathf.Sin(angle)) * 200f, Mathf.Cos(angle)*200f));
        
    }
}
