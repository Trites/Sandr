using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour {

	public float DropForce = 10f;

	private Rigidbody2D _body;
	
	private void Awake(){
		
		_body = GetComponent<Rigidbody2D>();
	}

	public void PickedUp(){
		
		_body.isKinematic = true;
	}

	public void Dropped(){
		
		_body.isKinematic = false;
		Vector2 direction = new Vector2(Random.Range(-1f, 1f), 1).normalized;	
		_body.velocity = DropForce * direction;
	}
}
