using UnityEngine;
using UnityEditor;
using System.Collections;

public class GUIEasyTouchMenu : Editor {

	[MenuItem ("Hedgehog Team/EasyTouch/Add EasyTouch for C#")]
	static void  AddEasyTouch4C(){
		GUIEasyTouchMenu.AddEasyTouch(true);
	}

	[MenuItem ("Hedgehog Team/EasyTouch/Add EasyTouch for javascript")]
	static void  AddEasyTouch4Java(){
		GUIEasyTouchMenu.AddEasyTouch(false);
	}
	
	[MenuItem ("Hedgehog Team/EasyTouch/Extensions/Adding a new joystick")]
	static void  AddJoystick(){
		
		EasyTouch  easyt =(EasyTouch) GameObject.FindObjectOfType(typeof(EasyTouch));
		if (easyt==null){
			GUIEasyTouchMenu.AddEasyTouch(true);	
		}
		else{
			if (easyt.useBroadcastMessage){
				easyt.joystickAddon = true;	
			}
		}
		
		GameObject joy = new GameObject("New joystick");
		joy.AddComponent<EasyJoystick>();
		 Selection.activeGameObject = joy;
	}
	
	static GameObject AddEasyTouch(bool c){
	
		if (GameObject.FindObjectOfType(typeof(EasyTouch))==null){
			GameObject easyTouch = new GameObject("EasyTouch");
			EasyTouch easy = easyTouch.AddComponent<EasyTouch>();
			
			if (c){
				easy.useBroadcastMessage = false;		
			}
			else{
				easy.useBroadcastMessage = true;
			}
			return easyTouch;
		}
		else{
			EditorUtility.DisplayDialog("Warning","EasyTouch is already exist in your scene","OK");
			return null;
		}
		
	}
}
