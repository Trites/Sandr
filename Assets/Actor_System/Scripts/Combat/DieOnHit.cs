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
	
	public void HitBy(MeleeWeapon.WeaponHitData hitData){
		
		print("Hit!");
		emitter.Emit(50);
		
		Particle[] particles = emitter.particles;
		
		print("Simulating " + particles.Length + " particles.");
		for(int i = 0; i < particles.Length; i++){
			
			float particleMass = Random.value * 10f + 2f;
			//particles[i].position = hitData.HitPosition;
			particles[i].velocity = Util.getBounceVelocity(particles[i].position, hitData.HitPosition, hitData.Direction*20,  -Vector2.right*10, particleMass, hitData.Force);
		}
		
		emitter.particles = particles;
		
		Debug.DrawLine(transform.position, hitData.HitPosition, Color.red, 3f);
	}
}
