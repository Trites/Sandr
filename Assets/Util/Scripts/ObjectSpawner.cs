using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

	public GameObject SpawnObject;
	public float TimeMin = 10f;
	public float TimeMax = 15f;

	private float _timer;

	void Awake(){
		
		_timer = 0;
		
		if(SpawnObject == null)
			Debug.LogWarning("Object spawner subject is null.");
	}
	
	void Update () {
	
		if(_timer <= 0){
			
			Spawn();
			_timer = Random.value*(TimeMax - TimeMin) + TimeMin;
		}else{
			
			_timer -= Time.deltaTime;
		}
	}
	
	private void Spawn(){
		
		DeathParticleEffect emitter = (Instantiate(SpawnObject, transform.position, Quaternion.identity) as GameObject).GetComponent<DeathParticleEffect>();
		
	}
}
