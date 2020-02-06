using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//アーカイブ画面。取得ボイス一覧表示部
public class WndVoices : MonoBehaviour
{
    [SerializeField] ToggleGroup tggVoices;
    AudioSource audioSource;
    List<string> acquiredVoices;
    // Start is called before the first frame update
    void Start()
    {
        acquiredVoices = GameMaster.gameMaster.acquiredVoices;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayVoice()
    {
        audioSource.Stop();
        //Resources.UnloadUnusedAssets();
        var path = tggVoices.ActiveToggles().FirstOrDefault().GetComponent<ListElementVoice>().path;

        var ac=Resources.Load<AudioClip>(path);
        audioSource.PlayOneShot(ac);
        
    }
}
