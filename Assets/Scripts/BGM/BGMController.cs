using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMController : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip[] acs; //BGMリスト
    [SerializeField] AudioMixerGroup amgSE;
    int playIdx = 0;
    bool inPreparation=false;   //準備中判定

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = amgSE;
        if (PlayingManager.playingManager.isTraining) //トレーニングモードは1曲ループ
        {
            audioSource.loop = true;
        }
        else
        {
            for (int i = 0; i < acs.Length; i++)　//BGMリストシャッフル
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
            
            StartCoroutine(PlayBGM(acs[playIdx]));
            playIdx = (playIdx + 1) % acs.Length;
        }


    }

    IEnumerator PlayBGM(AudioClip ac)
    {
        inPreparation = true;
        if (!PlayingManager.gameMaster.isTutorial)　//チュートリアルは開始時演出がないのでウェイトなしで開始
        {
            yield return new WaitForSeconds(6f);　//開始時演出の待機と次曲再生までの待機を兼用
        }
        audioSource.PlayOneShot(ac);
        inPreparation = false;
    }
}
