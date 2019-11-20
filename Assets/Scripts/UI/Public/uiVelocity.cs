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
        StartCoroutine(UpdateText());
    }

    // Update is called once per frame
    IEnumerator UpdateText()
    {
        while(true){
            textVelocity.text = "<#000000><mspace=0.65em>" + ((int)Mathf.Floor(MassPoint.velocity.magnitude * 3.6f)).ToString("D3") + "km/h";
            yield return new WaitForSeconds(0.3f);
        }
    }
}
