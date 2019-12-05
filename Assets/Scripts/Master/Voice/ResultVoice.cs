using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultVoice:MonoBehaviour
{
   

    VoiceCategory[] voices;
    // Start is called before the first frame update
    public ResultVoice()
    {
        var allComments = PlayingManager.playingManager.allComments;
        voices = new VoiceCategory[allComments.Count];
        for (int i = 0; i < allComments.Count; i++)
        {
            voices[i].DistanceL = allComments[i].DistanceL;
            voices[i].DistanceU = allComments[i].DistanceU;
            voices[i].HeightL = allComments[i].HeightL;
            voices[i].HeightU = allComments[i].HeightU;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
