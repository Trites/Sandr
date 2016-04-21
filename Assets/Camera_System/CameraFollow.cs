using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private const string PLAYER_TAG = "Player";

	public Transform Target = null;
	public float LerpFactor = 0.8f;
	
	public Vector2 Offset = Vector2.zero;

	private Transform _transform;

	public void Awake(){
		
		_transform = transform;
		
		if(Target == null)
			Target = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
		
		if(Target == null)
			Debug.LogError("Camera has no target.");
	}
	
	public void LateUpdate () {
	
		//_transform.position = Vector3.Lerp(_transform.position, Target.position + new Vector3(0, 0, -10), LerpFactor);		
		//_transform.position = new Vector3(Target.position.x, Target.position.y, -10);
		
		_transform.position = Vector3.Lerp(_transform.position, Target.position + new Vector3(Offset.x * Mathf.Sign(Target.localScale.x), Offset.y * Mathf.Sign(Target.localScale.y), -10), LerpFactor * Time.deltaTime);
	}
}
