using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMonitor : MonoBehaviour
{
    Titles titles;
    Rigidbody player;
    public List<int> acquiredTitles = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        titles = GameMaster.gameMaster.titles;
        player = PlayingManager.playingManager.playerControlPoint;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Result(float distance)
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
                break;
            }
        }





    }

}
