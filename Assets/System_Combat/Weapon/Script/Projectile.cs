using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour {

	public struct DamageModel{
		
		public DamageModel(float damage, float knockback){
			
			Damage = damage;
			Knockback = knockback;
		}
		
		public float Damage;
		public float Knockback;
	}
	
	public struct HitData{
		
		public Vector2 HitPosition { get; private set; }
		public Vector2 Direction{ get; private set; }
		public float Force { get; private set; }
		
		public HitData(Vector2 hitPosition, Vector2 direction, float force){
			this.HitPosition = hitPosition;
			this.Direction = direction;
			this.Force = force;
		}
	}
	
	public DamageModel Damage;

	public float ProjectileVelocity = 20;

	private Rigidbody2D _body;
	
	protected virtual void Awake(){
		
		_body = GetComponent<Rigidbody2D>();
	}
	
	public void SetVelocity(Vector2 direction){
		
		_body.velocity = direction * ProjectileVelocity;
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
        
		ProjectileTarget target = coll.GetComponent<ProjectileTarget>();
		
		if(target != null){
			
			target.InvokeHit(Damage, new HitData(transform.position, _body.velocity.normalized, _body.velocity.magnitude*20));
		}
		
		Destroy(gameObject);
    }
}
