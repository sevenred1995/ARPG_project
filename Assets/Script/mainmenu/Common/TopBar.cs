using UnityEngine;
using System.Collections;

public class TopBar : MonoBehaviour {
    public UILabel labeldiamondNum;
    public UILabel labelcoinNum;
    public UIButton diamondPlusBtn;
    public UIButton coinOPlusBtn;

    void Awake()
    {
        labeldiamondNum = GameObject.Find("DiamondBg/Labelnumber").GetComponent<UILabel>();
        labelcoinNum = GameObject.Find("CoinBg/Labelnumber").GetComponent<UILabel>();
        diamondPlusBtn = GameObject.Find("DiamondBg/DiamondPlusBtn").GetComponent<UIButton>();
        coinOPlusBtn = GameObject.Find("CoinBg/CoinPlusBtn").GetComponent<UIButton>();
        playerInfo._instance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;
    }

    public void OnDestory()
    {
        playerInfo._instance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
    }
    void OnPlayerInfoChanged(InfoType type)
    {
        if (type == InfoType.All || type == InfoType.Coin || type == InfoType.Diamond)
        {
            UpdateShow();
        }
    }
    void UpdateShow()
    {
        labelcoinNum.text = playerInfo._instance.Coin.ToString();
        labeldiamondNum.text = playerInfo._instance.Diamond.ToString();
    }
}
