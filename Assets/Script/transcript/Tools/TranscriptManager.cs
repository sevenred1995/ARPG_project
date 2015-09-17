using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TranscriptManager : MonoBehaviour {
	public static TranscriptManager _instance;
	public GameObject player;

    private List<GameObject> enemyList = new List<GameObject>();
    private Dictionary<string, GameObject> enemyDic = new Dictionary<string, GameObject>();
    private List<Enemy> enemyToSyncList = new List<Enemy>();//需要同步的敌人的集合
    private List<Enemy> enemyAnimationToSyncList = new List<Enemy>();
    public Boss bossToSync = null;
    private EnemyController enemyController;
	void Awake()
	{
		_instance=this;
		//player=GameObject.FindGameObjectWithTag("Player");
        enemyController = this.GetComponent<EnemyController>();
        enemyController.OnCreateEnemy += this.OnCreateEnemy;
        enemyController.OnAsyncEnemyPostionRotation += this.OnSyncEnemyPostionRotation;
        enemyController.OnSyncEnemyAnimation += this.OnSyncEnemyAnimation;
	}
    void Start() {
        if (GameManger._instance.battleType == BattleType.Team && GameManger._instance.isMaster)
        {
            InvokeRepeating("SyncEnemyPostionAndRotation", 0, 1f / 30);
            InvokeRepeating("SyncEnemyAnimation", 0, 1f / 30);
        }
    }
    void OnDestory() {
        if(enemyController!=null)
            enemyController.OnCreateEnemy -= this.OnCreateEnemy;
    }
    private void OnCreateEnemy(CreateEnemyModel model) {
        foreach(CreateEnemyProperty propety in model.list)
        {
            GameObject enemyPrefab = Resources.Load<GameObject>("enemy/" + propety.PrefabName);
            GameObject go=GameObject.Instantiate(enemyPrefab, propety.Postion.ToVector3(), Quaternion.identity) as GameObject;
            if (go.GetComponent<Enemy>() != null)
            {
                go.GetComponent<Enemy>().guid = propety.GUID;
            }
            else if (go.GetComponent<Boss>() != null)
            {
                go.GetComponent<Boss>().guid = propety.GUID;
            }
            //TranscriptManager._instance.enemyList.Add(go);//使得角色可以对敌人进行攻击
            AddEnemy(go);
        }
    }
    public void AddEnemy(GameObject enemyObj) {
        enemyList.Add(enemyObj);
        if (enemyObj.GetComponent<Enemy>() != null)
        {
            enemyDic.Add(enemyObj.GetComponent<Enemy>().guid, enemyObj);
        }
        else if (enemyObj.GetComponent<Boss>() != null)
        {
           enemyDic.Add(enemyObj.GetComponent<Boss>().guid, enemyObj);
        }
    }
    public void RemoveEnemy(GameObject enemyObj) {
        if (enemyObj.GetComponent<Enemy>() != null)
        {
            enemyDic.Remove(enemyObj.GetComponent<Enemy>().guid);
        }
        else if (enemyObj.GetComponent<Boss>() != null)
        {
            enemyDic.Remove(enemyObj.GetComponent<Boss>().guid);
        }
        enemyList.Remove(enemyObj);
    }
    public List<GameObject> GetEnemyList() {
        return enemyList;
    }
    public Dictionary<string, GameObject> GetEnmyDic() {
        return enemyDic;
    }
    public void AddEnemyToSync(Enemy enemy) {
        enemyToSyncList.Add(enemy);
    }
    public void AddEnemyAnimationToSync(Enemy enemy) {
        enemyAnimationToSyncList.Add(enemy);
    }
    void SyncEnemyPostionAndRotation() {
        if (enemyToSyncList != null && enemyToSyncList.Count > 0)
        {
            EnemyPostionModel model = new EnemyPostionModel();
            foreach (Enemy enemy in enemyToSyncList)
            {
                EnemyPostionProperty property = new EnemyPostionProperty()
                {
                    guid = enemy.guid,
                    postion = new Vector3Obj(enemy.transform.position),
                    eulerAnglers = new Vector3Obj(enemy.transform.eulerAngles)
                };
                model.list.Add(property);
            }
            if(bossToSync!=null)
            {
                EnemyPostionProperty property = new EnemyPostionProperty()
                {
                    guid = bossToSync.guid,
                    postion = new Vector3Obj(bossToSync.transform.position),
                    eulerAnglers = new Vector3Obj(bossToSync.transform.eulerAngles)
                };
                model.list.Add(property);
            }
            enemyController.AsyncEnemyPostion(model);//导致另外两个客户端掉线。。。。
            enemyToSyncList.Clear();
        }
    }
    void OnSyncEnemyPostionRotation(EnemyPostionModel model) {
        foreach (var property in model.list)
        {
            GameObject enemyGo = null;
            if (enemyDic.TryGetValue(property.guid, out enemyGo))
            {
                enemyGo.transform.position = property.postion.ToVector3();
                enemyGo.transform.eulerAngles = property.eulerAnglers.ToVector3();
            }
        }
    }
    void SyncEnemyAnimation() {
        if(enemyAnimationToSyncList!=null||enemyAnimationToSyncList.Count>0)
        {
            EnemyAnimationModel model = new EnemyAnimationModel();
            foreach (Enemy enemy in enemyAnimationToSyncList)
            {
                EnemyAnimationProperty property = new EnemyAnimationProperty()
                {
                    guid = enemy.guid,
                    isAttack = enemy.animation.IsPlaying("attack01"),
                    isDie = enemy.animation.IsPlaying("die"),
                    isIdle = enemy.animation.IsPlaying("idle"),
                    isTakeDamage = enemy.animation.IsPlaying("takedamage"),
                    isWalk = enemy.animation.IsPlaying("walk")
                };
                model.list.Add(property);
            }
            enemyController.SyncEnemyAnimation(model);
            enemyAnimationToSyncList.Clear();
        }
    }
    private void OnSyncEnemyAnimation(EnemyAnimationModel model) {
        foreach (var property in model.list)
        {
            GameObject enemyGo = null;
            if (enemyDic.TryGetValue(property.guid, out enemyGo))
            {
                Animation anim = enemyGo.GetComponent<Animation>();
                if (property.isWalk)
                {
                    anim.Play("walk");
                }
                else if (property.isIdle)
                {
                    anim.Play("idle");
                }else if(property.isDie)
                {
                    anim.Play("die");
                }else if(property.isTakeDamage)
                {
                    anim.Play("takedamage");
                }else if(property.isAttack)
                {
                    anim.Play("attack01");
                }
            }
        }
    }
}
