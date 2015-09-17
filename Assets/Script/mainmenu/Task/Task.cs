using UnityEngine;
using System.Collections;
using TaiDouCommon.Model;
public enum TaskType
{
    Main,
    Reward,
    Daily
}
public enum TaskProgress
{
    NoStart,
    Accept,
    Complete,
    Reward
}
public class Task{
    private int id;
    private TaskType taskType;
    private string name;
    private string icon;
    private string des;
    private int coin;
    private int diamond;
    private string talkNpc;
    private int idNpc;
    private int idTranscript;
    private TaskProgress taskProgress = TaskProgress.NoStart;
    
    
    public delegate void OnTaskChangeEvent();
    public event OnTaskChangeEvent OnTaskChanged;

    #region getset
    public TaskDB taskDB
    {
        get;
        set;
    }
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public TaskType TaskType
    {
        get { return taskType; }
        set { taskType = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
  
    public string Icon
    {
        get { return icon; }
        set { icon = value; }
    }
  
    public string Des
    {
        get { return des; }
        set { des = value; }
    }
  
    public int Coin
    {
        get { return coin; }
        set { coin = value; }
    }

    public int Diamond
    {
        get { return diamond; }
        set { diamond = value; }
    }

    public string TalkNpc
    {
        get { return talkNpc; }
        set { talkNpc = value; }
    }
    public int IdNpc
    {
        get { return idNpc; }
        set { idNpc = value; }
    }
    public int IdTranscript
    {
        get { return idTranscript; }
        set { idTranscript = value; }
    }

    public TaskProgress TaskProgress
    {
        get { return taskProgress; }
        set 
        {
            if(taskProgress!=value)
            {
                taskProgress = value;
                OnTaskChanged();
            }   
        }
    }
    #endregion 


    public void AsyncTask(TaskDB taskdb)
    {
        this.taskDB = taskdb;
        switch(taskdb.TaskState)
        {
            case TaskState.Accept:
                taskProgress = TaskProgress.Accept;
                break;
            case TaskState.NotStart:
                taskProgress = TaskProgress.NoStart;
                break;
            case TaskState.Complete:
                taskProgress = TaskProgress.Complete;
                break;
            case TaskState.Reward:
                taskProgress = TaskProgress.Reward;
                break;
        }
    }

    public void UpdateTAsk()
    {

    }
}
