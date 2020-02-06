using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]


public class PlayerController : MonoBehaviour
{
    // PUBLIC
    public bool JUMP_on = false; //ジャンプ待機スライダーからの有効通知用
    public float velocity { get; protected set; } //プレイヤーの移動速度（ブランコの計測点の速度。ジャンプ後はHipの速度）

    // PRIVATE

    [SerializeField] SimpleTouchController Controller;
    [SerializeField] Rigidbody rb_Trapaze;
    [SerializeField] float Multiplier = 10f;
    [SerializeField] float JumpMultiplier = 100f;
    [SerializeField] float Scale = 0.25f;
    [SerializeField] float offset_z, offset_y;
    [SerializeField] Rigidbody rb_tracePoint; //計測点
    [SerializeField] float y, z;
    [SerializeField] Transform Parachute;

    Rigidbody rb;
    Vector3 base_pos;
    Vector3 destination;    //Hipの目標位置のワールド座標
    LineRenderer line;
    GameObject parent;
    List<GameObject> parts;
    List<Component> clingJoints;
    List<mascle> mascles;
    public bool isOpenParachute { get; set; } = false;
    public enum stat_enum { row, pre_jump, jump, fly, finish };
    public stat_enum stat { get; private set; } = stat_enum.row;
    Rigidbody L_UpLeg, R_UpLeg, Spine1;
    mascle mslL_Leg, mslR_Leg;
    Trank mslL_UpLeg, mslR_UpLeg;

