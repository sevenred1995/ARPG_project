using UnityEngine;
using System.Collections;

public class shopoitem : MonoBehaviour {
    public UISprite shopinventory;
    public UILabel coinLabel;
    public UIButton btnExchange;
    public UILabel inventoryName;

    void Start() {
        EventDelegate ed = new EventDelegate(this, "OnExchange");
        btnExchange.onClick.Add(ed);
    }

    /// <summary>
    /// 根据金币数量购买物品
    /// </summary>
    public void OnExchange() {
       //连接服务器

    }
    public void Show(string name,int coin,string spritename) {
        inventoryName.text = name;
        coinLabel.text = coin.ToString();
        shopinventory.spriteName = spritename;
    }
}
