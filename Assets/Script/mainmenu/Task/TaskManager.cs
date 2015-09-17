using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TaiDouCommon.Model;

public class TaskManager : MonoBehaviour {
    public static TaskManager _instance;

    //读取配置文件中的内容
    public TextAsset taskInfo;
    private List<Task> TaskList = new List<Task>();
    private Dictionary<int, Task> TaskDict = new Dictionary<int, Task>();
    private Task currentTask;
    private PlayerMove _playerMove;
    private PlayerMove PlayerMove
    {
        get
        {
            if(_playerMove==null)
                _playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
            return _playerMove;
        }
    }

    private TaskDBController taskDBController;

    public event OnAsyncTaskCompleteEvent OnAsyncTaskComplete;
    
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        taskDBController = this.GetComponent<TaskDBController>();
        taskDBController.OnAddTask += this.OnAddTask;
        taskDBController.OnGetTaskList += this.OnGetTaskList;
        taskDBController.OnUpdateTask += this.OnUpdateTask;
        ReadTaskInfo();
        taskDBController.GetTaskDBList();
    }
    public void OnGetTaskList(List<TaskDB> list)
    {
        //if (list == null) return;
        foreach (TaskDB taskdb in list)
        {
            Task temp;
            if (TaskDict.TryGetValue(taskdb.TaskID, out temp))
            {
                temp.AsyncTask(taskdb);
            }
        }

        //TaskUI._instance.InitTaskList();//TaskUI在TaskManager之后执行
        ///event事件没有注册成功。。
        if (this.OnAsyncTaskComplete != null)
        {
            Debug.Log("async");
            OnAsyncTaskComplete();
        }    
        else
        {
            Debug.Log("OnSyncTaskComplete is null");
        }
    }
    public void OnAddTask(TaskDB taskdb)
    {

    }
    public void OnUpdateTask()
    {

    }
    /// <summary>
    /// 初始化任务信息
    /// </summary>
    void ReadTaskInfo()
    {
        string str = taskInfo.ToString();
        string[] taskArray = str.Split('\n');//按行划分
        foreach(string taskStr in taskArray)
        {
            string[] proArry = taskStr.Split('|');
            Task task = new Task();
            task.Id = int.Parse(proArry[0]);
            switch(proArry[1])
            {
                case "Main":
                    task.TaskType = TaskType.Main;
                    break;
                case "Reward":
                    task.TaskType = TaskType.Reward;
                    break;
                case "Daily":
                    task.TaskType = TaskType.Daily;
                    break;
            }
            task.Name = proArry[2];
            task.Icon = proArry[3];
            task.Des = proArry[4];
            task.Diamond = int.Parse(proArry[5]);
            task.Coin = int.Parse(proArry[6]);
            task.TalkNpc = proArry[7];
            task.IdNpc = int.Parse(proArry[8]);
            task.IdTranscript = int.Parse(proArry[9]);
            TaskList.Add(task);
            TaskDict.Add(task.Id, task); 
        }
    }

    public List<Task> GetTaskList()
    {
        return TaskList;
    }
    public void OnExcuteTask(Task task)
    {
        currentTask = task;
        if(task.TaskProgress==TaskProgress.NoStart)
        {
            GameObject tasknpc = NpcManager._instance.GetNpcById(task.IdNpc);
            PlayerMove.SetPostion(tasknpc.transform.position);
        }
    }
    public void OnAcceptTask()
    {
        currentTask.TaskProgress = TaskProgress.Accept;
        //前往副本入口点
        PlayerMove.SetPostion(PlayerMove.transcriptGo.transform.position);
    }
    public void OnDestination()
    {
        //if(currentTask.TaskProgress==TaskProgress.NoStart)
        //{
        //    //currentTask.TaskProgress = TaskProgress.Accept;
        //    NpcDialogUI._instance.Show(currentTask.TalkNpc);
        //}
        //到达入口点//
        TranscriptMap._instance.Show();//战斗直接进入副本
        //if(currentTask.TaskProgress==TaskProgress.Accept)
        //{

        //}
    }
}
