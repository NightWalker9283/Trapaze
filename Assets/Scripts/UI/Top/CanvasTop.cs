using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTop : MonoBehaviour
{
    [SerializeField]Image image;
    public static CanvasTop canvasTop ;

    // Start is called before the first frame update
    void Awake()
    {
        
        canvasTop = this;
        
    }
    public void FadeinScene()
    {
        image.gameObject.SetActive(true);
        image.transform.SetAsLastSibling();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        StartCoroutine(fadeinSceneProc());
    }

    public void FadeoutScene()
    {
        image.gameObject.SetActive(true);
        image.transform.SetAsLastSibling();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        StartCoroutine(fadeoutSceneProc());
    }

    
    public void ImmediatelyInScene()
    {
        image.gameObject.SetActive(false);
        image.transform.SetAsLastSibling();
    }
    public void ImmediatelyOutScene()
    {
        image.gameObject.SetActive(true);
        image.transform.SetAsLastSibling();
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
        image.gameObject.SetActive(false);

    }

    IEnumerator fadeoutSceneProc()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Clamp01(alpha));
            alpha = alpha + Time.deltaTime;
            yield return null;
        }
        image.gameObject.SetActive(false);

    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
