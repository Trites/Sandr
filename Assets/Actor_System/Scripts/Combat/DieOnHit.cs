using UnityEngine;
using System.Collections;

public class DieOnHit : MonoBehaviour {

	ParticleEmitter emitter;
	
	void Start(){
		
		emitter = GetComponent<ParticleEmitter>();
		emitter.emit = false;
		
		if(emitter == null)
			Debug.LogError("No emitter!");
	}
	
	void Update () {
	
	}
	
	public void HitBy(MeleeWeapon weapon){
		
		print("Hit!");
		emitter.Emit(50);
		
		Particle[] particles = emitter.particles;
		
		print("Simulating " + particles.Length + " particles.");
		for(int i = 0; i < particles.Length; i++){
			
			particles[i].velocity = new Vector2(0,1);
		}
		
		emitter.particles = particles;
	}
}
