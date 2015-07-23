using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    private float MoveSpeed = 15.0f;

    private Animator playeranim;
    private NavMeshAgent agent;

    private float minDistance = 10f;
    void Awake()
    {
        playeranim=this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 vel = rigidbody.velocity;
        rigidbody.velocity=new Vector3(-h,vel.y,-v)*MoveSpeed;
        if(rigidbody.velocity.magnitude>0.5f)
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
