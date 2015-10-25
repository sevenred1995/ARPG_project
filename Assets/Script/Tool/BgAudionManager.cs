using UnityEngine;
using System.Collections;

public class BgAudionManager : MonoBehaviour {
    public static BgAudionManager _instance;
    public AudioSource audio;
    public static BgAudionManager Instance() {
        if(_instance==null)
        {
            _instance = GameObject.Find("SoundManager").GetComponent<BgAudionManager>();
        }
        return _instance;
    }
    void Awake() {
        Instance();
        audio = this.GetComponent<AudioSource>();
    }
    public void OnOpen() {
        audio.volume = 0;
    }
    public void OnClose() {
        audio.volume = 0.5f;
    }
    public void SetAudio(UISlider slider) {
        audio.volume = slider.value;
    }

}
