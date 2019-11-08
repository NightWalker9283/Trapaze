using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnPause : MonoBehaviour
{
    enum stat_run { run, pause };
    stat_run stat = stat_run.run;
    [SerializeField] Image image;
    [SerializeField] Sprite sprRun, sprPause;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SwitchRunStat);
    }

    // Update is called once per frame
    void SwitchRunStat()
    {

        if (stat == stat_run.run)
        {
            Time.timeScale = 0f;
            image.sprite = sprRun;
            stat = stat_run.pause;
        }
        else if (stat == stat_run.pause)
        {
            Time.timeScale = 1f;
            image.sprite = sprPause;
            stat = stat_run.run;
        }
    }

}
