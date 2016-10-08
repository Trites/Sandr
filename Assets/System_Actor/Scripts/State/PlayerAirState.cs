using UnityEngine;
using System.Collections;

public class PlayerAirState : CharacterState {

	private float horizontalMovementDirection;
	public float MaxSpeed = 7f;
	public float Acceleration = 5f;
	
	public override void OnActivate(){
		
		_controller.Parameters.Gravity = -50f;
	}
	
	protected void Update(){

		HandleInput();
		_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, horizontalMovementDirection * MaxSpeed, Time.deltaTime * Acceleration));
		
		//float scaling = Mathf.Abs(_controller.Velocity.y)/100f;
		//transform.localScale = new Vector2(1f - scaling, 1f + scaling);
	}

    private void HandleInput()
    {
		if(_controller.CanJump && _input.Jump){
			
			_controller.Jump();
		}
		
		horizontalMovementDirection = _input.Horizontal;
		
		if(horizontalMovementDirection != 0){
			FaceRight(horizontalMovementDirection > 0.0f);
		}
		
		if(_input.Attack){
			
			_hands.InvokeUse();
		}
		
		if(_input.Drop){
			
			_hands.InvokeDrop();
		}
    }
	
	public override bool IsRelevant(){
		
		return !_controller.State.IsCollidingDown;
	}
}
