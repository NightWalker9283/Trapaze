using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMonitor : MonoBehaviour
{
    Titles titles;
    Rigidbody rbPlayer, rbTrapeze;
    public List<int> acquiredTitles = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        titles = GameMaster.gameMaster.titles;
        rbPlayer = PlayingManager.playingManager.playerControlPoint;
        rbTrapeze = PlayingManager.playingManager.rb_Trapeze;
        if (GameMaster.gameMaster.acquiredTitles.Find(id => id == titles.ttlTrapezeMachine.id) == 0)
        {
            StartCoroutine(MonitorGiantSwing());
        }
    }
    // Update is called once per frame
    void Update()
    {

    }


    float w, old_w;
    IEnumerator MonitorGiantSwing()
    {

        w = rbTrapeze.rotation.w;
        old_w = w;
        while (true)
        {
            if (PlayingManager.playingManager.isUsedMayo) break;

            w = rbTrapeze.rotation.w;
            if (Mathf.Sign(w) != Mathf.Sign(old_w))
            {
                acquiredTitles.Add(titles.ttlTrapezeMachine.id);
                WndGetTitle.wndGetTitle.ShowMessage(titles.ttlTrapezeMachine.name);
                break;
            }
            old_w = w;
            yield return null;
        }
    }
    public StandardTitle Result(float distance)
    {
        List<StandardTitle> standardTitles;
        if (Mathf.Sign(distance) >= 0 || Mathf.Abs(distance) < 10f)
        {
            standardTitles = titles.plusDistanceTitles;
        }
        else
        {
            standardTitles = titles.minusDistanceTitles;
        }

        for (int i = standardTitles.Count - 1; i >= 0; i--)
        {
            if (Mathf.Abs(distance) > Mathf.Abs(standardTitles[i].distance))
            {
                acquiredTitles.Add(standardTitles[i].id);
                return standardTitles[i];
            }
        }
        return null;
    }

    public void VoiceTrigger(AudioFile af)
    {
        if (GameMaster.gameMaster.acquiredTitles.Find(id => id == titles.ttlHyperOmoshiroi.id) == 0)
        {
            if (af.audioClip.name == "ハイパー面白いくん")
            {
                WndGetTitle.wndGetTitle.ShowMessage(titles.ttlHyperOmoshiroi.name);
                acquiredTitles.Add(titles.ttlHyperOmoshiroi.id);
            }
        }
    }
}
