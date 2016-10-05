using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {

	public GameObject PlayerPrefab = null;

	private GameObject _player = null;

	void Awake(){
		
		_player = GameObject.FindGameObjectWithTag("Player");
	}

	void FixedUpdate () {
	
		if(_player == null){
			
			_player = Instantiate(PlayerPrefab, transform.position, Quaternion.identity) as GameObject;
		}
	}
}
