using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVoice : MonoBehaviour
{
    AudioClip[] ac;
    public static StartVoice startVoice;
    // Start is called before the first frame update
    private void Awake()
    {
        ac = Resources.LoadAll<AudioClip>("Voice/Start");
        startVoice = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        VoiceManager.voiceManager.AddVoice(ac[Random.Range(0, ac.Length)]) ;
    }
}
