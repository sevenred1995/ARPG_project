using UnityEngine;
using System.Collections;
using TaiDouCommon;
using System.Collections.Generic;
using TaiDouCommon.Model;
using TaiDouCommon.Tools;
public class RoleController : ControllerBase {
    public override TaiDouCommon.OperationCode opCode
    {
        get { return OperationCode.Role; }
    }

    public override void Start()
    {
        base.Start();
    }
    public void GetRole()
    {
        Dictionary<byte, object> parametes = new Dictionary<byte, object>();
        parametes.Add((byte)ParameterCode.SubCode, SubCode.GetRole);
        PhotonEngine.Instance.SendRequest(OperationCode.Role, parametes);
    }
    public void AddRole(Role role)
    {
        Dictionary<byte, object> parametes = new Dictionary<byte, object>();
        parametes.Add((byte)ParameterCode.Role, LitJson.JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OperationCode.Role, SubCode.AddRole, parametes);
    }
    public void SelectRole(Role role)
    {
        Dictionary<byte, object> parametes = new Dictionary<byte, object>();
        parametes.Add((byte)ParameterCode.SubCode, SubCode.SelectRole);
        parametes.Add((byte)ParameterCode.Role, LitJson.JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OperationCode.Role, parametes);
    }
    public void UpdateRole(Role role)
    {
        Dictionary<byte, object> parametes = new Dictionary<byte, object>();
        parametes.Add((byte)ParameterCode.Role, LitJson.JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OperationCode.Role, SubCode.UpdateRole, parametes);
    }

    public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
    {
        object subobject;
        response.Parameters.TryGetValue((byte)ParameterCode.SubCode, out subobject);
        SubCode subCode = (SubCode)subobject;
        switch(subCode)
        {
            case SubCode.GetRole:
                switch(response.ReturnCode)
                {
                    case (short)ReturnCode.Success:
                        List<Role> list = ParameterTool.GetParameters<List<Role>>(response.Parameters, ParameterCode.RoleList);
                        OnGetRole(list);
                        break;
                    case (short)ReturnCode.Empty:
                        OnGetEmptyRole();//当服务器没有该角色信息的时候跳转到创建角色的界面。
                        break;
                }
                break;
            case SubCode.AddRole:
                switch(response.ReturnCode)
                {
                    case (short)ReturnCode.Success:
                        Role role =ParameterTool.GetParameters<Role>(response.Parameters,ParameterCode.Role);
                        OnAddRole(role);
                        break;
                    case (short)ReturnCode.Fail:
                        //添加角色失败
                        MessageManager._instance.SendMessage(response.DebugMessage);
                        break;
                }
                break;
            case SubCode.SelectRole:
                if (OnseletRole != null)
                    OnseletRole();
                break;

        }

    }
    public event OnGetRoleEvent OnGetRole;
    public event OnAddRoleEvent OnAddRole;
    public event OngetEmptyRoleEvent OnGetEmptyRole;
    public event OnSelectRoleEvent OnseletRole;
}
