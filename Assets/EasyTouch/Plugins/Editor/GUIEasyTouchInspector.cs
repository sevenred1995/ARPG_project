using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EasyTouch))]
public class GUIEasyTouchInspector : Editor {
	
	public override void OnInspectorGUI(){
			
		EasyTouch t = (EasyTouch)target;
		
		t.showGeneral = HTEditorToolKit.DrawTitleFoldOut( t.showGeneral,"General properties");
		if (t.showGeneral){
			t.enable = EditorGUILayout.Toggle("Enable EasyTouch",t.enable);
			t.enableRemote = EditorGUILayout.Toggle("Enable unity remote",t.enableRemote);
			t.useBroadcastMessage = EditorGUILayout.Toggle("Broadcast messages",t.useBroadcastMessage);
			
			if (t.useBroadcastMessage){
				t.joystickAddon = EditorGUILayout.Toggle("Joystick Add-on",t.joystickAddon);	
			}
		}
			
		if (t.enable){
			
			// Auto select porperties
			 t.showSelect = HTEditorToolKit.DrawTitleFoldOut( t.showSelect,  "Auto-select properties");
			if (t.showSelect){
				t.autoSelect = EditorGUILayout.Toggle("Enable auto-select",t.autoSelect);
				
				if (t.autoSelect){
					serializedObject.Update();
			   		EditorGUIUtility.LookLikeInspector();
			    	SerializedProperty layers = serializedObject.FindProperty("pickableLayers");
					EditorGUILayout.PropertyField( layers,true);
			   		serializedObject.ApplyModifiedProperties();
					EditorGUIUtility.LookLikeControls();
				}
			}
				
			// General gesture properties
			t.showGesture = HTEditorToolKit.DrawTitleFoldOut(t.showGesture, "General gesture properties");
				if (t.showGesture){
				t.StationnaryTolerance = EditorGUILayout.FloatField("Stationary tolerance",t.StationnaryTolerance);
				t.longTapTime = EditorGUILayout.FloatField("Long tap time",t.longTapTime);
				t.swipeTolerance = EditorGUILayout.FloatField("Swipe tolerance",t.swipeTolerance);
			}
			
			// Two fingers gesture
			t.showTwoFinger = HTEditorToolKit.DrawTitleFoldOut(t.showTwoFinger, "Two fingers gesture properties");
			if (t.showTwoFinger){
				t.enable2FingersGesture = EditorGUILayout.Toggle("2 fingers gesture",t.enable2FingersGesture);
		
				if (t.enable2FingersGesture){
					EditorGUILayout.Separator();
					t.enablePinch = EditorGUILayout.Toggle("Enable Pinch",t.enablePinch);
					if (t.enablePinch){
						t.minPinchLength = EditorGUILayout.FloatField("Min pinch length",t.minPinchLength);
					}
					EditorGUILayout.Separator();
					t.enableTwist = EditorGUILayout.Toggle("Enable twist",t.enableTwist);
					if (t.enableTwist){
						t.minTwistAngle = EditorGUILayout.FloatField("Min twist angle",t.minTwistAngle);
					}
				}
			}
			
		}		
		
	}
}
