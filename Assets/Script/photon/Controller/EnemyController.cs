using UnityEngine;
using System.Collections;
using TaiDouCommon;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using TaiDouCommon.Tools;

public class EnemyController : ControllerBase {
    public override OperationCode opCode {
        get { return OperationCode.Enemy; }
    }
    //向服务器端发送创建觉色的请求
    public void SendCreateEnemy(CreateEnemyModel model) {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameters<CreateEnemyModel>(parameters, ParameterCode.CreateEnemyModel, model);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.CreateEnemy, parameters);
    }
    public void AsyncEnemyPostion(EnemyPostionModel model) {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameters<EnemyPostionModel>(parameters,ParameterCode.EnemyPostionModel, model);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.AsyncPostionAndEularAngler, parameters);
    }
    public void SyncEnemyAnimation(EnemyAnimationModel model) {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameters<EnemyAnimationModel>(parameters, ParameterCode.EnemyAnimationModel, model);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.SyncEnemyAnimation, parameters);
    }
    public override void OnEvent(EventData eventData) {
        SubCode subCode = ParameterTool.GetParameters<SubCode>(eventData.Parameters, ParameterCode.SubCode, false);
        switch(subCode)
        {
            case SubCode.CreateEnemy:
                //Debug.Log("Get the response from server..");
                CreateEnemyModel model = ParameterTool.GetParameters<CreateEnemyModel>(eventData.Parameters, ParameterCode.CreateEnemyModel);
                if(OnCreateEnemy!=null)
                    OnCreateEnemy(model);
                break;
            case SubCode.AsyncPostionAndEularAngler:
                EnemyPostionModel enemyPosmodel = ParameterTool.GetParameters<EnemyPostionModel>(eventData.Parameters, ParameterCode.EnemyPostionModel);
                if (OnAsyncEnemyPostionRotation != null)
                    OnAsyncEnemyPostionRotation(enemyPosmodel);
                break;
            case SubCode.SyncEnemyAnimation:
                EnemyAnimationModel enemyAnimmodel = ParameterTool.GetParameters<EnemyAnimationModel>(eventData.Parameters, ParameterCode.EnemyAnimationModel);
                if (OnSyncEnemyAnimation != null)
                {
                    OnSyncEnemyAnimation(enemyAnimmodel);
                }
                break;
        }
    }
    public override void OnOperationResponse(OperationResponse response) {
        
    }

    public event OnCreateEnemyEvent OnCreateEnemy;
    public event OnAsyncEnemyPostionRotationEvent OnAsyncEnemyPostionRotation;
    public event OnSyncEnemyAnimationEvent OnSyncEnemyAnimation;
}
