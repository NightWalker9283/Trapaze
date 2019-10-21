using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class uiJUMP : MonoBehaviour
{
    public static Color txtcol_JUMPoff = new Color(255f / 255f, 250f / 255f, 136f / 255f);
    public static Color txtcol_JUMPon = new Color(255f / 255f, 32f / 255f, 140f / 255f);
    [SerializeField] Text textObject;
    [SerializeField] AudioClip ac_on;
    [SerializeField] AudioClip ac_off;
    AudioSource audioSource;
    float oldVal = 0f;


    Slider sld_JUMP;
    // Start is called before the first frame update
    void Start()
    {
        sld_JUMP = GetComponent<Slider>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.2f;

    }

    // Update is called once per frame
    void Update()
    {



    }


    public void monitorValue() //onEndDrag
    {
        if (sld_JUMP.value >= 0.5f)
        {
            if (oldVal != 1f)
            {
                audioSource.PlayOneShot(ac_on);
            }
            sld_JUMP.value = 1f;
            textObject.color = txtcol_JUMPon;
        }
        else
        {
            if (oldVal != 0f)
            {
                audioSource.PlayOneShot(ac_off);
            }
            sld_JUMP.value = 0f;
            textObject.color = txtcol_JUMPoff;
        }
        oldVal = sld_JUMP.value;
    }
}