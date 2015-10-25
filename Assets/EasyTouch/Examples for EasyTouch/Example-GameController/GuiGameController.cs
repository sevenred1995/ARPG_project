using UnityEngine;
using System.Collections;

public class GuiGameController : MonoBehaviour {

	void OnGUI() {
	            
		GUI.matrix = Matrix4x4.Scale( new Vector3( Screen.width / 1024.0f, Screen.height / 768.0f, 1 ) );
		
		GUI.Box( new Rect( 0, -4, 1024, 30 ), "" );
		GUILayout.Label("Game Controller example");
	
	
		// Back to menu menu
		if (GUI.Button( new Rect(412,30,200,50),"Main menu")){
			Application.LoadLevel("StartMenu");
		}	
	}
}
