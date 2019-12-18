using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBar : MonoBehaviour
{
    [SerializeField] Mask mask;
    [SerializeField] CanvasGroup cvsgtitleElements;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        if (mask.showMaskGraphic)
        {
            mask.showMaskGraphic = false;
            cvsgtitleElements.alpha=0f;
        }
        else
        {
            mask.showMaskGraphic = true;
            cvsgtitleElements.alpha=1f;
        }

    }

}
