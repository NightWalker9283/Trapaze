using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSound : MonoBehaviour
{
    public bool isCrackSound = true;

    Rigidbody rb;
    Vector3 old_velocity;
    bool sounded=false;
    float old_acceleration=0f;
    AudioSource audioSource;
    [SerializeField] AudioClip[] ac;
    [SerializeField] AudioMixerGroup amgSE;
    [SerializeField] float threshhold = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        old_velocity = rb.velocity;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = amgSE;
        StartCoroutine(CrackSound());
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CrackSound()
    {
        while (isCrackSound)
        {
            float acceleration = rb.velocity.y - old_velocity.y;
            float diffAcceleration = acceleration - old_acceleration;
            //Debug.Log(acceleration);
            if (!sounded && diffAcceleration > threshhold)
            {

                audioSource.PlayOneShot(ac[Random.Range(0, ac.Length)]);
                sounded = true;
            }
            if (sounded && diffAcceleration <= 0)
            {

                sounded = false;
            }
            old_acceleration = acceleration;
            yield return null;
        }
    }
}
