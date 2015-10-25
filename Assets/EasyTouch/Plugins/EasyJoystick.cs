// EasyTouch library is copyright (c) of Hedgehog Team
// Please send feedback or bug reports to the.hedgehog.team@gmail.com
using UnityEngine;
using System.Collections;

/// <summary>
/// Release notes:
/// 
/// V1.0 November 2012
/// =============================
/// 	- First release


[ExecuteInEditMode]
/// <summary>
/// Easy joystick allow to quickly create a virtual joystick
/// </summary>
public class EasyJoystick : MonoBehaviour {
	
	#region Delegate
	/// <summary>
	/// Joystick move handler.
	/// </summary>
	public delegate void JoystickMoveHandler(MovingJoystick move);
	/// <summary>
	/// Joystick move end handler.
	/// </summary>
	public delegate void JoystickMoveEndHandler(MovingJoystick move);
	#endregion
	
	#region Event
	/// <summary>
	/// Occurs when on_ joystick move.
	/// </summary>
	public static event JoystickMoveHandler On_JoystickMove;
	/// <summary>
	/// Occurs when on_ joystick move.
	/// </summary>
	public static event JoystickMoveEndHandler On_JoystickMoveEnd;
	#endregion
	
	#region Enumeration
	/// <summary>
	/// Properties influenced by the joystick
	/// </summary>
	public enum PropertiesInfluenced {Rotate, RotateLocal,Translate, TranslateLocal, Scale}
	/// <summary>
	/// Axis influenced by the joystick
	/// </summary>
	public enum AxisInfluenced{X,Y,Z,XYZ}
	/// <summary>
	/// Dynamic area zone.
	/// </summary>
	public enum DynamicArea {FullScreen, Left,Right,Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight};
	/// <summary>
	/// Interaction type.
	/// </summary>
	public enum InteractionType {Direct, Include, EventNotification, DirectAndEvent}
	/// <summary>
	/// Broadcast mode for javascript
	/// </summary>
	public enum Broadcast {SendMessage,SendMessageUpwards,BroadcastMessage }
	
	/// <summary>
	/// Message name.
	/// </summary>
	private enum MessageName{ On_JoystickMove, On_JoystickMoveEnd};
	#endregion
	
	#region Public Joystick return values read only property
	private Vector2 joystickAxis;
	/// <summary>
	/// Gets the joystick axis value between -1 & 1...
	/// </summary>
	/// <value>
	/// The joystick axis.
	/// </value>
	public Vector2 JoystickAxis {
		get {
			return this.joystickAxis;
		}
	}	
	
	private Vector2 joystickValue;
	/// <summary>
	/// Gets the joystick value = joystic axis value * jostick speed * Time.deltaTime...
	/// </summary>
	/// <value>
	/// The joystick value.
	/// </value>
	public Vector2 JoystickValue {
		get {
			return this.joystickValue;
		}
	}
	#endregion
	
	#region public members
	/// <summary>
	/// Enable or disable the joystick.
	/// </summary>
	public bool enable = true;
	/// <summary>
	/// Use fixed update.
	/// </summary>
	public bool useFixedUpdate = false;
	
	/// <summary>
	/// The zone radius size.
	/// </summary>
	public float zoneRadius=100f;
	
	[SerializeField]
	private float touchSize = 30;
	/// <summary>
	/// Gets or sets the size of the touch.
	/// 
	/// </summary>
	/// <value>
	/// The size of the touch.
	/// </value>
	public float TouchSize {
		get {
			return this.touchSize;
		}
		set {
			touchSize = value;
			if (touchSize>zoneRadius/2 && restrictArea){
				touchSize =zoneRadius/2; 	
			}
		}
	}
	
	/// <summary>
	/// The dead zone size. While the touch is in this area, the joystick is considered stalled
	/// </summary> 
	public float deadZone=20;
	
	[SerializeField]
	private bool dynamicJoystick=false;
	/// <summary>
	/// Gets or sets a value indicating whether this is a dynamic joystick.
	/// When this option is enabled, the joystick will display the location of the touch
	/// </summary>
	/// <value>
	/// <c>true</c> if dynamic joystick; otherwise, <c>false</c>.
	/// </value>
	public bool DynamicJoystick {
		get {
			return this.dynamicJoystick;
		}
		set {
			joystickIndex=-1;
			dynamicJoystick = value;
			if (dynamicJoystick){
				virtualJoystick=false;
			}
			else{
				virtualJoystick=true;
				joystickCenter = joystickPosition;
			}
		}
	}
	
