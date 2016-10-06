using UnityEngine;
using System.Collections;

public class MeleeWeapon : Item {

	public struct WeaponHitData{
		
		public Vector2 HitPosition { get; private set; }
		public Vector2 Direction{ get; private set; }
		public float Force { get; private set; }
		
		public WeaponHitData(Vector2 hitPosition, Vector2 direction, float force){
			this.HitPosition = hitPosition;
			this.Direction = direction;
			this.Force = force;
		}
	}

	public LayerMask TargetLayer;
	public float Reach = 3.5f;
	
	public AudioSource StabSound;
	public AudioSource HitSound;
	
	private Transform _transform;	
	private PlayerInput _input;
	private bool _attacking;
	private CameraEffects _camera;
	
	protected override void Awake(){
		base.Awake();
		_transform = transform;
		_input = GetComponentInParent<PlayerInput>();
		GetComponentInParent<CharacterAnimationInterface>().EndAttackEvent += EndAttackAnim;
		_attacking = false;
		_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraEffects>();
		//_audio = GetComponent<AudioSource>();
	}
	
	public override void Use(){
		Animator anim = GetComponent<Animator>();
		
		if(anim == null)
			Debug.LogWarning("NULL");
		
		anim.SetTrigger("Attack");
		_attacking = true;
		_camera.ShakeCamera(0, 0.08f, 0.1f);
		StabSound.Play();
		//Strike();
	}
	
	public void Strike(){
		
		Vector2 position = _transform.position;
		
		Debug.DrawRay(position, Vector2.right * Mathf.Sign(_transform.localScale.x) * Reach, Color.cyan, 0.5f);
		
		RaycastHit2D rayHit = Physics2D.Raycast(position, Vector2.right * Mathf.Sign(_transform.localScale.x), Reach, TargetLayer);
		
		if(rayHit){
			
			rayHit.collider.gameObject.SendMessage("HitBy", new WeaponHitData(rayHit.point, (position - rayHit.point).normalized, 100f), SendMessageOptions.DontRequireReceiver);
		}
	}

	
	public void Update(){
		
		if(!_attacking && _input.Attack){
					
			Animator anim = GetComponentInParent<Animator>();
			anim.SetTrigger("Attack");
			_attacking = true;
			_camera.ShakeCamera(0, 0.08f, 0.1f);
			StabSound.Play();
		}
		

	}
	
	public void FixedUpdate(){
		
		if(_attacking){
			
			Vector2 position = _transform.position;
			Vector2 direction = (Vector2.right * Mathf.Sign(_transform.lossyScale.x)).normalized;
			Debug.DrawRay(position, direction * Reach, Color.cyan, 0.5f);	
			RaycastHit2D rayHit = Physics2D.Raycast(position, direction, Reach, TargetLayer);
			
			if(rayHit){
				
				HitSound.Play();
				//Time.timeScale = 0.1f;
				//Invoke("ResetTimeScale", 0.002f);
				rayHit.collider.gameObject.SendMessage("HitBy", new WeaponHitData(rayHit.point, direction, 100f), SendMessageOptions.DontRequireReceiver);
				_camera.ShakeCamera(0.5f, 0.05f, 0.2f);

			}
		}
	}
	
	public void ResetTimeScale(){
		
		Time.timeScale = 1f;
	}
	
	public void EndAttackAnim(){
		
		//print("End attack.");
		_attacking = false;
	}
}
