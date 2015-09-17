using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using TaiDouCommon;

public abstract class ControllerBase : MonoBehaviour
{
    public abstract OperationCode opCode { get; }
    public virtual void OnEvent(EventData eventData) {

    }
    public virtual void Start()
    {
        PhotonEngine.Instance.RegisterController(opCode, this);//完成opCode和Controller注册
    }
    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.UnRegisterController(opCode);//注销
    }

    public abstract void OnOperationResponse(OperationResponse response);

}
