using UnityEngine;
using System.Collections;

public class PlayerWallState : CharacterState {

	private float horizontalMovementDirection;
	public float MaxSpeed = 7f;
	public float Acceleration = 5f;

	private int _wallDirection;

	void Start () {
	
		Priority = 5;
	}
	
	public override void OnActivate(){
		
		_controller.Parameters.Gravity = -5f;
		_controller.SetVerticalForce(0f);
		//_controller.SetHorizontalForce(0f);
	}
	
	void Update () {
	
		_wallDirection = _controller.State.IsCollidingRight ? 1 : -1;
		HandleInput();
	}
	
	private void HandleInput()
    {
		
        if(Input.GetKey(KeyCode.RightArrow)){

			horizontalMovementDirection = 1;
			
			if(!_isFacingRight)
				Flip();
				
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			
			horizontalMovementDirection = -1;
		
			if(_isFacingRight)
				Flip();
			
		}else{
			
			horizontalMovementDirection = 0;
		}
		
		if(Input.GetKeyDown(KeyCode.Space)){
			
			_controller.Jump();
			_controller.SetHorizontalForce(5f * -_wallDirection + 3f * horizontalMovementDirection);
		}
    }
	
	public override bool IsRelevant(){
		
		return !_controller.State.IsCollidingDown && (_controller.State.IsCollidingLeft || _controller.State.IsCollidingRight);
	}
}
