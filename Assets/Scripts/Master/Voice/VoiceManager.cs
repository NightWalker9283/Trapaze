using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{

    public static VoiceManager voiceManager;
    Queue<AudioClip> queue=new Queue<AudioClip>();
    AudioSource audioSource;
   
    // Start is called before the first frame update
    private void Awake()
    {

        voiceManager = this;
        audioSource = GetComponent<AudioSource>();
        
      

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (queue.Count > 0 && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(queue.Dequeue());

        }


    }

    public void AddVoice(AudioClip ac)
    {
        queue.Enqueue(ac);
    }

    public void AddVoice(AudioClip ac,Action callback)
    {
        queue.Enqueue(ac);
        StartCoroutine(MonitorIsPlaying());


        IEnumerator MonitorIsPlaying()
        {
            yield return null;
            while (audioSource.isPlaying)
            {

                yield return null;
            }
            callback();

        }

    }


    public void Reset()
    {
        audioSource.Stop();
        queue.Clear();
    }
    public int GetCount()
    {
        return queue.Count;
    }
   

}
