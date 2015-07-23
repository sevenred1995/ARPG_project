using UnityEngine;
using System.Collections;

public class InventoryItemUI : MonoBehaviour {
    public  UISprite _sprite;
    public  UILabel _label;
    public InventoryItem it;
    private UISprite Sprite
    {
        get
        {
            if(_sprite==null)
            {
                _sprite = transform.Find("Sprite").GetComponent<UISprite>();
            }
            return _sprite;
        }
    }
    private UILabel Label
    {
        get
        {
            if(_label==null)
            {
                _label = transform.Find("Label").GetComponent<UILabel>();
            }
            return _label;
        }
    }
    void Awake()
    {
        Label.text = "";
        Sprite.spriteName = "bg_道具";
    }
    public void SetInventoryItem(InventoryItem it)
    {
        this.it = it;
        Sprite.spriteName = it.Inventory.Icon;
        if(it.Count==1)
        {
            Label.text = "";
        }
        else
        {
            Label.text = it.Count.ToString();
        }
    }
    public void Clear()
    {
        it = null;
        Label.text = "";
        Sprite.spriteName = "bg_道具";
    }
    //NGUI自带的点击响应方法
    public void OnPress(bool isPress)
    {
        if (isPress && it != null&&Input.GetMouseButtonDown(0))
        {
            object[] objectArray = new object[3];
            objectArray[0] = it;
            objectArray[1] = true;
            objectArray[2] = this;
            transform.parent.parent.parent.SendMessage("OnInventoryClick", objectArray);
        }
    }
    public void ChangeCount(int count)
    {
        if(it.Count-count==0)
        {
            Clear();
        }
        else if(it.Count-count==1)
        {
            Label.text = null;
        }
        else
        {
            Label.text = (it.Count - count).ToString();
        }
    }
}
