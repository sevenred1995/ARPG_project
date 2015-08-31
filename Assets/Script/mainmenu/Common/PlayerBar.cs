using UnityEngine;
using System.Collections;

public class PlayerBar : MonoBehaviour {
   
    public UISprite HeadSprite;
    public UILabel Label_name;
    public UILabel Label_level;
    public UISlider Energyslider;
    public UILabel LabelEnergy;
    public UISlider Toughenslider;
    public UILabel LabelToughen;
    public UIButton EnergyPlusBtn;
    public UIButton ToughenPlusBtn;
    public UIButton HeadBtn;
    void Awake()
    {
        HeadSprite = GameObject.Find("PlayerBar/HeadSprite").GetComponent<UISprite>();
        Label_name = GameObject.Find("PlayerBar/Label-name").GetComponent<UILabel>();
        Label_level = GameObject.Find("PlayerBar/Label-level").GetComponent<UILabel>();
        Energyslider = GameObject.Find("PlayerBar/EnergyProgressBar").GetComponent<UISlider>();
        Toughenslider = GameObject.Find("PlayerBar/ToughenProgressBar").GetComponent<UISlider>();
        LabelToughen = GameObject.Find("PlayerBar/ToughenProgressBar/Label").GetComponent<UILabel>();
        LabelEnergy = GameObject.Find("PlayerBar/EnergyProgressBar/Label").GetComponent<UILabel>();
        EnergyPlusBtn = GameObject.Find("PlayerBar/EnergyPlusBtn").GetComponent<UIButton>();
        ToughenPlusBtn = GameObject.Find("PlayerBar/ToughenPlusBtn").GetComponent<UIButton>();
        HeadBtn = transform.Find("HeadButton").GetComponent<UIButton>();
        EventDelegate ed = new EventDelegate(this, "On_HeadButton_Click");
        HeadBtn.onClick.Add(ed);
        //注册事件
        playerInfo._instance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;
    }
    void Start()
    {
        
    }

    void OnDestory()
    {
        playerInfo._instance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
    }
    void OnPlayerInfoChanged(InfoType type)
    {
        if(type==InfoType.All||type==InfoType.Name||type==InfoType.Level||type==InfoType.HeadPortrait||type==InfoType.Energy||type==InfoType.Toughen)
        {
            UpdateShow();
        }
    }
    void UpdateShow()
    {
        playerInfo info = playerInfo._instance;
        HeadSprite.spriteName = info.HeadPortrait;
        Label_level.text = info.Level.ToString();
        Label_name.text = info.Name;
        Energyslider.value = info.Energy / 100f;
        LabelEnergy.text = info.Energy + "/100";
        Toughenslider.value = info.Toughen / 50f;
        LabelToughen.text = info.Toughen + "/50";
    }
    public void On_HeadButton_Click()
    {
        PlayerStatus._instance.Show();
    }
}
