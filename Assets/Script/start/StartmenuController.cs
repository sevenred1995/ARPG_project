using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TaiDouCommon.Model;
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
    [HideInInspector]
    public static List<Role> roleList = null;
    
    public GameObject server_selected_go;
    private bool isInitServerList = false;

    private LoginController login;
    private RegisterController register;
    private RoleController roleControll;

    public bool isRememberLoginInfo=false;
    public bool isLogin = false;
    public bool isSelectServer=false;

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
        login = this.GetComponent<LoginController>();
        register = this.GetComponent<RegisterController>();
        roleControll = this.GetComponent<RoleController>();

        roleControll.OnGetRole += this.OnGetRole;
        roleControll.OnGetEmptyRole += this.OnGetEmptyRole;
        roleControll.OnAddRole += this.OnAddRole;
        roleControll.OnseletRole += this.OnSelectRole;
    }
    public void Update()
    {
      if(isLogin&&isSelectServer)
      {
          btn_start_enter.isEnabled = true;
      }else
      {
          btn_start_enter.isEnabled = false;
      }
    }
 
    public void OnDestroy()
    {
        if (roleControll != null)
        {
            roleControll.OnGetRole -= this.OnGetRole;
            roleControll.OnGetEmptyRole -= this.OnGetEmptyRole;
            roleControll.OnAddRole -= this.OnAddRole;
        }
       
    }
    public void OnGetRole(List<Role> rolelist)
    {
        roleList = rolelist;
        //在服务器判断roleList是否为空
        HideStart();
        ShowRolePanel();
        //初始化角色面板信息
        if(roleList!=null&&roleList.Count>0)
        {
            Role role = rolelist[0];
            RoleShow(role);
        }
    }
    public void RoleShow(Role role)
    {
        PhotonEngine.Instance.role = role;///保存选择的角色信息
        bool isman = role.Isman;
        GameObject player=null;
        if(isman)
        {
             player = Resources.Load<GameObject>("boy_showselect");
        }else
        {
            player = Resources.Load<GameObject>("gril_showselect");
        }
        
        if (player != null)
        {
            GameObject.Destroy(playerselectedparent.GetComponentInChildren<Animation>().gameObject);
            GameObject playerGo = Instantiate(player, Vector3.zero, Quaternion.identity) as GameObject;
            playerGo.transform.parent = playerselectedparent.transform;
            playerGo.transform.localRotation = Quaternion.identity;
            playerGo.transform.localPosition = Vector3.zero;
            playerGo.transform.localScale = new Vector3(1, 1, 1);
        }
        //更新角色信息。
        label_playerselect_name.text = role.Name;
        label_playerselecte_level.text = "LV"+role.Level;
    }
    public void OnAddRole(Role role)
    {
        //创建角色成功，返回角色信息
        playershowselectTween.PlayReverse();
        StartCoroutine(HidePanel(playershowselectTween.gameObject));
        playerselectTween.gameObject.SetActive(true);
        playerselectTween.PlayForward();
    }
    public void OnGetEmptyRole()
    {
        roleList = null;
        HideStart();
        ShowRoleSelectPanel();
    }
    public void OnSelectRole()
    {
        HideRole();
        //显示加载进度条
        StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        yield return new WaitForSeconds(0.001f);
        AsyncOperation ao = Application.LoadLevelAsync(1);
        LoadSceneProgressBar._instance.Show(ao);
    }
    public void HideRole()
    {
        playerselectTween.PlayReverse();
        StartCoroutine(HidePanel(playerselectTween.gameObject));
    }
    public void OnEnterPlay()
    {
        roleControll.SelectRole(PhotonEngine.Instance.role);
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
    public void InitServerList(List<ServerPropety> serverlist)
    {
        btn_server_selected.isEnabled = false;
        if (isInitServerList) return;
        //连接服务器，取得游戏服务器的列表、、
        //TODO
        //初始化选择的服务器
        //TODO
        //添加服务器列表
        //server_selected_go.GetComponent<UISprite>().spriteName = servergo.GetComponent<UISprite>().spriteName;
        if(serverlist!=null)
        {
            if (serverlist[0].Num > 100)
            {
                server_selected_go.transform.Find("Label").GetComponent<UILabel>().color = Color.red;
                server_selected_go.GetComponent<UISprite>().spriteName = "btn_火爆1";
            }
            else
            {
                server_selected_go.transform.Find("Label").GetComponent<UILabel>().color = Color.green;
                server_selected_go.GetComponent<UISprite>().spriteName = "btn_流畅1";
            }
            server_selected_go.transform.Find("Label").GetComponent<UILabel>().text = serverlist[0].Name;
        }  
        foreach (ServerPropety spTemp in serverlist)
        {
            string ip = spTemp.IP+":4530";
            string name =spTemp.Name;
            int num = spTemp.Num;
            Color col = Color.red;

            serveritem_red = Resources.Load<GameObject>("btn-server-red");
            serveritem_green = Resources.Load<GameObject>("btn-server-green");
            GameObject go = null;
            if (num > 100)
            {
                go = NGUITools.AddChild(serverlistgrid.gameObject, serveritem_red);
                col = Color.red;
            }
            else
            {
                go = NGUITools.AddChild(serverlistgrid.gameObject, serveritem_green);
                col = Color.green;
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
        //向服务器发起请求加载角色信息
        roleControll.GetRole();
    }
    /// <summary>
    /// 显示角色信息界面
    /// </summary>
    public void ShowRolePanel()//
    {
        playerselectTween.gameObject.SetActive(true);
        playerselectTween.PlayForward();
    }

    /// <summary>
    /// 显示角色选择界面
    /// </summary>
    public void ShowRoleSelectPanel()
    {
        playershowselectTween.gameObject.SetActive(true);
        playershowselectTween.PlayForward();
    }
    public void HideStart()
    {
        startpanelTween1.PlayForward();
        StartCoroutine(HidePanel(startpanelTween1.gameObject));
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

        login.Login(username, password);

        
        //startpanelTween.gameObject.SetActive(true);
        //startpanelTween.PlayReverse();
        //label_start_user.text = username;
    }
    public void HideLogin()
    {
        ClearLoginInfo();
        loginpanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginpanelTween.gameObject));
    }
    public void On_Login_reg_Click()
    {
        ClearLoginInfo();
        //跳转到注册的界面
        loginpanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginpanelTween.gameObject));
        regpanelTween.gameObject.SetActive(true);
        regpanelTween.PlayForward();

    }
    public void On_Login_close_Click()
    {
        ClearLoginInfo();
        loginpanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginpanelTween.gameObject));
        startpanelTween.gameObject.SetActive(true);
        startpanelTween.PlayReverse();
    }
    public void On_Reg_cancel_Click()
    {
        ClearRegisterInfo();
        //返回
        regpanelTween.PlayReverse();
        StartCoroutine(HidePanel(regpanelTween.gameObject));
        loginpanelTween.gameObject.SetActive(true);
        loginpanelTween.PlayForward();
    }
    public void On_Reg_reg_Click()
    {
        //1、本地验证，连接服务器进行验证
        username = input_reg_account.value;
        password = input_reg_password.value;
        string repassword = input_reg_repassword.value;
        if(!password.Equals(repassword))
        {
            MessageManager._instance.ShowMessage("两次输入密码不相同！");
            return;
        }
        if(username==null||username.Length<=2)
        {
            MessageManager._instance.ShowMessage("用户名不符合要求！");
            return;
        }
        else if(password==null||password.Length<=4)
        {
            MessageManager._instance.ShowMessage("密码长度不符合要求！");
            return;
        }
        register.Register(username, password);
        //2、验证失败
        //3、连接成功
        //label_start_user.text = username;
    }
    public void HideRegister()
    {
        ClearRegisterInfo();
        regpanelTween.PlayReverse();
        StartCoroutine(HidePanel(regpanelTween.gameObject));
    }
    public void ShowStartMenu()
    {
        startpanelTween.gameObject.SetActive(true);
        startpanelTween.PlayReverse();
    }
    public void On_Reg_close_Click()
    {
        regpanelTween.PlayReverse();
        StartCoroutine(HidePanel(regpanelTween.gameObject));
        loginpanelTween.gameObject.SetActive(true);
        loginpanelTween.PlayForward();
        ClearRegisterInfo(); 
    }
    public void OnServerSelected(GameObject servergo)
    {
        btn_server_selected.isEnabled = true;
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
        if (sp!=null)
        {
            label_start_server.text = sp.name;
        }
        isSelectServer = true;
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
        if(roleList!=null)
        {
            foreach (Role role in roleList)
            {
                if (role.Isman && go.name.IndexOf("boy") >= 0 || (!role.Isman) && go.name.IndexOf("gril") >= 0)
                {
                    input_selecredShow_name.value = role.Name;
                }else
                {
                    input_selecredShow_name.value = "";
                }
            }
        }
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
        if(playerSelected==null)
        {
            return;
        }
        //1、检测姓名是否合法
        if(input_selecredShow_name.value.Length<3)
        {
            MessageManager._instance.ShowMessage("角色信息不符合要求");
            return;
        }
        Role role = null;
        if(roleList!=null)
        {
            foreach (Role roleTemp in roleList)
            {
                if ((roleTemp.Isman && playerSelected.name.IndexOf("boy") >= 0) || (!roleTemp.Isman) && playerSelected.name.IndexOf("gril") >= 0)
                {
                    Debug.Log(roleTemp.Isman);
                    role = roleTemp;
                }
            }
        }
        if(role==null)//不存在角色信息
        {
            Debug.Log("创建角色。。。。。");
            Role newrole = new Role();
            newrole.Name = input_selecredShow_name.value;
            newrole.Isman = playerSelected.name.IndexOf("boy") >= 0 ? true : false;
            newrole.Level = 1;
            newrole.Coin = 0;
            newrole.Diamond = 0;
            newrole.Exp = 0;
            newrole.Energy = 100;
            newrole.Toughen = 50;
            //申请创建角色
            //roleList.Add(newrole);
            roleControll.AddRole(newrole);
            HideSelectRolePanel();
            ShowRolePanel();
            RoleShow(newrole);/////////////////传错参数出现错误。
        }
        else
        {
            HideSelectRolePanel();
            ShowRolePanel();
            RoleShow(role);
        }
    }
    public void HideSelectRolePanel()
    {
        playershowselectTween.PlayReverse();
        StartCoroutine(HidePanel(playershowselectTween.gameObject));
    }
    public void ClearLoginInfo()
    {
        if(isRememberLoginInfo)
        {
            return;
        }
        input_login_account.value = "";
        input_login_password.value = "";
    }
    public void ClearRegisterInfo()
    {
        input_reg_account.value = "";
        input_reg_password.value = "";
        input_reg_repassword.value = "";
    }
}
