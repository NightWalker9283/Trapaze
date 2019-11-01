using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(test);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void test()
    {
        if (Time.timeScale >= 1f)
        {
            Time.timeScale = 0f;
        }else if (Time.timeScale <= 0f)
        {
            Time.timeScale = 1f;
        }
    }
}
