using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : ItemSlot {

	PlayerInput _input;

	// Use this for initialization
	void Awake () {
		
		_input = GetComponentInParent<PlayerInput>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(_input.Drop){
			
			Drop();
		}
		
		if(_input.Attack){
			
			if(HeldItem != null){
				
				HeldItem.Use();
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		
		if(HeldItem != null)
			return;
		
		Item item = other.GetComponent<Item>();
		
		if(item != null){
			
			PickUp(item);
		}
	}
	
	protected override void OnPickup(Item item){
		
		item.transform.localPosition = new Vector2(0.8f, 0);
	}
}
