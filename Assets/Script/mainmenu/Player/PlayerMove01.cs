using UnityEngine;
using System.Collections;

public class PlayerMove01 : MonoBehaviour {
	///在副本中实现对角色的控制移动
	public float velocity=5;
	
	private Animator anim;
	void Awake()
	{
		anim=this.GetComponent<Animator>();
	}
	
	void Update()
	{
		float h=Input.GetAxis("Horizontal");
		float v=Input.GetAxis("Vertical");
		
		Vector3 nowvel = rigidbody.velocity; 
		if (Mathf.Abs (h) > 0.05 || Mathf.Abs (v) > 0.05) {
            anim.SetBool("move", true);
            if (anim.GetCurrentAnimatorStateInfo(1).IsName("Empty State"))
            {
                rigidbody.velocity = new Vector3(velocity * h, nowvel.y, velocity * v);
                transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
            }
            else
                rigidbody.velocity = new Vector3(0, nowvel.y, 0);
        }
        else
        {
            rigidbody.velocity = new Vector3(0, nowvel.y, 0);
            anim.SetBool("move", false);
        }			
	}
}
