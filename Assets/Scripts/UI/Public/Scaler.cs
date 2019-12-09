using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    Camera cmrPublic;
    // Start is called before the first frame update
    void Start()
    {
        cmrPublic = PlayingManager.playingManager.cmrPublic;
    }

    // Update is called once per frame
    void Update()
    {
        var scale = 100f * (2f / cmrPublic.orthographicSize);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
