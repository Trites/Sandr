using UnityEngine;
using System.Collections;

public class ShootPlayer : CharacterState {

	public Transform target;
	public LayerMask BlockingLayers;
	
	private LaserRifle _laserRifle;
	private Vector2 _shootingAt;
	private bool _firing;
	private float _aimTimer;
	
	private float _maxRange = 30.0f;
	
	protected override void Awake(){
		base.Awake();
		
		_laserRifle = GetComponentInChildren<LaserRifle>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	protected void Start(){
		
	}
	
	public override void OnActivate(){
			
		_controller.SetForce(Vector2.zero);
		_firing = false;
		print("FIRE!");
	}
	
	protected void Update(){
		
		//if(!_laserRifle.ReadyToFire)
		//	return;
		
		if(!_firing){
			
			_shootingAt = (target.position - transform.position).normalized;
			_firing = true;
			_aimTimer = 1.0f;
			Debug.DrawRay(transform.position, _shootingAt * 30f, Color.cyan, 0.5f);
			
		}else{
		
				
			if(_aimTimer >= 0.0f){
			
				_laserRifle.Aim(_shootingAt, _maxRange, BlockingLayers);
				_aimTimer -= Time.deltaTime;
			}else{
			
				_laserRifle.Fire(_shootingAt, _maxRange, BlockingLayers);
				_firing = false;
			}	
		}
	}


	public override bool IsRelevant(){
		
		
		Debug.DrawLine(transform.position, target.position, Color.magenta);
		Vector2 targetVector = target.position - transform.position;
		RaycastHit2D rayHit = Physics2D.Raycast(transform.position, targetVector.normalized, targetVector.magnitude, BlockingLayers);		
		
		return (_firing || rayHit.fraction >= 0.9f) && _laserRifle.ReadyToFire;
	}
}
