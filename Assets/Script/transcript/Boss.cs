using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

    /// <summary>
    /// Boss AI
    /// Boss Attack
    /// </summary>
    public float hp = 1000000;
    public float viewAngle=60f;
    public float rotateSpeed = 0.5f;
    public float attackDistance = 5;
    public float moveSpeed = 2;
    public float attackTimeInterval=1;
    public string guid;
    private int attackIndex = 0;
    private bool isAttack = false;
    private float timer;
    private float distance;
    private Transform player;
    private BossAttackEffect[] atcArray;
    private int[] attackArray=new int[] {30,40,80};
    private GameObject bossBulletPrefab;
    private GameObject attack03Point;
    private Transform bloodPoint;
    public GameObject damageEffectPrefab;
    private Vector3 lastPostion = Vector3.zero;
    private Vector3 lastEulerAnglers = Vector3.zero;
    void Start()
    {
        player = TranscriptManager._instance.player.transform;
        InvokeRepeating("GetDistance", 0, 0.1f);
        atcArray = transform.GetComponentsInChildren<BossAttackEffect>();
        bossBulletPrefab = Resources.Load<GameObject>("BossBullet");
        attack03Point = transform.Find("attack03Point").gameObject;
        bloodPoint = transform.Find("BloodPoint").transform;
        if (GameManger._instance.battleType == BattleType.Team && GameManger._instance.isMaster)
        {
            InvokeRepeating("SyncBossPostionRotation", 0, 1f / 30);
        }
    }

    void Update()
    {
        Vector3 playerPos = player.position;
        playerPos.y = transform.position.y;
        float angle = Vector3.Angle(playerPos-transform.position,transform.forward);
        if ((GameManger._instance.battleType == BattleType.Team && GameManger._instance.isMaster) || GameManger._instance.battleType == BattleType.Person)
        {
            if (angle < (viewAngle / 2))
            {
                if (distance <= attackDistance)
                {
                    //进行攻击
                    if (isAttack == false)//不在播放攻击动画状态
                    {
                        animation.CrossFade("idle");
                        timer += Time.deltaTime;
                        if (timer >= attackTimeInterval)
                        {
                            // Debug.Log("123");
                            timer = 0;
                            BossAttack();
                        }
                    }

                }
                else
                {
                    if (!isAttack)//在播放攻击动画的时候不可以进行转向和行走运动
                    {
                        animation.CrossFade("walk");
                        rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
                    }

                }
            }
            else
            {
                if (!isAttack)
                {
                    animation.Play("walk");
                    Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
                }

            }
        }
    }
    void GetDistance()
    {
        distance = Vector3.Distance(player.position, transform.position);
    }

    void BossAttack()
    {
        isAttack = true;
        attackIndex++;
        animation.CrossFade("attack0"+attackIndex);
        if(attackIndex==3)
        {
            attackIndex = 0;
        }
    }
    void BackToIdle()
    {
        isAttack = false;
    }

    void BossAttackEffect01()
    {
        Vector3 playerPos = player.position;
        playerPos.y = transform.position.y;
        float angle = Vector3.Angle(playerPos - transform.position, transform.forward);
        float currentdistance = Vector3.Distance(playerPos, transform.position);
        if(angle<viewAngle)
        {
            if(distance<attackDistance+1)
            {
                player.SendMessage("TakeDamageByEnemy", attackArray[0], SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void BosssAttackEffect02()
    {
        Vector3 playerPos = player.position;
        playerPos.y = transform.position.y;
        float angle = Vector3.Angle(playerPos - transform.position, transform.forward);
        float currentdistance = Vector3.Distance(playerPos, transform.position);
        if (angle < viewAngle)
        {
            if (distance < attackDistance + 1)
            {
                player.SendMessage("TakeDamageByEnemy", attackArray[1], SendMessageOptions.DontRequireReceiver);

            }
        }
    }

    void BossAttackEffect03()
    {
        if (bossBulletPrefab == null) return;
        GameObject go = GameObject.Instantiate(bossBulletPrefab,attack03Point.transform.position,attack03Point.transform.rotation) as GameObject;
        go.GetComponent<BossBullet>().Damage = attackArray[2];
    }
    void ShowEffect(string effectName)
    {
        
        foreach(BossAttackEffect ae in atcArray)
        {
            if(ae.name==effectName)
            {
                ae.Show();
            }
        }
    }

    void TakeDamage(string args)
    {
        if (hp <= 0) return;

        Combo._instance.ShowConboPlus();

        string[] proArray = args.Split(',');
        int damage = int.Parse(proArray[0]);
        hp -= damage;
        //血量显示
        //伤害显示
        // hudText.Add("-" + damage, Color.red, 0.2f);
        //播放攻击动画
        float backdistance = 0;
        float jumpHeight = 0;

        iTween.MoveBy(this.gameObject,
            transform.InverseTransformDirection(TranscriptManager._instance.player.transform.forward) * backdistance + Vector3.up * jumpHeight,
             0.3f);
        GameObject.Instantiate(damageEffectPrefab, bloodPoint.transform.position, Quaternion.identity);

        if (hp <= 0)
        {
            Dead();
        }
    }
    void Dead()
    {

    }
    void SyncBossPostionRotation() {
        Vector3 postion = transform.position;
        Vector3 eularAnglers = transform.eulerAngles;
        if (postion.x != lastPostion.x || postion.y != lastPostion.y || postion.z != lastPostion.z ||
            eularAnglers.x != lastEulerAnglers.x || eularAnglers.y != lastEulerAnglers.y || eularAnglers.z != lastEulerAnglers.z)
        {
            //TranscriptManager._instance(this);
            TranscriptManager._instance.bossToSync = this;
            lastPostion = postion;
            lastEulerAnglers = eularAnglers;
        }
    }
}
