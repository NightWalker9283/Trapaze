﻿using UnityEngine;
using System.Collections;

//パラシュート
public class Parachute : MonoBehaviour
{
    public float coefficient = 10f;   // 空気抵抗係数
    [SerializeField] Rigidbody Player;
    //Cloth polyCloth;
    bool isFinish = false;
    bool isCollision = false;

    private void Start()
    {
        //polyCloth = GetComponentInChildren<Cloth>();
    }

    void FixedUpdate()
    {
        // 空気抵抗を与える
        var resistance = GetComponent<Rigidbody>().velocity;
        resistance.Set(resistance.x * -coefficient * 0.1f, resistance.y * -coefficient, resistance.z * -coefficient * 0.05f);
        GetComponent<Rigidbody>().AddForce(resistance);
    }

    private void Update()
    {

        if (!isFinish)
        {

            if (!isCollision && Player.velocity.y > -1f) isCollision = true;
            if (isCollision)　//何かに接触するとパラシュートが閉じる
            {
                isFinish = true;
                StartCoroutine(desableParachute());
                GetComponent<Animator>().SetTrigger("isCollision");

            }
        }
    }

    IEnumerator desableParachute()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        isCollision = true;
    }

}