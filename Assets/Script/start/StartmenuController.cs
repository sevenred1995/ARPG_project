using UnityEngine;
using System.Collections;

public class StartmenuController : MonoBehaviour
{

    public static StartmenuController instance;
    #region button
    public UIButton btn_start_user;
    public UIButton btn_start_server;
    public UIButton btn_start_enter;

    public UIButton btn_login_login;
    public UIButton btn_login_reg;
    public UIButton btn_login_close;

    public UIButton btn_reg_cancel;
    public UIButton btn_reg_reg;
    public UIButton btn_reg_close;

    public UIButton btn_server_selected;

    public UIButton btn_select_return;
    public UIButton btn_select_change;

    public UIButton btn_selectshow_back;
    public UIButton btn_selectshow_sure;

    #endregion
    #region Tween
    public TweenScale startpanelTween;
    public TweenScale loginpanelTween;
    public TweenScale regpanelTween;
    public TweenScale serverpanelTween;

    public TweenPosition startpanelTween1;
    public TweenPosition playerselectTween;//显示的界面
    public TweenPosition playershowselectTween;//更换角色的界面

    #endregion
    #region Input
    public UIInput input_login_account;
    public UIInput input_login_password;
    public UIInput input_reg_account;
    public UIInput input_reg_password;
    public UIInput input_reg_repassword;
    public UIInput input_selecredShow_name;
    #endregion
    public UILabel label_start_user;//开始界面上用户名的显示
    public UILabel label_start_server;
    public UILabel label_playerselect_name;
    public UILabel label_playerselecte_level;

    public UIGrid serverlistgrid;
    #region prefab
    private GameObject serveritem_red;
    private GameObject serveritem_green;
    public GameObject[] playerArray;
    public GameObject[] playerSelectedArray;
    #endregion
    public GameObject playerselectedparent;

    [HideInInspector]
    public static string username;
    [HideInInspector]
    public static string password;
    [HideInInspector]
    public static ServerProperty sp;
    [HideInInspector]
    private static GameObject playerSelected;
    