	/// <summary>
	/// When the joystick is dynamic mode, this value indicates the area authorized for display
	/// </summary>
	public DynamicArea area = DynamicArea.FullScreen;
	
	/// <summary>
	/// The joystick position on the screen
	/// </summary>
	public Vector2 joystickPosition = new Vector2( 135f,135f);
	
	[SerializeField]
	private bool restrictArea=false;
	/// <summary>
	/// Gets or sets a value indicating whether the touch must be in the radius area.
	/// </summary>
	/// <value>
	/// <c>true</c> if restrict area; otherwise, <c>false</c>.
	/// </value>
	public bool RestrictArea {
		get {
			return this.restrictArea;
		}
		set {
			restrictArea = value;
			if (restrictArea){
				touchSizeCoef = touchSize;
			}
			else{
				touchSizeCoef=0;	
			}
		}
	}	
	
	
	// Messaging
	/// <summary>
	/// The receiver gameobject when you re in broacast mode for events
	/// </summary>
	public GameObject ReceiverObjectGame; 
	/// <summary>
	/// The message sending mode fro broacast
	/// </summary>
	public Broadcast messageMode;
	
	/// <summary>
	/// The enable smoothing.When smoothing is enabled, resets the joystick slowly in the start position
	/// </summary>
	public bool enableSmoothing = false;
	
	[SerializeField]
	private Vector2 smoothing = new Vector2(2f,2f);
	/// <summary>
	/// Gets or sets the smoothing values
	/// </summary>
	/// <value>
	/// The smoothing.
	/// </value>
	public Vector2 Smoothing {
		get {
			return this.smoothing;
		}
		set {
			smoothing = value;
			if (smoothing.x<0.1f){
				smoothing.x=0.1f;
			}
			if (smoothing.y<0.1){
				smoothing.y=0.1f;	
			}
		}
	}
	
	/// <summary>
	/// The enable inertia. Inertia simulates sliding movements (like a hovercraft, for example)
	/// </summary>
	public bool enableInertia = false;
	
	[SerializeField]
	public Vector2 inertia = new Vector2(100,100);
	/// <summary>
	/// Gets or sets the inertia values
	/// </summary>
	/// <value>
	/// The inertia.
	/// </value>
	public Vector2 Inertia {
		get {
			return this.inertia;
		}
		set {
			inertia = value;
			if (inertia.x<=0){
				inertia.x=1;
			}
			if (inertia.y<=0){
				inertia.y=1;
			}
			
		}
	}	
	
	// Helper
	public bool showZone = true;
	public bool showTouch = true;
	public bool showDeadZone = true;
	public Texture areaTexture;
	public Texture touchTexture;
	public Texture deadTexture;
	#endregion
	
	#region Direct mode properties
	/// <summary>
	/// The use broadcast for javascript
	/// </summary>
	public bool useBroadcast = false;
	
	/// <summary>
	/// The speed of each joystick axis
	/// </summary>
	public Vector2 speed;
	
	/// <summary>
	/// The interaction.
	/// </summary>
	public  InteractionType interaction = InteractionType.Direct;
	
	// X axis
	[SerializeField]
	private Transform xAxisTransform;
	/// <summary>
	/// Gets or sets the transform influenced by x axis of the joystick.
	/// </summary>
	/// <value>
	/// The X axis transform.
	/// </value>
    /// 
	public Transform XAxisTransform {
		get {
			return this.xAxisTransform;
		}
		set {
			xAxisTransform = value;
			if (xAxisTransform!=null){
				xAxisCharacterController = xAxisTransform.GetComponent<CharacterController>();
			}
			else{
				xAxisCharacterController=null;	
				xAxisGravity=0;
			}
		}
	}	
	

	/// <summary>
	/// The character controller attached to the X axis transform (if exist)
	/// </summary>
	public CharacterController xAxisCharacterController;
	/// <summary>
	/// The gravity.
	/// </summary>
	public float xAxisGravity=0;
	
	/// <summary>
	/// The Property influenced by the x axis joystick
	/// </summary>
	public PropertiesInfluenced xTI;
	/// <summary>
	/// The axis influenced by the x axis joystick
	/// </summary>
	public AxisInfluenced xAI;
	/// <summary>
	/// Inverse X axis.
	/// </summary>
	public bool inverseXAxis=false;
		
