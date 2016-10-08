using UnityEngine;
using System.Collections;

public class LaserRifle : MonoBehaviour {

	private const float COOLDOWN_TIME = 2.0f;
	private const float FIRE_EFFECT_DURATION = 1f;

	public Material AimMaterial;
	public Material FireMaterial;
	
	public bool ReadyToFire { get { return _coolDown <= 0.0f; }}
	
	
	private LineRenderer _lineRenderer;
	
	private float _coolDown;
	
	private CameraEffects _camera;

	private void Awake(){
		
		_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraEffects>();
		_lineRenderer = GetComponent<LineRenderer>();
		_coolDown = 0f;
	}
	
	protected void Update(){
		
		if(_coolDown > 0.0f){
			
			_coolDown -= Time.deltaTime;	
		}
	}
	
	public void Fire(Vector2 direction, float range, LayerMask blockingLayers){
		
		_lineRenderer.startWidth = 0.5f;
		_lineRenderer.endWidth = 0.5f;
		_lineRenderer.material = FireMaterial;	
		_lineRenderer.SetPositions(new Vector3[]{transform.position, GetBeamHitPoint(direction, range, blockingLayers)});
		
		_coolDown = COOLDOWN_TIME;
		Invoke("HideBeam", FIRE_EFFECT_DURATION);
		
		RaycastHit2D rayHit = Physics2D.Raycast(transform.position, direction, range, blockingLayers);
		
		if(rayHit){
			
			if(rayHit.collider.tag == "Player"){
				
				rayHit.collider.GetComponent<DieOnHit>().HitBy(new MeleeWeapon.WeaponHitData(rayHit.point, ((Vector2)transform.position - rayHit.point).normalized, 100f));
			}
		}
		
		_camera.ShakeCamera(1f, 0f, 0.5f);
	}

	public void Aim(Vector2 direction, float range, LayerMask blockingLayers){
		
		_lineRenderer.enabled = true;
		_lineRenderer.startWidth = 0.5f;
		_lineRenderer.endWidth = 0.5f;
		_lineRenderer.material = AimMaterial;	
		_lineRenderer.SetPositions(new Vector3[]{transform.position, GetBeamHitPoint(direction, range, blockingLayers)});
	}
	
	private Vector2 GetBeamHitPoint(Vector2 direction, float range, LayerMask blockingLayers){
		
		RaycastHit2D rayHit = Physics2D.Raycast(transform.position, direction, range, blockingLayers);
		Debug.DrawRay(transform.position, direction * range, Color.blue);
		
		if(rayHit){
			
			return (Vector2)transform.position + direction * range * rayHit.fraction;
		}else{
			
			return (Vector2)transform.position + direction * range;
		}
	}
	
	private void HideBeam(){
		
		_lineRenderer.enabled = false;
	}
}
