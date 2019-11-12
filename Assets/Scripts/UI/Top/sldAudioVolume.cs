using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sldAudioVolume : MonoBehaviour
{
    Slider sld;
    float oldVolume;
    // Start is called before the first frame update
    void Start()
    {
        
        sld = GetComponent<Slider>();
        sld.onValueChanged.AddListener(CangeVolume);
        sld.value = GameMaster.gameMaster.settings.audio_volume;
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(GameMaster.gameMaster.settings.audio_volume);
        if (oldVolume != GameMaster.gameMaster.settings.audio_volume)
        {
            sld.value = GameMaster.gameMaster.settings.audio_volume;
        }

        oldVolume = GameMaster.gameMaster.settings.audio_volume;
    }

    void CangeVolume(float value)
    {
        if(GameMaster.gameMaster.settings.audio_enabled)
            GameMaster.gameMaster.SetBgmVolume(value);

        GameMaster.gameMaster.settings.audio_volume = value;

    }
}
