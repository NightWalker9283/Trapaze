using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnParachute : MonoBehaviour
{

    [SerializeField] Rigidbody Player;

    Button btn;
    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(callOpenParachute);
        btn.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            if (!btn.interactable && Player.velocity.y < -1f)
            {
                btn.interactable = true;
            }
            if (btn.interactable && Player.velocity.y > -1f)
            {
                btn.interactable = false;
            }
        }
    }

    void callOpenParachute()
    {
        GetComponent<Button>().interactable = false;
        isOpen = true;
    }
}
