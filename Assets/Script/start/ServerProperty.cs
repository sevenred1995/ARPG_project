using UnityEngine;
using System.Collections;

public class ServerProperty : MonoBehaviour {
    public string ip="127.0.0.1";
    public string name="1区 大地飞鹰";
    public int num = 100;
    public Color col;
    public UILabel lbname;
    public void ServerPropertyInit(string ip,string name,int num)
    {
        this.ip = ip;
        this.name = name;
        lbname.text = name;
        this.num = num;
    }
    public void OnPress(bool isPress)
    {
        if(isPress==false)
        {
            transform.root.SendMessage("OnServerSelected", this.gameObject);
        }
    }

}
