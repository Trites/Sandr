using UnityEngine;
using System.Collections;

public class DeathParticleEffect : MonoBehaviour {

	ParticleEmitter emitter;
	
	public void Awake(){
		
		
	}

	public void Spawn(Vector2 impactPosition, Vector2 impactVelocity, Vector2 particleVelocity, float impactMass, float particleMassSpan, float particleMassAdjust, int count){
		
		emitter = GetComponent<ParticleEmitter>();
		emitter.Emit(count);
		
		//emitter.emit = false;
		
		Particle[] particles = emitter.particles;
	
		for(int i = 0; i < particles.Length; i++){
			
			float particleMass = Random.value * particleMassSpan + particleMassAdjust;
			particles[i].velocity = Util.getBounceVelocity(particles[i].position, impactPosition, particleVelocity, impactVelocity, particleMass, impactMass);
		}
		
		emitter.particles = particles;
	}
}
