using UnityEngine;
using System.Collections;

public class EnemySpawnTrigger : MonoBehaviour {
    public GameObject[] enemyPrefabs;//生成的敌人
    public Transform[] spawnPoint;

    public GameObject[] door;

    public float repeateTime = 2f;
    public float time = 0;
    public bool isSpawn = false;
    void Start()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && isSpawn == false)
        {
            StartCoroutine(SpawnEnemy());
            isSpawn = true;
        } 
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
           // this.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(time);
        foreach(GameObject go in enemyPrefabs)
        {
            foreach(Transform t in spawnPoint)
            {
                GameObject enemy=GameObject.Instantiate(go, t.position, Quaternion.identity) as GameObject;
                TranscriptManager._instance.enemyList.Add(enemy);
                yield return new WaitForSeconds(repeateTime);
            }
        }
        yield return new WaitForSeconds(repeateTime);
    }

 
}