    //全部位IsKinematic一括操作
    public void SetAllIsKinematic(bool value)
    {
        foreach (var item in parts)
        {
            var rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = value;
            }
        }
    }
    //GetChildrenでリスト化した全部位ゲームオブジェクトを返す
    List<GameObject> GetAllChildren(GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren;
    }
    //再帰的に全部位ゲームオブジェクトをメンバ変数のリストに追加する
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
    //姿勢制御用の各種ジョイントをリスト化して返す
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
    //全部位にアタッチされているmuscleをリスト化して返す
    List<mascle> GetAllMascle()
    {
        List<mascle> allMascles = new List<mascle>();

        foreach (GameObject obj in parts)
        {
            var _rb = obj.GetComponent<Rigidbody>();
            var _masle = obj.GetComponent<mascle>();
            if (_masle != null) allMascles.Add(_masle);
        }
        return allMascles;

    }
    //名称で指定した部位のゲームオブジェクトを返す（先にGetAllChildrenを呼び出しておくこと）
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

    //ジャンプ（ジャンプ待機）
    void Jump()
    {
        StartCoroutine(PreJumpProc());
    }
    //ジャンプ待機
    IEnumerator PreJumpProc()
    {
        bool oldDrag = false;
        float draggingSpan = 0f; //ドラッグ開始からの時間[s]
        float draggingSpanLimit = 0.5f; //ドラッグ終了までの制限時間。ゆっくりドラッグすると無視する。
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
            SetDestination(0.7f, -1f);
            //Debug.Log(preForce);

            //フリック監視
            if (DragMonitor.Drag) //ドラッグ中
            {
                if (Vector3.Distance(DragMonitor.TapPosition, DragMonitor.DragPosition) > 30f) //一定距離以上ドラッグ
                {
                    draggingSpan += Time.deltaTime;
                }
            }


            if (oldDrag && !DragMonitor.Drag)
            {
                if (draggingSpan <= draggingSpanLimit && draggingSpan > 0) //一定時間以内にドラッグ終了
                {
                   

                    jumpForce = (DragMonitor.DragPosition - DragMonitor.TapPosition).normalized * JumpMultiplier;
                    jumpForce.z = -jumpForce.x;
                    jumpForce.x = 0;
                    jumpForce = rb_Trapaze.transform.TransformDirection(jumpForce);
                    //Debug.Log(jumpForce);
                    PlayingManager.playingManager.Stat = PlayingManager.Stat_global.jump;
                    stat = stat_enum.jump;
                    StartCoroutine(JumpProc(jumpForce, force_pos));
                    yield break;
                }
                else
                {
                    draggingSpan = 0f; //ゆっくりドラッグされたことを検知した場合最初から
                }

            }
            oldDrag = DragMonitor.Drag;

            yield return new WaitForFixedUpdate();

        }
    }
    //ジャンプ（実際のジャンプ）
    IEnumerator JumpProc(Vector3 jumpForce, Vector3 forcePos)
    {
        rb.AddForceAtPosition(jumpForce * Multiplier, forcePos, ForceMode.Force);
        Spine1.AddForceAtPosition(jumpForce * Multiplier, forcePos, ForceMode.Force);
        //R_UpLeg.AddForceAtPosition(jumpForce * Multiplier / 2, forcePos, ForceMode.Force);
        //L_UpLeg.AddForceAtPosition(jumpForce * Multiplier / 2, forcePos, ForceMode.Force);
        rb_Trapaze.mass = 115f;
        rb_Trapaze.AddForceAtPosition(-jumpForce * Multiplier * 3, forcePos, ForceMode.Force);
        FreeClingJoints();
        FreeHoldMascles();
        mslL_UpLeg.Hold(180f);
        mslR_UpLeg.Hold(180f);
        mslL_Leg.Hold(180f);
        mslR_Leg.Hold(180f);
        stat = stat_enum.fly;
        StartCoroutine(DelayFreeHoldMasclesProc());
        StartCoroutine(FlyProc());
        yield break;
    }
    //ジャンプ後
    IEnumerator FlyProc()
    {
        Vector3 stopPos;
        float LIMIT_WDT = 3f;
        float LIMIT_DISTANCE = 0.1f;
        float LOWER_LIMIT_VELOCITY = 5f;
        float wdt;
        StartCoroutine(MonitorDistancefromTrapeze());
        while (stat == stat_enum.fly)
        {
            //  Debug.Log(velocity);
            if (velocity < LOWER_LIMIT_VELOCITY)
            {
                stopPos = rb.position;
                wdt = 0f;
                while (Vector3.Distance(rb.position, stopPos) < LIMIT_DISTANCE)
                {
                    //                    Debug.Log("b");
                    if (wdt > LIMIT_WDT)
                    {
                        stat = stat_enum.finish;
                        yield break;
                    }
                    wdt += Time.deltaTime;
                    yield return null;
                }
            }
            yield return null;
        }
    }
    //ジャンプ後のブランコ引っかかり検知
    IEnumerator MonitorDistancefromTrapeze()
    {

        float LIMIT_WDT = 5f;
        float LIMIT_DISTANCE = 1f;

        float wdt;

        while (stat == stat_enum.fly)
        {
            wdt = 0f;
            while (Vector3.Distance(rb.position, rb_tracePoint.position) < LIMIT_DISTANCE)
            {
                if (wdt > LIMIT_WDT)
                {
                    stat = stat_enum.finish; //ブランコから一定範囲内に一定時間とどまっていると強制的にリザルト
                    yield break;
                }
                wdt += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }
    //全身の筋肉を脱力。遅延付き
    IEnumerator DelayFreeHoldMasclesProc()
    {
        yield return new WaitForSeconds(1f);
        FreeHoldMascles();
        mslL_UpLeg.isHold = false;
        mslL_UpLeg.Free();
        mslR_UpLeg.isHold = false;
        mslR_UpLeg.Free();
        GetComponent<PlayerSound>().isCrackSound = false;

    }
    //姿勢制御用各種ジョイントを開放
    void FreeClingJoints()
    {
        foreach (Component joint in clingJoints)
        {
            Destroy(joint);
        }

    }
    //全身の筋肉（Trank以外）を脱力
    void FreeHoldMascles()
    {
        foreach (var _mascle in mascles)
        {
            _mascle.Free();

        }
    }

    //準備
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
                tmpRb.maxAngularVelocity = 30f;
            }
        }

        Spine1 = FindChild("Spine1").GetComponent<Rigidbody>();
        R_UpLeg = FindChild("R_UpLeg").GetComponent<Rigidbody>();
        L_UpLeg = FindChild("L_UpLeg").GetComponent<Rigidbody>();

        mslL_Leg = FindChild("L_Leg").GetComponent<mascle>();
        mslR_Leg = FindChild("R_Leg").GetComponent<mascle>();
        mslL_UpLeg = FindChild("L_UpLeg").GetComponent<Trank>();
        mslR_UpLeg = FindChild("R_UpLeg").GetComponent<Trank>();


        rb = GetComponent<Rigidbody>();
        base_pos = rb_Trapaze.transform.InverseTransformPoint(rb.position);
        base_pos.x = 0f;
    }

    //仮想コントローラーの入力からHipの目標位置を計算
    void SetDestination()
    {
        destination = rb_Trapaze.transform.TransformPoint(base_pos.x,
            base_pos.y + (Controller.GetTouchPosition.y * Scale) + offset_y,
            base_pos.z - (Controller.GetTouchPosition.x * Scale) + offset_z);
    }
    //Hipの目標位置を指定
    void SetDestination(float z, float y)
    {
        destination = rb_Trapaze.transform.TransformPoint(base_pos.x,
            base_pos.y + (y * Scale) + offset_y,
            base_pos.z - (z * Scale) + offset_z);
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
        //速度表示
        if (stat == stat_enum.row || stat == stat_enum.pre_jump)
        {
            this.velocity = rb_tracePoint.velocity.magnitude * 3.6f;
        }
        else
        {
            this.velocity = rb.velocity.magnitude * 3.6f;
        }

        //漕いでいる最中
        if (stat == stat_enum.row)
        {
            SetDestination();
            if (JUMP_on)
            {
                Jump();
                stat = stat_enum.pre_jump;
            }

        }

        //ジャンプ待機
        else if (stat == stat_enum.pre_jump)
        {

            if (!JUMP_on) stat = stat_enum.row;
        }
        //ジャンプ後
        else if (stat == stat_enum.fly)
        {

        }

        //Debug.Log(this.velocity);
    }


    private void FixedUpdate()
    {


        if (stat == stat_enum.row || stat == stat_enum.pre_jump)
        {
            foreach (var _mascle in mascles)
            {
                Vector3 _targetDirection = destination - _mascle.rb.position;

                _mascle.Hold(Vector3.Angle(_mascle.direction, _targetDirection));

            }

        }

    }





    /*
    Debug.Log(Controller.GetTouchPosition + " "
    + rb_Trapaze.transform.InverseTransformPoint(destination)

    ) ;
    */

    //パラシュートを開く
    public void OpenParachute()
    {
        StartCoroutine(openParachuteProc());
    }
    //パラシュートを開く実際の処理
    IEnumerator openParachuteProc()
    {
        Parachute.gameObject.SetActive(true);
        yield break;
    }
    //デバッグ用
    void PointDraw(Vector3 pos)
    {


        line.SetPosition(0, pos); // オブジェクトの位置情
    }

    //未使用
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
