using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ParachutePayloadLogic : MonoBehaviour
{
    public ParachuteLogic coreParachuteLogic;

    public void OnCollisionEnter(Collision collision)
    {
        coreParachuteLogic.payloadCollisionEnter(collision);
    }
}
