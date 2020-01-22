using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderOnCanvas : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float x, y, z;


    Camera cmrPublic, cmrUI;
    Rigidbody rigidbody;
    BoxCollider bc;

    // Start is called before the first frame update
    void Start()
    {
        cmrPublic = PlayingManager.playingManager.cmrPublic;
        cmrUI = PlayingManager.playingManager.cmrUI;
        rigidbody = Target.GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target.gameObject.activeSelf)
        {
            if (!bc.enabled)
            {
                bc.enabled = true;

            }
            var posViewportPublic = cmrPublic.WorldToViewportPoint(Target.position);
            var posWorldUi = cmrUI.ViewportToWorldPoint(posViewportPublic);
            transform.position = posWorldUi;
            transform.localPosition = new Vector3(0f, transform.localPosition.y, transform.localPosition.z);
            transform.rotation = Target.rotation;
            transform.Rotate(x, y, z);
        }
        else
        {
            if (bc.enabled) bc.enabled = false;
        }
    }
    bool isHitBack = false;
    private void OnTriggerEnter(Collider other)
    {
        if (PlayingManager.playingManager.Stat == PlayingManager.Stat_global.play)
        {
            if (rigidbody.velocity.z >= 0f)
            {
                rigidbody.AddForce(0f, 5f, 20f, ForceMode.Impulse);
                other.enabled = false;
            }
            else
            {
                if (!isHitBack)
                {
                    rigidbody.AddForce(0f, 0f, 5f, ForceMode.Impulse);
                    isHitBack = true;
                }
            }
        }
        if (PlayingManager.playingManager.Stat == PlayingManager.Stat_global.jump ||
            PlayingManager.playingManager.Stat == PlayingManager.Stat_global.fly)
        {

            rigidbody.AddForce(0f, 0f, 30f, ForceMode.Impulse);
            other.enabled = false;
        }
    }

}
