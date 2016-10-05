using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour {

	public float DropForce = 20f;

	private Rigidbody2D _body;
	
	private void Awake(){
		
		_body = GetComponent<Rigidbody2D>();
	}

	public void PickedUp(){
		
		_body.isKinematic = true;
		_body.velocity = Vector2.zero;
		_body.angularVelocity = 0f;
	}

	public void Dropped(){
		
		_body.isKinematic = false;
		Vector2 direction = new Vector2(Random.Range(-0.5f, 0.5f), 1).normalized;	
		_body.velocity = DropForce * direction;
	}
}
