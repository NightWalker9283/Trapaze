using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]


public class PlayerController : MonoBehaviour
{
    // PUBLIC
    public bool JUMP_on = false;
    public float velocity { get; protected set; }

    // PRIVATE

    [SerializeField] SimpleTouchController Controller;
    [SerializeField] Rigidbody rb_Trapaze;
    [SerializeField] float Multiplier = 10f;
    [SerializeField] float JumpMultiplier = 100f;
    [SerializeField] float Scale = 0.25f;
    [SerializeField] float offset_z, offset_y;
    [SerializeField] Rigidbody rb_tracePoint;
    [SerializeField] float y, z;


    Rigidbody rb;
    Vector3 base_pos;
    Vector3 destination;    //目的地のワールド座標
    LineRenderer line;
    GameObject parent;
    List<GameObject> parts;
    List<Component> clingJoints;
    List<mascle> mascles;
    enum stat_enum { row, pre_jump, jump, fly, finish };
    stat_enum stat = stat_enum.row;
    Rigidbody L_UpLeg, R_UpLeg, Spine1;

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

    List<mascle> GetAllMascle()
    {
        List<mascle> allMascles = new List<mascle>();

        foreach(GameObject obj in parts)
        {
            var _rb = obj.GetComponent<Rigidbody>();
            var _masle = obj.GetComponent<mascle>();
            if (_masle != null) allMascles.Add(_masle);
        }
        return allMascles;

    }

    GameObject FindChild(string targetName)
    {
        foreach (var item in parts)
        {
            if (item.name == targetName)
            {
                return item;

            }
        }
        return null;
    }


    void Jump()
    {

        StartCoroutine(PreJumpProc());
        //rb.AddForceAtPosition(force, rb.position, ForceMode.Impulse);
        //rb_Trapaze.AddForceAtPosition(-force, rb.position, ForceMode.Impulse);
    }
    IEnumerator PreJumpProc()
    {
        bool oldDrag = false;
        float draggingSpan = 0f;
        float draggingSpanLimit = 0.5f;
        Vector3 jumpForce;

        while (DragMonitor.Drag)
        {
            yield return null;
        }
        while (JUMP_on)
        {
            //待機モーション生成
            Vector3 force_pos = rb.position;
            force_pos.x = 0f;
            Vector3 preForce = rb_Trapaze.transform.TransformPoint(new Vector3(0f, -1f, -0.2f) + base_pos) - force_pos;
            //Debug.Log(preForce);
            rb.AddForceAtPosition(preForce * Multiplier, force_pos);
            rb_Trapaze.AddForceAtPosition(-preForce * Multiplier, force_pos);

            //フリック監視
            if (DragMonitor.Drag)
            {
                if (Vector3.Distance(DragMonitor.TapPosition, DragMonitor.DragPosition) > 30f)
                {
                    draggingSpan += Time.deltaTime;
                }
            }


            if (oldDrag && !DragMonitor.Drag)
            {
                if (draggingSpan <= draggingSpanLimit)
                {
                    jumpForce = (DragMonitor.DragPosition - DragMonitor.TapPosition) * JumpMultiplier / draggingSpan;
                    jumpForce.z = -jumpForce.x;
                    jumpForce.x = 0;
                    //if (jumpForce.magnitude > 10f) jumpForce.magnitude = 10f;
                    Debug.Log(jumpForce);
                    StartCoroutine(JumpProc(jumpForce, force_pos));
                    yield break;
                }
                else
                {
                    draggingSpan = 0f;
                }

            }
            oldDrag = DragMonitor.Drag;

            yield return new WaitForFixedUpdate();

        }
    }

    IEnumerator JumpProc(Vector3 jumpForce, Vector3 forcePos)
    {
        jumpForce.y *= 3;
        rb.AddForceAtPosition(jumpForce * Multiplier, forcePos, ForceMode.Force);
        Spine1.AddForceAtPosition(jumpForce * Multiplier, forcePos, ForceMode.Force);
        R_UpLeg.AddForceAtPosition(jumpForce * Multiplier / 2, forcePos, ForceMode.Force);
        L_UpLeg.AddForceAtPosition(jumpForce * Multiplier / 2, forcePos, ForceMode.Force);
        rb_Trapaze.mass = 115f;
        rb_Trapaze.AddForceAtPosition(-jumpForce * Multiplier * 3, forcePos, ForceMode.Force);
        FreeClingJoints();
        yield break;

    }

    void FreeClingJoints()
    {
        foreach (Component joint in clingJoints)
        {
            Destroy(joint);
        }

    }
    void Preparation()
    {
        parent = transform.root.gameObject;
        parts = GetAllChildren(parent);
        clingJoints = GetAllClingJoint();
        mascles = GetAllMascle();
        foreach (GameObject obj in parts)
        {
            var tmpRb = obj.GetComponent<Rigidbody>();
            if (tmpRb != null)
            {
                tmpRb.maxAngularVelocity = 20f;
            }
        }

    

        rb = GetComponent<Rigidbody>();
        base_pos = rb_Trapaze.transform.InverseTransformPoint(rb.position);
        base_pos.x = 0f;
    }

    void SetDestination()
    {
        destination = rb_Trapaze.transform.TransformPoint(base_pos.x,
            base_pos.y + (Controller.GetTouchPosition.y * Scale) + offset_y,
            base_pos.z - (Controller.GetTouchPosition.x * Scale) + offset_z);
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
        if (stat == stat_enum.row && JUMP_on)
        {
            Jump();
            stat = stat_enum.pre_jump;
        }


        if (stat == stat_enum.pre_jump && !JUMP_on) stat = stat_enum.row;
        //Debug.Log(this.velocity);
    }


    private void FixedUpdate()
    {




        //Vector3 force = destination - rb.position; //ワールド座標差分ベクトル
        //Vector3 force_pos = rb.position;
        //force.y = y;
        //force.z *= z;
        if (stat == stat_enum.row)
        {
            foreach (var _mascle in mascles)
            {
                Vector3 _targetDirection = destination - _mascle.rb.position;
                
                _mascle.Hold(Vector3.Angle(_mascle.direction, _targetDirection));

            }

        //    rb.AddForceAtPosition(force * Multiplier, force_pos);
        //    rb_Trapaze.AddForceAtPosition(-force * Multiplier, force_pos);
        }



        /*
        Debug.Log(Controller.GetTouchPosition + " "
        + rb_Trapaze.transform.InverseTransformPoint(destination)
        
        ) ;
        */
    }

    void PointDraw(Vector3 pos)
    {


        line.SetPosition(0, pos); // オブジェクトの位置情
    }

    private class MascalSim
    {
        List<GameObject> _parts;

        public MascalSim(List<GameObject> parts)
        {
            _parts = parts;
            
        }

        GameObject FindChild(string targetName)
        {
            foreach (var item in _parts)
            {
                if (item.name == targetName)
                {
                    return item;

                }
            }
            return null;
        }
    }
}
