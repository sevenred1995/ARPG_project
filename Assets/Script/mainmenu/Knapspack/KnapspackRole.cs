using UnityEngine;
using System.Collections;

public class KnapspackRole : MonoBehaviour
{
    #region EquipSprite
    public KnapspackRoleEquip helmEquip;
    public KnapspackRoleEquip clothEquip;
    public KnapspackRoleEquip weaponEquip;
    public KnapspackRoleEquip shoesEquip;
    public KnapspackRoleEquip necklaceEquip;
    public KnapspackRoleEquip braceletEquip;
    public KnapspackRoleEquip ringEquip;
    public KnapspackRoleEquip wingEquip;
    #endregion
    #region Button
    public UIButton detailBtn;
    #endregion
    #region msg
    public UILabel lifeLabel;
    public UILabel damageLabel;
    public UISlider expSlider;
    public UILabel explabel;
    public UILabel labelName;
    #endregion
    void Awake()
    {
        helmEquip = transform.Find("HelmSprite").GetComponent<KnapspackRoleEquip>();
        clothEquip = transform.Find("ClothSprite").GetComponent<KnapspackRoleEquip>();
        weaponEquip = transform.Find("WeaponSprite").GetComponent<KnapspackRoleEquip>();
        shoesEquip = transform.Find("ShoesSprite").GetComponent<KnapspackRoleEquip>();
        necklaceEquip = transform.Find("NecklaceSprite").GetComponent<KnapspackRoleEquip>();
        braceletEquip = transform.Find("BraceletSprite").GetComponent<KnapspackRoleEquip>();
        ringEquip = transform.Find("RingSprite").GetComponent<KnapspackRoleEquip>();
        wingEquip = transform.Find("WingSprite").GetComponent<KnapspackRoleEquip>();

        detailBtn = transform.Find("DetialButton").GetComponent<UIButton>();

        lifeLabel = transform.Find("LifeBg/Label").GetComponent<UILabel>();
        damageLabel = transform.Find("DamageBg/Label").GetComponent<UILabel>();
        expSlider = transform.Find("ExpProgressBar/expslider").GetComponent<UISlider>();
        explabel = transform.Find("ExpProgressBar/expslider/Label").GetComponent<UILabel>();
        labelName = transform.Find("LabelName").GetComponent<UILabel>();

        playerInfo._instance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;
    }
    void Start() {
        //UpdateShow();
    }
    void OnDestory()
    {
        playerInfo._instance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
    }
    void OnPlayerInfoChanged(InfoType type)
    {
        if (type == InfoType.All || type == InfoType.Hp || type == InfoType.Damage || type == InfoType.Exp||type==InfoType.Equip)
        {
            UpdateShow();
        }
    }
    void UpdateShow()
    {
        playerInfo info = playerInfo._instance;
        //装备的更新；
        helmEquip.SetInventoryItem(info.helmItem);
        clothEquip.SetInventoryItem(info.clothItem);
        weaponEquip.SetInventoryItem(info.weaponItem);
        shoesEquip.SetInventoryItem(info.shoesItem);
        necklaceEquip.SetInventoryItem(info.necklaceItem);
        braceletEquip.SetInventoryItem(info.braceletItem);
        ringEquip.SetInventoryItem(info.ringItem);
        wingEquip.SetInventoryItem(info.wingItem);

        lifeLabel.text = info.HP.ToString();
        damageLabel.text = info.Damage.ToString();
        int requireExp = GameManger.GetRequireExpByLevel(info.Level + 1);
        expSlider.value = (float)info.Exp / requireExp;
        explabel.text = info.Exp + "/" + requireExp;
        labelName.text = info.Name;
    }
}
