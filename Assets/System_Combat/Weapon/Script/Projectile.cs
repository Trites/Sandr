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
	
	public GameObject CollisionEffect;

	private Rigidbody2D _body;
	
	protected virtual void Awake(){
		
		_body = GetComponent<Rigidbody2D>();
	}
	
	public void SetVelocity(Vector2 direction){
		
		_body.velocity = direction * ProjectileVelocity;
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
 
		ProjectileTarget target = coll.gameObject.GetComponent<ProjectileTarget>();
		
		if(target != null){
			
			target.InvokeHit(Damage, new HitData(transform.position, _body.velocity.normalized, _body.velocity.magnitude*5000));
		}
		
		float ang = Vector2.Angle(coll.contacts[0].normal, Vector2.up);
		Vector3 cross = Vector3.Cross(coll.contacts[0].normal, Vector2.up);
		
		if(cross.z > 0)
			ang = 360 - ang;
		
		GameObject obj = Instantiate(CollisionEffect, coll.contacts[0].point + coll.contacts[0].normal * 0.1f, Quaternion.Euler(0, 0, ang));
		obj.GetComponent<CollisionParticles>().Spawn(30, coll.contacts[0].normal, _body.velocity.normalized);
		
		Debug.DrawRay(coll.contacts[0].point, coll.contacts[0].normal, Color.red, 5f);
		
		Destroy(gameObject);
    }
}
