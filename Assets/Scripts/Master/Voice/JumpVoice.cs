using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpVoice : MonoBehaviour
{
	AudioClip[] ac;
	public static JumpVoice jumpVoice;
	// Start is called before the first frame update
	private void Awake()
	{
		ac = Resources.LoadAll<AudioClip>("Voice/Jump");
		jumpVoice = this;
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
		VoiceManager.voiceManager.AddVoice(ac[UnityEngine.Random.Range(0, ac.Length)]);
	}

	public void Play(Action action)
	{
		VoiceManager.voiceManager.AddVoice(ac[UnityEngine.Random.Range(0, ac.Length)],action);
	}
}
