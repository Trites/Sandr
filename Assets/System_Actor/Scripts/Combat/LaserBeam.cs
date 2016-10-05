using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {


	void Awake(){
		
		Respawn();
	}

	public void Spawn(){
		
		gameObject.SetActive(true);
		Invoke("Respawn", 2f);
	}
	
	public void Respawn(){
		
		gameObject.SetActive(false);
		Invoke("Spawn", 1f);
	}
}
