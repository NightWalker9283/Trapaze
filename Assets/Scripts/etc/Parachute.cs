using UnityEngine;
using System.Collections;

public class Parachute : MonoBehaviour
{
    public float coefficient = 10f;   // 空気抵抗係数
    [SerializeField] Rigidbody Player;
    //Cloth polyCloth;
    bool isFinish = false;

    private void Start()
    {
        //polyCloth = GetComponentInChildren<Cloth>();
    }

    void FixedUpdate()
    {
        // 空気抵抗を与える
        var resistance = GetComponent<Rigidbody>().velocity;
        resistance.Set(resistance.x * -coefficient * 0.1f, resistance.y * -coefficient, resistance.z * -coefficient * 0.1f);
        GetComponent<Rigidbody>().AddForce(resistance);
    }

    private void Update()
    {
        if (!isFinish && Player.velocity.y > -1f)
        {
            GetComponent<Animator>().SetTrigger("isCollision");
            StartCoroutine(desableParachute());
            isFinish = true;
        }
    }
    IEnumerator desableParachute()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}