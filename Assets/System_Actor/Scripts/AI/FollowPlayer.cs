using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public abstract class FollowPlayer : CharacterState {

	protected const string _playerTag = "Player";


	public  float ExploreTimerMin = 2f;
	public float ExploreTimerMax = 5f;	
	public LayerMask BlockerLayer;
	public float MaxSpeed = 4f; 	
	
	public Vector2 DirectionToTarget { get { return _directionToTarget; }}
	public Vector2 DirectionToPlayer { get { return (_targetTransform.position - _transform.position).normalized; }}
	public float DistanceToPlayer { get { return (_targetTransform.position - _transform.position).magnitude; }}
	
	private Transform _transform;
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
	
	protected void Update(){
		
		if(_seePlayer && Vector2.Distance(_transform.position, _targetTransform.position) > 1f){
			
			_directionToTarget = DirectionToPlayer;
	
		}else{
			
			if(_exploreTimer <= 0){
				_directionToTarget = SelectExploreDirection();
				_exploreTimer = Random.value * (ExploreTimerMax - ExploreTimerMin) + ExploreTimerMin;
			}else{
				
				_exploreTimer -= Time.deltaTime;
			}
		}
		
		MoveTowardsTarget();
	}
	
	protected void FixedUpdate(){
		
		Vector2 targetVector = (_targetTransform.position - _transform.position);
		Vector2 directionToTarget = targetVector.normalized;
		float targetDistance = targetVector.magnitude;
		
		RaycastHit2D rayHit = Physics2D.Raycast(_transform.position, directionToTarget, targetDistance, BlockerLayer);
		
		if(rayHit){
			
			_seePlayer = false;

		}else{
			
			_seePlayer = true;
		}
		
	}
	
	public override bool IsRelevant(){
		
		return true;
	}
	
	protected abstract Vector2 SelectExploreDirection();
	
	protected abstract void MoveTowardsTarget();
}
