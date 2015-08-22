using UnityEngine;
using System.Collections;

public class BloodScene : MonoBehaviour {
    public static BloodScene _instance;
    private TweenAlpha tween;
    private UISprite sprite;
    void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenAlpha>();
        sprite = this.GetComponent<UISprite>();
    }

    public void showBloodScene()
    {
        sprite.alpha = 1;
        tween.ResetToBeginning();
        tween.PlayForward();
    }
}
