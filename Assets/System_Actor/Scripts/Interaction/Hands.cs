using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : ItemSlot {

	PlayerInput _input;

	// Use this for initialization
	void Awake () {
		
		_input = GetComponent<PlayerInput>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(_input.Drop){
			
			Drop();
		}
	}
}
