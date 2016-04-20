using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	private bool isFacingRight;
	private CharacterController2D _controller;
	private float horizontalMovementDirection;
	
	public float MaxSpeed = 7f;
	public float AccelerationGround = 10f;
	public float AccelerationAir = 5f;
	
	private bool _isWallHanging;
	
	void OnGUI(){
	
		GUI.Label(new Rect(0, 15, 300, 50), "Velocity: " + _controller.Velocity + " (" + _controller.Velocity.magnitude + ")");
		GUI.Label(new Rect(0, 30, 300, 50), "Wall hanging: " + _isWallHanging);
	}
	
	public void Start(){
		
		_controller = GetComponent<CharacterController2D>();
		isFacingRight = transform.localScale.x > 0;
	}
	
	public void Update(){
		
		_isWallHanging = false;
		HandleInput();

		if(_isWallHanging){		
			
			//_controller.SetForce(Vector2.zero);
			//_controller.Parameters.Gravity = 0f;
			//_controller.Parameters.JumpRestrictions = ControllerParameters.JumpBehaviour.CanJumpAnywhere;
		}else{
			
			//_controller.Parameters.Gravity = -50f;
			//_controller.Parameters.JumpRestrictions = ControllerParameters.JumpBehaviour.CanJumpOnGround;
		}

		float movementFactor = _controller.State.IsCollidingDown ? AccelerationGround : AccelerationAir;
		_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, horizontalMovementDirection * MaxSpeed, Time.deltaTime * movementFactor));
	}

    private void HandleInput()
    {
        if(Input.GetKey(KeyCode.RightArrow)){
		
			horizontalMovementDirection = 1;
			
			if(!isFacingRight)
				Flip();
				
			if(_controller.State.IsCollidingRight && !_controller.State.IsCollidingDown){
				
				_isWallHanging = true;
			}
				
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			
			horizontalMovementDirection = -1;
		
			if(isFacingRight)
				Flip();
				
			if(_controller.State.IsCollidingLeft && !_controller.State.IsCollidingDown){
				
				_isWallHanging = true;
			}
			
		}else{
			
			horizontalMovementDirection = 0;
		}
		
		if(_controller.CanJump && Input.GetKeyDown(KeyCode.Space)){
			
			_controller.Jump();
		}
    }

    private void Flip()
    {
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		isFacingRight = transform.localScale.x > 0;
    }
}
