using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMController : MonoBehaviour
{




    AudioSource audioSource;
    [SerializeField] AudioClip[] acs;
    [SerializeField] AudioMixerGroup amgSE;
    int playIdx = 0;
    bool inPreparation=false;   //準備中判定

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = amgSE;
        if (PlayingManager.playingManager.isTraining)
        {
            audioSource.loop = true;
        }
        else
        {
            for (int i = 0; i < acs.Length; i++)
            {
                acs[Random.Range(0, acs.Length)] = acs[Random.Range(0, acs.Length)];
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && !inPreparation)
        {
            
            StartCoroutine(playBGM(acs[playIdx]));
            playIdx = (playIdx + 1) % acs.Length;
        }


    }
    IEnumerator playBGM(AudioClip ac)
    {
        inPreparation = true;
        yield return new WaitForSeconds(6f);
        audioSource.PlayOneShot(ac);
        inPreparation = false;
    }
}
