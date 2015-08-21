using UnityEngine;
using System.Collections;

public class PlayerAnimationControll : MonoBehaviour {
    private Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }
	public void OnAttackButtonClick(bool isPress,PosType posType)
	{
        if (posType == PosType.Basic)
        {
            if (isPress)
            {
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            switch (posType)
            {
                case PosType.One:
                    if (isPress)
                        anim.SetBool("skill1", true);
                    else
                        anim.SetBool("skill1", false);
                    break;
                case PosType.Two:
                    if (isPress)
                        anim.SetBool("skill2", true);
                    else
                        anim.SetBool("skill2", false);
                    break;
                case PosType.Three:
                    if (isPress)
                        anim.SetBool("skill3", true);
                    else
                        anim.SetBool("skill3", false);
                    break;
            }
        } 
    }
}
