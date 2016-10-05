using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBurst : MonoBehaviour {

	ParticleSystem _particle;

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
		if(Time.timeSinceLevelLoad > 2){
			_particle = GetComponent<ParticleSystem>();
			_particle.Emit(200);
			Destroy(this);
		}
	}
}
