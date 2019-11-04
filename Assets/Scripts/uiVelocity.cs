using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uiVelocity : MonoBehaviour
{
    TextMeshProUGUI textVelocity;
    public Rigidbody MassPoint;
    // Start is called before the first frame update
    void Start()
    {
        textVelocity = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textVelocity.text = Mathf.Floor(MassPoint.velocity.magnitude*3.6f) + "km/h";
    }
}
