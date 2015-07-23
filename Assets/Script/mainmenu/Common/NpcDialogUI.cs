using UnityEngine;
using System.Collections;

public class NpcDialogUI : MonoBehaviour {
    public static NpcDialogUI _instance;
    private UIButton acceptBtn;
    private UILabel talkLabel;
    private TweenScale tween;

    void Awake()
    {
        _instance = this;
        acceptBtn = transform.Find("Accept-Button").GetComponent<UIButton>();
        talkLabel = transform.Find("Talk-Label").GetComponent<UILabel>();
        tween = this.GetComponent<TweenScale>();
        EventDelegate ed = new EventDelegate(this, "OnAccept");
        acceptBtn.onClick.Add(ed);
        EventDelegate ed1 = new EventDelegate(this,"OnTweenFinish");
        tween.onFinished.Add(ed1);
        gameObject.SetActive(false);
    }

    public void Show(string text)
    {
        tween.gameObject.SetActive(true);
        tween.PlayForward();
        talkLabel.text = text;
    }
    void OnAccept()
    {
        TaskManager._instance.OnAcceptTask();
        //通知任务状态的通知
        tween.PlayReverse();
    }
    void OnTweenFinish()
    {
        if (transform.localScale.x == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
