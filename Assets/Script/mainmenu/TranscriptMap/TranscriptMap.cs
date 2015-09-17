using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TranscriptMap : MonoBehaviour {
    public static TranscriptMap _instance;
    public TweenPosition tween;
    public Dictionary<int, BtnTranscriptUI> transcriptDict = new Dictionary<int, BtnTranscriptUI>();

    private TranscriptWindow window;
    void Awake()
    {
        _instance = this;
        tween = this.gameObject.GetComponent<TweenPosition>();
        window = transform.Find("TranscriptWindow").GetComponent<TranscriptWindow>();
        BtnTranscriptUI[] transcripts = this.GetComponentsInChildren<BtnTranscriptUI>();
        foreach(var temp in transcripts)
        {
            transcriptDict.Add(temp.transcriptID, temp);
        }
    }

    public void OnBack() {
        Hide();
    }
    public void Show()
    {
        tween.PlayForward();
    }
    public void Hide()
    {
        tween.PlayReverse();
    }
    IEnumerator HidePanel(GameObject go)
    {
        yield return new WaitForSeconds(0.4f);
        go.SetActive(false);
    }
    public void OnBtnTranscriptClick(BtnTranscriptUI transcript)
    {
        if(playerInfo._instance.Level>=transcript.needLevel)
        {
            window.ShowDialog(transcript);
        }
        else
        {
            window.ShowWarn();
        }
      
    }
}
