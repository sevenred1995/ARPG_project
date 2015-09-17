using UnityEngine;
using System.Collections;
using TaiDouCommon;
using TaiDouCommon.Model;
using LitJson;
using System.Collections.Generic;

public class RegisterController:ControllerBase{
    private User user;
    public override OperationCode opCode
    {
        get { return OperationCode.Register; }
    }
    public override void Start()
    {
        base.Start();
    }
    public void Register(string username, string password)
    {
        user = new User() { UserName = username, Password = password };
        string userJson = JsonMapper.ToJson(user);
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.User, userJson);
        PhotonEngine.Instance.SendRequest(OperationCode.Register, parameters);//向服务器发起注册请求
    }
    public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
    {
        MessageManager._instance.ShowMessage(response.DebugMessage);
        switch(response.ReturnCode)
        {
            case (short)ReturnCode.Success:
                StartmenuController.instance.isLogin = true;
                StartmenuController.instance.HideRegister();
                StartmenuController.instance.ShowStartMenu();
                StartmenuController.instance.label_start_user.text = user.UserName;
                break;
            case (short)ReturnCode.Fail:
                StartmenuController.instance.isLogin = false;
                break;
            default:
                break;
        }
    }
}
