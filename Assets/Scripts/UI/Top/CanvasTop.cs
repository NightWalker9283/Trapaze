using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTop : MonoBehaviour
{
    Image image;
    public static CanvasTop canvasTop ;

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
        canvasTop = this;
        
    }
    public void FadeinScene()
    {
        image.enabled = true;
        StartCoroutine(fadeinSceneProc());
    }

    public void FadeoutScene()
    {
        image.enabled = true;
        StartCoroutine(fadeoutSceneProc());
    }

    public void ImmediatelyInScene()
    {
        image.enabled = false;
    }
    public void ImmediatelyOutScene()
    {
        image.enabled = true;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }


    IEnumerator fadeinSceneProc()
    {
        float alpha = 1f;
        while (alpha > 0f){
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Clamp01(alpha));
            alpha = alpha-Time.deltaTime;
            yield return null;
        }
        image.enabled = false;

    }

    IEnumerator fadeoutSceneProc()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f-Mathf.Clamp01(alpha));
            alpha = alpha - Time.deltaTime;
            yield return null;
        }

    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
