using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{




    AudioSource audioSource;
    [SerializeField] AudioClip[] acs;
    int playIdx = 0;
    bool inPreparation=false;   //準備中判定

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.05f;
        for (int i = 0; i < acs.Length; i++)
        {
            acs[Random.Range(0, acs.Length)] = acs[Random.Range(0, acs.Length)];
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && !inPreparation)
        {
            
            StartCoroutine(playBGM(acs[playIdx]));
            playIdx = (playIdx + 1) % 5;
        }


    }
    IEnumerator playBGM(AudioClip ac)
    {
        inPreparation = true;
        yield return new WaitForSeconds(3);
        audioSource.PlayOneShot(ac);
        inPreparation = false;
    }
}
