using UnityEngine;

public abstract class ProjectileTarget : MonoBehaviour {

	protected abstract void OnHit(Projectile.DamageModel damage, Projectile.HitData hitData);

	public void InvokeHit(Projectile.DamageModel damage, Projectile.HitData hitData){
		
		OnHit(damage, hitData);
	}
}
