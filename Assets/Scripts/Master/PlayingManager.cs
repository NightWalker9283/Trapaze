using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayingManager : MonoBehaviour
{
    [SerializeField] uiDistance txtDistance;
    [SerializeField] CanvasGroup ugForPlay, ugController, ugForAfterJump;
    [SerializeField] Camera cmrPlayerView, cmrPublic, cmrPlayer,cmrUI;
    [SerializeField] Rigidbody Player;
    [SerializeField] uiVelocity txtVelocity;


    public static stat_global stat { get; set; }
    public enum stat_global { play, pause, jump, result };
    static stat_global _oldStat;
    // Start is called before the first frame update
    void Start()
    {
        ugForAfterJump.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (stat != _oldStat)
        {
            switch (stat)
            {
                case stat_global.play:

                    break;
                case stat_global.jump:
                    StartCoroutine(fadein(ugForAfterJump));
                    StartCoroutine(fadeout(ugForPlay));
                    StartCoroutine(cameraRectChangeRight(cmrUI, 1f));
                    StartCoroutine(cameraRectChangeRight(cmrPublic, 1f));
                    StartCoroutine(cameraRectChangeRight(cmrPlayerView, 1f));
                    StartCoroutine(cameraRectChangeLeft(cmrPlayer, 0f));
                    txtDistance.StartMessDistance();
                    cmrPlayerView.transform.parent = Player.transform;
                    
                    cmrPublic.GetComponent<PublicCamera>().stat = PublicCamera.stat_publicCamera.jump;
                    txtVelocity.MassPoint = Player;
                    break;
                case stat_global.pause:

                    break;
                case stat_global.result:

                    break;
                default:
                    break;
            }
        }

        _oldStat = stat;
    }
   

    IEnumerator fadeout(CanvasGroup cg)
    {
        for (float f = 1f; f > 0f; f -= 0.2f)
        {
            cg.alpha = f;
            yield return null;
        }
        cg.interactable = false;

    }

   

    IEnumerator fadein(CanvasGroup cg)
    {
        for (float f = 0f; f <= 1f; f += 0.2f)
        {
            cg.alpha = f;
            yield return null;
        }
        cg.interactable = true;

    }

  
    IEnumerator cameraRectChangeRight(Camera targetCamera,float width)
    {
        float nowWidth=targetCamera.rect.width;
        float diff = 0.05f;

        if (nowWidth < width)
        {
            while (nowWidth<width)
            {
                Rect rect = new Rect(targetCamera.rect);
                nowWidth += diff;
                rect.width = Mathf.Clamp01(nowWidth);
                targetCamera.rect = rect;
                yield return null;
            }
        }
        else if (nowWidth >= width)
        {
            while (nowWidth>width)
            {
                Rect rect = new Rect(targetCamera.rect);
                nowWidth -= diff;
                rect.width = Mathf.Clamp01(nowWidth);
                targetCamera.rect = rect;
                yield return null;
            }

        }
    }
    IEnumerator cameraRectChangeLeft(Camera targetCamera, float width)
    {
        float nowWidth = targetCamera.rect.width;
        float nowX = targetCamera.rect.x;
        float diff = 0.05f;

        if (nowWidth < width)
        {
            while (nowWidth < width)
            {
                Rect rect = new Rect(targetCamera.rect);
               
                nowWidth += diff;
                nowX -= diff;
                rect.width = Mathf.Clamp01(nowWidth);
                rect.x = Mathf.Clamp01(nowX);
                targetCamera.rect = rect;
                yield return null;
            }
        }
        else if (nowWidth >= width)
        {
            while (nowWidth > width)
            {
                Rect rect = new Rect(targetCamera.rect);
                nowWidth -= diff;
                nowX += diff;
                rect.width = Mathf.Clamp01(nowWidth);
                rect.x = Mathf.Clamp01(nowX);
                targetCamera.rect = rect;
                yield return null;
            }

        }
    }

}
