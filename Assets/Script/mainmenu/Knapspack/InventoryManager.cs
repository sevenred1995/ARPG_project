using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TaiDouCommon.Model;
public class InventoryManager : MonoBehaviour {
    public static InventoryManager _instance;

    public TextAsset listInfo;
    //存放物品信息
    public Dictionary<int, Inventory> inventoryDic = new Dictionary<int, Inventory>();
    //存放当前角色的物品信息
    //public Dictionary<int, InventoryItem> inventoryItemDic = new Dictionary<int, InventoryItem>();
    public List<InventoryItem> inventoryItemList = new List<InventoryItem>();

    public delegate void OnInventoryChangeEvent();
    public event OnInventoryChangeEvent OnInventoryChange;

    private InventoryItemDBController idbController;
    void Awake()
    {
        _instance = this;
        ReadInventoryInfo();
        idbController = this.GetComponent<InventoryItemDBController>();
        idbController.OnGetInventoryItemDB += this.OnGetInventoryItemDB;
        idbController.OnAddInventoryItemDB += this.OnAddInventoryItemDB;
        idbController.OnUpdateInventoryItemDB += this.OnUpdateInventoryItemDB;
      
    }
    void Start()
    {
        ReadInventoryItemInfo();
    }
    void Update() {
        PickUp();
    }
    public void ReadInventoryInfo()
    {
        string str=listInfo.ToString();
        string[] itemStrArray = str.Split('\n');
        foreach(string itemStr in itemStrArray)
        {
            string[] proArray = itemStr.Split('|');
            Inventory inventory = new Inventory();
            inventory.Id = int.Parse(proArray[0]);
            inventory.Name = proArray[1];
            inventory.Icon = proArray[2];
            switch(proArray[3])
            {
                case "Equip":
                    inventory.InventoryTYPE = InventoryType.Equip;
                    break;
                case "Drug":
                    inventory.InventoryTYPE = InventoryType.Drug;
                    break;
                case "Box":
                    inventory.InventoryTYPE = InventoryType.Box;
                    break;
            }
            if(inventory.InventoryTYPE==InventoryType.Equip)
            {
                switch(proArray[4])
                {
                    case"Helm":
                        inventory.EquipTYPE = EquipType.Helm;
                        break;
                    case "Cloth":
                        inventory.EquipTYPE = EquipType.Cloth;
                        break;
                    case "Weapon":
                        inventory.EquipTYPE = EquipType.Weapon;
                        break;
                    case "Shoes":
                        inventory.EquipTYPE = EquipType.Shoes;
                        break;
                    case "Necklace":
                        inventory.EquipTYPE = EquipType.Necklace;
                        break;
                    case "Bracelet":
                        inventory.EquipTYPE = EquipType.Bracelet;
                        break;
                    case "Ring":
                        inventory.EquipTYPE = EquipType.Ring;
                        break;
                    case "Wing":
                        inventory.EquipTYPE = EquipType.Wing;
                        break;
                }
            }
            //售价 星级 品质 伤害 生命 战斗力 作用类型 作用值 描述
            inventory.Price = int.Parse(proArray[5]);
            if(inventory.InventoryTYPE==InventoryType.Equip)
            { 
                inventory.StarLevel = int.Parse(proArray[6]);
                inventory.Quality = int.Parse(proArray[7]);
                inventory.Damage = int.Parse(proArray[8]);
                inventory.HP = int.Parse(proArray[9]);
                inventory.Power = int.Parse(proArray[10]);
            }
            if(inventory.InventoryTYPE==InventoryType.Drug)
            {      
                inventory.InfoTYPE = InfoType.Energy;
                inventory.ApplayValue = int.Parse(proArray[12]);
            }
            inventory.Des = proArray[13];
            inventoryDic.Add(inventory.Id, inventory);
        }
    }
    void ReadInventoryItemInfo()
    {
        //连接fuwuqi
        #region random
        //随机生成角色物品信息；
        //for(int i=0;i<20;i++)
        //{
        //    int id = Random.Range(1001, 1020);
        //    Inventory inventory = null;
        //    inventoryDic.TryGetValue(id, out inventory);
        //    if(inventory.InventoryTYPE==InventoryType.Equip)
        //    {
        //        InventoryItem it = new InventoryItem();
        //        it.Inventory = inventory;
        //        it.Level = Random.Range(1, 10);
        //        it.Count = 1;
        //        inventoryItemList.Add(it);
        //    }
        //    else
        //    {
        //        InventoryItem it = null;
        //        bool isExit = false;
        //        foreach(InventoryItem temp in inventoryItemList)
        //        {
        //            if(temp.Inventory.Id==id)
        //            {
        //                isExit = true;
        //                it = temp;
        //                break;
        //            }
        //        }
        //        if(isExit)
        //        {
        //            it.Count++;
        //        }
        //        else
        //        {
        //            it = new InventoryItem();
        //            it.Inventory = inventory;
        //            it.Count = 1;
        //            inventoryItemList.Add(it);
        //        }
        //    }
        //}
        #endregion
        idbController.GetInventoryItemDBList();
        OnInventoryChange();
    }
    void PickUp() {
        if(Input.GetKeyDown(KeyCode.P))
        {
            int id = Random.Range(1001, 1020);
            Inventory inventory = null;
            inventoryDic.TryGetValue(id, out inventory);
            Debug.Log("装备类型：" + inventory.InventoryTYPE);
            if(inventory.InventoryTYPE==InventoryType.Equip)
            {
                InventoryItemDB itemDB = new InventoryItemDB();
                itemDB.InventoryID = id;
                itemDB.Count = 1;
                itemDB.IsDressed = false;
                itemDB.Level = Random.Range(1, 10);
                idbController.AddInventoryItemDB(itemDB);
            }
            else
            {
                InventoryItem it = null;
                bool isExit = false;
                foreach (InventoryItem temp in inventoryItemList)
                {
                    
                    if (temp.Inventory.Id == id)
                    { 
                        Debug.Log("具有相同物品");
                        isExit = true;
                        it = temp;
                        break;
                    }
                }
                if (isExit)
                {
                    it.Count++;
                    Debug.Log("更新物品信息");
                    idbController.UpdateInventoryItemDB(it.InventoryItemDB);
                }
                else
                {
                    InventoryItemDB itemDB = new InventoryItemDB();
                    itemDB.InventoryID = id;
                    itemDB.Count = 1;
                    itemDB.IsDressed = false;
                    itemDB.Level = Random.Range(1, 10);
                    idbController.AddInventoryItemDB(itemDB);
                }
            }
        }
    }
    public void OnAddInventoryItemDB(InventoryItemDB itemDB) {
        InventoryItem it = new InventoryItem(itemDB);
        inventoryItemList.Add(it);
        OnInventoryChange();
    }
    public void OnUpdateInventoryItemDB() {
        OnInventoryChange();
    }
    public void OnGetInventoryItemDB(List<InventoryItemDB> list) {
        foreach (InventoryItemDB itemDB in list)
        {
            InventoryItem it = new InventoryItem(itemDB);
            inventoryItemList.Add(it);
            OnInventoryChange();//跟新显示背包物品信息
        }
        //OnInitInventoryItemDBDressed();
        playerInfo._instance.InitEquip();
    }
    void OnDestroy() {
        if (idbController != null)
            idbController.OnGetInventoryItemDB -= this.OnGetInventoryItemDB;
    }
    


}
