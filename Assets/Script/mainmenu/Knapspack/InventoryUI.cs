using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {
    public static InventoryUI _instance;
    //背包格子
    public List<InventoryItemUI> inventoryItemUIList = new List<InventoryItemUI>();

    #region buttonlabel
    public UIButton arrangeBtn;
    public UILabel numLabel;
    #endregion
    private int count;//记录背包中的物品数目

    void Awake()
    {
        _instance = this;
        
        
        arrangeBtn = transform.Find("ArrangeButton").GetComponent<UIButton>();
        numLabel = transform.Find("numberLabel").GetComponent<UILabel>();

        //委托事件的注册
        InventoryManager._instance.OnInventoryChange += this.OnInventoryChange;

        EventDelegate ed2 = new EventDelegate(this, "On_knapSpack_arrange_Click");
        arrangeBtn.onClick.Add(ed2);

    }

    void OnInventoryChange()
    {
        UpdateShow();
    }
    //将该角色所有的背包物品同步过来
    public void UpdateShow()
    {
        int temp = 0;
        //背包里存放的物品只能是没有装备的物品
        for(int i=0;i<InventoryManager._instance.inventoryItemList.Count;i++)
        {
            InventoryItem it = InventoryManager._instance.inventoryItemList[i];
            if (it.IsDressed == false)
            {
                inventoryItemUIList[temp++].SetInventoryItem(it);
            } 
        }
        for(int j=temp;j<inventoryItemUIList.Count;j++)
        {
            inventoryItemUIList[j].Clear();
        }
        count = temp;
        numLabel.text = temp + "/" + inventoryItemUIList.Count;
    }

    void UpdateCount()
    {
        count = 0;
        foreach (InventoryItemUI itUI in inventoryItemUIList)
        {
            if (itUI.it != null)
            {
                count++;
            }
        }
        numLabel.text = count + "/" + inventoryItemUIList.Count;
    }
    public void AddInventoryItem(InventoryItem it)
    {
        //遍历所有的背包格子，发现空格子就往里面添加；
        foreach(InventoryItemUI itUI in inventoryItemUIList)
        {
            if(itUI.it==null)
            {
                itUI.SetInventoryItem(it);
                break;
            }
        }
        numLabel.text = count + "/" + inventoryItemUIList.Count;
    }
    void On_knapSpack_arrange_Click()
    {
        UpdateShow();
    }

} 
