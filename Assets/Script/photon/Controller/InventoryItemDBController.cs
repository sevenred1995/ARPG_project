using UnityEngine;
using System.Collections;
using TaiDouCommon;
using TaiDouCommon.Model;
using System.Collections.Generic;
using TaiDouCommon.Tools;
public class InventoryItemDBController : ControllerBase {

    public override TaiDouCommon.OperationCode opCode {
        get {return OperationCode.InventoryItemDB;}
    }
    //向服务器发起获取装备列表的请求的接口
    public void GetInventoryItemDBList() {
        PhotonEngine.Instance.SendRequest(OperationCode.InventoryItemDB, SubCode.GetInventoryItemDB, new Dictionary<byte, object>());
    }
    public void AddInventoryItemDB(InventoryItemDB iidb) {
        Dictionary<byte,object> paramreters=new Dictionary<byte,object>();
        //paramreters.Add((byte)ParameterCode.InventoryItemdb,iidb);
        ParameterTool.AddParameters<InventoryItemDB>(paramreters, ParameterCode.InventoryItemdb, iidb);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.AddInventoryItemDB,paramreters);
    }
    public void UpdateInventoryItemDB(InventoryItemDB iidb) {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameters<InventoryItemDB>(parameters, ParameterCode.InventoryItemdb, iidb);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.UpdateInventoryItemDB, parameters);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemDB1">未穿上装备的信息更新</param>
    /// <param name="itemDB2"></param>
    public void UpdateInventoryItemDBList(InventoryItemDB itemDB1, InventoryItemDB itemDB2) {
        List<InventoryItemDB> itemDBList = new List<InventoryItemDB>();
        itemDB1.Role = null;
        itemDB2.Role = null;
        itemDBList.Add(itemDB1);
        itemDBList.Add(itemDB2);
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameters<List<InventoryItemDB>>(parameters, ParameterCode.InventoryItemdbList, itemDBList);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.UpdateInventoryItemDBList, parameters);
    }
    public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response) {
        SubCode subCode = ParameterTool.GetParameters<SubCode>(response.Parameters, ParameterCode.SubCode, false);
        //Debug.Log("InventoryItemDB:"+subCode);
        switch(subCode)
        {
            case SubCode.GetInventoryItemDB:
                List<InventoryItemDB> list
                    =ParameterTool.GetParameters<List<InventoryItemDB>>(response.Parameters, ParameterCode.InventoryItemdbList);
                //Debug.Log(list);
                if (list==null||list.Count==0)
                {
                    //Debug.Log("数据库没有装备信息");
                    return;
                } 
                if(OnGetInventoryItemDB!=null)
                {
                    OnGetInventoryItemDB(list);
                }
                break;
            case SubCode.AddInventoryItemDB:
                InventoryItemDB itemDB = ParameterTool.GetParameters<InventoryItemDB>(response.Parameters, ParameterCode.InventoryItemdb);
                if (OnAddInventoryItemDB != null)
                    OnAddInventoryItemDB(itemDB);
                break;
            case SubCode.UpdateInventoryItemDB:
                OnUpdateInventoryItemDB();
                break;
            case SubCode.UpdateInventoryItemDBList:
                if (OnUpdateInventoryItemDBList!=null)
                    OnUpdateInventoryItemDBList();
                break;
        }
    }
    public event OnGetInventoryItemDBEvent OnGetInventoryItemDB;
    public event OnAddInventoryItemDBEvent OnAddInventoryItemDB;
    public event OnUpdateInventoryItemDBEvent OnUpdateInventoryItemDB;
    public event OnUpdateInventoryItemDBListEvent OnUpdateInventoryItemDBList;
}
