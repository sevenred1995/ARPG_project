using UnityEngine;
using System.Collections;
using TaiDouCommon;
using TaiDouCommon.Model;
using LitJson;
using System.Collections.Generic;

public class LoginController:ControllerBase {
    User user;
    public override OperationCode opCode
    {
        get { return OperationCode.Login; }
    }
    public override void Start()
    {
        base.Start();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    public void Login(string username,string password)
    {
        user = new User() { UserName = username, Password = password };
        string userJson = JsonMapper.ToJson(user);
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.User, userJson);
        PhotonEngine.Instance.SendRequest(OperationCode.Login, parameters);
    } 
    public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
    {
        MessageManager._instance.ShowMessage(response.DebugMessage);
        switch(response.ReturnCode)
        {
            case (short)ReturnCode.Success:
                StartmenuController.instance.HideLogin();
                StartmenuController.instance.label_start_user.text = user.UserName;
                StartmenuController.instance.ShowStartMenu();
                StartmenuController.instance.isLogin = true;
                break;
            case (short)ReturnCode.Fail:
                StartmenuController.instance.isLogin = false;
                break;
        }
    }
}
