using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
    public float viewAngle=60f;
    public float rotateSpeed = 0.5f;
    public float attackDistance = 5;
    public float moveSpeed = 2;

    private float distance;
    private Transform player;

    void Start()
    {
        player = TranscriptManager._instance.player.transform;
        InvokeRepeating("GetDistance", 0, 0.1f);
    }

    void Update()
    {
        Vector3 playerPos = player.position;
        playerPos.y = transform.position.y;
        float angle = Vector3.Angle(playerPos-transform.position,transform.forward);
        if(angle<30)
        {
            if(distance<=attackDistance)
            {
                //进行攻击
            }
            else
            {
                animation.Play("walk");
                rigidbody.MovePosition(transform.position + transform.forward * moveSpeed*Time.deltaTime);
            }
        }
        else
        {
            animation.Play("walk");
            Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }
    void GetDistance()
    {
        distance = Vector3.Distance(player.position, transform.position);
    }

}
