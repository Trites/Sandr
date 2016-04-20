using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour {

	private Transform _transform;
	private Camera _camera;
	private Vector3 _cameraTargetOffset;
	private float _cameraTargetSize;
	private bool _isShaking;
	private float _shakeTimer;
	private float _defaultSize;	

	public void Awake(){
		
		_transform = transform;
		_camera = GetComponent<Camera>();
		_cameraTargetOffset = Vector3.zero;
		_defaultSize = _camera.orthographicSize;
		_cameraTargetSize = _defaultSize;
		_isShaking = false;
		_shakeTimer = 0;
	}
	
	public void Update(){
		
		ShakeCamera();
		_transform.localPosition = Vector3.Lerp(_transform.position, _transform.position + _cameraTargetOffset, 20 * Time.deltaTime);
		_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _cameraTargetSize, 9 * Time.deltaTime);
	}
	
	private void ShakeCamera(){
		
		if(_isShaking && _shakeTimer > 0){
			
			Vector2 direction = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0).normalized;
			float distance = Random.value*0.5f;
			_cameraTargetOffset = direction * distance;
			
			_shakeTimer -= Time.deltaTime;
		}else{
			
			
			_cameraTargetSize = _defaultSize;
			_cameraTargetOffset = Vector3.zero;
		}
		
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			
			_isShaking = true;
			_shakeTimer = 0.2f;
			_cameraTargetSize = 7.95f;
		}
	}
}
