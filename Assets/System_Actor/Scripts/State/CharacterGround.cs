using UnityEngine;

public class CharacterGround : CharacterState {

	private float horizontalMovementDirection;
	public float MaxSpeed = 7f;
	public float Acceleration = 10f;
	
	public void Start(){

	}
	
	protected void Update(){

		HandleInput();

		_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, horizontalMovementDirection * MaxSpeed, Time.deltaTime * Acceleration));
	}

    private void HandleInput()
    {
		if(_controller.CanJump && _input.Jump){
			
			_controller.Jump();
		}
		
		horizontalMovementDirection = _input.Horizontal;
		
		if(horizontalMovementDirection != 0.0f){
			FaceRight(horizontalMovementDirection > 0.0f);
		}
		
        /*if(Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0){

			horizontalMovementDirection = 1;
			
			if(!_isFacingRight)
				Flip();
				
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			
			horizontalMovementDirection = -1;
		
			if(_isFacingRight)
				Flip();
			
		}else{
			
			horizontalMovementDirection = 0;
		}*/
    }
	
	public override bool IsRelevant(){
		
		return _controller.State.IsCollidingDown;
	}
}
