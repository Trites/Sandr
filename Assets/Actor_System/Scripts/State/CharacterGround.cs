using UnityEngine;

public class CharacterGround : CharacterState {

	private float horizontalMovementDirection;
	public float MaxSpeed = 7f;
	public float Acceleration = 10f;
	
	public void Start(){

	}
	
	protected override void Update(){
		base.Update();

		HandleInput();

		_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, horizontalMovementDirection * MaxSpeed, Time.deltaTime * Acceleration));
	}

    private void HandleInput()
    {
		if(_controller.CanJump && Input.GetKeyDown(KeyCode.Space)){
			
			_controller.Jump();
		}
		
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
    }
	
	public override bool IsRelevant(){
		
		return _controller.State.IsCollidingDown;
	}
}
