using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum InfoType
{
    Name,
    HeadPortrait,
    Level,
    Power,
    Exp,
    Diamond,
    Coin,
    Energy,
    Toughen,
    Hp,
    Damage,
    Equip,
    All
}
public enum PlayerType
{
    Warrior,
    FemaleAssassin
}
public class playerInfo : MonoBehaviour {
    public static playerInfo _instance;
    #region property
    private string _name;
    private string _headPortrait;
    private int _level=1;
    private int _power=1;
    private int _exp = 0;
    private int _diamond=0;
    private int _coin = 0;
    private int _energy = 100;
    private int _toughen = 50;
    private int _hp;
    private int _damage;
    private PlayerType playerType;
//     private int _helmID=0;
//     private int _clothID=0;
//     private int _weaponID=0;
//     private int _shoesID=0;
//     private int _necklaceID=0;
//     private int _braceletID=0;
//     private int _ringID=0;
//     private int _wingID=0;
    #endregion
    #region setget
  public string Name
    {
        get { return _name;}
        set { _name = value; }
    }
    public string HeadPortrait
    {
        get  { return _headPortrait;}
        set  {_headPortrait = value;}
    }
    public int Level
    {
        get{ return _level; }
        set{ _level = value;}
    }
    public int Power
    {
        get
        { return _power; }
        set
        { _power = value;}
    }
    public int Exp
    {
        get { return _exp;}
        set{_exp=value;}
    }
    public int Diamond
    {
        get { return _diamond; }
        set { _diamond = value; }
    }
    public int Coin
    {
        get { return _coin; }
        set { _coin = value; }
    }
    public int Energy
    {
        get { return _energy; }
        set { _energy = value; }
    }
    public int Toughen
    {
        get { return _toughen; }
        set { _toughen = value; }
    }
    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    public PlayerType PlayerType
    {
        get { return playerType; }
        set { playerType = value; }
    }
    #region Test
    //     public int HelmID
//     {
//         get { return _helmID; }
//         set { _helmID = value; }
//     }
//     public int ClothID
//     {
//         get { return _clothID; }
//         set { _clothID = value; }
//     }
//     public int WeaponID
//     {
//         get { return _weaponID; }
//         set { _weaponID = value; }
//     }
//     public int ShoesID
//     {
//         get { return _shoesID; }
//         set { _shoesID = value; }
//     }
//     public int NecklaceID
//     {
//         get { return _necklaceID; }
//         set { _necklaceID = value; }
//     }
//     public int BraceletID
//     {
//         get { return _braceletID; }
//         set { _braceletID = value; }
//     }
//     public int RingID
//     {
//         get { return _ringID; }
//         set { _ringID = value; }
//     }
//     public int WingID
//     {
//         get { return _wingID; }
//         set { _wingID = value; }
    //     }
    #endregion

    #endregion
    public float energyTimer = 0;
    public float toughenTimer = 0;


    public InventoryItem helmItem;
    public InventoryItem clothItem;
    public InventoryItem weaponItem;
    public InventoryItem shoesItem;
    public InventoryItem necklaceItem;
    public InventoryItem braceletItem;
    public InventoryItem ringItem;
    public InventoryItem wingItem;


    private InventoryItem inventoryItemWillDressed;
    //定义委托事件
    public delegate void OnPlayerInfoChangedEvent(InfoType type);
    public event OnPlayerInfoChangedEvent OnPlayerInfoChanged;

    private RoleController roleController;
    private InventoryItemDBController itemDBController;
    void Awake()
    {
        _instance = this;
        roleController=this.GetComponent<RoleController>();
        itemDBController = this.GetComponent<InventoryItemDBController>();
    }
    void Start()
    {
        OnPlayerInfoChanged += this.OnPlayerInfoChange;
        itemDBController.OnUpdateInventoryItemDB += this.OnUpdateInventoryItemDB;
        itemDBController.OnUpdateInventoryItemDBList += this.OnUpdateInventoryItemDBList;
        Init();
    }

