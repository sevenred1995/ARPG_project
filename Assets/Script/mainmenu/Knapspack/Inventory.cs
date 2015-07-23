using UnityEngine;
using System.Collections;
public enum InventoryType
{
    Equip,
    Drug,
    Box
}
public enum EquipType
{
    Helm,
    Cloth,
    Weapon,
    Shoes,
    Necklace,
    Bracelet,
    Ring,
    Wing
}
public class Inventory{

    #region info
    private int id;
    private string name;
    private string icon;
    private InventoryType inventoryType;
    private EquipType equipType;
    private int price = 0;
    private int starLevel = 1;
    private int quality = 1;
    private int hp = 0;
    private int damage = 0;
    private int power = 0;
    private InfoType infoType;//作用属性
    private int applyValue;//作用值
    private string des;//描述
    #endregion
    #region getset
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Icon
    {
        get { return icon; }
        set { icon = value; }
    }
    public InventoryType InventoryTYPE
    {
        get { return inventoryType; }
        set { inventoryType = value; }
    }
    public EquipType EquipTYPE
    {
        get { return equipType; }
        set { equipType = value; }
    }
    public int Price
    {
        get { return price; }
        set { price = value; }
    }
    public int StarLevel
    {
        get { return starLevel; }
        set { starLevel = value; }
    }
    public int Quality
    {
        get { return quality; }
        set { quality = value; }
    }
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public int Power
    {
        get { return power; }
        set { power = value; }
    }
    public InfoType InfoTYPE
    {
        get { return infoType; }
        set { infoType = value; }
    }
    public int ApplayValue
    {
        get { return applyValue; }
        set { applyValue = value; }
    }
    public string Des
    {
        get { return des; }
        set { des = value; }
    }
    #endregion
}
