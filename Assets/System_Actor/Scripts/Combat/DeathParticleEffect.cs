using UnityEngine;
using System.Collections;

public class DeathParticleEffect : MonoBehaviour {

	ParticleSystem emitter;
	
	public void Awake(){
		
		
	}

	public void Spawn(Vector2 impactPosition, Vector2 impactVelocity, Vector2 particleVelocity, float impactMass, float particleMassSpan, float particleMassAdjust, int count){
		
		emitter = GetComponent<ParticleSystem>();
		emitter.Emit(count);
		
		//emitter.emit = false;
		Debug.Log("Eeey");
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[emitter.particleCount];
		int a = emitter.GetParticles(particles);
	
		for(int i = 0; i < particles.Length; i++){
			
			float particleMass = Random.value * particleMassSpan + particleMassAdjust;
			particles[i].velocity = Util.getBounceVelocity(particles[i].position, impactPosition, particleVelocity, impactVelocity, particleMass, impactMass);
			//particles[i].velocity = new Vector2(0, 3);
		}
		
		emitter.SetParticles(particles, a);
	}
}
