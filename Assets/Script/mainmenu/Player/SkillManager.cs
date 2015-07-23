using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {
    public TextAsset skilltext;
    private List<Skill> skillListInfo = new List<Skill>();

    void Awake()
    {
        InitSkillList();
    }
    public void InitSkillList()
    {
        string str = skilltext.ToString();
        string[] skillArray = str.Split('\n');
        foreach(string sk in skillArray)
        {
            string[] proArray = sk.Split(',');
            Skill skill=new Skill();
            skill.Id=int.Parse(proArray[0]);
            skill.Name=proArray[1];
            skill.Icon = proArray[2];
            switch(proArray[3])
            {
                case "Warrior":
                    skill.PlayerType = PlayerType.Warrior;
                    break;
                case "FemaleAssassin":
                    skill.PlayerType=PlayerType.FemaleAssassin;
                    break;
            }
            switch(proArray[4])
            {
                case "Basic":
                    skill.SkillType=SkillType.Basic;
                    break;
                case "Skill":
                    skill.SkillType = SkillType.Skill;
                    break;
            }
            switch(proArray[5])
            {
                case "Basic":
                    skill.Postype = PosType.Basic;
                    break;
                case "One":
                    skill.Postype = PosType.One;
                    break;
                case "Two":
                    skill.Postype = PosType.Two;
                    break;
                case "Three":
                    skill.Postype = PosType.Three;
                    break;
            }
            skill.ColdTime = int.Parse(proArray[6]);
            skill.Damage = int.Parse(proArray[7]);
        }
    }
}
