using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	Rigidbody2D body;

	// Use this for initialization
	void Start () {

		body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		float move = Input.GetAxis("Horizontal");
		body = GetComponent<Rigidbody2D>();
		body.velocity = new Vector2(move * 10, body.velocity.y); 
	}
}
