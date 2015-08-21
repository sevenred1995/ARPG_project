using UnityEngine;
using System.Collections;

public class HpBarManager : MonoBehaviour {
    public static HpBarManager _instance;
    private GameObject hpBarPrefab;

    void Awake()
    {
        _instance = this;
        hpBarPrefab = Resources.Load<GameObject>("HpBar");
    }
    public GameObject GetHpBar(GameObject target)
    {
        GameObject go = NGUITools.AddChild(this.gameObject, hpBarPrefab);
        go.GetComponent<UIFollowTarget>().target = target.transform;
        return go;
    }


}
