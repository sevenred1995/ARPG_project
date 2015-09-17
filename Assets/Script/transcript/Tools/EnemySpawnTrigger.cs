using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemySpawnTrigger : MonoBehaviour {
    public GameObject[] enemyPrefabs;//生成的敌人
    public Transform[] spawnPoint;
    public GameObject[] door;
    public float repeateTime = 2f;
    public float time = 0;
    public bool isSpawn = false;
    private EnemyController enemyController;
    void Start()
    {
        if (GameManger._instance.battleType == BattleType.Team && GameManger._instance.isMaster)
            enemyController = TranscriptManager._instance.GetComponent<EnemyController>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(GameManger._instance.battleType==BattleType.Person)
        {
            if (other.transform.tag == "Player" && isSpawn == false)
            {
                isSpawn = true;
                StartCoroutine(SpawnEnemy());
            } 
        }
        else if(GameManger._instance.battleType==BattleType.Team)
        {
            if (other.transform.tag == "Player" && isSpawn == false&&GameManger._instance.isMaster)
            {
                isSpawn = true;
                StartCoroutine(SpawnEnemy());
            } 
        }
    }
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(time);
        foreach(GameObject go in enemyPrefabs)
        {
            List<CreateEnemyProperty> list = new List<CreateEnemyProperty>();
            foreach(Transform t in spawnPoint)
            {
                GameObject enemy=GameObject.Instantiate(go, t.position, Quaternion.identity) as GameObject;
                string guid = Guid.NewGuid().ToString();
                if (enemy.GetComponent<Enemy>() != null)
                {
                    enemy.GetComponent<Enemy>().guid = guid;
                }
                else if (enemy.GetComponent<Boss>() != null)
                {
                    enemy.GetComponent<Boss>().guid = guid;
                }
                CreateEnemyProperty enemyProperty = new CreateEnemyProperty() {
                    GUID = guid,
                    PrefabName = go.name, 
                    Postion = new Vector3Obj(t.position)
                };
                list.Add(enemyProperty);
                TranscriptManager._instance.AddEnemy(enemy);
                yield return new WaitForSeconds(repeateTime);
            }
            if(GameManger._instance.battleType==BattleType.Team&&GameManger._instance.isMaster)
            {
                CreateEnemyModel model = new CreateEnemyModel();
                model.list = list;
                enemyController.SendCreateEnemy(model);
            }
        }
        yield return new WaitForSeconds(repeateTime);
    }
}
