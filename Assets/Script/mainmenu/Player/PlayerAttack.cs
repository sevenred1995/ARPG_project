using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerAttack : MonoBehaviour {
    private Dictionary<string, PlayerAttackEffect> effectDic = new Dictionary<string, PlayerAttackEffect>();
    public PlayerAttackEffect[] playerEffectArray;
    private float distanceAttackForward = 1.4f;
    private float distanceAttackAround = 6f;
    private int[] damageArray = new int[] { 20, 30, 30, 30 };
    public  int hp = 1000;//表示的是角色当前血量
    private Animator anim;
    private GameObject hudText;
    private GameObject hpPoint;
    private Player player;
    private BattleController battleController;
    private bool isSyncPlayerAnimation = false;
    public enum AttackRange
    {
        Forward,
        Around
    }
    void Start()
    {
        player = GetComponent<Player>();
        if(GameManger._instance.battleType==BattleType.Team&&PhotonEngine.Instance.role.ID==player.roleID)
        {
            battleController = GameManger._instance.GetComponent<BattleController>();
            isSyncPlayerAnimation = true;
        }
        hp = playerInfo._instance.HP;
        PlayerAttackEffect[] peArray=transform.GetComponentsInChildren<PlayerAttackEffect>();
        foreach(PlayerAttackEffect pe in peArray)
        {
            effectDic.Add(pe.name, pe);
        }
        foreach (PlayerAttackEffect pe in playerEffectArray)
        {
            effectDic.Add(pe.name, pe);
        }
        anim = this.GetComponent<Animator>();
        hpPoint = transform.Find("HpPoint").gameObject;
        hudText = HpBarManager._instance.GetHudText(hpPoint);
        
    }

    //0 normal,skill1,skill2,skill3
    //1 effectname
    //2 soundname
    //3 movefoward
    //4 jump height
    void Attack(string args)
    {
        string[] proArray = args.Split(',');
        string effectName = proArray[1];
        ShowAttackEffect(effectName);

        string soundName = proArray[2];
        SoundManager._instance.Play(soundName);

        float movefoward = float.Parse(proArray[3]);
        if(movefoward>=0.1f)
        {
            iTween.MoveBy(this.gameObject,Vector3.forward*movefoward,0.3f);
        }
        //发送攻击消息给敌人
        if(proArray[0]=="normal")
        {
            ArrayList enemyArray = GetEnemyInAttackRanage(AttackRange.Forward);
            foreach(GameObject go in enemyArray)
            {
                go.SendMessage("TakeDamage",damageArray[0]+","+proArray[3]+","+proArray[4],SendMessageOptions.DontRequireReceiver);
            }
        }else if(proArray[0]=="skill1")
        {
            ArrayList enemyArray = GetEnemyInAttackRanage(AttackRange.Around);
            foreach (GameObject go in enemyArray)
            {
                go.SendMessage("TakeDamage", damageArray[1] + "," + proArray[3] + "," + proArray[4]);
            }
        }
    }
    //0 skill1 skill2 skill3
    //1 movefoward
    //2 jumpheight
    void SkillAttack(string args)
    {
        string[] proArray = args.Split(',');
        string type = proArray[0];
        if(type=="skill2")
        {
            ArrayList enemyArray = GetEnemyInAttackRanage(AttackRange.Around);
            foreach (GameObject go in enemyArray)
            {
                go.SendMessage("TakeDamage", damageArray[2] + "," + proArray[1] + "," + proArray[2]);
            }
        }else if(type=="skill3")
        {
            ArrayList enemyArray = GetEnemyInAttackRanage(AttackRange.Forward);
            foreach (GameObject go in enemyArray)
            {
                go.SendMessage("TakeDamage", damageArray[3] + "," + proArray[1] + "," + proArray[2]);
            }
        }
    }

    void PlayAudio(string soundName)
    {
        SoundManager._instance.Play(soundName);
    }

    public void ShowAttackEffect(string effectName)
    {
        PlayerAttackEffect pe=null;
        effectDic.TryGetValue(effectName, out pe);
        if(pe!=null)
        {
            pe.ShowEffect();
        }
    }
   void PlayEffectDevilHand()
    {
        string effectName = "DevilHandMobile";
        PlayerAttackEffect pe;
        effectDic.TryGetValue(effectName, out pe);
        ArrayList enemyArray = GetEnemyInAttackRanage(AttackRange.Forward);
        foreach(GameObject go in enemyArray)
        {
            RaycastHit hit;
            bool collider = Physics.Raycast(go.transform.position + Vector3.up, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));
            if(collider)
            {
                GameObject.Instantiate(pe.transform.gameObject, hit.point, Quaternion.identity);
            }
        }
    }
   void ShowEffectOnTarget(string effectName)
   {
       PlayerAttackEffect pe;
       effectDic.TryGetValue(effectName, out pe);
       ArrayList enemyArray = GetEnemyInAttackRanage(AttackRange.Around);
       foreach (GameObject go in enemyArray)
       {
           RaycastHit hit;
           bool collider = Physics.Raycast(go.transform.position + Vector3.up, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));
           if (collider)
           {
               GameObject.Instantiate(pe.transform.gameObject, hit.point, Quaternion.identity);
           }
       }
   }

    void ShowEffectSelfToTarget(string effectName)
    {
        PlayerAttackEffect pe;
        effectDic.TryGetValue(effectName, out pe);
        ArrayList enemyArray = GetEnemyInAttackRanage(AttackRange.Around);
     
        foreach(GameObject go in enemyArray)
        {
            //Debug.Log(effectName);
            PlayerAttackEffect goEffect = GameObject.Instantiate(pe) as PlayerAttackEffect;
            goEffect.transform.position = transform.position+Vector3.up;
            goEffect.transform.GetComponent<EffectSettings>().Target = go;
        }
    }
    


    ArrayList GetEnemyInAttackRanage(AttackRange attackRange)
    {
        ArrayList arrList = new ArrayList();
        if(attackRange==AttackRange.Forward)
        {
            foreach(GameObject go in TranscriptManager._instance.GetEnemyList())
            {
                Vector3 pos=transform.InverseTransformPoint(go.transform.position);
                 if(pos.z>0)
                 {
                     float distance = Vector3.Distance(Vector3.zero, pos);
                     if(distance<distanceAttackForward)
                     {
                         arrList.Add(go);
                     }
                 }
            }
        }
        else 
        {
            foreach (GameObject go in TranscriptManager._instance.GetEnemyList())
            {

                float distance = Vector3.Distance(transform.position,go.transform.position);
                if (distance < distanceAttackAround)
                {
                    arrList.Add(go);
                }        
            }
        }
        return arrList;
    }

    void TakeDamageByEnemy(int damage)
    {
        if (hp <= 0)
        {
            Dead();
            return;
        }
        hp -= damage;
        //播放受伤动画
        PlayerBarInTranscript._instance.Show(hp);
        int randomNum = Random.Range(0, 100);
        if(randomNum<damage)
        {
            anim.SetTrigger("takedamage");
            if(isSyncPlayerAnimation)
            {
                PlayerAnimationModel model = new PlayerAnimationModel() { takedamage = true };
                battleController.SyncPlayerAnimation(model);
            }
        }
        //显示血量减少
        hudText.GetComponent<HUDText>().Add("-" + damage, Color.red, 0.2f);
        BloodScene._instance.showBloodScene();
    }
    void Dead() {
        anim.SetBool("die", true);
        if (isSyncPlayerAnimation)
        {
            PlayerAnimationModel model = new PlayerAnimationModel() { die = true };
            battleController.SyncPlayerAnimation(model);
        }
        this.GetComponent<PlayerMove01>().enabled = false;
    }
}
