using UnityEngine;
using System.Collections;
using TaiDouCommon;
using TaiDouCommon.Tools;
using System.Collections.Generic;
using TaiDouCommon.Model;
using ExitGames.Client.Photon;
public class BattleController : ControllerBase {

    public override TaiDouCommon.OperationCode opCode {
        get { return TaiDouCommon.OperationCode.Battle; }
    }
    public override void Start() {
        base.Start();
    }
    public void SyncPlayerAnimation(PlayerAnimationModel model) {
        Dictionary<byte,object> parameters=new Dictionary<byte,object>();
        ParameterTool.AddParameters<PlayerAnimationModel>(parameters, ParameterCode.PlayerAnimationModel,model);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.SyncPlayerAnimation, parameters);
    }
    //向服务器发起角色移动动画信息的同步
    public void AsyncPlayerMoveAnimation(PlayerMoveAnimationModel model) {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameters<PlayerMoveAnimationModel>(parameters, ParameterCode.PlayerMoveAnimationModel,model);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.AsyncPlayerMoveAnimation, parameters);
    }
    //发起位置转向同步信息
    public void AsyncPostionAndRotation(Vector3 postion, Vector3 eulerAngles) {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameters(parameters, ParameterCode.RoleID, PhotonEngine.Instance.role.ID, false);
        ParameterTool.AddParameters(parameters, ParameterCode.Postion, new Vector3Obj(postion));
        ParameterTool.AddParameters(parameters, ParameterCode.EulerAnglers, new Vector3Obj(eulerAngles));
        PhotonEngine.Instance.SendRequest(opCode, SubCode.AsyncPostionAndEularAngler, parameters);
    }
    //发起组队请求
    public void SendTeam() {
        PhotonEngine.Instance.SendRequest(opCode, SubCode.SendTeam, new System.Collections.Generic.Dictionary<byte, object>());
    }
    public void CancelTeam() {
        PhotonEngine.Instance.SendRequest(opCode, SubCode.CancelTeam, new Dictionary<byte, object>());
    }
    public override void OnEvent(EventData eventData) {
        SubCode subcode = ParameterTool.GetParameters<SubCode>(eventData.Parameters, ParameterCode.SubCode, false);
        switch(subcode)
        {
            case SubCode.GetTeam:
                List<Role> list = ParameterTool.GetParameters<List<Role>>(eventData.Parameters, ParameterCode.RoleList);
                int masterRoleID = ParameterTool.GetParameters<int>(eventData.Parameters, ParameterCode.MasterRoleID, false);       
                 //组队成功
                if (OnGetTeam != null)
                    OnGetTeam(list, masterRoleID);
                break;
            case SubCode.AsyncPostionAndEularAngler:
                int roleid = ParameterTool.GetParameters<int>(eventData.Parameters, ParameterCode.RoleID,false);
                Vector3 pos = ParameterTool.GetParameters<Vector3Obj>(eventData.Parameters, ParameterCode.Postion).ToVector3();
                Vector3 eularAnglers = ParameterTool.GetParameters<Vector3Obj>(eventData.Parameters, ParameterCode.EulerAnglers).ToVector3();
                if (OnAsyncPostionAndRotation != null)
                    OnAsyncPostionAndRotation(roleid, pos, eularAnglers);
                break;
            case SubCode.AsyncPlayerMoveAnimation:
                int roleid2 = ParameterTool.GetParameters<int>(eventData.Parameters, ParameterCode.RoleID, false);
                PlayerMoveAnimationModel model = ParameterTool.GetParameters<PlayerMoveAnimationModel>(eventData.Parameters, ParameterCode.PlayerMoveAnimationModel);
                if (OnAsyncPlayerMoveAnimation != null)
                    OnAsyncPlayerMoveAnimation(roleid2, model);
                break;
            case SubCode.SyncPlayerAnimation:
                int roleid3 = ParameterTool.GetParameters<int>(eventData.Parameters, ParameterCode.RoleID, false);
                PlayerAnimationModel animationModel = ParameterTool.GetParameters<PlayerAnimationModel>(eventData.Parameters, ParameterCode.PlayerAnimationModel);
                if(OnSyncPlayerAnimation!=null)
                {
                    OnSyncPlayerAnimation(roleid3, animationModel);
                }
                break;
        }
    }
    public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response) {
        SubCode subCode = ParameterTool.GetParameters<SubCode>(response.Parameters, ParameterCode.SubCode, false);
        switch(subCode)
        {
            case SubCode.SendTeam:
                if(response.ReturnCode==(short)ReturnCode.GetTeam)
                {
                    List<Role> list = ParameterTool.GetParameters<List<Role>>(response.Parameters, ParameterCode.RoleList);
                    int masterRoleID = ParameterTool.GetParameters<int>(response.Parameters, ParameterCode.MasterRoleID,false);
                    //组队成功
                    if(OnGetTeam!=null)
                        OnGetTeam(list,masterRoleID);
                }
                else if(response.ReturnCode==(short)ReturnCode.WaitTeam)
                {
                    if(OnWaitTeam!=null)
                        OnWaitTeam();
                    //正在组队
                }
                break;
            case SubCode.CancelTeam:
                if(OnCancelTeam!=null&&response.ReturnCode==(short)ReturnCode.Success)
                {
                    OnCancelTeam();
                }
                break;
        }
    }
    
    public event OnGetTemEvent OnGetTeam;
    public event OnwaitTeamEvent OnWaitTeam;
    public event OnCancelTeamEvent OnCancelTeam;
    public event OnAsyncPostionAndRotationEvent OnAsyncPostionAndRotation;
    public event OnAsyncPlayerMoveAnimationEvent OnAsyncPlayerMoveAnimation;
    public event OnSyncPlayerAnimationEvent OnSyncPlayerAnimation;
}