	// Y axis
	[SerializeField]
	private Transform yAxisTransform;
	/// <summary>
	/// Gets or sets the transform influenced by y axis of the joystick.
	/// </summary>
	/// <value>
	/// The Y axis transform.
	/// </value>
	public Transform YAxisTransform {
		get {
			return this.yAxisTransform;
		}
		set {
			yAxisTransform = value;
			if (yAxisTransform!=null){
				yAxisCharacterController = yAxisTransform.GetComponent<CharacterController>();
			}
			else{
				yAxisCharacterController=null;
				yAxisGravity=0;
			}
		}
	}	
	
	/// <summary>
	/// The character controller attached to the X axis transform (if exist)
	/// </summary>
	public CharacterController yAxisCharacterController;
	/// <summary>
	/// The y axis gravity.
	/// </summary>
	public float yAxisGravity=0;

	/// <summary>
	/// The Property influenced by the y axis joystick
	/// </summary>
	public PropertiesInfluenced yTI;
	/// <summary>
	/// The axis influenced by the y axis joystick
	/// </summary>
	public AxisInfluenced yAI;
	/// <summary>
	/// Inverse Y axis.
	/// </summary>	
	public bool inverseYAxis=false;

	#endregion
		
	#region private members
	// Joystick properties
	private Vector2 joystickCenter;
	private Vector2 joyTouch;
	private bool virtualJoystick = true;
	private int joystickIndex=-1;
	private float touchSizeCoef=0;
	private bool sendEnd=false;
	#endregion
	
	#region Inspector
	public bool showProperties=true;
	public bool showInteraction=true;
	public bool showAppearance=true;
	#endregion

	#region Monobehaviour methods
	void OnEnable(){
		EasyTouch.On_TouchStart += On_TouchStart;
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_TouchDown += On_TouchDown;
	}
	
	void OnDisable(){
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_TouchDown -= On_TouchDown;		
	}
		
	void OnDestroy(){
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_TouchDown -= On_TouchDown;		
	}		
			
	void Start(){
					
		if (!dynamicJoystick){
			joystickCenter = joystickPosition;
			virtualJoystick = true;
		}
		else{
			virtualJoystick = false;	
		}
	}
	
	void Update(){
		if (!useFixedUpdate && enable){
			UpdateJoystick();	
		}
	}
	
	void FixedUpdate(){
		if (useFixedUpdate && enable){
			UpdateJoystick();	
		}		
	}
	
	void UpdateJoystick(){
		if (Application.isPlaying){
									
			// Reset to initial position
			if (joystickIndex==-1){
				if (!enableSmoothing){
					joyTouch = Vector2.zero;
				}
				else{ 
					if (joyTouch.sqrMagnitude>0.1){
						joyTouch = new Vector2( joyTouch.x - joyTouch.x*smoothing.x*Time.deltaTime, joyTouch.y - joyTouch.y*smoothing.y*Time.deltaTime);	
					}
					else{
						joyTouch = Vector2.zero;
					}
				}
			}
			
			// Joystick Axis 
			if (joyTouch.sqrMagnitude>deadZone*deadZone){
				
				joystickAxis = Vector2.zero;
				if (Mathf.Abs(joyTouch.x)> deadZone){
					joystickAxis = new Vector2( (joyTouch.x -(deadZone*Mathf.Sign(joyTouch.x)))/(zoneRadius-touchSizeCoef-deadZone),joystickAxis.y);
				}
				else{
					joystickAxis = new Vector2( joyTouch.x /(zoneRadius-touchSizeCoef),joystickAxis.y);
				}
				if (Mathf.Abs(joyTouch.y)> deadZone){
					joystickAxis = new Vector2( joystickAxis.x,(joyTouch.y-(deadZone*Mathf.Sign(joyTouch.y)))/(zoneRadius-touchSizeCoef-deadZone));
				}
				else{
					joystickAxis = new Vector2( joystickAxis.x,joyTouch.y/(zoneRadius-touchSizeCoef));	
				}
			
			}
			else{
				joystickAxis = new Vector2(0,0);	
			}
			
			// Inverse axis ?
			if (inverseXAxis){
				joystickAxis.x *= -1;	
			}
			if (inverseYAxis){
				joystickAxis.y *= -1;	
			}
			
			// Joystick Value
			Vector2 realvalue = new Vector2(  speed.x*joystickAxis.x,speed.y*joystickAxis.y);
			if (enableInertia){
				Vector2 tmp = (realvalue - joystickValue);
				tmp.x /= inertia.x;
				tmp.y /= inertia.y;
				joystickValue += tmp;
				
			}
			else{
				joystickValue = realvalue;	
				
			}
			
			// interaction manager
			if (joystickAxis != Vector2.zero){
				sendEnd = false;
				switch(interaction){
					case InteractionType.Direct:
						UpdateDirect();
						break;
					case InteractionType.EventNotification:
						CreateEvent(MessageName.On_JoystickMove);
						break;
					case InteractionType.DirectAndEvent:
						UpdateDirect();
						CreateEvent(MessageName.On_JoystickMove);
						break;
				}
			}
			else{
				if (!sendEnd){
					CreateEvent(MessageName.On_JoystickMoveEnd);
					sendEnd = true;
				}
			}
			
		}		
	}
	
