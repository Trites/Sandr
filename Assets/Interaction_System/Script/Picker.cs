using System;
using UnityEngine;

public class Picker : MonoBehaviour {

	public GameObject PickUpType;
	public GameObject Refill;
	
	Action<GameObject> PickedUpEvent;
	
	protected void OnTriggerEnter2D(Collider2D collider){
		
		if(collider.gameObject.GetType() == PickUpType.GetType()){
			
			Destroy(collider.gameObject);
			Refill.SetActive(true);
		}
	}
}
