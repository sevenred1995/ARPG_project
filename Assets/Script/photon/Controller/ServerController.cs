using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using TaiDouCommon;
using System.Collections.Generic;
using TaiDouCommon;
using LitJson;

public class ServerController:ControllerBase {

    public override TaiDouCommon.OperationCode opCode
    {
        get { return OperationCode.GetServer; }
    }
    public override void Start()
    {
        base.Start();
        PhotonEngine.Instance.OnconnectedToServer += GetServerList;
       
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        PhotonEngine.Instance.OnconnectedToServer -= GetServerList;
    }
    public void GetServerList()
    {
        //向服务器发起请求
        PhotonEngine.Instance.SendRequest(OperationCode.GetServer, new Dictionary<byte, object>());
    }
    //接收服务器的响应
    public override void OnOperationResponse(OperationResponse response)
    {
        Dictionary<byte, object> parameters = response.Parameters;
        object jsonObject = null;
        parameters.TryGetValue((byte)ParameterCode.ServerList, out jsonObject);
        
        if(jsonObject!=null)
        {
            List<TaiDouCommon.Model.ServerPropety> serverList=
            JsonMapper.ToObject<List<TaiDouCommon.Model.ServerPropety>>((jsonObject.ToString()));
            StartmenuController.instance.InitServerList(serverList);
        }
        else
        {
            Debug.Log("Don't receieve right serverlist");
        }

    }

}
