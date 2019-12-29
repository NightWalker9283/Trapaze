using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sun : MonoBehaviour
{

    //　1秒間の回転角度
    [SerializeField]
    private float rotateSpeed = 0.1f;
    [SerializeField]
    private Vector3 rotater=new Vector3(1f, 0.3f, 0f);
    //　0時の角度
    [SerializeField]
    private Vector3 rot = new Vector3(270f, 330f, 0f);

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //　徐々に回転させる、X軸を反対方向に回転
        transform.Rotate(rotater * rotateSpeed * Time.deltaTime);
    }
}