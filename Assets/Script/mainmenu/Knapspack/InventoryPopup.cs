using UnityEngine;
using System.Collections;

public class InventoryPopup : MonoBehaviour {
    public UILabel nameLabel;
    public UILabel desLabel;
    public UISprite iconSprite;
    public UIButton useBtn;
    public UIButton usebatchingBtn;
    public UIButton closeBtn;

    [HideInInspector]
    public InventoryItem it;
    [HideInInspector]
    public InventoryItemUI itUI;
    void Awake()
    {
        nameLabel = transform.Find("NameLabel").GetComponent<UILabel>();
        desLabel = transform.Find("DescriptLabel").GetComponent<UILabel>();
        iconSprite = transform.Find("IconSprite/Sprite").GetComponent<UISprite>();
        useBtn = transform.Find("UseButton").GetComponent<UIButton>();
        usebatchingBtn = transform.Find("UseBatchingButton").GetComponent<UIButton>();
        closeBtn = transform.Find("CloseButton").GetComponent<UIButton>();

        EventDelegate ed = new EventDelegate(this, "On_InventoryPopup_Close_Click");
        closeBtn.onClick.Add(ed);
        EventDelegate ed1 = new EventDelegate(this, "On_Use_Click");
        useBtn.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "On_UseBatching_Click");
        usebatchingBtn.onClick.Add(ed2);
    }  
    public void Show(InventoryItem it,InventoryItemUI itUI)
    {
        this.it = it;
        this.itUI = itUI;
        gameObject.SetActive(true);
        nameLabel.text = it.Inventory.Name;
        desLabel.text = it.Inventory.Des;
        iconSprite.spriteName = it.Inventory.Icon;
    }
    public void On_InventoryPopup_Close_Click()
    {
        it = null;
        itUI = null;
        nameLabel.text = "";
        desLabel.text = "";
        transform.parent.parent.SendMessage("DisenableButton");
        gameObject.SetActive(false);
    }
    public void On_Use_Click()
    {
        itUI.ChangeCount(1);
        playerInfo._instance.InventoryUse(it, 1);
        On_InventoryPopup_Close_Click();
    }
    public void On_UseBatching_Click()
    {
        itUI.ChangeCount(it.Count);
        playerInfo._instance.InventoryUse(it, it.Count);
        On_InventoryPopup_Close_Click();
    }
}
