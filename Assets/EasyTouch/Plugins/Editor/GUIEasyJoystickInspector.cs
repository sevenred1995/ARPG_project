using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EasyJoystick))]
public class GUIEasyJoystickInspector : Editor{
	
	private Texture2D gradientTexture;

	void OnEnable(){
			
		EasyJoystick t = (EasyJoystick)target;
		if (t.areaTexture==null){
			t.areaTexture = (Texture)Resources.Load("RadialJoy_Area");
			EditorUtility.SetDirty(t);
		}
		if (t.touchTexture==null){
			t.touchTexture = (Texture)Resources.Load("RadialJoy_Touch");
			EditorUtility.SetDirty(t);
		}
		if (t.deadTexture==null){
			t.deadTexture =  (Texture)Resources.Load("RadialJoy_Dead");
			EditorUtility.SetDirty(t);
		}
	}
	
	public override void OnInspectorGUI(){
		
		EasyJoystick t = (EasyJoystick)target;
				
		t.showProperties = HTEditorToolKit.DrawTitleFoldOut( t.showProperties,"Joystick properties");
		if (t.showProperties){
			t.enable = EditorGUILayout.Toggle("Enable joystick",t.enable);
			t.useFixedUpdate = EditorGUILayout.Toggle("Use fixed update",t.useFixedUpdate);
			
			HTEditorToolKit.DrawSeparatorLine();
			EditorGUILayout.Separator();
			
			t.zoneRadius = EditorGUILayout.FloatField("Area radius",t.zoneRadius);
			
			t.TouchSize = EditorGUILayout.FloatField("Touch radius",t.TouchSize);
			t.RestrictArea = EditorGUILayout.Toggle("Restrict to area",t.RestrictArea);
			
			t.deadZone =  EditorGUILayout.FloatField("Dead zone radius",t.deadZone);
			
			EditorGUILayout.Separator();
			
			// Dynamic joystick
			t.DynamicJoystick = EditorGUILayout.Toggle("Dynamic joystick",t.DynamicJoystick);
			if (t.DynamicJoystick){
				t.area = (EasyJoystick.DynamicArea) EditorGUILayout.EnumPopup("Free area",t.area);
			}
			else{
				t.joystickPosition = EditorGUILayout.Vector2Field("Joystick position",t.joystickPosition);	
			}
			
			HTEditorToolKit.DrawSeparatorLine();
			
			// Smoothing return
			t.enableSmoothing = EditorGUILayout.Toggle("Smoothing return",t.enableSmoothing);
			if (t.enableSmoothing){
				t.Smoothing = EditorGUILayout.Vector2Field( "Smoothing",t.Smoothing);
			}
			
			HTEditorToolKit.DrawSeparatorLine();
			// Inertie
			t.enableInertia = EditorGUILayout.Toggle("Enable inertia",t.enableInertia);
			if (t.enableInertia){
				t.Inertia = EditorGUILayout.Vector2Field( "Inertia",t.Inertia);
			}
			
			//EditorGUILayout.Separator();
		}
			
		// Interaction

		t.showInteraction =HTEditorToolKit.DrawTitleFoldOut( t.showInteraction,"Interaction");
		if (t.showInteraction){
			
			
			// Joystick speed
			t.interaction = (EasyJoystick.InteractionType)EditorGUILayout.EnumPopup("Interaction type",t.interaction);
			
			if (t.interaction == EasyJoystick.InteractionType.EventNotification || t.interaction == EasyJoystick.InteractionType.DirectAndEvent){
				t.useBroadcast = EditorGUILayout.Toggle("Broadcast messages",t.useBroadcast); 
				if (t.useBroadcast){
					t.ReceiverObjectGame =(GameObject) EditorGUILayout.ObjectField("Receiver objet",t.ReceiverObjectGame,typeof(GameObject),true);
					t.messageMode =(EasyJoystick.Broadcast) EditorGUILayout.EnumPopup("Sending mode",t.messageMode);
				}
			}
			
			HTEditorToolKit.DrawSeparatorLine();
			
			// x Axis
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("X axis speed");
			t.speed.x = EditorGUILayout.FloatField(t.speed.x);
			t.inverseXAxis = EditorGUILayout.Toggle(t.inverseXAxis); 
			EditorGUILayout.LabelField("inverse axis");
			EditorGUILayout.EndHorizontal();
			
			// y Axis
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Y axis speed");
			t.speed.y = EditorGUILayout.FloatField(t.speed.y);
			t.inverseYAxis = EditorGUILayout.Toggle(t.inverseYAxis); 
			EditorGUILayout.LabelField("inverse axis");
			EditorGUILayout.EndHorizontal();
			
			
			HTEditorToolKit.DrawSeparatorLine();
			EditorGUILayout.Separator();
			// Interaction direct
			if (t.interaction == EasyJoystick.InteractionType.Direct || t.interaction == EasyJoystick.InteractionType.DirectAndEvent){
				t.XAxisTransform = (Transform)EditorGUILayout.ObjectField("Joystick X to",t.XAxisTransform,typeof(Transform),true);
				if ( t.XAxisTransform!=null){
					// characterCollider
					if (t.XAxisTransform.GetComponent<CharacterController>() && (t.xTI==EasyJoystick.PropertiesInfluenced.Translate || t.xTI==EasyJoystick.PropertiesInfluenced.TranslateLocal)){
						EditorGUILayout.HelpBox("CharacterController detected",MessageType.Info);
						t.xAxisGravity = EditorGUILayout.FloatField("Gravity",t.xAxisGravity);
					}
					else{
						t.xAxisGravity=0;	
					}
					t.xTI = (EasyJoystick.PropertiesInfluenced)EditorGUILayout.EnumPopup("Influenced",t.xTI);
					t.xAI = (EasyJoystick.AxisInfluenced)EditorGUILayout.EnumPopup("Axis influenced",t.xAI);
					
				}
				EditorGUILayout.Separator();
				HTEditorToolKit.DrawSeparatorLine();
				EditorGUILayout.Separator();
				t.YAxisTransform = (Transform)EditorGUILayout.ObjectField("Joystick Y to",t.YAxisTransform,typeof(Transform),true);
				if (t.YAxisTransform!=null){
					// characterCollider
					if (t.YAxisTransform.GetComponent<CharacterController>() && (t.yTI==EasyJoystick.PropertiesInfluenced.Translate || t.yTI==EasyJoystick.PropertiesInfluenced.TranslateLocal)){
						EditorGUILayout.HelpBox("CharacterController detected",MessageType.Info);
						t.yAxisGravity = EditorGUILayout.FloatField("Gravity",t.yAxisGravity);
					}
					else{
						t.yAxisGravity=0;
					}
					t.yTI = (EasyJoystick.PropertiesInfluenced)EditorGUILayout.EnumPopup("Influenced",t.yTI);
					t.yAI = (EasyJoystick.AxisInfluenced)EditorGUILayout.EnumPopup("Axis influenced",t.yAI);
				}
			}
		}
		
		
		// Joystick Appearance 
		t.showAppearance = HTEditorToolKit.DrawTitleFoldOut( t.showAppearance,"Joystick Appearance");
		if (t.showAppearance){
			t.showZone = EditorGUILayout.Toggle("Show area",t.showZone);
			if (t.showZone){ 
				t.areaTexture = (Texture)EditorGUILayout.ObjectField("Area texture",t.areaTexture,typeof(Texture),true);
			}
			
			t.showTouch = EditorGUILayout.Toggle("Show touch",t.showTouch);
			if (t.showTouch){
				t.touchTexture = (Texture)EditorGUILayout.ObjectField("Area texture",t.touchTexture,typeof(Texture),true);
			}
	
			t.showDeadZone = EditorGUILayout.Toggle("Show touch",t.showDeadZone);
			if (t.showDeadZone){
				t.deadTexture = (Texture)EditorGUILayout.ObjectField("Dead zone texture",t.deadTexture,typeof(Texture),true);
			}
		}
		
		// Refresh
		if (GUI.changed){
		 	EditorUtility.SetDirty(t);
		}
	}
}
