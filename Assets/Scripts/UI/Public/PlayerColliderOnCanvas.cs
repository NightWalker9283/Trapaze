using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderOnCanvas : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float x, y, z;

    Camera cmrPublic, cmrUI;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        cmrPublic = PlayingManager.playingManager.cmrPublic;
        cmrUI = PlayingManager.playingManager.cmrUI;
        rigidbody = Target.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var posViewportPublic = cmrPublic.WorldToViewportPoint(Target.position);
        var posWorldUi = cmrUI.ViewportToWorldPoint(posViewportPublic);
        transform.position = posWorldUi;
        transform.localPosition = new Vector3(0f, transform.localPosition.y, transform.localPosition.z);
        transform.rotation = Target.rotation;
        transform.Rotate(x, y, z);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayingManager.playingManager.Stat == PlayingManager.Stat_global.play)
        {
            rigidbody.AddForce(0f, 0f, 20f, ForceMode.Impulse);
        }
        else
        {
            rigidbody.AddForce(0f, 2f, 50f, ForceMode.Impulse);
        }
    }

}
