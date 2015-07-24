using UnityEngine;
using System.Collections;

public class BottonBar : MonoBehaviour {
    private UIButton bag;
    private UIButton shop;
    private UIButton skill;
    private UIButton combat;
    private UIButton system;
    private UIButton task;

    public TweenScale TaskUItween;
    public TweenScale skillTween;
    public TweenPosition bagTween;
    void Awake()
    {
        task = transform.Find("task").GetComponent<UIButton>();
        skill = transform.Find("skill").GetComponent<UIButton>();
        bag = transform.Find("bag").GetComponent<UIButton>();

        EventDelegate tasked = new EventDelegate(this, "OnTask");
        task.onClick.Add(tasked);

        EventDelegate skilled = new EventDelegate(this, "OnSkill");
        skill.onClick.Add(skilled);

        EventDelegate baged = new EventDelegate(this, "OnBag");
        bag.onClick.Add(baged);
    }
    void OnTask()
    {
        TaskUItween.gameObject.SetActive(true);
        TaskUItween.PlayForward();
    }

    void OnSkill()
    {
        skillTween.gameObject.SetActive(true);
        skillTween.PlayForward();
    }
    void OnBag()
    {
        bagTween.gameObject.SetActive(true);
        bagTween.PlayReverse();
    }
}
