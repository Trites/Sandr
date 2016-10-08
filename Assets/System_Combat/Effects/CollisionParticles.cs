using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticles : MonoBehaviour {
	
	void Start(){
		
		_timer = 2;
		//((Spawn(10, new Vector2(0, 1), new Vector2(1, -1));
	}
	
	
	float _timer;
	
	void Update(){
		return;
		_timer -= Time.deltaTime;
		
		if(_timer <= 0){
			
			Spawn(10, new Vector2(0, 1), new Vector2(1, -1));
			_timer = 2f;
		}
	}
	
	public void Spawn(int count, Vector2 normal, Vector2 inDirection){
		
		ParticleSystem emitter = GetComponent<ParticleSystem>();
		emitter.Emit(count);
		
		//emitter.emit = false;
		
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[emitter.particleCount];
		int a = emitter.GetParticles(particles);
	
		Vector2 outDirection = inDirection + 2 * normal;
	
		float jitter = Mathf.PI / 3f;
		
		float rotZ = transform.rotation.z;
		
		Vector2 pos = (Vector2)emitter.transform.position - new Vector2(Mathf.Cos(rotZ), Mathf.Sin(rotZ)).normalized * emitter.shape.radius;
	
		float angle = Mathf.Atan2(normal.y, normal.x);
	
	
		Debug.DrawRay(pos, outDirection, Color.cyan, 2f);
		
		
		Debug.DrawRay(pos, new Vector2(Mathf.Cos(angle + jitter), Mathf.Sin(angle + jitter)), Color.blue, 2f);		
		Debug.DrawRay(pos, new Vector2(Mathf.Cos(angle - jitter), Mathf.Sin(angle - jitter)), Color.green, 2f);	
	
		for(int i = 0; i < particles.Length; i++){
			
			//float angle = Random.Range(0, 2*Mathf.PI);
			//particles[i].velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(5f, 50f);
			
			//float frac = ((Vector2)particles[i].position - pos).magnitude / 2 * emitter.shape.radius;		
			float angleAdjust = Random.Range(-Mathf.PI/2, Mathf.PI/2);//jitter - (2 * jitter * frac);
			
			
			particles[i].velocity = new Vector2(Mathf.Cos(angle + angleAdjust), Mathf.Sin(angle + angleAdjust)).normalized * Random.Range(5f, 50f);
			
		}
		
		emitter.SetParticles(particles, a);
	
		Destroy(gameObject, 2f);
	}
}
