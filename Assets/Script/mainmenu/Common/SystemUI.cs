using UnityEngine;
using System.Collections;

public class SystemUI : MonoBehaviour {
    public TweenScale tween;
    public bool isAudio=true;
    public GameObject AudioSlider;
    void Awake() {
        tween = transform.GetComponent<TweenScale>();
        BoxCollider[] boxs = transform.GetComponentsInChildren<BoxCollider>(true);
        foreach (var co in boxs)
        {
            UIEventListener listener = UIEventListener.Get(co.gameObject);
            listener.onClick = ButtonClick;
        }
        AudioSlider = transform.Find("AudioSlider").gameObject;
    }

    void ButtonClick(GameObject click) {
        if (click.name.Equals("AudioSprite"))
        {
            if(isAudio)
            {
                click.GetComponent<UISprite>().spriteName = "pic_音效关闭";
                isAudio = false;
                BgAudionManager._instance.OnClose();
                AudioSlider.SetActive(false);
                return;
            }
            else
            {
                click.GetComponent<UISprite>().spriteName = "pic_音效开启";
                isAudio = true;
                BgAudionManager._instance.OnOpen();
                AudioSlider.SetActive(true);
                //return;
            }
        }
        else if (click.name.Equals("ConnectSprite"))
        {
            Application.OpenURL("www.baidu.com");
        }
        else if(click.name.Equals("BtnSure"))
        {
            OnClose();
        }
        else if(click.name.Equals("BtnExit"))
        {
            //加载返回场景
        }
    }
    void OnClose() {
        tween.PlayReverse();
    }
}