	void OnGUI(){
					
		// area zone
		if ((showZone && areaTexture!=null && !dynamicJoystick && enable) || (showZone && dynamicJoystick && virtualJoystick && areaTexture!=null && enable)  ){
			GUI.DrawTexture( new Rect(joystickCenter.x -zoneRadius ,Screen.height- joystickCenter.y-zoneRadius,zoneRadius*2,zoneRadius*2), areaTexture,ScaleMode.ScaleToFit,true);
		}
		
		// area touch
		if ((showTouch && touchTexture!=null && !dynamicJoystick && enable)|| (showTouch && dynamicJoystick && virtualJoystick && touchTexture!=null &enable) ){
			GUI.DrawTexture( new Rect(joystickCenter.x+(joyTouch.x -touchSize) ,Screen.height-joystickCenter.y-(joyTouch.y+touchSize),touchSize*2,touchSize*2), touchTexture,ScaleMode.ScaleToFit,true);
			
		}	

		// dead zone
		if ((showDeadZone && deadTexture!=null && !dynamicJoystick && enable)|| (showDeadZone && dynamicJoystick && virtualJoystick && deadTexture!=null && enable) ){
			GUI.DrawTexture( new Rect(joystickCenter.x -deadZone,Screen.height-joystickCenter.y-deadZone,deadZone*2,deadZone*2), deadTexture,ScaleMode.ScaleToFit,true);
			
		}	
	}
	
	void OnDrawGizmos(){
	}
	
	#endregion
	
	#region Private methods
	void CreateEvent(MessageName message){
		
		
		MovingJoystick move = new MovingJoystick();
		move.joystickName = gameObject.name;
		move.joystickAxis = joystickAxis;
		move.joystickValue = joystickValue;
		
		//
		if (!useBroadcast){
			switch (message){
			case MessageName.On_JoystickMove:
				if (On_JoystickMove!=null){
					On_JoystickMove( move);	
				}
				break;
			case MessageName.On_JoystickMoveEnd:
				if (On_JoystickMoveEnd!=null){
					On_JoystickMoveEnd( move);	
				}
				break;	
			}
		}
		else{
			switch(messageMode){
			case Broadcast.BroadcastMessage:
				ReceiverObjectGame.BroadcastMessage( message.ToString(),move,SendMessageOptions.DontRequireReceiver);
				break;
			case Broadcast.SendMessage:
				ReceiverObjectGame.SendMessage( message.ToString(),move,SendMessageOptions.DontRequireReceiver);
				break;
			case Broadcast.SendMessageUpwards:
				ReceiverObjectGame.SendMessageUpwards( message.ToString(),move,SendMessageOptions.DontRequireReceiver);
				break;
			}
		}
		
	}
	
	void UpdateDirect(){

		// Gravity
		if (xAxisCharacterController!=null && xAxisGravity>0){
			xAxisCharacterController.Move( Vector3.down*xAxisGravity*Time.deltaTime);	
		}
			
		if (yAxisCharacterController!=null && yAxisGravity>0){
			yAxisCharacterController.Move( Vector3.down*yAxisGravity*Time.deltaTime);
		}
			
		
		// X joystick axis
		if (xAxisTransform !=null){
			// Axis influenced
			Vector3 axis =GetInfluencedAxis( xAI);
			// Action
			DoActionDirect( xAxisTransform, xTI, axis, joystickValue.x, xAxisCharacterController);
			
		}
		
		// y joystick axis
		if (YAxisTransform !=null){
			// Axis
			Vector3 axis = GetInfluencedAxis(yAI);
			// Action
			DoActionDirect( yAxisTransform, yTI, axis,joystickValue.y, yAxisCharacterController);
		}
		
	}
	

	
	Vector3 GetInfluencedAxis(AxisInfluenced axisInfluenced){
		
		Vector3 axis = Vector3.zero;
		
		switch(axisInfluenced){
			case AxisInfluenced.X:
				axis = Vector3.right;
				break;
			case AxisInfluenced.Y:
				axis = Vector3.up;
				break;
			case AxisInfluenced.Z:
				axis = Vector3.forward;
				break;
			case AxisInfluenced.XYZ:
				axis = Vector3.one;
				break;
				
		}	
		
		return axis;
	}
	
