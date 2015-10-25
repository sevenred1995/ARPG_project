using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    private float MoveSpeed = 14.0f;

    private Animator playeranim;
    private NavMeshAgent agent;

    private float minDistance = 5f;

    public GameObject transcriptGo;

    public bool isCanController=true;
    void Start()
    {
        playeranim=this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        transcriptGo = GameManger._instance.player;
    }
    void Update()
    {
        if(!isCanController)
        {
            return;
        }
        float h = this.GetComponent<TouchControl>().joyPositionX;
        float v = this.GetComponent<TouchControl>().joyPositionY;
        Vector3 vel = rigidbody.velocity;
        rigidbody.velocity=new Vector3(-h,vel.y,-v)*MoveSpeed;
        //添加角色停止优化
        if (Mathf.Abs(v) == 0 && Mathf.Abs(h) == 0 && !agent.enabled)
        {
            rigidbody.velocity = Vector3.zero;
        }
        if(rigidbody.velocity.magnitude>0.05f)
        {
            agent.enabled = false;
            transform.rotation = Quaternion.LookRotation(new Vector3(-h, 0, -v));
            playeranim.SetBool("isMove", true);
        }
        else
        {
            playeranim.SetBool("isMove", false);
        }
        if(agent.enabled)
        {
            //transform.rotation = Quaternion.LookRotation(agent.velocity);
            if (agent.remainingDistance > minDistance)
            {
                playeranim.SetBool("isMove", true);
            }
            else if(agent.remainingDistance!=0) 
            {
                TaskManager._instance.OnDestination();
            }
        }
    }
    public void SetPostion(Vector3 targetpos)
    {
        agent.enabled = true;
        agent.stoppingDistance = minDistance;
        agent.SetDestination(targetpos);
    }
}
