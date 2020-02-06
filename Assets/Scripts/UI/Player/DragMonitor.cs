using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//画面上のドラッグ、タップ監視
[DefaultExecutionOrder(-1)] //Updateの一番最初に呼び出されることを保証
public sealed class DragMonitor : MonoBehaviour
{

    

    static public bool IsPointerOver { get; protected set; }
    static public bool Tap { get; protected set; } //タップ検出
    static public bool Drag { get; protected set; } //ドラッグ検出（ドラッグ中true）
    static public Vector3 TapPosition { get; protected set; } //タップ位置
    static public Vector3 DragPosition { get; protected set; } //ドラッグ中の現在位置（タップ位置との差分でドラッグ方向と距離算出可）



    //private
    Touch firstTouch;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTouch();
      
    }

    private int GetTouchCount()
    {


#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButton(0) ? 1 : 0;
#elif UNITY_ANDROID || UNITY_IOS
        return Input.touchCount;
#else
        return 0;
#endif
    }

    private Touch GetTouch(int touchIndex)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return new Touch
        {
            fingerId = 0,
            position = Input.mousePosition,
        };
#elif UNITY_ANDROID || UNITY_IOS
        return Input.GetTouch(touchIndex);
#else
        return new Touch(){
            fingerId = -1,
        };
#endif
    }

    private void UpdateTouch()
    {
        Tap = false;
        Drag = false;

        var currentTouchCount = GetTouchCount();
        if (currentTouchCount <= 0)
        {
            firstTouch.fingerId = -1;
            firstTouch.position = Vector3.zero;
        }
        else
        {
            var currentTouch = GetTouch(0);
            if (firstTouch.fingerId == -1)
            {
                Tap = true;
                firstTouch = currentTouch;
            }
            else
            {
                if (firstTouch.fingerId == currentTouch.fingerId)
                {
                    Drag = true;
                    firstTouch = currentTouch;
                }
            }

        }
        if (Tap) TapPosition = firstTouch.position;
        if (Drag) DragPosition = firstTouch.position;
    }



}
