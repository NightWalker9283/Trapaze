using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnViewPrivacyPolicy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(()=>
        {
            Application.OpenURL("https://nightwalker9283.wixsite.com/trapeze");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
