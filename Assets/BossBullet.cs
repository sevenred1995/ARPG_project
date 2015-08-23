using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BossBullet : MonoBehaviour {
    public float moveSpeed = 3;
    public float repeatRate = 0.5f;
    public int Damage
    {
        set;
        private get;
    }
    private List<GameObject> playerList = new List<GameObject>();//攻击对象

    void Start()
    {
        InvokeRepeating("Attack", 0, repeatRate);
        StartCoroutine(destory());
    }
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if(playerList.IndexOf(other.gameObject)<0)
            {
                playerList.Add(other.gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (playerList.IndexOf(other.gameObject) >= 0)
            {
                playerList.Remove(other.gameObject);
            }
        }
    }
    void Attack()
    {
        foreach(GameObject player in playerList)
        {
            player.SendMessage("TakeDamageByEnemy", Damage*repeatRate, SendMessageOptions.DontRequireReceiver);
        }
    }

    IEnumerator destory()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
    
}
