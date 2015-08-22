using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public GameObject damageEffectPrefab;
    public int TotalHp = 200;
  
    public float speed = 2;
    public float attackRate = 2;//表示的是敌人的攻击速度
    public float attackDistance = 1;
    public float attack;//敌人的攻击力

    private float attackTimer = 0;//攻击计时器
    private float distance = 0;//当前与player的距离
    private int hp;//保存当前血量

    //死亡后；
    public float downSpeed = 2.0f;
    public float downDistance = 4;

    private Transform bloodPoint;
    private Transform hpPoint;
    private CharacterController cc;
    private GameObject hpBar;
    private GameObject damageHudText;

    private UISlider hpSlider;
    private HUDText hudText;
    private MeshExploder me;
 

    
    void Start()
    {
        bloodPoint = transform.Find("BloodPoint").transform;
        hpPoint = transform.Find("HpPoint").transform;
        cc = this.GetComponent<CharacterController>();
        InvokeRepeating("GetCurrentDistance", 0, 0.1f);
        attackTimer = attackRate;
        hp = TotalHp;
        hpBar = HpBarManager._instance.GetHpBar(hpPoint.gameObject);
        damageHudText = HpBarManager._instance.GetHudText(hpPoint.gameObject);

        hpSlider = hpBar.GetComponentInChildren<UISlider>();
        hudText = damageHudText.GetComponent<HUDText>();
        me = gameObject.GetComponentInChildren<MeshExploder>();
    }
    void Update()
    {
        if (hp <= 0)
        {
            if (downSpeed * Time.deltaTime*100>=downDistance)
            {
                Destroy(this.gameObject);
            }
            transform.Translate(-transform.up * downSpeed*Time.deltaTime);
            return;
        }
        if(distance<=attackDistance)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer>=attackRate)
            {
               // if(!animation.IsPlaying("takedamage"))
               // {
                    //面向主角
                    Transform player = TranscriptManager._instance.player.transform;
                    Vector3 targetPos = player.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);
                    animation.Play("attack01");
                    attackTimer = 0;
                //}
            }
            if(!animation.IsPlaying("attack01"))
            {
                animation.Play("idle");
            }
        }
        else
        {
            animation.Play("walk");
            Move();
        }
       
    }
    void GetCurrentDistance()
    {
        distance = Vector3.Distance(TranscriptManager._instance.player.transform.position, transform.position);
    }
    void Move()
    {
        Transform player = TranscriptManager._instance.player.transform;
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
        cc.SimpleMove(transform.forward * speed);
    }

    void Attack()
    {
        TranscriptManager._instance.player.transform.SendMessage("TakeDamageByEnemy",attack,SendMessageOptions.DontRequireReceiver);
    }
    //0受到多少伤害
    //1 后退距离
    //3 浮空高度
    void TakeDamage(string args)
    {
        if (hp <= 0) return;

        Combo._instance.ShowConboPlus();

        string[] proArray = args.Split(',');
        int damage = int.Parse(proArray[0]);
        hp -= damage;
        //血量显示
        hpSlider.value=(float)hp/TotalHp;
        //伤害显示
        hudText.Add("-" + damage, Color.red, 0.2f);
        //播放攻击动画
        animation.Play("takedamage");

        float backdistance = float.Parse(proArray[1]);
        float jumpHeight =0;

        iTween.MoveBy(this.gameObject,
            transform.InverseTransformDirection(TranscriptManager._instance.player.transform.forward) * backdistance + Vector3.up * jumpHeight,
             0.3f);
        GameObject.Instantiate(damageEffectPrefab, bloodPoint.transform.position, Quaternion.identity);

        if(hp<=0)
        {
            Dead();
        }
    }
    void Dead()
    {

        cc.enabled = false;
        Destroy(hpBar);
        Destroy(damageHudText);
        TranscriptManager._instance.enemyList.Remove(this.gameObject);
        int randomNum = Random.Range(0, 10);
        if(randomNum<5)
        {
            //播放死亡动画
             animation.Play("die");
             downSpeed = 0.3f;
        }
        else
        {
            //死亡碎片
            downSpeed = 10.0f;
            me.Explode();
        }
    }
}
