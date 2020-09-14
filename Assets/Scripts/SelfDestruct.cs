using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    public float destroyDelay = 5f;
    public AudioClip onDestroyAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyDelay);
        AudioSource audioSource = this.GetComponent<AudioSource>();
        if(audioSource != null && onDestroyAudioClip != null)
        {
            audioSource.PlayOneShot(onDestroyAudioClip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
