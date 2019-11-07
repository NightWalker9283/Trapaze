using UnityEngine;
using System.Collections;

public class Parachute : MonoBehaviour
{
    public float coefficient = 10f;   // 空気抵抗係数
    [SerializeField] Rigidbody Player;
    Cloth polyCloth;

    private void Start()
    {
        polyCloth = GetComponentInChildren<Cloth>();
    }

    void FixedUpdate()
    {
        // 空気抵抗を与える
        var resistance = GetComponent<Rigidbody>().velocity;
        resistance.Set(resistance.x * -coefficient*0.1f, resistance.y * -coefficient, resistance.z * -coefficient * 0.1f);
        GetComponent<Rigidbody>().AddForce(resistance);
    }

    private void Update()
    {
        if (!polyCloth.enabled && Player.velocity.y > -1f)
        {
            polyCloth.enabled = true;
            StartCoroutine(desableParachute());
        }
    }
    IEnumerator desableParachute()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}