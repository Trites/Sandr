using UnityEngine;

[RequireComponent(typeof(CharacterController2D), typeof(PlayerInput))]
public abstract class CharacterState : MonoBehaviour {

	private const int LEFT_FACING_ROTATION = 180;
	private const int RIGHT_FACING_ROTATION = 0;

	public int Priority = 0;
	
	protected bool _isFacingRight { get{ return _facingDirection == 1; }}	
	protected CharacterController2D _controller;
	protected PlayerInput _input;
	protected int _facingDirection;
	
	void OnGUI(){
	
		if(this.tag == "Player"){
		
			GUI.Label(new Rect(0, 15, 300, 50), "Velocity: " + _controller.Velocity + " (" + _controller.Velocity.magnitude + ")");
			GUI.Label(new Rect(0, 30, 300, 50), "State: " + _controller.State);
			GUI.Label(new Rect(0, 60, 300, 50), "Facing: " + _facingDirection);	
		}
	}
	
	protected virtual void Awake(){
				
		_controller = GetComponent<CharacterController2D>();
		_input = GetComponent<PlayerInput>();
		_facingDirection = 1;
	}
	
	protected void FaceRight(bool faceRight){
		
		_facingDirection = faceRight ? FaceRight() : FaceLeft();
	}
	
    protected void Flip()
    {
		_facingDirection = _isFacingRight ? FaceLeft() : FaceRight();
    }
	
	protected int FaceRight(){
		
		transform.localRotation = Quaternion.Euler(0, RIGHT_FACING_ROTATION, 0);
		return 1;
	}
	
	protected int FaceLeft(){
		
		transform.localRotation = Quaternion.Euler(0, LEFT_FACING_ROTATION, 0);
		return -1;
	}
	
	public virtual void OnActivate(){}
	
	public abstract bool IsRelevant();
}
