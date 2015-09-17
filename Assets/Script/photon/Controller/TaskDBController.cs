using UnityEngine;
using System.Collections;
using TaiDouCommon;
using TaiDouCommon.Model;
using System.Collections.Generic;
using TaiDouCommon.Tools;

public class TaskDBController : ControllerBase {

    public override OperationCode opCode
    {
        get { return OperationCode.TaskDB; }
    }
    public override void Start()
    { 
        base.Start();
    }
    public void GetTaskDBList()
    {
        PhotonEngine.Instance.SendRequest(OperationCode.TaskDB, SubCode.GetTask, new Dictionary<byte, object>());
    }
    public void AddTaskDB(TaskDB taskDB)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        taskDB.Role = null;
        ParameterTool.AddParameters<TaskDB>(parameters, ParameterCode.Taskdb, taskDB);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.AddTask, parameters);
    }
    public void UpdateTaskDB(TaskDB taskDB)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        taskDB.Role = null;
        ParameterTool.AddParameters<TaskDB>(parameters, ParameterCode.Taskdb, taskDB);
        PhotonEngine.Instance.SendRequest(opCode, SubCode.UpdateTask, parameters);
    }
    /// <summary>
    /// 从服务器接接到响应
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
    {
        object subobject;
        response.Parameters.TryGetValue((byte)ParameterCode.SubCode, out subobject);
        SubCode subCode = (SubCode)subobject;
        switch (subCode)
        {
            case SubCode.GetTask:
                List<TaskDB> list = ParameterTool.GetParameters<List<TaskDB>>(response.Parameters, ParameterCode.TaskdbList);
                if (OnGetTaskList != null)
                {
                    OnGetTaskList(list);
                }
                break;
            case SubCode.AddTask:
                TaskDB taskDB = ParameterTool.GetParameters<TaskDB>(response.Parameters, ParameterCode.Taskdb);
                if (OnAddTask != null)
                {
                    if (response.ReturnCode == (short)ReturnCode.Success)
                    {
                        OnAddTask(taskDB);
                    }
                }
                break;
            case SubCode.UpdateTask:
                if (OnUpdateTask != null)
                {
                    if (response.ReturnCode == (short)ReturnCode.Success)
                        OnUpdateTask();
                }
                break;
        }
    }
    public event OnAddTaskEvent OnAddTask;
    public event OnGetTaskListEvent OnGetTaskList;
    public event OnUpdateTaskEvent OnUpdateTask;
}
