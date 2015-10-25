using UnityEngine;
using System.Collections;

public class BottonBar : MonoBehaviour {
    private UIButton bag;
    private UIButton shop;
    private UIButton skill;
    private UIButton combat;
    private UIButton system;
    private UIButton task;
    private PlayerMove _playerMove;
    private PlayerMove PlayerMove {
        get {
            if (_playerMove == null)
                _playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
            return _playerMove;
        }
    }
    public TweenScale TaskUItween;
    public TweenScale skillTween;
    public TweenPosition bagTween;
    public TweenPosition shopTween;
    public TweenScale SystemTween;
    void Awake()
    {
        task = transform.Find("task").GetComponent<UIButton>();
        skill = transform.Find("skill").GetComponent<UIButton>();
        bag = transform.Find("bag").GetComponent<UIButton>();
        combat=transform.Find("combat").GetComponent<UIButton>();
        shop = transform.Find("shop").GetComponent<UIButton>();
        system = transform.Find("system").GetComponent<UIButton>();

        EventDelegate tasked = new EventDelegate(this, "OnTask");
        task.onClick.Add(tasked);

        EventDelegate skilled = new EventDelegate(this, "OnSkill");
        skill.onClick.Add(skilled);

        EventDelegate baged = new EventDelegate(this, "OnBag");
        bag.onClick.Add(baged);
        EventDelegate.Set(combat.onClick, () =>
        {
            OnCombat();
        });
        EventDelegate.Set(shop.onClick, () =>
        {
            OnShop();
        });
        EventDelegate.Set(system.onClick, () =>
        {
            OnSystem();
        });
    }

    private void OnSystem() {
        SystemTween.PlayForward();
    }

    void OnCombat() {
        PlayerMove.SetPostion(PlayerMove.transcriptGo.transform.position);
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
    void OnShop() {
        shopTween.gameObject.SetActive(true);
        shopTween.PlayForward();
    }
}
