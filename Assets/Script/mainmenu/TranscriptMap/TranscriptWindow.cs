using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TaiDouCommon.Model;

public class TranscriptWindow : MonoBehaviour {

    public UILabel desLabel;
    public UILabel energyLabel;
    public UILabel energyTitleLabel;
    public UIButton btnsignleEnter;
    public UIButton btnteamEnter;
    public UIButton btnClose;

    public TweenScale tween;
    private BtnTranscriptUI currTranscript;

    private BattleController battleController;

    public void Awake()
    {
        desLabel = transform.Find("DesLabel").GetComponent<UILabel>();
        energyLabel = transform.Find("EnergyLabel").GetComponent<UILabel>();
        energyTitleLabel = transform.Find("EnergyTitleLabel").GetComponent<UILabel>();
        btnsignleEnter = transform.Find("BtnSignleEnter").GetComponent<UIButton>();
        btnteamEnter = transform.Find("BtnTeamEnter").GetComponent<UIButton>();

        btnClose = transform.Find("BtnClose").GetComponent<UIButton>();

        tween = transform.GetComponent<TweenScale>();

        //注册按钮点击事件
        EventDelegate ed1 = new EventDelegate(this,"OnSignleEnter");
        btnsignleEnter.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "OnClose");
        btnClose.onClick.Add(ed2);
        EventDelegate.Set(btnteamEnter.onClick, () =>
        {
            OnteamEnter();
        });
        battleController = GameManger._instance.GetComponent<BattleController>();

        battleController.OnGetTeam += this.OnGetTeam;
        battleController.OnWaitTeam += this.OnWaitTeam;
        battleController.OnCancelTeam += this.OnCancelTeam;
    }
    public void ShowWarn()
    {
        //等级不足
        MessageManager._instance.ShowMessage("当前等级不足，无法进入！！");
        //tween.PlayForward();
    }
    public void ShowDialog(BtnTranscriptUI transcript)
    {
        currTranscript = transcript;
        desLabel.text = transcript.des;
        energyLabel.text = 3+"";
        tween.PlayForward();
    }
    public void Hide()
    {
        tween.PlayReverse();
    }
    public void OnSignleEnter()
    {
        GameManger._instance.battleType = BattleType.Person;
        GameManger._instance.trancriptID = currTranscript.transcriptID;
        AsyncOperation op=Application.LoadLevelAsync(currTranscript.transcriptName);
        LoadSceneProgressBar._instance.Show(op);
    }
    public void OnteamEnter() {
        btnteamEnter.isEnabled = false;
        TimeDialogUI._instance.ShowTimer();
        battleController.SendTeam();
    }
    void OnGetTeam(List<Role> list,int MasterRoleID) {
        //TODO
        //Debug.Log("组队成功");
        PhotonEngine.Instance.rolelist = list;
        if (PhotonEngine.Instance.role.ID == MasterRoleID)
        {
            GameManger._instance.isMaster = true;//判断当前客户端是否为主机
        }else
        {
            GameManger._instance.isMaster = false;
        }
        GameManger._instance.battleType = BattleType.Team;
        GameManger._instance.trancriptID = currTranscript.transcriptID;
        AsyncOperation op = Application.LoadLevelAsync(currTranscript.transcriptName);
        LoadSceneProgressBar._instance.Show(op);
    }
    void OnWaitTeam() {
        Debug.Log("waiting Team....");
    }
    void OnCancelTeam() {
        print("candel Team success");
        btnteamEnter.isEnabled = true;
    }

    public void OnClose()
    {
        Hide();
    }
    //取消组队请求
    public void OnEndTimer() {
        battleController.CancelTeam();
    }
    void OnDestroy() {
        if(battleController!=null)
        {
            battleController.OnGetTeam -= this.OnGetTeam;
            battleController.OnWaitTeam -= this.OnWaitTeam;
            battleController.OnCancelTeam -= this.OnCancelTeam;
        }
    }

}
