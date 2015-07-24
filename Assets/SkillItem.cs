using UnityEngine;
using System.Collections;

public class SkillItem : MonoBehaviour {
    public PosType postype;
    private UISprite skillsprite;
    private UIButton skillButton;
    private Skill skill;
    private UISprite SkillSprite
    {
        get
        {
            if(skillsprite==null)
                skillsprite = this.GetComponent<UISprite>();
            return skillsprite;
        }
    }
    private UIButton SkillButton
    {
        get
        {
            if(skillButton==null)
            {
                skillButton = this.GetComponent<UIButton>();
            }
            return skillButton;
        }
    }
    
    void Start()
    {
        UpdateShow();
    }
    void UpdateShow()
    {
        skill = SkillManager._instance.GetSkillByPos(postype);
        SkillSprite.spriteName = skill.Icon;
        SkillButton.normalSprite = skill.Icon;
    }
    void OnPress(bool isPress)
    {
        if(isPress)
        {
            this.transform.parent.parent.parent.SendMessage("OnSkillClick", skill);
        }
    }
}
