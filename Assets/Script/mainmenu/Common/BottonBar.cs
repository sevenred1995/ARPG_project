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
    void Awake()
    {
        task = transform.Find("task").GetComponent<UIButton>();
        EventDelegate tasked = new EventDelegate(this, "OnTask");
        task.onClick.Add(tasked);
    }
    void OnTask()
    {
        TaskUItween.gameObject.SetActive(true);
        TaskUItween.PlayForward();
    }

}
