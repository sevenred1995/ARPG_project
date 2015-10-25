using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaiDouCommon;
using TaiDouCommon.Model;
using TaiDouCommon.Tools;
namespace Assets.Script.photon.Controller {
    public class PlayerController:ControllerBase {
        public override TaiDouCommon.OperationCode opCode {
            get {
                return OperationCode.Player;
            }
        }
        public void GetPlayerList() {

            PhotonEngine.Instance.SendRequest(opCode,SubCode.GetPlayerList, new Dictionary<byte, object>());
        }
        public override void OnEvent(EventData eventData) {
            SubCode subcode = ParameterTool.GetParameters<SubCode>(eventData.Parameters, ParameterCode.SubCode, false);
            switch(subcode)
            {
                case SubCode.GetPlayerList:
                    List<Role> list = ParameterTool.GetParameters<List<Role>>(eventData.Parameters, ParameterCode.PlayerList);
                    if(OnGetPlayerList!=null)
                    {
                        OnGetPlayerList(list);
                    }
                    break;
            }
        }
        public override void OnOperationResponse(OperationResponse response) {
            SubCode subcode = ParameterTool.GetParameters<SubCode>(response.Parameters, ParameterCode.SubCode, false);
            switch(subcode)
            {
                case SubCode.GetPlayerList:
                    List<Role> playerList = ParameterTool.GetParameters<List<Role>>(response.Parameters,ParameterCode.PlayerList);
                    if (OnGetPlayerList!=null)
                    {
                        OnGetPlayerList(playerList);
                    }
                    break;
            }
        }
        public event OnGetPlayerListEvent OnGetPlayerList;
    }
}
