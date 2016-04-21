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
	
	protected override void Update () {
		base.Update();
	
		_wallDirection = _controller.State.IsCollidingRight ? 1 : (_controller.State.IsCollidingLeft) ? -1 : 0;
		
		if(_facingDirection == _wallDirection)
			Flip();
		
		HandleInput();
		_controller.SetVerticalForce(Mathf.Lerp(_controller.Velocity.y, -_slideSpeed, Time.deltaTime * Acceleration));
	}
	
	private void HandleInput()
    {
		
        if(Input.GetKey(KeyCode.RightArrow)){

			horizontalMovementDirection = 1;
				
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			
			horizontalMovementDirection = -1;
			
		}else{
			
			horizontalMovementDirection = 0;
		}
		
		if(Input.GetKeyDown(KeyCode.Space)){
						
			_controller.SetHorizontalForce(5f * -_wallDirection);
			
			if(!Input.GetKey(KeyCode.DownArrow))
				_controller.Jump();
			
			if(_wallDirection == -horizontalMovementDirection)
				_controller.AddHorizontalForce(3f * horizontalMovementDirection);
		}
		
		if(Input.GetKey(KeyCode.DownArrow)){
			
			_slideSpeed = FastSlideSpeed;
		}else{
			
			_slideSpeed = DefaultSlideSpeed;
		}
    }
	
	public override bool IsRelevant(){
		
		return !_controller.State.IsCollidingDown && (_controller.State.IsCollidingLeft || _controller.State.IsCollidingRight);
	}
}
