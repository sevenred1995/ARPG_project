using UnityEngine;
using System.Collections;

public class DemonController : MonoBehaviour {

	private GameObject  demon;
	private CharacterController controller;
	private Vector3 moveDirection;
	
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;	
		EasyTouch.On_TouchStart += On_TouchStart;	
	}
	
	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;	
		EasyTouch.On_TouchStart -= On_TouchStart;		
	}
	
	void Start () {
	
		demon = GameObject.Find("demon").gameObject;
		controller = demon.GetComponent<CharacterController>();
	}
	
	void Update(){
	
		if (EasyTouch.GetTouchCount()==0)
			demon.animation.CrossFade ("idle");
		
		if (!controller.isGrounded){
			demon.animation.CrossFade ("jump");
			moveDirection.y -= 5 * Time.deltaTime;
		}
		
		controller.Move(moveDirection * Time.deltaTime);
		moveDirection = new Vector3(0,moveDirection.y,0);
	}
	
	
	
	void On_TouchDown( Gesture gesture){
		
		GameObject pickedObject = EasyTouch.GetCurrentPickedObject( gesture.fingerIndex);
		// if something is picked
		if (pickedObject!=null){
			// test the object name Right
			if (pickedObject.name == "Right"){
				demon.transform.localEulerAngles = new Vector3(0,90f,0);
				moveDirection.x = 0.7f;
				demon.animation.CrossFade ("walk");
			}
			// test the object name Lefy
			else if (pickedObject.name == "Left"){
				demon.transform.localEulerAngles = new Vector3(0,-90f,0);
				moveDirection.x = -0.7f;
				demon.animation.CrossFade ("walk");
			}
		}
	}
	
	void On_TouchUp(Gesture gesture){
		// clean up
		moveDirection = new Vector3(0,moveDirection.y,0);
	}
	
	
	void On_TouchStart( Gesture gesture){
	
		// Jump ?
		if (gesture.pickObject!=null){
			if (controller.isGrounded){
				if (gesture.pickObject.name == "Up"){
					moveDirection.y = 3;
				}
			}	
		}
	}
}
