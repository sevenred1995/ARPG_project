using UnityEngine;
using System.Collections;

public class TranscriptWindow : MonoBehaviour {

    public UILabel desLabel;
    public UILabel energyLabel;
    public UILabel energyTitleLabel;
    public UIButton btnEnter;
    public UIButton btnClose;

    public TweenScale tween;

    public void Awake()
    {
        desLabel = transform.Find("DesLabel").GetComponent<UILabel>();
        energyLabel = transform.Find("EnergyLabel").GetComponent<UILabel>();
        energyTitleLabel = transform.Find("EnergyTitleLabel").GetComponent<UILabel>();
        btnEnter = transform.Find("BtnEnter").GetComponent<UIButton>();
        btnClose = transform.Find("BtnClose").GetComponent<UIButton>();

        tween = transform.GetComponent<TweenScale>();

        //注册按钮点击事件
        EventDelegate ed1 = new EventDelegate(this,"OnEnter");
        btnEnter.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "OnClose");
        btnClose.onClick.Add(ed2);
    }
    public void ShowWarn()
    {
        //等级不足
        MessageManager._instance.ShowMessage("当前等级不足，无法进入！！");
        //tween.PlayForward();
    }
    public void ShowDialog(BtnTranscriptUI transcript)
    {
        desLabel.text = transcript.des;
        energyLabel.text = 3+"";
        tween.PlayForward();
    }
    public void Hide()
    {
        tween.PlayReverse();
    }
    public void OnEnter()
    {

    }
    public void OnClose()
    {
        Hide();
    }
}
