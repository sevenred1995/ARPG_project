using UnityEngine;
using System.Collections;
using TaiDouCommon.Model;

public class InventoryItem {
    //InventoryItem表示的就是玩家的一个装备

    //表示玩家所带有的装备
    private Inventory inventory;
    private int level;
    private int count;
    private bool isDressed = false;
    private int p;
    private InventoryItemDB inventoryItemDB;
    public InventoryItem() {

    }
    public InventoryItem(InventoryItemDB itemDB) {
        inventoryItemDB = itemDB;
        Inventory inventoryTemp;
        InventoryManager._instance.inventoryDic.TryGetValue(itemDB.InventoryID, out inventoryTemp);
        inventory = inventoryTemp;
        level = itemDB.Level;
        count = itemDB.Count;
        isDressed = itemDB.IsDressed;
    }
    public InventoryItemDB CreateInventoryDB() {
        inventoryItemDB = new InventoryItemDB();
        inventoryItemDB.InventoryID = inventory.Id;
        inventoryItemDB.Count = count;
        inventoryItemDB.IsDressed = isDressed;
        inventoryItemDB.Level = level;
        return inventoryItemDB;

    }
   public Inventory Inventory
   {
       get { return inventory; }
       set { inventory = value; }
   }
   public int Level
   {
       get { return level; }
       set { level = value;
       inventoryItemDB.Level = level;
       }
   }
    public int Count
   {
       get { return count; }
       set { count = value;
       inventoryItemDB.Count = count;
       }
   }
    public bool IsDressed
    {
        get { return isDressed; }
        set { isDressed = value;
        inventoryItemDB.IsDressed = isDressed;
        }
    }
    public InventoryItemDB InventoryItemDB {
        get { return inventoryItemDB; }
    }
}