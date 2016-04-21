using UnityEngine;

public class CharacterGround : CharacterState {

	private float horizontalMovementDirection;
	
	public float MaxSpeed = 7f;
	public float AccelerationGround = 10f;
	public float AccelerationAir = 5f;
	
	private bool _isWallHanging;
	
	void OnGUI(){
	
		GUI.Label(new Rect(0, 15, 300, 50), "Velocity: " + _controller.Velocity + " (" + _controller.Velocity.magnitude + ")");
		GUI.Label(new Rect(0, 30, 300, 50), "State: " + _controller.State);
	}
	
	public void Start(){

		_isWallHanging =false;
	}
	
	private float _wallTimer = 0f;
	
	public void Update(){

		
		if(_isWallHanging){		
			
			//_controller.SetForce(Vector2.zero);
			//_controller.Parameters.Gravity = 0f;
			//_controller.Parameters.JumpRestrictions = ControllerParameters.JumpBehaviour.CanJumpAnywhere;
		}else{
			
			//_controller.Parameters.Gravity = -50f;
			//_controller.Parameters.JumpRestrictions = ControllerParameters.JumpBehaviour.CanJumpOnGround;
		}
		
		if(!_controller.State.IsCollidingDown && (_controller.State.IsCollidingLeft || _controller.State.IsCollidingRight)){
									
			_isWallHanging = true;
			_wallTimer = 0.2f;
			_controller.Parameters.JumpRestrictions = ControllerParameters.JumpBehaviour.CanJumpAnywhere;
			
		}else if(_isWallHanging){
			
			_wallTimer -= Time.deltaTime;
			if(_wallTimer <= 0){
				
				_wallTimer = 0;
				_isWallHanging = false;
				_controller.Parameters.JumpRestrictions = ControllerParameters.JumpBehaviour.CanJumpOnGround;
			}
		}

		HandleInput();

		float movementFactor = _controller.State.IsCollidingDown ? AccelerationGround : AccelerationAir;
		_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, horizontalMovementDirection * MaxSpeed, Time.deltaTime * movementFactor));
	}

    private void HandleInput()
    {
		if(_controller.CanJump && Input.GetKeyDown(KeyCode.Space)){
			
			_isWallHanging = false;
			_wallTimer = 0;
			_controller.Jump();
		}
		
        if(Input.GetKey(KeyCode.RightArrow)){
		
			if(!(_isWallHanging && _controller.State.IsCollidingLeft))
				horizontalMovementDirection = 1;
			
			if(!_isFacingRight)
				Flip();
				
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			
			if(!(_isWallHanging && _controller.State.IsCollidingRight))
				horizontalMovementDirection = -1;
		
			if(_isFacingRight)
				Flip();
			
		}else{
			
			horizontalMovementDirection = 0;
		}
    }
	
	public override bool IsRelevant(){
		
		return true;
	}
}
