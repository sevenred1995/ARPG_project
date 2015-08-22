using UnityEngine;
using System.Collections;

public class HpBarManager : MonoBehaviour {
    public static HpBarManager _instance;
    private GameObject hpBarPrefab;
    private GameObject damageHudTextPrefab;

    void Awake()
    {
        _instance = this;
        hpBarPrefab = Resources.Load<GameObject>("HpBar");
        damageHudTextPrefab = Resources.Load<GameObject>("DamageHudText");
    }
    public GameObject GetHpBar(GameObject target)
    {
        GameObject go = NGUITools.AddChild(this.gameObject, hpBarPrefab);
        go.GetComponent<UIFollowTarget>().target = target.transform;
        return go;
    }
    public GameObject GetHudText(GameObject target)
    {
        GameObject go = NGUITools.AddChild(this.gameObject, damageHudTextPrefab);
        go.GetComponent<UIFollowTarget>().target = target.transform;
        return go;
    }

}
