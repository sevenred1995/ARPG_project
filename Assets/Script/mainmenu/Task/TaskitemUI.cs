using UnityEngine;
using System.Collections;

public class TaskitemUI : MonoBehaviour {
    private UISprite taskTypeSprite;
    private UISprite iconSprite;
    private UILabel nameLabel;
    private UILabel desLabel;
    private UISprite reward1Sprite;
    private UISprite reward2Sprite;
    private UILabel reward1Label;
    private UILabel reward2Label;
    private UIButton battleBtn;
    private UIButton getRewardBtn;
    private UILabel battleLabel;

    private Task task;

    void Awake()
    {
        taskTypeSprite = transform.Find("TaskTypeSprite").GetComponent<UISprite>();
        iconSprite = transform.Find("IconBg/Icon").GetComponent<UISprite>();
        nameLabel = transform.Find("TransName").GetComponent<UILabel>();
        desLabel = transform.Find("Des").GetComponent<UILabel>();
        reward1Label = transform.Find("Reward1Sprite/Reward1Num").GetComponent<UILabel>();
        reward1Sprite = transform.Find("Reward1Sprite").GetComponent<UISprite>();
        reward2Label = transform.Find("Reward2Sprite/Reward2Num").GetComponent<UILabel>();
        reward2Sprite = transform.Find("Reward2Sprite").GetComponent<UISprite>();
        battleBtn = transform.Find("Battle-Button").GetComponent<UIButton>();
        getRewardBtn = transform.Find("GetReward-Button").GetComponent<UIButton>();
        battleLabel = transform.Find("Battle-Button/Label").GetComponent<UILabel>();
        //为点击战斗按钮注册事件
        EventDelegate ed1 = new EventDelegate(this, "OnBattle");
        battleBtn.onClick.Add(ed1);

    }
    public void InitItem(Task t)
    {
        this.task = t;
        task.OnTaskChanged += this.OnTaskChanged;
        UpdateShow();
    }
    void UpdateShow()
    {
        switch (task.TaskType)
        {
            case TaskType.Main:
                taskTypeSprite.spriteName = "pic_主线";
                break;
            case TaskType.Reward:
                taskTypeSprite.spriteName = "pic_奖赏";
                break;
            case TaskType.Daily:
                taskTypeSprite.spriteName = "pic_日常";
                break;
        }
        switch (task.TaskProgress)
        {
            case TaskProgress.NoStart:
                battleBtn.gameObject.SetActive(true);
                getRewardBtn.gameObject.SetActive(false);
                battleLabel.text = "下一步";
                break;
            case TaskProgress.Accept:
                battleBtn.gameObject.SetActive(true);
                getRewardBtn.gameObject.SetActive(false);
                battleLabel.text = "战斗";
                break;
            case TaskProgress.Complete:
                battleBtn.gameObject.SetActive(false);
                getRewardBtn.gameObject.SetActive(true);
                break;
        }
        iconSprite.spriteName = task.Icon;
        nameLabel.text = task.Name;
        desLabel.text = task.Des;
        if (task.Coin == 0 && task.Diamond != 0)
        {
            reward1Sprite.spriteName = "钻石";
            reward1Label.text = "X" + task.Diamond.ToString();
            reward2Sprite.gameObject.SetActive(false);
        }
        else if (task.Coin != 0 && task.Diamond == 0)
        {
            reward1Sprite.spriteName = "金币";
            reward1Label.text = "X" + task.Coin.ToString();
            reward2Sprite.gameObject.SetActive(false);
        }
        else if (task.Coin != 0 && task.Diamond != 0)
        {
            reward1Sprite.spriteName = "金币";
            reward1Label.text = "X" + task.Coin.ToString();
            reward2Sprite.spriteName = "钻石";
            reward2Label.text = "X" + task.Diamond.ToString();
        }
    }

    void OnBattle()
    {
        TaskUI._instance.Hide();
        TaskManager._instance.OnExcuteTask(task);
    }
    void OnReward()
    {
        //领取奖励

    }
    void OnTaskChanged()
    {
        UpdateShow();
    }

}
