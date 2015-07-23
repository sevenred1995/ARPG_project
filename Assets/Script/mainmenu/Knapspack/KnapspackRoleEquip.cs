using UnityEngine;
using System.Collections;

public class KnapspackRoleEquip : MonoBehaviour {

    //取得装备，同步装备信息
    private UISprite _sprite;
    private InventoryItem it;
    private UISprite Sprite
    {
        get
        {
            if (_sprite == null)
            {
                _sprite = this.GetComponent<UISprite>();
            }    
            return _sprite;
        }
    }
    public void SetEquip(int id)
    {
        Inventory inventory = null;
        bool  isExit = InventoryManager._instance.inventoryDic.TryGetValue(id, out inventory);
        if(isExit)
        {
            Sprite.spriteName = inventory.Icon;
        } 
    }   

    //为角色的装备格子赋值
    public void SetInventoryItem(InventoryItem it)
    {
        if(it==null)
        {
            Sprite.spriteName = "bg_道具";
            return;
        }
        this.it = it;
        Sprite.spriteName = it.Inventory.Icon;
    }

    public void OnPress(bool isPress)
    {
        if (isPress && it != null&&Input.GetMouseButtonDown(0))
        {
            object[] objectArray=new object[3];
            objectArray[0]=it;
            objectArray[1]=false;
            objectArray[2] = this;
            transform.parent.parent.SendMessage("OnInventoryClick", objectArray);
        }
    }
}
