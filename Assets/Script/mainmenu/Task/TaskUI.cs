using UnityEngine;
using System.Collections;

public class TaskUI : MonoBehaviour {
    private UIGrid taskListGrid;
    private GameObject taskItemPrefab;
    private UIButton closeBtn;

    private TweenScale tween;
    void Awake()
    {
        taskListGrid = transform.Find("Scroll View/Grid").GetComponent<UIGrid>();
        taskItemPrefab = Resources.Load<GameObject>("Task-Item");
        closeBtn = transform.Find("btn-close").GetComponent<UIButton>();
        tween = this.GetComponent<TweenScale>();
        EventDelegate ed = new EventDelegate(this, "On_TaskUI_Close_Click");
        closeBtn.onClick.Add(ed);
        EventDelegate ed1 = new EventDelegate(this,"OnTweenFinish");
        tween.onFinished.Add(ed1);
    }
    void Start()
    {
        InitTaskList();
    }
    /// <summary>
    /// 初始化任务面板的任务列表
    /// </summary>
    public void InitTaskList()
    {

        foreach(Task task in TaskManager._instance.GetTaskList())
        {
            GameObject go = NGUITools.AddChild(taskListGrid.gameObject, taskItemPrefab);
            taskListGrid.AddChild(go.transform);
            TaskitemUI taskitemUI = go.GetComponent<TaskitemUI>();
            taskitemUI.InitItem(task);
        }
    }
    
    void On_TaskUI_Close_Click()
    {
        tween.PlayReverse();
    }
    void OnTweenFinish()
    {
        if (transform.localScale.x ==0)
        {
            gameObject.SetActive(false);
        }
    }
    
}
