using UnityEngine;

public class CameraEffects : MonoBehaviour {

	private Transform _transform;
	private Camera _camera;
	private Vector3 _cameraTargetOffset;
	private float _cameraTargetSize;
	private bool _isShaking;
	private float _shakeMagnitude;
	private float _shakeTimer;
	private float _shakeLerpFactor;
	private float _zoomLerpFactor;
	private float _defaultSize;	

	public void Awake(){
		
		_transform = transform;
		_camera = GetComponent<Camera>();
		_cameraTargetOffset = Vector3.zero;
		_defaultSize = _camera.orthographicSize;
		_cameraTargetSize = _defaultSize;
		_isShaking = false;
		_shakeTimer = 0;
		_shakeLerpFactor = 0f;
		_zoomLerpFactor = 0f;
	}
	
	public void Update(){
		
		UpdateCameraShake();
		_transform.localPosition = Vector3.Lerp(_transform.position, _transform.position + _cameraTargetOffset, _shakeLerpFactor * Time.deltaTime);
		_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _cameraTargetSize, _zoomLerpFactor * Time.deltaTime);
	}
	
	private void UpdateCameraShake(){
		
		if(_isShaking && _shakeTimer > 0){
			
			Vector2 direction = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0).normalized;
			float distance = Random.value*_shakeMagnitude;
			_cameraTargetOffset = direction * distance;
			
			_shakeTimer -= Time.deltaTime;
		}else{
			
			
			_cameraTargetSize = _defaultSize;
			_cameraTargetOffset = Vector3.zero;
		}
	}
	
	public void ShakeCamera(float magnitude, float deltaZoom, float duration, float shakeLerpFactor = 20f, float zoomLerpFactor = 10f){
		
		_isShaking = true;
		_shakeMagnitude = magnitude;	
		_cameraTargetSize -= deltaZoom;
		_shakeTimer = duration;
		_shakeLerpFactor = shakeLerpFactor;
		_zoomLerpFactor = zoomLerpFactor;
	}
}