    void Update()
    {
        //需要实现体力值与历练值得自动增加
        if(this.Energy<100)
        {
            energyTimer += Time.deltaTime;
            if(energyTimer>60.0f)
            {
                this.Energy += 1;
                PhotonEngine.Instance.role.Energy += 1;
                energyTimer -= 60f;
                OnPlayerInfoChanged(InfoType.Energy);
            }
        }
        else
        {
            energyTimer = 0;
        }
        if(this.toughenTimer<50)
        {
            toughenTimer += Time.deltaTime;
            if(toughenTimer>60f)
            {
                this.Toughen += 1;
                PhotonEngine.Instance.role.Toughen += 1;
                toughenTimer -= 60f;
                OnPlayerInfoChanged(InfoType.Toughen);
            }
        }
        else
        {
            toughenTimer = 0;
        }
    }
    public void Init()
    {
        //Debug.Log(PhotonEngine.Instance.role.Coin+"asdddddddddddddddddddddddddd");
        this.Coin =PhotonEngine.Instance.role.Coin;
        this.Diamond = PhotonEngine.Instance.role.Diamond;
        this.Energy = PhotonEngine.Instance.role.Energy;
        this.Exp = PhotonEngine.Instance.role.Exp;
        this.Level = PhotonEngine.Instance.role.Level;
        this.Energy = PhotonEngine.Instance.role.Energy;
        this.Toughen = PhotonEngine.Instance.role.Toughen;
        this.HeadPortrait =PhotonEngine.Instance.role.Isman?"头像底板男性":"头像底板女性";
        this.Name = PhotonEngine.Instance.role.Name;
        this.playerType = PlayerType.Warrior;
        InitHpDamagePower();
        //更新装备的装备信息
        OnPlayerInfoChanged(InfoType.All);//显示信息
    }
    public void OnPlayerInfoChange(InfoType infotype)
    {
        if (infotype == InfoType.Name || infotype == InfoType.Energy || infotype == InfoType.Toughen||infotype==InfoType.Coin){
            roleController.UpdateRole(PhotonEngine.Instance.role);  
        }
    }
    void InitHpDamagePower()
    {
        this.HP = this.Level * 1000;
        this.Damage = this.Level * 50;
        this.Power = this.HP + this.Damage;
    }
    //穿上装备时候的数值计算
    void PutOnEquip(int id)//穿戴装备
    {
        if (id == 0) return;
        Inventory inventory = null;
        InventoryManager._instance.inventoryDic.TryGetValue(id, out inventory);
        this.HP += inventory.HP;
        this.Damage+=inventory.Damage;
        this.Power += inventory.Power;
    }
    //脱下装备时候的数值计算
    void GetOffEquip(int id)
    {
        if (id == 0) return;
        if (id == 0) return;
        Inventory inventory = null;
        InventoryManager._instance.inventoryDic.TryGetValue(id, out inventory);
        this.HP -= inventory.HP;
        this.Damage -= inventory.Damage;
        this.Power -= inventory.Power;
    }
    public int GetAllPower()
    {
        float power = this.Power;
        if(helmItem!=null)
        {
            power += helmItem.Inventory.Power * (1 + (helmItem.Level - 1) / 10f);
        }
        if (clothItem != null)
        {
            power += clothItem.Inventory.Power * (1 + (clothItem.Level - 1) / 10f);
        }
        if (weaponItem != null)
        {
            power += weaponItem.Inventory.Power * (1 + (weaponItem.Level - 1) / 10f);
        }
        if (shoesItem != null)
        {
            power += shoesItem.Inventory.Power * (1 + (shoesItem.Level - 1) / 10f);
        }
        if(necklaceItem!=null)
        {
            power += necklaceItem.Inventory.Power * (1 + (necklaceItem.Level - 1) / 10f);
        }
        if(braceletItem!=null)
        {
            power += braceletItem.Inventory.Power * (1 + (braceletItem.Level - 1) / 10f);
        }
       if(ringItem!=null)
        {
            power += ringItem.Inventory.Power * (1 + (ringItem.Level - 1) / 10f);
        }
        if(wingItem!=null)
        {
            power += wingItem.Inventory.Power * (1 + (wingItem.Level - 1) / 10f);
        }
        return (int)power;
    }
    public void ChangeName(string newName)
    {
        OnPlayerInfoChanged(InfoType.Name);
        PhotonEngine.Instance.role.Name = newName;
        this.Name = newName;
    }
    public void DressOn(InventoryItem it)
    {
        it.IsDressed = true;
        //检测是否穿有相同的装备
        bool isDress = false;//当前格子是否穿上
        InventoryItem inventoryItemDressed=null;
        switch(it.Inventory.EquipTYPE)
        {
            case EquipType.Helm:
                if(helmItem!=null)
                {
                    isDress = true;
                    inventoryItemDressed = helmItem;
                }
                helmItem = it;
                break;
            case EquipType.Cloth:
                if(clothItem!=null)
                {
                    isDress = true;
                    inventoryItemDressed = clothItem;
                }
                clothItem = it;
                break;
            case EquipType.Weapon:
                if (weaponItem != null)
                {
                    isDress = true;
                    inventoryItemDressed = weaponItem;
                }
                weaponItem = it;
                break;
            case EquipType.Shoes:
                if(shoesItem!=null)
                {
                    isDress = true;
                    inventoryItemDressed = shoesItem;
                }
                shoesItem = it;
                break;
            case EquipType.Necklace:
                if(necklaceItem!=null)
                {
                    isDress = true;
                    inventoryItemDressed = necklaceItem;
                }
                necklaceItem = it;
                break;
            case EquipType.Bracelet:
                if(braceletItem!=null)
                {
                    isDress = true;
                    inventoryItemDressed = braceletItem;
                }
                braceletItem = it;
                break;
            case EquipType.Ring:
                if(ringItem!=null)
                {
                    isDress = true;
                    inventoryItemDressed = ringItem;
                }
                ringItem = it;
                break;
            case EquipType.Wing:
                if(wingItem!=null)
                {
                    isDress = true;
                    inventoryItemDressed = wingItem;
                }
                wingItem = it;
                break;
            default:
                break;
        }
        //有装备穿上
        if(isDress)
        {
            inventoryItemDressed.IsDressed = false;
            inventoryItemWillDressed = inventoryItemDressed;
            itemDBController.UpdateInventoryItemDBList(it.InventoryItemDB, inventoryItemDressed.InventoryItemDB);
        }
        else
        {
            itemDBController.UpdateInventoryItemDB(it.InventoryItemDB);
        }
    }
    public void DressUp(InventoryItem it)
    {
        it.IsDressed = false;
        InventoryUI._instance.AddInventoryItem(it);
        switch (it.Inventory.EquipTYPE)
        {
            case EquipType.Helm:
                helmItem = null;
                break;
            case EquipType.Cloth:
                clothItem = null;
                break;
            case EquipType.Weapon:
                weaponItem = null;
                break;
            case EquipType.Shoes:
                shoesItem = null;
                break;
            case EquipType.Necklace:
                necklaceItem = null;
                break;
            case EquipType.Bracelet:
                braceletItem = null;
                break;
            case EquipType.Ring:
                ringItem = null;
                break;
            case EquipType.Wing:
                wingItem = null;
                break;
            default:

                break;
        }
        itemDBController.UpdateInventoryItemDB(it.InventoryItemDB);
        //OnPlayerInfoChanged(InfoType.Equip);
    }
    public void OnUpdateInventoryItemDB() {
        Debug.Log("装备更新成功");
        OnPlayerInfoChanged(InfoType.Equip);
    }
    public void OnUpdateInventoryItemDBList() {
        Debug.Log("装备列表更新成功");
        InventoryUI._instance.AddInventoryItem(inventoryItemWillDressed);//更新成功后将物品放回背包
        OnPlayerInfoChanged(InfoType.Equip);
    }

