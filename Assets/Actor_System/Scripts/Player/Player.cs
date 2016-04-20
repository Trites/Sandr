using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	private bool isFacingRight;
	private CharacterController2D controller;
	private float horizontalMovementDirection;
	
	public float MaxSpeed = 7f;
	public float AccelerationGround = 10f;
	public float AccelerationAir = 5f;
	
	void OnGUI(){
	
		GUI.Label(new Rect(0, 15, 300, 50), "Velocity: " + controller.Velocity + " (" + controller.Velocity.magnitude + ")");
	}
	
	public void Start(){
		
		controller = GetComponent<CharacterController2D>();
		isFacingRight = transform.localScale.x > 0;
	}
	
	public void Update(){
		
		HandleInput();

		float movementFactor = controller.State.IsCollidingDown ? AccelerationGround : AccelerationAir;
		controller.SetHorizontalForce(Mathf.Lerp(controller.Velocity.x, horizontalMovementDirection * MaxSpeed, Time.deltaTime * movementFactor));
	}

    private void HandleInput()
    {
        if(Input.GetKey(KeyCode.RightArrow)){
		
			horizontalMovementDirection = 1;
			
			if(!isFacingRight)
				Flip();
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			
			horizontalMovementDirection = -1;
		
			if(isFacingRight)
				Flip();
		}else{
			
			horizontalMovementDirection = 0;
		}
		
		if(controller.CanJump && Input.GetKeyDown(KeyCode.Space)){
			
			controller.Jump();
		}
    }

    private void Flip()
    {
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		isFacingRight = transform.localScale.x > 0;
    }
}
