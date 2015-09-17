using UnityEngine;
using System.Collections;

public class TimeDialogUI : MonoBehaviour {

    public static TimeDialogUI _instance;
    public UILabel timeLabel;
    public UILabel tipLabel;
    public UIButton cacelButton;
    public TweenScale tween;

    private float waitTime=20;
    private float timer;
    private bool isStart=false;

    
    void Awake() {
        _instance = this;
        EventDelegate.Set(cacelButton.onClick, () => { OnCancel(); });
        tween = this.GetComponent<TweenScale>();
    }
    void Update() {
        if(isStart)
        {
            timer += Time.deltaTime;
            int currTime = (int)(waitTime - timer);
            timeLabel.text = currTime.ToString();
            if(timer>waitTime)
            {
                timer = 0;
                isStart = false;
                OnEndTimer();
            }
        }
    }

    private void OnEndTimer() {
        waitTime = 20;
        HideTimer();
        transform.parent.SendMessage("OnEndTimer");
    }
    public void HideTimer() {
        tween.PlayReverse();
    }
    //显示倒计时
    public void ShowTimer() {
        tween.PlayForward();
        waitTime = 20;
        isStart = true;
    }
    //手动点击匹配取消
    void OnCancel() {
        timer = 0;
        HideTimer();
        transform.parent.SendMessage("OnEndTimer");
    }



}
