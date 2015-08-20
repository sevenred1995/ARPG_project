using UnityEngine;
using System.Collections;

public class BtnTranscriptUI : MonoBehaviour {

    public static BtnTranscriptUI _instance;
    public int transcriptID;
    public int needLevel;
    public string transcriptName;
    public string des = "hasgdjdasbxahsdinaskchadfgjacakshdjkahs";
    
    public void Awake()
    {
        _instance = this;
    }
    public void OnPress(bool isPress)
    {
        if(isPress)
        {
            transform.parent.SendMessage("OnBtnTranscriptClick",this);
        }
    }
}
