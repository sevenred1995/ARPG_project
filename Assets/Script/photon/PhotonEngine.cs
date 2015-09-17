using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TaiDouCommon;
using TaiDouCommon.Model;
using TaiDouCommon.Tools;

public class PhotonEngine : MonoBehaviour,IPhotonPeerListener {

    private static PhotonEngine _instance;
    public static PhotonEngine Instance
    {
        get {return _instance;}
    }

    public ConnectionProtocol protocol = ConnectionProtocol.Tcp;
    public string serverAddress = "127.0.0.1:4530";
    public string applicationName = "TaiDouServer";

    private Dictionary<byte, ControllerBase> controllers = new Dictionary<byte, ControllerBase>();

    public PhotonPeer peer;
    public bool isConnected=false;

    public delegate void OnConnectedToServerEvent();
    public event OnConnectedToServerEvent OnconnectedToServer;

    public float time = 3f;
    private float timer;

    public static User user;
    public Role role;
    public List<Role> rolelist;
 
    void Awake()
    {
        _instance = this;
        peer = new PhotonPeer(this, protocol);
        peer.Connect(serverAddress, applicationName);
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (peer != null)
            peer.Service();//向服务器发起请求
    }

    public void RegisterController(OperationCode opCode,ControllerBase controll)
    {
        if (!controllers.ContainsKey((byte)opCode))
        {
            controllers.Add((byte)opCode, controll);
        }
    }

    public void UnRegisterController(OperationCode opCode)
    {
        controllers.Remove((byte)opCode);
    }

    public void SendRequest(OperationCode opCode,Dictionary<byte,object> parameters)
    {
        //Debug.Log("sendrequest to server,opCode" + opCode);
        peer.OpCustom((byte)opCode, parameters,true);//向服务器发送消息
    }
    public void SendRequest(OperationCode opCode, SubCode subCode, Dictionary<byte, object> parameters)
    {
        //Debug.Log("sendrequest to server,opCode: " + opCode+"SubCode: "+subCode);
        parameters.Add((byte)ParameterCode.SubCode,subCode);
        peer.OpCustom((byte)opCode, parameters, true);//向服务器发送消息
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        //Debug.Log(level + ":" + message);
    }

    public void OnEvent(EventData eventData)
    {
        ControllerBase controller;
        OperationCode opCode = ParameterTool.GetParameters<OperationCode>(eventData.Parameters, ParameterCode.OperationCode, false);
        controllers.TryGetValue((byte)opCode, out controller);
        if (controller != null)
            controller.OnEvent(eventData);
        else
            Debug.LogWarning("receieve unknown event+OperationCode:"+opCode);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        ControllerBase controller;
        controllers.TryGetValue(operationResponse.OperationCode, out controller);
        if(controller!=null)
        {
            controller.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.Log("Receieve a unknow response :"+operationResponse.OperationCode);
        }
    }
    public void OnStatusChanged(StatusCode statusCode)
    {
        switch(statusCode)
        {
          case StatusCode.Connect:
                isConnected = true;
                OnconnectedToServer();
                break;
            case StatusCode.Disconnect:
                isConnected = false;
                MessageManager._instance.ShowMessage("与服务器失去连接");
                break;
            case StatusCode.TimeoutDisconnect:
                MessageManager._instance.ShowMessage("连接超时。。");
                break;
            default:
                isConnected = false;
                break;
        }
    }
}
