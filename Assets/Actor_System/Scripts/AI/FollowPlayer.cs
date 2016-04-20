using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class FollowPlayer : MonoBehaviour {

	private const string _playerTag = "Player";
	private const float _exploreTimerMax = 5f;
	private const float _exploreTimerMin = 2f;
	
	public LayerMask BlockerLayer;
	public float MaxSpeed = 4f; 	
	
	private Transform _transform;
	private CharacterController2D _controller;
	private Transform _targetTransform;
	private bool _seePlayer;
	private float _exploreTimer;
	private Vector2 _directionToTarget;

	public void Awake(){
		
		_transform = transform;
		_controller = GetComponent<CharacterController2D>();
		_targetTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;
		_seePlayer = false;
		_exploreTimer = 0f;
		_directionToTarget = Vector2.zero;
		
		if(_targetTransform == null)
			Debug.LogError("Could not find target with tag: " + _playerTag);
	
		if(_controller == null)
			Debug.LogError("No controller.");
	}
	
	public void Update(){

		
		if(_seePlayer && Vector2.Distance(_transform.position, _targetTransform.position) > 1f){
			
			_directionToTarget = (_targetTransform.position - _transform.position).normalized;
	
		}else{
			
			if(_exploreTimer <= 0){
				
				_directionToTarget = new Vector2(Random.value - 0.5f, 0).normalized;
				_exploreTimer = Random.value * (_exploreTimerMax - _exploreTimerMin) + _exploreTimerMin;
			}else{
				
				_exploreTimer -= Time.deltaTime;
			}
		}
		
		
		_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _directionToTarget.x * MaxSpeed, Time.deltaTime * 5f));
	}
	
	public void FixedUpdate(){
		
		Vector2 targetVector = (_targetTransform.position - _transform.position);
		Vector2 directionToTarget = targetVector.normalized;
		float targetDistance = targetVector.magnitude;
		
		RaycastHit2D rayHit = Physics2D.Raycast(_transform.position, directionToTarget, targetDistance, BlockerLayer);
		
		if(rayHit){
			
			if(rayHit.distance >= targetDistance - 0.5f){
				_seePlayer = true;
			}else{
				
				_seePlayer = false;
			}
		}else{
			
			_seePlayer = true;
		}
		
	}
}
