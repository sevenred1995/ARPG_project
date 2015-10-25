using UnityEngine;
using System.Collections;

public class TouchLayer : MonoBehaviour {
	
	private TextMesh textMesh;
	
	// Subscribe to events
	void OnEnable(){
		EasyTouch.On_TouchStart += On_TouchStart;
	}
	
	void OnDisable(){
		EasyTouch.On_TouchStart -= On_TouchStart;
	}
	
	void OnDestroy(){
		EasyTouch.On_TouchStart -= On_TouchStart;
	}
	

	void Start () {
		textMesh =(TextMesh)GameObject.Find("TouchOnLayer").transform.gameObject.GetComponent("TextMesh");
	}
	
	// At the touch beginning 
	public void On_TouchStart(Gesture gesture){
		
		// Verification that the action on the object
		if (gesture.pickObject !=null){
			gesture.pickObject.renderer.material.color = new Color( Random.Range(0.0f,1.0f),  Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			textMesh.text = "Touch a sphere on layer :" + LayerMask.LayerToName( gesture.pickObject.layer);
		}
		else{
			textMesh.text = "Touch a sphere on layer :";	
		}
			
	}
}
