using UnityEngine;
using System.Collections;
public class SkillUI : MonoBehaviour {
    private UILabel skillNameLabel;
    private UILabel skillDesLabel;
    private UILabel upgradeBtnLabel;
    private UIButton upgradeBtn;
    private UIButton closeBtn;

    private Skill skill;
    void Awake()
    {
        skillNameLabel = transform.Find("Bg/skillBg/skill_name_Label").GetComponent<UILabel>();
        skillDesLabel=transform.Find("Bg/skillBg/skill_des_Label").GetComponent<UILabel>();
        upgradeBtnLabel = transform.Find("Bg/upgrade-button/Label").GetComponent<UILabel>();
        upgradeBtn = transform.Find("Bg/upgrade-button").GetComponent<UIButton>();
        closeBtn = transform.Find("Bg/btn-close").GetComponent<UIButton>();

        EventDelegate ed = new EventDelegate(this,"OnUpgradeClick");
        upgradeBtn.onClick.Add(ed);

        EventDelegate ed1 = new EventDelegate(this, "OnClose");
        closeBtn.onClick.Add(ed1);

        skillNameLabel.text = "";
        skillDesLabel.text = "";
        upgradeBtnLabel.text = "选择技能";
        upgradeBtn.isEnabled = false;
    }
    void OnSkillClick(Skill skill)
    {
        this.skill = skill;
        UpdateShow();
    }
    void UpdateShow()
    {
        skillNameLabel.text = skill.Name + "LV." + skill.Level;
        skillDesLabel.text = skill.Name + ":当前攻击力伤害为 " + (skill.Level * skill.Damage) + ",下一等级伤害+" + ((skill.Level + 1) * skill.Damage) + ",所需金币数为" + (500 * (skill.Level + 1));
        if (skill.Level < playerInfo._instance.Level)
        {
            upgradeBtn.isEnabled = true;
            upgradeBtnLabel.text = "升级";
        }
        else
        {
            upgradeBtn.isEnabled = false;
            upgradeBtnLabel.text = "已达最大等级";
        }
    }
    void OnUpgradeClick()
    {
        int coin = playerInfo._instance.Coin;
        bool isScuess = playerInfo._instance.GetCoin(500 * (skill.Level + 1));
        if (!isScuess)
        {
            MessageManager._instance.ShowMessage("金币不足，升级失败");
        }
        else
        {
            //增加概率事件
            //TODO
            //
            skill.Level++;//技能等级增加
            skill.Damage += 1;//基础伤害值+1;
            MessageManager._instance.ShowMessage("升级成功，伤害+"+skill.Damage);
            UpdateShow();
        }
    }
    void OnClose()
    {
        this.GetComponent<TweenScale>().PlayReverse();
    }
    IEnumerator Hide(GameObject go)
    {
        yield return new WaitForSeconds(0.4f);
        this.gameObject.SetActive(false);
    }

}
