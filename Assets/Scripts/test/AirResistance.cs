using UnityEngine;
using System.Collections;

public class AirResistance : MonoBehaviour
{
    public float coefficient = 10f;   // 空気抵抗係数

    void FixedUpdate()
    {
        // 空気抵抗を与える
        var resistance = GetComponent<Rigidbody>().velocity;
        resistance.Set(resistance.x * -coefficient*0.1f, resistance.y * -coefficient, resistance.z * -coefficient * 0.1f);
        GetComponent<Rigidbody>().AddForce(resistance);
    }
}