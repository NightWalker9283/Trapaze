using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultVoice : MonoBehaviour
{
    string[] path = {
        "Voice/Result/Category0",
        "Voice/Result/Category1",
        "Voice/Result/Category2",
        "Voice/Result/Category3",
        "Voice/Result/Category4"
    };


    VoiceCategory[] voices;
    AudioFile vicDaichiSansho;
    public static ResultVoice resultVoice;


    private void Awake()
    {
        resultVoice = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        vicDaichiSansho = VoiceManager.LoadAudioFile("Voice/Result/大地讃頌");

        voices = new VoiceCategory[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            voices[i].AudioFiles = VoiceManager.LoadAllAudioFile(path[i]);
        }

        voices[0].DistanceL = 0;
        voices[0].DistanceU = 40f;

        voices[1].DistanceL = 40f;
        voices[1].DistanceU = 80f;

        voices[2].DistanceL = 80f;
        voices[2].DistanceU = 100f;

        voices[3].DistanceL = 100f;
        voices[3].DistanceU = 140f;

        voices[4].DistanceL = 140f;
        voices[4].DistanceU = 10000f;
    }

    public void Play(TitleObject titleObject)
    {
        bool isSpecialTitle = false;

        if (titleObject.id == 200)
        {
            VoiceManager.voiceManager.AddVoice(vicDaichiSansho);
            isSpecialTitle = true;
        }
        if (!isSpecialTitle)
        {
            var Player = PlayingManager.playingManager.playerControlPoint;
            var PlayerPosZ = Mathf.Abs(Player.position.z);
            VoiceCategory TargetCategory = new VoiceCategory();

            for (int i = 0; i < voices.Length; i++)
            {

                if (PlayerPosZ >= voices[i].DistanceL && PlayerPosZ < voices[i].DistanceU)
                {
                    TargetCategory = voices[i];
                    break;
                }
            }

            VoiceManager.voiceManager.AddVoice(
                TargetCategory.AudioFiles[Random.Range(0, TargetCategory.AudioFiles.Count)]
            );
        }
    }

}
