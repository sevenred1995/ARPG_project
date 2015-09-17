  using UnityEngine;
using System.Collections;

public class EquipPopup : MonoBehaviour {
    
    private UISprite icon;
    private UILabel nameLabel;
    private UILabel qualityLabel;
    private UILabel damageLabel;
    private UILabel lifeLabel;
    private UILabel powerLabel;
    private UILabel desLabel;
    private UILabel levelLabel;
    private UILabel buttonnameLbel;

    private UIButton equipBtn;
    private UIButton upgradeBtn;
    private UIButton closeBtn;
    
    private InventoryItemUI itUI;
    private bool isLeft;
    private InventoryItem it;

    public PowerShow powerShow;

    private InventoryItemDBController itemController;
    void Awake()
    {
        icon = transform.Find("IconBg/IconSprite").GetComponent<UISprite>();
        nameLabel = transform.Find("NameLabel").GetComponent<UILabel>();
        qualityLabel=transform.Find("QualityLabel/Label").GetComponent<UILabel>();
        damageLabel = transform.Find("DamageLabel/Label").GetComponent<UILabel>();
        lifeLabel = transform.Find("LifeLabel/Label").GetComponent<UILabel>();
        powerLabel = transform.Find("PowerLabel/Label").GetComponent<UILabel>();
        desLabel = transform.Find("DesLabel").GetComponent<UILabel>();
        levelLabel = transform.Find("LevelLabel/Label").GetComponent<UILabel>();
        buttonnameLbel = transform.Find("EquipButton/Label").GetComponent<UILabel>();

        equipBtn = transform.Find("EquipButton").GetComponent<UIButton>();
        upgradeBtn = transform.Find("EquipUpgradeButton").GetComponent<UIButton>();
        closeBtn = transform.Find("CloseButton").GetComponent<UIButton>();
    
        EventDelegate ed = new EventDelegate(this, "On_EquipPopup_Close_Click");
        closeBtn.onClick.Add(ed);

        EventDelegate ed1 = new EventDelegate(this, "On_EquipPopup_Dress_Click");
        equipBtn.onClick.Add(ed1);

        EventDelegate ed2 = new EventDelegate(this, "On_EquipPopup_Upgrade_Click");
        upgradeBtn.onClick.Add(ed2);
        itemController = GameObject.Find("GameManager").GetComponent<InventoryItemDBController>();
        itemController.OnUpdateInventoryItemDB += this.OnUpdateInventoryItemDB;
    
    }
    public void Show(InventoryItem it,InventoryItemUI itUI,bool isleft=true)
    {
        this.it = it;
        this.itUI = itUI;
        gameObject.SetActive(true);
        Vector3 pos = transform.localPosition;
        this.isLeft = isleft;
        if(isleft)
        {
            transform.localPosition = new Vector3(-Mathf.Abs(pos.x),pos.y,pos.z);
            buttonnameLbel.text = "装备";
            upgradeBtn.isEnabled = false;
        }
        else
        {
            transform.localPosition = new Vector3(Mathf.Abs(pos.x), pos.y, pos.z);
            buttonnameLbel.text = "卸下";
            upgradeBtn.isEnabled = true;
        }
        icon.spriteName = it.Inventory.Icon;
        nameLabel.text = it.Inventory.Name;
        qualityLabel.text = it.Inventory.Quality.ToString();
        lifeLabel.text = it.Inventory.HP.ToString();
        damageLabel.text = it.Inventory.Damage.ToString();
        powerLabel.text = it.Inventory.Power.ToString();
        desLabel.text = it.Inventory.Des.ToString();
        levelLabel.text = it.Level.ToString();
    }
    void On_EquipPopup_Close_Click()
    {
        this.gameObject.SetActive(false);
        transform.parent.parent.SendMessage("DisenableButton");
    }
    void On_EquipPopup_Dress_Click()
    {
       int startValue = playerInfo._instance.GetAllPower();
       if(isLeft)
       {
           playerInfo._instance.DressOn(it);
           itUI.Clear();
       }
       else
       {
           playerInfo._instance.DressUp(it);
       }
       ClearObject();
       gameObject.SetActive(false);
       int endValue = playerInfo._instance.GetAllPower();
       powerShow.ShowNumChange(startValue, endValue);
       //向背包发送消息
       InventoryUI._instance.SendMessage("UpdateCount");
       transform.parent.parent.SendMessage("DisenableButton");
    }
    void On_EquipPopup_Upgrade_Click()
    {
        int coinNeed = (it.Level + 1) * it.Inventory.Price;
        bool isSuccess = playerInfo._instance.GetCoin(coinNeed);
        if(isSuccess)
        {
            it.Level += 1;
            //等级改变，其他战斗力属性值也会随之改变。。。
            //更改装备等级属性
            itemController.UpdateInventoryItemDB(it.InventoryItemDB);
            //TODO  
        }
        else
        {
            MessageManager._instance.ShowMessage("金币不足，无法升级");
        } 
    }
    void OnUpdateInventoryItemDB() {
        levelLabel.text = it.Level.ToString();
    }
    void ClearObject()
    {
        it=null;
        itUI = null;
    }
}
