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
    [SerializeField] bool jump;
    [SerializeField] Rigidbody rb_tracePoint;

    // PRIVATE

    Rigidbody rb;
    Vector3 base_pos;
    //Vector2 old_ct_pos;
    Vector3 destination;    //目的地のワールド座標
    LineRenderer line;
    GameObject parent;
    List<GameObject> parts;
    List<Component> clingJoints;
    bool jumped = false;
    public float velocity { get; protected set; }

    List<GameObject> GetAllChildren(GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren;
    }

    void GetChildren(GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
        if (children.childCount == 0)
        {
            return;
        }
        foreach (Transform ob in children)
        {
            allChildren.Add(ob.gameObject);
            GetChildren(ob.gameObject, ref allChildren);
        }
    }

    List<Component> GetAllClingJoint()
    {
        List<Component> allClingJoint = new List<Component>();

        foreach (GameObject obj in parts)
        {
            ConfigurableJoint[] cjs = obj.GetComponents<ConfigurableJoint>();
            SpringJoint[] sjs = obj.GetComponents<SpringJoint>();

            foreach (ConfigurableJoint cj in cjs)
            {
                if (cj.connectedBody == rb_Trapaze) allClingJoint.Add(cj);
            }
            foreach (SpringJoint sj in sjs)
            {
                if (sj.connectedBody == rb_Trapaze) allClingJoint.Add(sj);
            }


        }
        return allClingJoint;

    }

    void Preparation()
    {
        parent = transform.root.gameObject;
        parts = GetAllChildren(parent);
        clingJoints = GetAllClingJoint();

        

        rb = GetComponent<Rigidbody>();
        base_pos = rb_Trapaze.transform.InverseTransformPoint(rb.position);
    }

    void SetDestination()
    {
        destination = rb_Trapaze.transform.TransformPoint(base_pos.x,
            base_pos.y + (Controller.GetTouchPosition.y * Scale) + offset_y,
            base_pos.z - (Controller.GetTouchPosition.x * Scale) + offset_z);
    }

    void Jump()
    {
        Vector3 force = new Vector3(0f, 300f, 300f);
        foreach (Component joint in clingJoints)
        {
            Destroy(joint);
        }
        rb.AddForceAtPosition(force, rb.position, ForceMode.Impulse);
        rb_Trapaze.AddForceAtPosition(-force, rb.position, ForceMode.Impulse);
    }

    void Awake()
    {
        Preparation();
        SetDestination();
        //line=this.gameObject.AddComponent<LineRenderer>();
        //line.startWidth = 0.1f;
        //line.endWidth = 0.1f;
        //line = GetComponent<LineRenderer>();

    }


    void Update()
    {
        SetDestination();
        this.velocity = rb_tracePoint.velocity.magnitude * 3.6f;
        //Debug.Log(this.velocity);
        if (jump && !jumped)
        {
            Jump();
            jumped = true;
        }
    }

    void PointDraw(Vector3 pos)
    {


        line.SetPosition(0, pos); // オブジェクトの位置情
    }

    private void FixedUpdate()
    {



        Vector3 force = destination - rb.position; //ワールド座標差分ベクトル
        Vector3 force_pos = rb.position;



        if (!jumped)
        {
            rb.AddForceAtPosition(force * Multiplier, force_pos);
            rb_Trapaze.AddForceAtPosition(-force * Multiplier, force_pos);
        }

        /*
        Debug.Log(Controller.GetTouchPosition + " "
        + rb_Trapaze.transform.InverseTransformPoint(destination)
        
        ) ;
        */
    }



}
