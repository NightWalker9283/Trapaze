using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//パラシュートオープンボタン。下降中のみ有効。一度押すと消える
public class btnParachute : MonoBehaviour
{

    [SerializeField] Rigidbody Player;
    [SerializeField] GameObject btnComment;

    Button btn;
    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CallOpenParachute);
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

    void CallOpenParachute()
    {
        gameObject.SetActive(false);
        btnComment.SetActive(true);
        isOpen = true;
    }
}
