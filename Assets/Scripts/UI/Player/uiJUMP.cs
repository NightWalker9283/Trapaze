using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
//ジャンプ待機スライダー
public class uiJUMP : MonoBehaviour
{
    public static Color txtcol_JUMPoff = new Color(80f / 255f, 80f / 255f, 80f / 255f);
    public static Color txtcol_JUMPon = new Color(255f / 255f, 32f / 255f, 140f / 255f);
    [SerializeField] TextMeshProUGUI textJUMP;
    [SerializeField] AudioClip ac_on_SE,ac_on_Voice;
    [SerializeField] AudioClip ac_off_SE,ac_off_Voice;
    [SerializeField] AudioMixerGroup amgSE,amgVoice;
    [SerializeField] GameObject PlayerControllPoint;
    [SerializeField] GameObject Joystick;


    Settings settings;
    AudioSource audioSourceSE,audioSourceVoice;
    float oldVal = 0f;


    Slider sld_JUMP;
    // Start is called before the first frame update
    void Start()
    {
        sld_JUMP = GetComponent<Slider>();
        audioSourceSE = gameObject.AddComponent<AudioSource>();
        audioSourceVoice = gameObject.AddComponent<AudioSource>();
        audioSourceSE.volume = 0.2f;
        audioSourceVoice.volume = 0.5f;
        audioSourceSE.outputAudioMixerGroup = amgSE;
        audioSourceVoice.outputAudioMixerGroup = amgVoice;

        settings = GameMaster.gameMaster.settings;

    }

    // Update is called once per frame
    void Update()
    {



    }

    //スライダーの値監視
    public void monitorValue() //onEndDrag
    {
        if (sld_JUMP.value >= 0.5f)
        {
            if (oldVal != 1f)
            {
                if (settings.enable_voice)
                    audioSourceVoice.PlayOneShot(ac_on_Voice);
                else
                    audioSourceSE.PlayOneShot(ac_on_SE);
            }
            sld_JUMP.value = 1f;
            textJUMP.color = txtcol_JUMPon;
            fadeout(Joystick);
            PlayerControllPoint.GetComponent<PlayerController>().JUMP_on = true; //PlayerControllerに通知
        }
        else
        {
            if (oldVal != 0f)
            {
                audioSourceSE.PlayOneShot(ac_off_SE);
            }
            sld_JUMP.value = 0f;
            fadein(Joystick);
            textJUMP.color = txtcol_JUMPoff;
            PlayerControllPoint.GetComponent<PlayerController>().JUMP_on = false; //PlayerControllerに通知
        }
        oldVal = sld_JUMP.value;
    }

    //フェードアウト
    void fadeout(GameObject obj)
    {
        StartCoroutine(fadeout_proc(obj));
    }

    IEnumerator fadeout_proc(GameObject obj)
    {
        for (float f = 1f; f > 0f; f-=0.2f)
        {
            Color color=obj.GetComponent<Image>().color;
            Color newColor = new Color(color.r, color.g, color.b, f);
            obj.GetComponent<Image>().color = newColor;
            yield return null;
        }
        obj.SetActive(false);

    }

    //フェードイン
    void fadein(GameObject obj)
    {
        StartCoroutine(fadein_proc(obj));
    }

    IEnumerator fadein_proc(GameObject obj)
    {
        for (float f = 0f; f <= 1f; f += 0.2f)
        {
            Color color = obj.GetComponent<Image>().color;
            Color newColor = new Color(color.r, color.g, color.b, f);
            obj.GetComponent<Image>().color = newColor;
            yield return null;
        }
        obj.SetActive(true);

    }


}