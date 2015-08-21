using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
    public static SoundManager _instance;
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
    public AudioClip[] audioClipArray;
    public AudioSource audioSource;
    public bool isQuiet = false;

    void Awake()
    {
        _instance = this;
        audioSource = transform.GetComponent<AudioSource>();
    }
    void Start()
    {
        foreach(AudioClip ac in audioClipArray)
        {
            audioDic.Add(ac.name, ac);
        }
    }

    public void Play(string audioName)
    {
        if (isQuiet) return;
        AudioClip ac;
        if(audioDic.TryGetValue(audioName,out ac))
        {
            this.audioSource.PlayOneShot(ac);
        }
    }
    public void Play(string audioName,AudioSource audioSource)
    {
        if (isQuiet) return;
        AudioClip ac;
        if (audioDic.TryGetValue(audioName, out ac))
        {
            audioSource.PlayOneShot(ac);
        }
    }
}
