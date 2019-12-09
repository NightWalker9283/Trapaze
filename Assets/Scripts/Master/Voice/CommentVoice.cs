using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentVoice : MonoBehaviour
{
    string path = "Voice/Comment";
    List<AudioFile> lstAf;
    public static CommentVoice commentVoice;
    // Start is called before the first frame update
    private void Awake()
    {
        lstAf = VoiceManager.LoadAllAudioFile(path);
        commentVoice = this;
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