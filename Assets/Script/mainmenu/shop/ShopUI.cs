using UnityEngine;
using System.Collections;
using TaiDouCommon.Model;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour {
    private GameObject grid;
    private UIButton btnclose;
    private UIButton btnrefresh;
    private UILabel timeLabel;
    private TweenPosition tween;
    private GameObject shopitemPrefab;
    void Awake() {
        
    }
    void Start() {
        grid = transform.Find("coinlist/Grid").gameObject;
        btnclose = transform.Find("Btnclose").GetComponent<UIButton>();
        btnrefresh = transform.Find("Btnrefresh").GetComponent<UIButton>();
        timeLabel = transform.Find("TimeLabel").GetComponent<UILabel>();
        tween = transform.GetComponent<TweenPosition>();
        EventDelegate ed1 = new EventDelegate(this,"OnClose");
        btnclose.onClick.Add(ed1);
        GetShop();
    }

    public void show() {
        tween.PlayForward();
    }

    void OnClose() {
        tween.PlayReverse();
    }
    /// <summary>
    /// 跟新商店物品
    /// </summary>
    void GetShop() {
        for(int i=0;i<16;i++)
        {
            shopitemPrefab = Resources.Load<GameObject>("shop/shopitem");
            int inventoryID = Random.Range(1001,1019);
            Inventory it;
            InventoryManager._instance.inventoryDic.TryGetValue(inventoryID,out it);
            GameObject go=NGUITools.AddChild(grid, shopitemPrefab);
            if (it != null)
            {
                go.GetComponent<shopoitem>().Show(it.Name, it.Price, it.Icon);
            }
          
            grid.GetComponent<UIGrid>().AddChild(go.transform);
        }
    }
    void OnGetShopInventoryDBList(List<ShopInventoryDB> list){

    }

}