	void DoActionDirect(Transform axisTransform, PropertiesInfluenced inlfuencedProperty,Vector3 axis, float sensibility, CharacterController charact){
		

		switch(inlfuencedProperty){
			case PropertiesInfluenced.Rotate:
				axisTransform.Rotate( axis * sensibility * Time.deltaTime,Space.World);
				break;	
			case PropertiesInfluenced.RotateLocal:
				axisTransform.Rotate( axis * sensibility * Time.deltaTime,Space.Self);
				break;
			case PropertiesInfluenced.Translate:
				if (charact==null){
					axisTransform.Translate(axis * sensibility * Time.deltaTime,Space.World);
				}
				else{
					charact.Move( axis * sensibility * Time.deltaTime );
				}
				break;
			case PropertiesInfluenced.TranslateLocal:
				if (charact==null){
					axisTransform.Translate(axis * sensibility * Time.deltaTime,Space.Self);
				}
				else{
					charact.Move( charact.transform.TransformDirection(axis) * sensibility * Time.deltaTime );
				}
				break;	
			case PropertiesInfluenced.Scale:
				axisTransform.localScale +=  axis * sensibility * Time.deltaTime;
				break;
		}
		
	}
	
	#endregion
	
	#region EasyTouch events
	void On_TouchStart(Gesture gesture){
		
		if (!dynamicJoystick){
			if ((gesture.position - joystickCenter).sqrMagnitude < (zoneRadius+touchSizeCoef/2)*(zoneRadius+touchSizeCoef/2)){
				joystickIndex = gesture.fingerIndex;
			}
		}
		else{
			if (!virtualJoystick){
				
				#region area restriction
				switch (area){
					// full
					case DynamicArea.FullScreen:
						virtualJoystick = true;	;
						break;
					// bottom
					case DynamicArea.Bottom:
						if (gesture.position.y< Screen.height/2){
							virtualJoystick = true;	
						}
						break;
					// top
					case DynamicArea.Top:
						if (gesture.position.y> Screen.height/2){
							virtualJoystick = true;	
						}
						break;
					// Right
					case DynamicArea.Right:
						if (gesture.position.x> Screen.width/2){
							virtualJoystick = true;	
						}
						break;
					// Left
					case DynamicArea.Left:
						if (gesture.position.x< Screen.width/2){
							virtualJoystick = true;	
						}
						break;				
					
					// top Right
					case DynamicArea.TopRight:
						if (gesture.position.y> Screen.height/2 && gesture.position.x> Screen.width/2){
							virtualJoystick = true;	
						}
						break;	
					// top Left
					case DynamicArea.TopLeft:
						if (gesture.position.y> Screen.height/2 && gesture.position.x< Screen.width/2){
							virtualJoystick = true;	
						}
						break;					
					// bottom Right
					case DynamicArea.BottomRight:
						if (gesture.position.y< Screen.height/2 && gesture.position.x> Screen.width/2){
							virtualJoystick = true;	
						}
						break;					
					// bottom left
					case DynamicArea.BottomLeft:
						if (gesture.position.y< Screen.height/2 && gesture.position.x< Screen.width/2){
							virtualJoystick = true;	
						}
					break;							
				}				
				#endregion
				
				if (virtualJoystick){
					joystickCenter = gesture.position;
					joystickIndex = gesture.fingerIndex;
				}	
			}
		}
	}
	
	// Joystick move
	void On_TouchDown(Gesture gesture){
		
		if (gesture.fingerIndex == joystickIndex){
			joyTouch  = new Vector2( gesture.position.x, gesture.position.y) - joystickCenter;
	
			if ((joyTouch/(zoneRadius-touchSizeCoef)).sqrMagnitude > 1){
				joyTouch.Normalize();
				joyTouch *= zoneRadius-touchSizeCoef;
			}
		}
	}
	
	// Touch end
	void On_TouchUp( Gesture gesture){
		
		if (gesture.fingerIndex == joystickIndex){
			joystickIndex=-1;
			if (dynamicJoystick){
				virtualJoystick=false;	
			}
		}
	}

	#endregion

}
