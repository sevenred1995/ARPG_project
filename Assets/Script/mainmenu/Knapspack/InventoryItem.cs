using UnityEngine;
using System.Collections;

public class InventoryItem {
    //InventoryItem表示的就是玩家的一个装备

    //表示玩家所带有的装备
    private Inventory inventory;
    private int level;
    private int count;
    private bool isDressed = false;

   public Inventory Inventory
   {
       get { return inventory; }
       set { inventory = value; }
   }
   public int Level
   {
       get { return level; }
       set { level = value; }
   }
    public int Count
   {
       get { return count; }
       set { count = value; }
   }
    public bool IsDressed
    {
        get { return isDressed; }
        set { isDressed = value; }
    }

}
