using UnityEngine;
using System.Collections;

public class PlayerAttackEffect : MonoBehaviour {
    private Renderer[] effectRenderArray;
    private NcCurveAnimation[] animArray;
    private GameObject effectOffset;
    void Awake()
    {
        effectRenderArray = this.GetComponentsInChildren<Renderer>();
        animArray = this.GetComponentsInChildren<NcCurveAnimation>();
        if (transform.Find("EffectOffset"))
        effectOffset = transform.Find("EffectOffset").gameObject;
    }
    
    public void ShowEffect()
    {
        if(effectOffset!=null)
        {
            effectOffset.SetActive(false);
            effectOffset.SetActive(true);
        }
        else
        {
            foreach (Renderer render in effectRenderArray)
            {
                render.enabled = true;
            }
            foreach (NcCurveAnimation anim in animArray)
            {
                anim.ResetAnimation();
            }
        }
    }
}
