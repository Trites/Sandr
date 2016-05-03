using UnityEngine;


public class Throwable : MonoBehaviour {


	public GameObject Thrown =  null;
	
	private PlayerInput _input;
	
	protected void Awake(){
		_input = GetComponentInParent<PlayerInput>();
	}
	
	protected void Update(){
		
		

		if(_input.Aim){
			
			Debug.DrawRay(transform.position, _input.RightStick * 5, Color.green);
			Time.timeScale = 0.1f;
			
			Vector2 direction = _input.RightStick.normalized;
			
			Vector2 vectorToTarget = new Vector2(direction.x * transform.lossyScale.x, direction.y).normalized;
			
			print(vectorToTarget);
			
 			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
 			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = q;
			
			if(_input.Attack){
			
				Throw(_input.RightStick.normalized, 50f);
			}
		}else{
			
			transform.rotation = transform.parent.rotation;
			Time.timeScale = 1f;
		}
		

	}
	
	private void Throw(Vector2 direction, float velocity){
		
		
			GameObject thrownSpear = Instantiate(Thrown, transform.position, Quaternion.identity) as GameObject;
			//thrownSpear.transform.localScale = new Vector3(Mathf.Sign(direction.x), transform.localScale.y, transform.localScale.z);		
			thrownSpear.GetComponent<Rigidbody2D>().velocity = direction * velocity;
			gameObject.SetActive(false);
			
			Vector2 vectorToTarget = direction;// * transform.localScale.x;
 			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
 			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			thrownSpear.transform.rotation = q;
			
			Time.timeScale = 1f;
	}
}
