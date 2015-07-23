using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

    public static PlayerStatus _instance;

    #region Topmessage
    public UISprite headSprite;
    public UILabel labellevel;
    public UILabel labelname;
    public UILabel labelpower;
    public UISlider expslider;
    public UILabel explabel;
    public UIButton changeNameBtn;
    public UIButton closeBtn;
    #endregion

    #region Sourcemessage
    public UILabel labelDiamondnum;
    public UILabel labelCoinnum;
    #endregion

    #region Timemessage
    public UILabel LabelEnergynum;
    public UILabel LabelToughennum;
    public UILabel labelEnergyRecoverTime;
    public UILabel labelEnergyRecoverallTime;
    public UILabel labelToughenRecoverTime;
    public UILabel labelToughenRecoverallTime;
    #endregion
    #region changepanel
    public GameObject ChangePanel;
    public UIInput input_name;
    public UIButton SureBtn;
    public UIButton CancelBtn;
    #endregion



    private TweenPosition tween;
    void Awake()
    {
        _instance = this;
        headSprite = GameObject.Find("Topmessage/HeadSprite1").GetComponent<UISprite>();
        labellevel = GameObject.Find("Topmessage/Label-level1").GetComponent<UILabel>();
        labelname = GameObject.Find("Topmessage/Label-name1").GetComponent<UILabel>();
        labelpower = GameObject.Find("Topmessage/Label-power1").GetComponent<UILabel>();
        expslider = GameObject.Find("Topmessage/ExpProgressBar/Expslider").GetComponent<UISlider>();
        explabel = GameObject.Find("Topmessage/ExpProgressBar/Label").GetComponent<UILabel>();
        changeNameBtn = GameObject.Find("Topmessage/ChangeNameButton").GetComponent<UIButton>();
        closeBtn = GameObject.Find("Topmessage/CloseButton").GetComponent<UIButton>();
        labelDiamondnum = GameObject.Find("Sourcemessage/Label-diamond/Label-dnum").GetComponent<UILabel>();
        labelCoinnum = GameObject.Find("Sourcemessage/Label-coin/Label-cnum").GetComponent<UILabel>();
        LabelEnergynum = GameObject.Find("Energymessage/Label-Energy-num").GetComponent<UILabel>();
        labelEnergyRecoverTime = GameObject.Find("Energymessage/Label-recover-time").GetComponent<UILabel>();
        labelEnergyRecoverallTime = GameObject.Find("Energymessage/Label-recoverall-time").GetComponent<UILabel>();
        LabelToughennum = GameObject.Find("Toughenmessage/Label-Toughen-num").GetComponent<UILabel>();
        labelToughenRecoverTime = GameObject.Find("Toughenmessage/Label-recover-time").GetComponent<UILabel>();
        labelToughenRecoverallTime = GameObject.Find("Toughenmessage/Label-recoverall-time").GetComponent<UILabel>();
        tween = this.gameObject.GetComponent<TweenPosition>();

        ChangePanel = transform.Find("ChangeNamePanel").gameObject;
        input_name = transform.Find("ChangeNamePanel/Input-name").GetComponent<UIInput>();
        SureBtn = transform.Find("ChangeNamePanel/ChangeSureButton").GetComponent<UIButton>();
        CancelBtn = transform.Find("ChangeNamePanel/ChangeCancelButton").GetComponent<UIButton>();
        
        EventDelegate ed = new EventDelegate(this, "On_CloseButton_Click");
        closeBtn.onClick.Add(ed);
        EventDelegate ed1 = new EventDelegate(this, "On_Change_Click");
        changeNameBtn.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "On_ChangeSure_Click");
        SureBtn.onClick.Add(ed2);
        EventDelegate ed3 = new EventDelegate(this, "On_ChangeCancel_Click");
        CancelBtn.onClick.Add(ed3);

        //注册事件
        playerInfo._instance.OnPlayerInfoChanged+=this.OnPlayerInfoChanged;
    
    }
    void OnDestory()
    {
        playerInfo._instance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
    }
    void OnPlayerInfoChanged(InfoType type)
    {
        UpdateShow();
        UpodateEnergyAndToughenShow();
    }
    private void UpdateShow()
    {
        playerInfo info = playerInfo._instance;
        headSprite.spriteName = info.HeadPortrait;
        labellevel.text = info.Level.ToString();
        labelname.text = info.Name;
        labelpower.text = info.Power.ToString();

        int requireExp = GameManger.GetRequireExpByLevel(info.Level+1);
        expslider.value = (float)info.Exp / requireExp;
        explabel.text = info.Exp + "/" + requireExp;

        labelDiamondnum.text = info.Diamond.ToString();
        labelCoinnum.text = info.Coin.ToString();

    }

    //计时器的显示
    void UpodateEnergyAndToughenShow()
    {
        playerInfo info = playerInfo._instance;
        LabelEnergynum.text = info.Energy + "/100";
        LabelToughennum.text = info.Toughen + "/50";
        if(info.Energy>=100)
        {
            labelEnergyRecoverTime.text = "00:00:00";
            labelEnergyRecoverallTime.text = "00:00:00";
        }
        else
        {
            int remainTime = 60 - (int)info.energyTimer;
            string secondStr = remainTime <= 9 ? "0" + remainTime : remainTime.ToString();
            labelEnergyRecoverTime.text  = "00:00:" + secondStr;

            int minutes = 99 - info.Energy;
            int hour = minutes / 60;
            minutes = minutes % 60;

            string hourStr = hour <= 9 ? "0" + hour : hour.ToString();
            string minuteStr = minutes <= 9 ? "0" + minutes : minutes.ToString();
            labelEnergyRecoverallTime.text = hourStr + ":" + minuteStr + ":" + secondStr;
        }


        if (info.Toughen >= 50)
        {
            labelToughenRecoverTime.text = "00:00:00";
            labelToughenRecoverallTime.text = "00:00:00";
        }
        else
        {
            int remainTime = 60 - (int)info.toughenTimer;
            string secondStr = remainTime <= 9 ? "0" + remainTime : remainTime.ToString();
            labelToughenRecoverTime.text = "00:00:" + secondStr;

            int minutes = 49 - info.Toughen;
            int hour = minutes / 60;
            minutes = minutes % 60;

            string hourStr = hour <= 9 ? "0" + hour : hour.ToString();
            string minuteStr = minutes <= 9 ? "0" + minutes : minutes.ToString();
            labelToughenRecoverallTime.text = hourStr + ":" + minuteStr + ":" + secondStr;
        }

    }
    public void Show()
    {
        tween.PlayForward();
    }
    public void On_CloseButton_Click()
    {
        tween.PlayReverse();
    }
    public void On_Change_Click()
    {
        ChangePanel.SetActive(true);
    }
    public void On_ChangeSure_Click()
    {
        string newName = input_name.value;
        //1：连接服务器，执行判断
        //2：修改昵称
        playerInfo._instance.ChangeName(newName);
        On_ChangeCancel_Click();
    }
    public void On_ChangeCancel_Click()
    {
        ChangePanel.SetActive(false);
    }

}