    public bool GetCoin(int count)
    {
       if(Coin>count)
       {
           Coin -= count;
           PhotonEngine.Instance.role.Coin -= count;
           OnPlayerInfoChanged(InfoType.Coin);
           return true;
       }
       return false;
    }
    public void AddCoin(int count)
    {
        Coin += count;
        PhotonEngine.Instance.role.Coin+=count;
        OnPlayerInfoChanged(InfoType.Coin);
    }
    public void InventoryUse(InventoryItem it,int count)
    {

        //使用效果
        if(it.Inventory.InventoryTYPE==InventoryType.Drug)
        {
            //TODO
        }
        else if(it.Inventory.InventoryTYPE==InventoryType.Box)
        {
            //TODO
        }
        //物品减少
        it.Count -= count;
        if (it.Count <= 0)
        {
            InventoryManager._instance.inventoryItemList.Remove(it);
        }
        else
        {
            
        }
    }


    public void InitEquip() {
        foreach (InventoryItem it in InventoryManager._instance.inventoryItemList)
        {
            // Debug.Log(it.IsDressed + "=======" + it.Inventory.EquipTYPE);
            if (it.IsDressed)
            {
                switch (it.Inventory.EquipTYPE)
                {
                    case EquipType.Helm:
                        helmItem = it;
                        break;
                    case EquipType.Bracelet:
                        braceletItem = it;
                        break;
                    case EquipType.Cloth:
                        clothItem = it;
                        break;
                    case EquipType.Necklace:
                        necklaceItem = it;
                        break;
                    case EquipType.Ring:
                        ringItem = it;
                        break;
                    case EquipType.Shoes:
                        shoesItem = it;
                        break;
                    case EquipType.Weapon:
                        weaponItem = it;
                        break;
                    case EquipType.Wing:
                        wingItem = it;
                        break;
                }
            }
            OnPlayerInfoChanged(InfoType.All);//显示信息
        }
    }
}
