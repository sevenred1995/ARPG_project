using UnityEngine;
using System.Collections;

public class PlayerBarInTranscript : MonoBehaviour {
    public static PlayerBarInTranscript _instance; 
    public UISlider hpslider;
    public UILabel hpLabel;
    public UISprite headSprite;
    void Awake() {
        _instance = this;
        hpLabel.text = playerInfo._instance.HP + "/" + playerInfo._instance.HP;
        hpslider.value = playerInfo._instance.HP / playerInfo._instance.HP;
        headSprite.spriteName = playerInfo._instance.HeadPortrait;
    }
    public void Show(int hp) {
        hpLabel.text = hp + "/" + playerInfo._instance.HP;
        hpslider.value = (float)hp / playerInfo._instance.HP;
    }

}
