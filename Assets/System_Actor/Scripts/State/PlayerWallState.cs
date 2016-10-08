using UnityEngine;
using System.Collections;

public class PlayerWallState : CharacterState {

	private float horizontalMovementDirection;
	public float DefaultSlideSpeed = 3f;
	
	public float FastSlideSpeed = 7f;
	public float Acceleration = 10f;

	private int _wallDirection;
	private float _slideSpeed;

	void Start () {
	
		Priority = 5;
	}
	
	public override void OnActivate(){
		
		_controller.Parameters.Gravity = 0f;
		_controller.SetVerticalForce(0f);
		_slideSpeed = DefaultSlideSpeed;
	}
	
	protected void Update () {
	
		_wallDirection = _controller.State.IsCollidingRight ? 1 : (_controller.State.IsCollidingLeft) ? -1 : 0;
		FaceRight(_wallDirection == -1);
		
		HandleInput();
		_controller.SetVerticalForce(Mathf.Lerp(_controller.Velocity.y, -_slideSpeed, Time.deltaTime * Acceleration));
	}
	
	private void HandleInput()
    {
		
		horizontalMovementDirection = _input.Horizontal;
		
		/*if(horizontalMovementDirection != 0.0f){
			FaceRight(horizontalMovementDirection > 0.0f);
		}*/
		
		if(_input.Jump){
						
			_controller.SetHorizontalForce(5f * -_wallDirection);
			
			if(_input.Vertical >= 0)
				_controller.Jump();
			
			if(_wallDirection == -horizontalMovementDirection)
				_controller.AddHorizontalForce(3f * horizontalMovementDirection);
		}
		
		if(_input.Vertical < 0){
			
			_slideSpeed = FastSlideSpeed;
		}else{
			
			_slideSpeed = DefaultSlideSpeed;
		}
		
		if(_input.Attack){
			
			_hands.InvokeUse();
		}
		
		if(_input.Drop){
			
			_hands.InvokeDrop();
		}
    }
	
	public override bool IsRelevant(){
		
		return !_controller.State.IsCollidingDown && (_controller.State.IsCollidingLeft || _controller.State.IsCollidingRight) && _controller.Velocity.y <= 0f;
	}
}
