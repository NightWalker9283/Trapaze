using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//ボイス再生機能の実装
public class VoiceManager : MonoBehaviour
{

    public static VoiceManager voiceManager;
    Queue<AudioFile> queue = new Queue<AudioFile>(); //再生予約キュー
    AudioSource audioSource;

    // Start is called before the first frame update
    private void Awake()
    {

        voiceManager = this;
        audioSource = GetComponent<AudioSource>();



    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (queue.Count > 0 && !audioSource.isPlaying)
        {
            var af = queue.Dequeue();

            audioSource.PlayOneShot(af.audioClip);
            PlayingManager.playingManager.titleMonitor.VoiceTrigger(af);
            if (!PlayingManager.playingManager.isTraining &&
                GameMaster.gameMaster.acquiredVoices.Find(str => str == af.path) == null)
            {
                GameMaster.gameMaster.acquiredVoices.Add(af.path);
                GameMaster.gameMaster.Save();
            }

        }


    }
    //ボイスをキューに登録(再生予約)
    public void AddVoice(AudioFile af)
    {
        queue.Enqueue(af);
    }
    //ボイスをキューに登録(再生予約)。コールバック付き
    public void AddVoice(AudioFile af, Action callback)
    {
        queue.Enqueue(af);
        StartCoroutine(MonitorIsPlaying());


        IEnumerator MonitorIsPlaying()
        {
            yield return null;
            while (audioSource.isPlaying)
            {

                yield return null;
            }
            callback();

        }

    }

    //ボイス強制停止
    public void Reset()
    {
        audioSource.Stop();
        queue.Clear();
    }
    public int GetCount()
    {
        return queue.Count;
    }
    //指定したフォルダ内のボイスを全てロードしAudioFile化
    public static List<AudioFile> LoadAllAudioFile(string directlyPath)
    {
        var lst = new List<AudioFile>();
        var voices = Resources.LoadAll<AudioClip>(directlyPath);

        foreach (AudioClip ac in voices)
        {
            var af = new AudioFile();
            af.path = directlyPath + "/" + ac.name;
            af.audioClip = ac;
            lst.Add(af);
        }
        return lst;
    }
    //指定したボイスをロードしAudioFile化
    public static AudioFile LoadAudioFile(string filePath)
    {
        var voice = Resources.Load<AudioClip>(filePath);
        var af = new AudioFile();
        af.path = filePath;
        af.audioClip = voice;
        return af;
    }


}

//AudioClipのパスを保持するためのクラス
public class AudioFile
{
    public string path;
    public AudioClip audioClip;

    public AudioFile() { }
    public AudioFile(string path, AudioClip audioClip)
    {
        this.path = path;
        this.audioClip = audioClip;
    }
}