    public GameObject server_selected_go;
    private bool isInitServerList = false;
    void Start()
    {
        EventDelegate.Set(btn_start_user.onClick, () => { On_Start_Username_Click();});
        EventDelegate.Set(btn_start_server.onClick, () => { On_Start_Server_Click();});
        EventDelegate.Set(btn_start_enter.onClick, () => { On_Start_Enter_Click();});

        EventDelegate.Set(btn_login_login.onClick, () => { On_Login_login_Click(); });
        EventDelegate.Set(btn_login_reg.onClick, () => { On_Login_reg_Click(); });
        EventDelegate.Set(btn_login_close.onClick, () => { On_Login_close_Click();});

        EventDelegate.Set(btn_reg_cancel.onClick, () => { On_Reg_cancel_Click(); });
        EventDelegate.Set(btn_reg_reg.onClick, () => { On_Reg_reg_Click(); });
        EventDelegate.Set(btn_reg_close.onClick, () => { On_Reg_close_Click(); });
        EventDelegate.Set(btn_server_selected.onClick, () => {On_server_selected_Click(); });

        EventDelegate.Set(btn_select_return.onClick, () => { On_select_return_Click(); });
        EventDelegate.Set(btn_select_change.onClick, () => { On_select_change_Click(); });

        EventDelegate.Set(btn_selectshow_sure.onClick, () => { On_selectedshow_sure_click(); });
        EventDelegate.Set(btn_selectshow_back.onClick, () => { On_selectedshow_back_Clik(); });
        if(instance==null)
        {
            instance = this;
        }

        InitServerList();
    }
    public void On_Start_Username_Click()
    {
        startpanelTween.PlayForward();
        StartCoroutine(HidePanel(startpanelTween.gameObject));
        loginpanelTween.gameObject.SetActive(true);
        loginpanelTween.PlayForward();
    }
    public void On_Start_Server_Click()
    {
        //跳转选择服务器的界面。
        startpanelTween.PlayForward();
        StartCoroutine(HidePanel(startpanelTween.gameObject));
        serverpanelTween.gameObject.SetActive(true);
        serverpanelTween.PlayForward();
    }
    private void InitServerList()
    {
        if (isInitServerList) return;
        //连接服务器，取得游戏服务器的列表、、
        //TODO
        //初始化选择的服务器
        //TODO
        //添加服务器列表
        for (int i = 0; i < 20;i++ ) 
        {
            string ip = "127.0.0.1";
            string name = (i + 1) + "区 仙剑奇侠传";
            int num = Random.Range(0, 100);
            Color col=Color.red;

            serveritem_red = Resources.Load<GameObject>("btn-server-green");
            serveritem_green = Resources.Load<GameObject>("btn-server-red");
            GameObject go=null;
            if(num>50)
            {
                go=NGUITools.AddChild(serverlistgrid.gameObject, serveritem_red);
                col = Color.green;
            }
            else
            {
                go = NGUITools.AddChild(serverlistgrid.gameObject, serveritem_green);
                col = Color.red;
            }
            ServerProperty sp = go.GetComponent<ServerProperty>();
            sp.ServerPropertyInit(ip, name, num);
            sp.col = col;
            serverlistgrid.AddChild(go.transform);
        }
        isInitServerList = true;
    }
    public void On_Start_Enter_Click()
    {
        //1、连接服务器；

        //2\
        startpanelTween1.PlayForward();
        StartCoroutine(HidePanel(startpanelTween1.gameObject));
        playerselectTween.gameObject.SetActive(true);
        playerselectTween.PlayForward();
    }
    IEnumerator HidePanel(GameObject go)
    {
        yield return new WaitForSeconds(0.4f);
        go.SetActive(false);
    }
    public void On_Login_login_Click()
    {
        //保存用户名和密码
        username = input_login_account.value;
        password = input_login_password.value;

        loginpanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginpanelTween.gameObject));
        startpanelTween.gameObject.SetActive(true);
        startpanelTween.PlayReverse();

        label_start_user.text = username;

    }
    public void On_Login_reg_Click()
    {
        //跳转到注册的界面
        loginpanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginpanelTween.gameObject));
        regpanelTween.gameObject.SetActive(true);
        regpanelTween.PlayForward();

    }
    public void On_Login_close_Click()
    {
        loginpanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginpanelTween.gameObject));
        startpanelTween.gameObject.SetActive(true);
        startpanelTween.PlayReverse();
    }
    public void On_Reg_cancel_Click()
    {
        //返回
        regpanelTween.PlayReverse();
        StartCoroutine(HidePanel(regpanelTween.gameObject));
        loginpanelTween.gameObject.SetActive(true);
        loginpanelTween.PlayForward();
    }
    public void On_Reg_reg_Click()
    {
        //1、本地验证，连接服务器进行验证
        
        //2、验证失败

        //3、连接成功
        username = input_reg_account.value;
        password = input_reg_password.value;

        regpanelTween.PlayReverse();
        StartCoroutine(HidePanel(regpanelTween.gameObject));
        startpanelTween.gameObject.SetActive(true);
        startpanelTween.PlayReverse();

        label_start_user.text = username;
    }
    public void On_Reg_close_Click()
    {
        regpanelTween.PlayReverse();
        StartCoroutine(HidePanel(regpanelTween.gameObject));
        loginpanelTween.gameObject.SetActive(true);
        loginpanelTween.PlayForward();
    }
    public void OnServerSelected(GameObject servergo)
    {
        sp = servergo.GetComponent<ServerProperty>();
        server_selected_go.GetComponent<UISprite>().spriteName=servergo.GetComponent<UISprite>().spriteName;
        server_selected_go.transform.Find("Label").GetComponent<UILabel>().text = sp.name;
        server_selected_go.transform.Find("Label").GetComponent<UILabel>().color = sp.col;
    }

    //确定选择
    public void On_server_selected_Click()
    {
        serverpanelTween.PlayReverse();
        StartCoroutine(HidePanel(serverpanelTween.gameObject));
        startpanelTween.gameObject.SetActive(true);
        startpanelTween.PlayReverse();
        label_start_server.text = sp.name;
    }

    public void On_select_return_Click()
    {
        playerselectTween.PlayReverse();
        StartCoroutine(HidePanel(playerselectTween.gameObject));
        startpanelTween1.gameObject.SetActive(true);
        startpanelTween1.PlayReverse();
    }
    public void On_select_change_Click()
    {
        playerselectTween.PlayReverse();
        StartCoroutine(HidePanel(playerselectTween.gameObject));

        playershowselectTween.gameObject.SetActive(true);
        playershowselectTween.PlayForward();
    }
    public void OnPlayerSelect(GameObject go)
    {
        if(go==playerSelected)
        {
            return;
        }
        iTween.ScaleTo(go, new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
        if (playerSelected != null)
        {
            iTween.ScaleTo(playerSelected, new Vector3(1f, 1f, 1f), 0.5f);
        }
        playerSelected = go;
    }

    public void On_selectedshow_back_Clik()
    {
        playershowselectTween.PlayReverse();
        StartCoroutine(HidePanel(playershowselectTween.gameObject));
        playerselectTween.gameObject.SetActive(true);
        playerselectTween.PlayForward();
    }
    public void On_selectedshow_sure_click()
    {
        //1、检测姓名是否合法

        //2、检测是否选择了该角色。
        //信息获取
        //确定了选择哪个角色

        string player_hieracchy_name = null;
        if (playerSelected != null)
        {
            player_hieracchy_name = playerSelected.name;
        }
        else
        {
            Debug.Log("请选择角色。。。。");
            return; 
        }
        
        playershowselectTween.PlayReverse();
        StartCoroutine(HidePanel(playershowselectTween.gameObject));
        playerselectTween.gameObject.SetActive(true);
        playerselectTween.PlayForward();

        GameObject player = Resources.Load<GameObject>(player_hieracchy_name + "select");
        if(player!=null)
        {
            GameObject.Destroy(playerselectedparent.GetComponentInChildren<Animation>().gameObject);
            GameObject playerGo = Instantiate(player,Vector3.zero,Quaternion.identity) as GameObject;
            playerGo.transform.parent = playerselectedparent.transform;
            playerGo.transform.localRotation = Quaternion.identity;
            playerGo.transform.localPosition = Vector3.zero;
            playerGo.transform.localScale = new Vector3(1, 1, 1);
        }
        //更新角色信息。
        label_playerselect_name.text = input_selecredShow_name.value;
        label_playerselecte_level.text = "LV:0";
    }

}
