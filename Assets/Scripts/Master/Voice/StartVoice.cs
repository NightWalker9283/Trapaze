using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVoice : MonoBehaviour
{
    string path = "Voice/Start";
    List<AudioFile> lstAf;
    public static StartVoice startVoice;
    // Start is called before the first frame update
    private void Awake()
    {
        lstAf = VoiceManager.LoadAllAudioFile(path);
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
        
        VoiceManager.voiceManager.AddVoice(lstAf[UnityEngine.Random.Range(0, lstAf.Count)]) ;
    }
}
