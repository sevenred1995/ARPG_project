using UnityEngine;
using System.Collections;

public class Knapspack : MonoBehaviour {

    public static Knapspack _instance;

    private EquipPopup equip;
    private InventoryPopup inventoryPopup;
    private UILabel priceLabel;
    private UIButton sellBtn;
    private UIButton closeBtn;
    private TweenPosition knapspackTween;
    private InventoryItemUI itUI;
    void Awake()
    {
        equip = transform.Find("EquipPanel/EquipPopup").GetComponent<EquipPopup>();
        inventoryPopup = transform.Find("InventoryPanel/inventoryPopup").GetComponent<InventoryPopup>();
       
        priceLabel = transform.Find("Inventory/PriceBg/Label").GetComponent<UILabel>();
        sellBtn = transform.Find("Inventory/SellButton").GetComponent<UIButton>();
        closeBtn = transform.Find("CloseButton").GetComponent<UIButton>();
        knapspackTween = this.GetComponent<TweenPosition>();

        //注册点击事件
        EventDelegate ed1 = new EventDelegate(this, "On_knapSpack_Close_Click");
        closeBtn.onClick.Add(ed1);
        EventDelegate ed3 = new EventDelegate(this, "On_knapspack_sell_Click");
        sellBtn.onClick.Add(ed3);

        DisenableButton();

    }

    //接受点击每个小格子传来消息
    public void OnInventoryClick(object[] objectArray)
    {
        InventoryItem it = objectArray[0] as InventoryItem;
        InventoryItemUI itUI =null;
        KnapspackRoleEquip kre = null;
        bool isLeft=false;
        if(it.Inventory.InventoryTYPE==InventoryType.Equip)
        {
           isLeft= (bool)objectArray[1];
            if(isLeft)
            {
                itUI = objectArray[2] as InventoryItemUI;
            }
            else
            {
                kre = objectArray[2] as KnapspackRoleEquip;
            }
            equip.Show(it,itUI,isLeft);
        }
        else
        {
            itUI = objectArray[2] as InventoryItemUI;
            inventoryPopup.Show(it,itUI);
        }
        if((it.Inventory.InventoryTYPE==InventoryType.Equip&&isLeft)||it.Inventory.InventoryTYPE!=InventoryType.Equip)
        {
            this.itUI = objectArray[2] as InventoryItemUI;
            EnableButton(itUI.it.Inventory.Price*itUI.it.Count);
        }
    }
    public void Show()
    {
        knapspackTween.gameObject.SetActive(true);
        knapspackTween.PlayReverse();
    }
    void DisenableButton()
    {
        sellBtn.isEnabled = false;
        priceLabel.text = "";
    }
    void EnableButton(int price)
    {
        sellBtn.isEnabled = true;
        priceLabel.text =price.ToString();
    } 
    void On_knapspack_sell_Click()
    {
        //角色所拥有的金钱数量增加  
        playerInfo._instance.AddCoin(itUI.it.Inventory.Price);
        //点击出售减少该物品,从内存中清楚该物品
        InventoryManager._instance.inventoryItemList.Remove(itUI.it);
        itUI.Clear();
        equip.gameObject.SetActive(false);
        inventoryPopup.gameObject.SetActive(false);
        //数量的更新
        InventoryUI._instance.SendMessage("UpdateCount");
        DisenableButton();
    } 
    void On_knapSpack_Close_Click()
    {
        knapspackTween.PlayForward();
        StartCoroutine(HidePanel(knapspackTween.gameObject));
    }
    IEnumerator HidePanel(GameObject go)
    {
        yield return new WaitForSeconds(0.4f);
        go.SetActive(false);
    }
}
