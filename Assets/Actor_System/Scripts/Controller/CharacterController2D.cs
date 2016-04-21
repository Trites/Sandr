using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour {

	private const float SkinWidth = 0.02f;
	private const int HorizontalRaysCount = 8;
	private const int VerticalRaysCount = 4;
	private static readonly float SlopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);

	public LayerMask PlatformMask;
	public ControllerParameters DefaultParameters;
	public ControllerState State { get; private set; }	
	public Vector2 Velocity { get { return _velocity; } }
	public bool HandleCollisions{ get; set; }
	public ControllerParameters Parameters { get{ return _overrideParameters ?? DefaultParameters; } }
	public GameObject StandingOn { get; private set; }
	
	public Vector3 PlatformVelocity { get; private set; }
	public bool CanJump{ 
		get { 
				if(Parameters.JumpRestrictions == ControllerParameters.JumpBehaviour.CanJumpAnywhere)
					return _jumpIn < 0;
					
					
				if(Parameters.JumpRestrictions == ControllerParameters.JumpBehaviour.CanJumpOnGround)			
					return State.IsCollidingDown || State.IsCollidingLeft || State.IsCollidingRight;
			
				return false;
			} 
	}
	
	private Vector2 _velocity;
	private Transform _transform;
	private Rigidbody2D _body;
	private Vector2 _localScale;
	private BoxCollider2D _boxCollider;
	private ControllerParameters _overrideParameters;	
    private float _jumpIn;
    private GameObject _lastStandingOn;
	
	private Vector3 _raycastTopLeft;
	private Vector3 _raycastBottomLeft;
	private Vector3 _raycastBottomRight;
	
	private float _horizontalRaySpacing;
	private float _verticalRaySpacing;
	
	private Vector3 _activeGlobalPlatformPoint;
	private Vector3 _activeLocalPlatformPoint;
	
	public void Awake(){
		
		HandleCollisions = true;
		State = new ControllerState();
		_transform = transform;
		_body = GetComponent<Rigidbody2D>();
		_localScale = transform.localScale;
		_boxCollider = GetComponent<BoxCollider2D>();
	
		float colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
		_horizontalRaySpacing = colliderHeight/(HorizontalRaysCount - 1);
		
		
		float colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
		_verticalRaySpacing = colliderWidth/(VerticalRaysCount - 1);
	}
	
	public void AddForce(Vector2 force){
		
		_velocity += force;
	}

	public void SetForce(Vector2 force){
		
		_velocity = force;
	}
	
	public void SetHorizontalForce(float force){
		
		_velocity.x = force;
	}
	
	public void SetVerticalForce(float force){
		
		_velocity.y = force;
	}
	
	public void Jump(){
		
		SetVerticalForce(Parameters.JumpMagnitude);
		//AddForce(new Vector2(0, Parameters.JumpMagnitude));
		_jumpIn = Parameters.JumpFrequency;
	}

    public void LateUpdate(){
		
        _jumpIn -= Time.deltaTime;
		
		_velocity.y += Parameters.Gravity * Time.deltaTime;
		Move(Velocity * Time.deltaTime);
	}

    private void Move(Vector2 deltaMove){
		
		bool wasGrounded = State.IsCollidingDown;
		State.Reset();
			
		if(HandleCollisions){
			
			HandlePlatforms();
			CalculateRayOrigins();
			
			if(deltaMove.y < 0 && wasGrounded)
				HandleSlopeVertical(ref deltaMove);
			
			float leftDist = HorizontalCollision(ref deltaMove, -1);
			float rightDist = HorizontalCollision(ref deltaMove, 1);
				
			//if(Mathf.Abs(deltaMove.x) > 0.001f){
			//	MoveHorizontally(ref deltaMove, (Mathf.Sign(deltaMove.x) == 1) ? rightDist : leftDist, 0f);
			//}
				//MoveHorizontally(ref deltaMove);
				
			MoveVertically(ref deltaMove);
			//CorrectHorizontalPlacement(ref deltaMove, true);
			//CorrectHorizontalPlacement(ref deltaMove, false);
		}
		
		_transform.Translate(deltaMove, Space.World);
		
		if(Time.deltaTime > 0)
			_velocity = deltaMove / Time.deltaTime;
			
		_velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
		_velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);
		
		if(State.IsMovingUpSlope)
			_velocity.y = 0;
			
		if(StandingOn != null){
			
			_activeGlobalPlatformPoint = transform.position;
			_activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(_transform.position);
			
			if(_lastStandingOn != StandingOn){
				
				if(_lastStandingOn != null)
					_lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
					
				StandingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
				_lastStandingOn = StandingOn;
			}else if(StandingOn != null){
				
				StandingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
			}
		}else if(_lastStandingOn != null){
			
			_lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	private void HandlePlatforms(){
		
		if(StandingOn != null){
			
			Vector3 newGlobalPlatformPoint = StandingOn.transform.TransformPoint(_activeLocalPlatformPoint);
			Vector3 moveDistance = newGlobalPlatformPoint - _activeGlobalPlatformPoint;
			
			if(moveDistance != Vector3.zero)
				transform.Translate(moveDistance, Space.World);
				
			PlatformVelocity = (newGlobalPlatformPoint - _activeGlobalPlatformPoint) / Time.deltaTime;
		}else{
			
			PlatformVelocity = Vector3.zero;
		}
		
		StandingOn = null;
	}
	
	private void CorrectHorizontalPlacement(ref Vector2 deltaMove, bool isRight){
		
		float halfWidth = (_boxCollider.size.x * _localScale.x) / 2f;
		Vector2 rayOrigin = isRight ? _raycastBottomRight : _raycastBottomLeft;
		
		if(isRight){
			
			rayOrigin.x -= (halfWidth + SkinWidth);
		}else{
			rayOrigin.x += (halfWidth + SkinWidth);
		}
		
		Vector2 rayDirection = isRight ? Vector2.right : -Vector2.right;
		float offset = 0f;
		
		for(int i = 1; i < HorizontalRaysCount - 1; i++){
			
			Vector2 rayVector = new Vector2(rayOrigin.x + deltaMove.x, deltaMove.y + rayOrigin.y + (i*_horizontalRaySpacing));
			//Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);
			
			RaycastHit2D rayHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PlatformMask);
			
			if(!rayHit)
				continue;
				
			offset = isRight ? ((rayHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - rayHit.point.x));
		}
		
		deltaMove.x += offset;
	}
	
	private void CalculateRayOrigins(){
		
		Vector2 size = new Vector2(_boxCollider.size.x * Mathf.Abs(_localScale.x), _boxCollider.size.y * Mathf.Abs(_localScale.y)) /2;
		Vector2 center = new Vector2(_boxCollider.offset.x * _localScale.x, _boxCollider.offset.y * _localScale.y);
		
		_raycastTopLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
		_raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
		//_raycastTopRight = _transform.position + new Vector3(center.x + size.x - SkinWidth, center.y + size.y - SkinWidth);
		_raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);
	}
	
	private float HorizontalCollision(ref Vector2 deltaMove, int direction){
		
		float slopeAngle = 0f;
		float rayDistance = SkinWidth + 0.05f;
		if(Mathf.Sign(deltaMove.x) == direction){
		
			rayDistance += Mathf.Abs(deltaMove.x);
		}

		Vector2 rayDirection = Vector2.right * direction; //isGoingRight ? Vector2.right : -Vector2.right;
		Vector3 rayOrigin = (direction == 1) ? _raycastBottomRight : _raycastBottomLeft;
		
		for(int i = 0; i < HorizontalRaysCount; i++){
			
			Vector2 rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _horizontalRaySpacing));
			Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);
			
			RaycastHit2D rayHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
			
			if(!rayHit)
				continue;

			rayDistance = rayHit.distance;//Mathf.Abs(deltaMove.x);
			slopeAngle = Vector2.Angle(rayHit.normal, Vector2.up);
			
			if(direction == 1){
				
				State.IsCollidingRight = true;
			}else{
				
				State.IsCollidingLeft = true;	
			}
			
			if(rayDistance < SkinWidth + 0.0001f)
				break;
		}
		
		if(direction == Mathf.Sign(deltaMove.x)){
		
			MoveHorizontally(ref deltaMove, rayDistance - SkinWidth, slopeAngle);
		}
		
		return rayDistance - SkinWidth;
	}
	
	private void MoveHorizontally(ref Vector2 deltaMove, float wallDistance, float slopeAngle){
		
		bool isGoingRight = deltaMove.x > 0;
			
		if(Mathf.Abs(deltaMove.x) > wallDistance){
			
			if(!HandleSlopeHorizontal(ref deltaMove, slopeAngle, isGoingRight))
				deltaMove.x =  wallDistance * Mathf.Sign(deltaMove.x);			
		}		
	}
	
	private void MoveHorizontally(ref Vector2 deltaMove){
		
		bool isGoingRight = deltaMove.x > 0;
		float rayDistance = Mathf.Abs(deltaMove.x) + SkinWidth;
		Vector2 rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
		Vector3 rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;
		
		for(int i = 0; i < HorizontalRaysCount; i++){
			
			Vector2 rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _horizontalRaySpacing));
			Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);
			
			RaycastHit2D rayHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
			
			if(!rayHit)
				continue;
				
				
			deltaMove.x -= (rayHit.point.x - rayVector.x);// + (isGoingRight ? -SkinWidth : SkinWidth);
			if(HandleSlopeHorizontal(ref deltaMove, Vector2.Angle(rayHit.normal, Vector2.up), isGoingRight)){	
				deltaMove.x += (rayHit.point.x - rayVector.x);// + (isGoingRight ? -SkinWidth : SkinWidth);								
				continue;
			}
			//deltaMove.x += (rayHit.point.x - rayVector.x);

				
			deltaMove.x = rayHit.point.x - rayVector.x;
			rayDistance = Mathf.Abs(deltaMove.x);
			
			if(isGoingRight){
				
				deltaMove.x -= SkinWidth;
				State.IsCollidingRight = true;
			}else{
				
				deltaMove.x += SkinWidth;
				State.IsCollidingLeft = true;	
			}
			
			if(rayDistance < SkinWidth + 0.0001f)
				break;
		}
		
	}
	
	private void MoveVertically(ref Vector2 deltaMove){
		
		bool isGoingUp = deltaMove.y > 0;
		float rayDistance = Mathf.Abs(deltaMove.y) + SkinWidth;
		Vector2 rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
		Vector2 rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;
		
		rayOrigin.x += deltaMove.x;
		
		float standingOnDistance = float.MaxValue;
		
		for(int i = 0; i < VerticalRaysCount; i++){
			
			Vector2 rayVector = new Vector2(rayOrigin.x + (i * _verticalRaySpacing), rayOrigin.y);
			Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);
			
			RaycastHit2D rayHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
			
			if(!rayHit)
				continue;
				
			if(!isGoingUp){
				
				float verticalDistanceToHit = _transform.position.y - rayHit.point.y;
				
				if(verticalDistanceToHit < standingOnDistance){
					
					standingOnDistance = verticalDistanceToHit;
					StandingOn = rayHit.collider.gameObject;
				}
			}
			
			deltaMove.y = rayHit.point.y - rayVector.y;
			rayDistance = Mathf.Abs(deltaMove.y);
			
			if(isGoingUp){
				
				deltaMove.y -= SkinWidth;
				State.IsCollidingUp = true;
			}else{
				
				deltaMove.y += SkinWidth;
				State.IsCollidingDown = true;
			}
			
			if(!isGoingUp && deltaMove.y > 0.0001f){
				State.IsMovingUpSlope = true;
			}
			
			if(rayDistance < SkinWidth + 0.0001f)
				break;
		}
	}
	
	//TODO: This results on problems where player is standing on a horizontal "box" in a slope.
	private void HandleSlopeVertical(ref Vector2 deltaMove){
		
		float center = (_raycastBottomLeft.x + _raycastBottomRight.x) / 2f;
		Vector2 direction = -Vector2.up;
		
		float slopeDistance = SlopeLimitTangent * (_raycastBottomRight.x - center);
		Vector2 slopeRayVector = new Vector2(center, _raycastBottomRight.y);
		
		Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.yellow);
		RaycastHit2D rayHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, PlatformMask);
		
		if(!rayHit)
			return;
			
		bool isMovingDownSlope = Mathf.Sign(rayHit.normal.x) == Mathf.Sign(deltaMove.x);
		
		if(!isMovingDownSlope)
			return;
			
		float angle = Vector2.Angle(rayHit.normal, Vector2.up);
		
		if(Mathf.Abs(angle) < 0.0001f)
			return;
			
		State.IsMovingDownSlope = true;
		State.SlopeAngle = angle;
		deltaMove.y = rayHit.point.y - slopeRayVector.y;
	}
	
	private bool HandleSlopeHorizontal(ref Vector2 deltaMove, float angle, bool isGoingRight){
		
		if(Mathf.RoundToInt(angle) == 90)
			return false;
			
		if(angle > Parameters.SlopeLimit){
			
			deltaMove.x = 0;
			return true;
		}
		
		//if(deltaMove.y > 0.07f)
		//	return true;
			
		//deltaMove.x += (isGoingRight ? -SkinWidth : SkinWidth);
		//deltaMove.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMove.x);
		float deltaV = deltaMove.x;
		deltaMove.x = Mathf.Cos(angle * Mathf.Deg2Rad) * deltaV;
		deltaMove.y = Mathf.Sin(angle * Mathf.Deg2Rad) * deltaV + SkinWidth;
		
		State.IsMovingUpSlope = true;
		State.IsCollidingDown = true;
		
		return true;
	}
	
	public void OnTriggerEnter2D(Collider2D other){
		
	}
	public void OnTriggerExit2D(Collider2D other){
		
	}
}
