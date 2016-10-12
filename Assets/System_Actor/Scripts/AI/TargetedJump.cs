using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TargetedJump : MonoBehaviour {

	public LayerMask RayLayer;

	public Transform Target;
	
	public Vector2 MaxJumpForce = new Vector2(10, 10);

	public float[] JumpFractions = { 1f, 0.65f, 0.5f};

	private Vector2 _jumpVector;

	private Rigidbody2D _body;
	
	private float timer = 2f;
	
	private Vector2 _halfSize;

	void Awake () {
		
		_jumpVector = Vector2.zero;
		
		_halfSize = GetComponent<BoxCollider2D>().size / 2f;
		_body = GetComponent<Rigidbody2D>();
	}
	
	bool _canJump = false;
	
	void FixedUpdate () {
		
		
					
		Vector2 targetVector = Target.position - transform.position;
			
		foreach(float frac in JumpFractions){
				
			_canJump = CalculateJump(targetVector, frac);
			
			if(_canJump)
				break;
		}
		
		
		if(_canJump){
			
			timer -= Time.deltaTime;
			if(timer <= 0f){
			
				Jump();
			}
		}else{
			
			timer = 2f;
		}

	}
	
	private void Jump(){
		
		Debug.Log(_jumpVector);
		_body.velocity = _jumpVector;
		this.enabled = false;
	}
	
	private bool CalculateJump(Vector2 targetVector, float fracX){
		

		float jumpX = MaxJumpForce.x * fracX;
		float timeToTaget = (Mathf.Abs(targetVector.x) - _halfSize.x) / jumpX;
		
		Vector2 jumpForce = new Vector2(jumpX * Mathf.Sign(targetVector.x), GetJumpForce(targetVector, timeToTaget));
		
		float offsetX = Mathf.Sign(targetVector.x) * _halfSize.x;
		
		Vector2 posUpper = (Vector2)transform.position + new Vector2(offsetX, _halfSize.y);
		Vector2 posLower = (Vector2)transform.position + new Vector2(offsetX, -_halfSize.y);
		
		float timeUpper = CheckArc(posUpper, jumpForce, timeToTaget);
		float timeLower = CheckArc(posLower, jumpForce, timeToTaget);
		
		bool validUpper = timeUpper >= timeToTaget;
		bool validLower = timeLower >= timeToTaget;
		
		if(validUpper && validLower){
			
			_jumpVector = jumpForce;
		}
		

		DrawArc(posUpper, jumpForce, timeUpper, (int)(timeToTaget / Time.deltaTime), validUpper ? Color.green : Color.red);
		DrawArc(posLower, jumpForce, timeLower, (int)(timeToTaget / Time.deltaTime), validLower ? Color.green : Color.red);
		
		return validUpper && validLower;
	}
	
	private float GetJumpForce(Vector2 targetVector, float timeToTaget){
		
		return targetVector.y / timeToTaget + Mathf.Abs(Physics2D.gravity.y) * timeToTaget /2f;
	}
	
	private float CheckArc(Vector2 origin, Vector2 initVel, float targetTime){
		
		Vector2 currentPos = origin;
		Vector2 velocity = initVel;
		
		float time = 0f;
		
		RaycastHit2D hit;
		do
		{
			
			hit = Physics2D.Raycast(currentPos, velocity.normalized, velocity.magnitude * Time.deltaTime, RayLayer);
			
			currentPos += velocity * Time.deltaTime;
			velocity += Physics2D.gravity * Time.deltaTime;
			
			
			if(hit)
				return time;
				
			time += Time.deltaTime;
		} while (time < targetTime);
		
		return targetTime;
	}
	
	private void DrawArc(Vector2 origin, Vector2 initVel, float targetTime, int steps, Color color){
		
		Vector2 velocity = initVel;
		float stepSize = targetTime / steps;
		Vector2 currentPos = origin;
		
		
		for(int i = 0; i < steps; ++i){
			
			Debug.DrawRay(currentPos, velocity * stepSize, color);
			
			currentPos += velocity * stepSize;
			velocity += Physics2D.gravity * stepSize;
			
		}
	}
}
