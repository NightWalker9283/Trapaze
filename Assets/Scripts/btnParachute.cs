using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnParachute : MonoBehaviour
{
    [SerializeField] PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(callOpenParachute);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void callOpenParachute()
    {
        pc.isOpenParachute = true;
        GetComponent<Button>().interactable = false;
    }
}
