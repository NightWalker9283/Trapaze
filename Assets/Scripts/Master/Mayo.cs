using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mayo : MonoBehaviour
{
    [SerializeField] RectTransform virtualMayo;
    float deadTime = 3f;
    Coroutine crtnJudge = null, crtnDrop = null;
    Camera cmrPublic, cmrUI;
    Canvas cvsPublic;
    Rect rect = new Rect(0, -0.1f, 1, 1.2f);

    // Start is called before the first frame update
    void Start()
    {

        cmrPublic = PlayingManager.playingManager.cmrPublic;
        cmrUI = PlayingManager.playingManager.cmrUI;
        cvsPublic = PlayingManager.playingManager.cvsPublic;
    }

    // Update is called once per frame
    void Update()
    {
        if (crtnDrop == null &&
            PlayingManager.playingManager.mayoCount < 3 &&
            PlayingManager.playingManager.Stat == PlayingManager.Stat_global.play &&
            Time.timeScale > 0)
        {


            if (deadTime > 0f)
            {
                deadTime -= Time.deltaTime;
            }
            else
            {
                if (crtnJudge == null) crtnJudge = StartCoroutine(JudgeStartMayo());
            }

        }
        if (crtnJudge != null && PlayingManager.playingManager.Stat != PlayingManager.Stat_global.play)
        {
            StopCoroutine(crtnJudge);
            crtnJudge = null;
        }
    }

    IEnumerator JudgeStartMayo()
    {
        bool isStart = false;
        while (!isStart)
        {
            if (Random.Range(0, 2) == 0)
            {
                crtnDrop = StartCoroutine(DropMayo());

                isStart = true;
            }
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator DropMayo()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<AudioSource>().enabled = true;

        Vector3 posViewportPublic, posScreenCvsPublic;
        var posWorldMayo = cmrPublic.ViewportToWorldPoint(new Vector3(0.2f, 1f));
        var isIgnore=false;
        var direction = new Vector3(0, -0.01f, 0);
        var canvasRect = cvsPublic.GetComponent<RectTransform>().rect;
        transform.position = new Vector3(4f, posWorldMayo.y, posWorldMayo.z);
        while (!isIgnore)
        {
            if (Time.timeScale > 0)
            {
                transform.position+=direction;
                posViewportPublic = cmrPublic.WorldToViewportPoint(transform.position);
                posScreenCvsPublic = cmrUI.ViewportToScreenPoint(posViewportPublic);
           
                virtualMayo.localPosition = new Vector2(posScreenCvsPublic.x-canvasRect.width/2f, posScreenCvsPublic.y-canvasRect.height/2f);
                if (!rect.Contains(posViewportPublic)) isIgnore = true;
            }

            yield return null;
        }
        deadTime = 180f;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
        crtnDrop = null;
    }

    public void Finish()
    {
        StopCoroutine(crtnDrop);
        crtnDrop = null;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
        deadTime = 180f;
    } 
}
