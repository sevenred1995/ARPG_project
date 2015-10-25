using UnityEngine;
using System.Collections;

public class PlayerAnimationControll : MonoBehaviour {
    private Animator anim;
    private Player player;
    private BattleController battleController;
    private bool isSyncPlayerAnimation = false;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        player = GetComponent<Player>();
        if (GameManger._instance.battleType == BattleType.Team && PhotonEngine.Instance.role.ID == player.roleID)
        {
            battleController = GameManger._instance.GetComponent<BattleController>();
            isSyncPlayerAnimation = true;
        }
    }
	public void OnAttackButtonClick(bool isPress,PosType posType)
	{
        if (posType == PosType.Basic)
        {
            if (isPress)
            {
                anim.SetTrigger("Attack");
                if(isSyncPlayerAnimation)
                    battleController.SyncPlayerAnimation(new PlayerAnimationModel() { attack = true });
            }
        }
        else
        {
            switch (posType)
            {
                case PosType.One:
                    if (isPress){
                        anim.SetBool("skill1", true);
                        if (isSyncPlayerAnimation)
                        {
                            battleController.SyncPlayerAnimation(new PlayerAnimationModel() { skill1 = true });
                        }
                    }
                        
                    else
                    {
                        anim.SetBool("skill1", false);
                        if (isSyncPlayerAnimation)
                        {
                            PlayerAnimationModel model = new PlayerAnimationModel() {};
                            battleController.SyncPlayerAnimation(model);
                        }
                    }
                    break;
                case PosType.Two:
                    if (isPress){
                        anim.SetBool("skill2", true);
                        if (isSyncPlayerAnimation)
                        {
                            battleController.SyncPlayerAnimation(new PlayerAnimationModel() { skill2 = true });
                        }
                    }
                    else
                    {
                        anim.SetBool("skill2", false);
                        if (isSyncPlayerAnimation)
                        {
                            PlayerAnimationModel model = new PlayerAnimationModel() {};
                            battleController.SyncPlayerAnimation(model);
                        }
                    }
                    break;
                case PosType.Three:
                    if (isPress)
                    {
                        anim.SetBool("skill3", true);
                        if (isSyncPlayerAnimation)
                        {
                            PlayerAnimationModel model = new PlayerAnimationModel() { skill3 = true };
                            battleController.SyncPlayerAnimation(model);
                        }
                    }
                    else
                    {
                        anim.SetBool("skill3", false);
                        if (isSyncPlayerAnimation)
                        {
                            PlayerAnimationModel model = new PlayerAnimationModel() { };
                            battleController.SyncPlayerAnimation(model);
                        }
                    }   
                    break;
            }
        } 
    }

    public void SetAnimation(PlayerAnimationModel model) {
        if (model.attack)
        {
            anim.SetTrigger("Attack");
        }
        else if (model.takedamage)
        {
            anim.SetTrigger("takedamage");
        }
        anim.SetBool("skill1", model.skill1);
        anim.SetBool("skill2", model.skill2);
        anim.SetBool("skill3", model.skill3);
    }
}
