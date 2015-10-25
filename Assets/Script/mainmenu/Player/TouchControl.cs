using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour {
    public float joyPositionX=0;
    public float joyPositionY = 0;

    public float speed = 7;
    //private Animator anim;
    
    void Awake()
    {
        //anim = transform.GetComponent<Animator>();
    }
    void OnEnable() 
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }
    void OnJoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystickName == "Myjoystick")
        {
            joyPositionX = 0;
            joyPositionY = 0;
        }
    }
    //移动摇杆中  
    void OnJoystickMove(MovingJoystick move)
    {
        //获取摇杆中心偏移的坐标  
        joyPositionX = move.joystickAxis.x;
        joyPositionY = move.joystickAxis.y;
        if (move.joystickName == "Myjoystick")
        {
            if (joyPositionY != 0 || joyPositionX != 0)
            {    
            }    
        }
    }  
}
