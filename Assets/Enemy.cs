using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public GameObject damageEffectPrefab;
    public int TotalHp = 200;

    public float attackRate = 2;//表示的是敌人的攻击速度
    public float attackDistance = 1;
    private float distance = 0;
    private float attackTimer = 0;

    //死亡后下落的速度；
    public float downSpeed = 1.5f;
    public float downDistance = 4;

    private Transform bloodPoint;
    private Transform hpPoint;
    private CharacterController cc;

    private GameObject hpBar;

    private int hp;

    public float speed = 2;
    void Awake()
    {
        bloodPoint = transform.Find("BloodPoint").transform;
        hpPoint = transform.Find("HpPoint").transform;
        cc = this.GetComponent<CharacterController>();
        InvokeRepeating("GetCurrentDistance", 0, 0.1f);
        attackTimer = attackRate;
        hp = TotalHp;
        hpBar = HpBarManager._instance.GetHpBar(hpPoint.gameObject);
    }
    void Update()
    {
        if (hp <= 0)
        {
            if (downDistance < downSpeed * Time.deltaTime)
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
                if(!animation.IsPlaying("takedamage"))
                {
                    //面向主角
                    Transform player = TranscriptManager._instance.player.transform;
                    Vector3 targetPos = player.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);

                    animation.Play("attack01");
                    attackTimer = 0;
                }
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
        hpBar.GetComponentInChildren<UISlider>().value=(float)hp/TotalHp;
        //播放攻击动画
        animation.Play("takedamage");

        float backdistance = float.Parse(proArray[1]);
        float jumpHeight = float.Parse(proArray[2]);

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
        animation.Play("die");
        TranscriptManager._instance.enemyList.Remove(this.gameObject);
        Destroy(hpBar);
        Destroy(this.GetComponent<CharacterController>());
    }


}
