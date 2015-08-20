using UnityEngine;
using System.Collections;

public class TranscriptMap : MonoBehaviour {
    public static TranscriptMap _instance;
    public TweenPosition tween;

    private TranscriptWindow window;
    void Awake()
    {
        _instance = this;
        tween = this.gameObject.GetComponent<TweenPosition>();
        window = transform.Find("TranscriptWindow").GetComponent<TranscriptWindow>();
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
