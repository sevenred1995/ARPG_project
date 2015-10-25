// HTEditorTool v1.0 (September 2012)
// HTEditorTool library is copyright (c) of Hedgehog Team
// Please send feedback or bug reports to the.hedgehog.team@gmail.com
using UnityEngine;
using System.Collections;
using UnityEditor;

public class HTEditorToolKit{
	
	public static  Texture2D gradientTexture;
	
	#region Helper
	public static bool DrawTitleFoldOut( bool foldOut,string text){
		
		GUIStyle foldOutStyle =  new GUIStyle( EditorStyles.foldout);
		foldOutStyle.fontStyle = FontStyle.Bold;
		
		Color foldTextColor=Color.black;
		if (EditorGUIUtility.isProSkin){
			foldTextColor = new Color( 242f/255f,152f/255f,13f/255f);	
		}
		
		foldOutStyle.onActive.textColor = foldTextColor;
		foldOutStyle.onFocused.textColor = foldTextColor;
		foldOutStyle.onHover.textColor = foldTextColor;
		foldOutStyle.onNormal.textColor = foldTextColor;
		foldOutStyle.active.textColor = foldTextColor;
		foldOutStyle.focused.textColor = foldTextColor;
		foldOutStyle.hover.textColor = foldTextColor;
		foldOutStyle.normal.textColor = foldTextColor;
		
		Rect lastRect = DrawTitleGradient();
		GUI.color = Color.white;
		bool value = EditorGUI.Foldout(new Rect(lastRect.x + 3, lastRect.y+1, lastRect.width - 5, lastRect.height),foldOut,text,foldOutStyle);
		GUI.color = Color.white;
		
		return value;
	}
		
	public static void DrawSeparatorLine()
	{
		
	    GUILayout.Space(10);
        Rect lastRect = GUILayoutUtility.GetLastRect();
		
		GUI.color = Color.gray;
	    GUI.DrawTexture(new Rect(0, lastRect.yMax-0, Screen.width, 1f), EditorGUIUtility.whiteTexture);
		GUI.color = Color.white;
	}
	
	public static Rect DrawTitleGradient()
	{
	    GUILayout.Space(30);
		Rect lastRect = GUILayoutUtility.GetLastRect();
	    lastRect.yMin = lastRect.yMin + 5;
	    lastRect.yMax = lastRect.yMax - 5;
	    lastRect.width =  Screen.width;
		
		
		GUI.DrawTexture(new Rect(0,lastRect.yMin,Screen.width, lastRect.yMax- lastRect.yMin), GetGradientTexture());
		GUI.color = new Color(0.54f,0.54f,0.54f);
		GUI.DrawTexture(new Rect(0,lastRect.yMin,Screen.width,1f),  EditorGUIUtility.whiteTexture);
		GUI.DrawTexture(new Rect(0,lastRect.yMax- 1f,Screen.width,1f),  EditorGUIUtility.whiteTexture);
		
		return lastRect;
	}
	
	private static Texture2D GetGradientTexture(){
		
		if (HTEditorToolKit.gradientTexture==null){
			HTEditorToolKit.gradientTexture = CreateGradientTexture();
		}
		return gradientTexture;
	}
		
	private static Texture2D CreateGradientTexture()
	{
		Texture2D myTexture = new Texture2D(1, 16);
		myTexture.set_name("Gradient Texture by Hedgehog Team");
		myTexture.hideFlags = HideFlags.HideInInspector;
		myTexture.filterMode = FilterMode.Bilinear;
		myTexture.hideFlags = HideFlags.DontSave;
		float start=0.4f;
		float end= 0.8f;
		float step = (end-start)/16;
		Color color = new Color(start,start,start);
		
		Color pixColor = color;
		for (int i = 0; i < 16; i++)
		{
			pixColor = new Color (pixColor.r+step, pixColor.b+step, pixColor.b+step,0.5f);
			myTexture.SetPixel(0, i, pixColor);
		}
		myTexture.Apply();

		return myTexture;
	}
	#endregion
	
	#region Asset tool
	public static bool CreateAssetDirectory(string rootPath,string name){
		string directory = rootPath + "/" +  name;
		if (!System.IO.Directory.Exists(directory)){
			AssetDatabase.CreateFolder(rootPath,name);
			return true;
		}
		return false;
	}

	public static string GetAssetRootPath( string path){
		
		string[] tokens = path.Split('/');
		
		path="";
		for (int i=0;i<tokens.Length-1;i++){
			path+= tokens[i] +"/";
		}
		return path;
	}
	#endregion
	
	
}
