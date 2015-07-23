using UnityEngine;
using System.Collections;
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

    //定义委托事件
    public delegate void OnPlayerInfoChangedEvent(InfoType type);
    public event OnPlayerInfoChangedEvent OnPlayerInfoChanged;


    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
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
                energyTimer -= 60f;
            }
            OnPlayerInfoChanged(InfoType.Energy);
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
                toughenTimer -= 60f;
               
            }
            OnPlayerInfoChanged(InfoType.Toughen);
        }
        else
        {
            toughenTimer = 0;
        }
    }
    void Init()
    {
        this.Coin = 9999;
        this.Diamond = 233;
        this.Energy = 100;
        this.Exp = 1000;
        this.Level = 11;
        this.Energy = 89;
        this.Toughen = 30;
        this.HeadPortrait = "头像底板女性";
        this.Name = "lqhhh";
         
        //穿戴装备初始化
        //this.HelmID = 1001;
        //this.ClothID = 1002;
        //this.WeaponID = 1003;
        //this.ShoesID = 1004;
        //this.NecklaceID = 1005;
        //this.BraceletID = 1006;
        //this.NecklaceID = 1007;
        //this.BraceletID = 1008;
        //this.RingID = 1009;
        //this.WingID = 1010;
        InitHpDamagePower();
        OnPlayerInfoChanged(InfoType.All);
    }
    void InitHpDamagePower()
    {
        this.HP = this.Level * 100;
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
            InventoryUI._instance.AddInventoryItem(inventoryItemDressed);
        }
       OnPlayerInfoChanged(InfoType.Equip);
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

        OnPlayerInfoChanged(InfoType.Equip);
    }
    public bool GetCoin(int count)
    {
       if(Coin>count)
       {
           Coin -= count;
           OnPlayerInfoChanged(InfoType.Coin);
           return true;
       }
       return false;
    }
    public void AddCoin(int count)
    {
        Coin += count;
        OnPlayerInfoChanged(InfoType.Coin);
    }
    public void InventoryUse(InventoryItem it,int count)
    {

        //使用效果
        if(it.Inventory.InventoryTYPE==InventoryType.Drug)
        {

        }
        else if(it.Inventory.InventoryTYPE==InventoryType.Box)
        {

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


}
