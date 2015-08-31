using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour {
    
    public static MessageManager _instance;
    private UILabel messageLabel;
    private TweenAlpha messagePanel;
    private TweenPosition msgTween;

    private bool isActive;
    void Awake()
    {
        _instance = this;
        messageLabel = transform.Find("MsgBg/Label").GetComponent<UILabel>();
        messagePanel = this.GetComponent<TweenAlpha>();
        msgTween = this.GetComponent<TweenPosition>();
        EventDelegate ed = new EventDelegate(this,"OnTweenFinished");
        messagePanel.onFinished.Add(ed);
        gameObject.SetActive(false);
    }
    public void ShowMessage(string msg,float time=0.7f)
    {
        gameObject.SetActive(true);
        StartCoroutine(Show(msg, time));
    }
    IEnumerator Show(string msg,float time)
    {
        isActive = true;
        messagePanel.PlayForward();
        msgTween.PlayForward();
        messageLabel.text = msg;
        yield return new WaitForSeconds(time);
        isActive = false;
        messagePanel.PlayReverse();
        msgTween.PlayReverse();
    }
    public void OnTweenFinished()
    {
        if(isActive==false)
        {
            gameObject.SetActive(false);
        }
    }

}
