using UnityEngine;

public class CharacterGround : CharacterState {

	private float horizontalMovementDirection;
	public float MaxSpeed = 7f;
	public float Acceleration = 10f;
	
	public void Start(){

	}
	
	public override void OnActivate(){
		
		transform.localScale = new Vector2(1.1f, 1f);
	}
	
	protected void Update(){
		
		HandleInput();

		_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, horizontalMovementDirection * MaxSpeed, Time.deltaTime * Acceleration));
	}

    private void HandleInput()
    {
		if(_controller.CanJump && _input.Jump){
			
			_controller.Jump();
			
			transform.localScale = new Vector2(0.8f, 1.4f);
		}
		
		horizontalMovementDirection = _input.Horizontal;
		
		if(horizontalMovementDirection != 0.0f){
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
		
		return _controller.State.IsCollidingDown;
	}
}
