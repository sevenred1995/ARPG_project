using UnityEngine;
using System.Collections;

public class BossAttackEffect : MonoBehaviour {
    public static BossAttackEffect _insatnce;
    private GameObject effect;
    void Awake()
    {
        _insatnce = this;
        effect = transform.Find("Effect").gameObject;
    }
    public void Show()
    {
       effect.SetActive(false);
       effect.SetActive(true);
    }

}
