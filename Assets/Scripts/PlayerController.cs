using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]


public class PlayerController : MonoBehaviour
{
    	// PUBLIC
	[SerializeField] SimpleTouchController Controller;
    [SerializeField] Rigidbody rb_Trapaze;
   
    [SerializeField] float Multiplier = 10f;
    [SerializeField] float Scale = 0.25f;
    [SerializeField] float offset_z, offset_y;

    // PRIVATE

    Rigidbody rb;
    Vector3 base_pos;
    //Vector2 old_ct_pos;
    Vector3 destination;    //目的地のワールド座標
    LineRenderer line;
    GameObject parent;



    void SetDestination()
    {
        destination = rb_Trapaze.transform.TransformPoint(base_pos.x ,
            base_pos.y + (Controller.GetTouchPosition.y*Scale)+offset_y,
            base_pos.z - (Controller.GetTouchPosition.x*Scale)+offset_z);
    }

	void Awake()
	{
        parent = transform.root.gameObject;
        
		rb = GetComponent<Rigidbody>();
        line=this.gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        base_pos = rb_Trapaze.transform.InverseTransformPoint(rb.position);
        //old_ct_pos = Controller.GetTouchPosition;
        SetDestination();
        

        //line = GetComponent<LineRenderer>();
        
	}


	void Update()
	{

        SetDestination();
    }
    
    void PointDraw(Vector3 pos)
    {
       
         
        line.SetPosition(0,pos ); // オブジェクトの位置情
    }
    
    private void FixedUpdate()
    {



        Vector3 force =destination - rb.position; //ワールド座標差分ベクトル
        Vector3 force_pos = rb.position;
        //force.x = 0;
        //PointDraw(destination);
        //Vector3 anchor_pos = rb_Trapaze.transform.TransformPoint(cj_Foot.connectedAnchor);
        //anchor_pos.x = 0;
        //Vector3 reaction_pos = anchor_pos * 2 - (new Vector3(0,rb.position.y,rb.position.z));
        //LineDraw(reaction_pos);
        //rb.transform.localPosition = destination;
        rb.AddForceAtPosition(force * Multiplier, force_pos);
        rb_Trapaze.AddForceAtPosition(-force * Multiplier, force_pos);


        Debug.Log(Controller.GetTouchPosition + " "
        + rb_Trapaze.transform.InverseTransformPoint(destination)

        ) ;
        
    }



}
