using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpVoice : MonoBehaviour
{
    string path = "Voice/Jump";
    List<AudioFile> lstAf;
	public static JumpVoice jumpVoice;
	// Start is called before the first frame update
	private void Awake()
	{
        lstAf = VoiceManager.LoadAllAudioFile(path);
		jumpVoice = this;
	}

	

	public void Play()
	{
        

        VoiceManager.voiceManager.AddVoice(lstAf[UnityEngine.Random.Range(0, lstAf.Count)]);
	}

	public void Play(Action action)
	{
		VoiceManager.voiceManager.AddVoice(lstAf[UnityEngine.Random.Range(0, lstAf.Count)], action);
	}
}
