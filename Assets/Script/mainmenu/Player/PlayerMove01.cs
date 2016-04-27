using UnityEngine;
using System.Collections;
using System;

public class PlayerMove01 : MonoBehaviour {
	///在副本中实现对角色的控制移动
	public  float velocity=5;
    public  bool isCanController = true;
	private Animator anim;
    private Vector3 lastPostion=Vector3.zero;
    private Vector3 lastEulerAnglers = Vector3.zero;
    private bool isMove = false;
    private DateTime lastdateTime = DateTime.Now;
    private BattleController battleController;
	void Awake()
	{
		anim=this.GetComponent<Animator>();
        battleController = GameManger._instance.GetComponent<BattleController>();
	}
    void Start() {
        if (GameManger._instance.battleType == BattleType.Team && isCanController)//是当前角色
        {
            InvokeRepeating("AsyncPositionAndRotation", 0, 1f / 30);
            InvokeRepeating("AsyncPlayerMoveAnimation", 0, 1f / 30);
        }
    }
	void Update()
	{
        if (!isCanController)
            return;
        float h=0;
        float v=0;
        if(Application.platform==RuntimePlatform.Android)
        {
            h = transform.GetComponent<TouchControl>().joyPositionX;
            v =transform.GetComponent<TouchControl>().joyPositionY;
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
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
    void AsyncPositionAndRotation() {
        Vector3 postion = transform.position;
        Vector3 eularAnglers = transform.eulerAngles;
        if(postion.x!=lastPostion.x||postion.y!=lastPostion.y||postion.z!=lastPostion.z||
            eularAnglers.x!=lastEulerAnglers.x||eularAnglers.y!=lastEulerAnglers.y||eularAnglers.z!=lastEulerAnglers.z)
        {
            battleController.AsyncPostionAndRotation(postion, eularAnglers);//向服务器发起角色信息同步的请求
            lastPostion = postion;
            lastEulerAnglers = eularAnglers;
        }
    }
    public void SetPostionAndRotation(Vector3 pos,Vector3 eulerAnglers) {
        transform.position = pos;
        transform.eulerAngles = eulerAnglers;
    }
    void AsyncPlayerMoveAnimation() {
        if(isMove!=anim.GetBool("move"))
        {
            //动画状态不同步时候
            PlayerMoveAnimationModel model = new PlayerMoveAnimationModel() { IsMove = anim.GetBool("move") };
            model.SetTime(DateTime.Now);
            //向服务器发起动画同步的请求
            battleController.AsyncPlayerMoveAnimation(model);
            isMove = anim.GetBool("move");
        }
    }
    public void SetAnimation(PlayerMoveAnimationModel model) {
        DateTime dt = model.GetTime();
        if(dt>lastdateTime)
        {
            anim.SetBool("move", model.IsMove);
            lastdateTime = dt;
        }
    }
}
