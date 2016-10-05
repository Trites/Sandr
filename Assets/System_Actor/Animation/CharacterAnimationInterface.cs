using UnityEngine;
using System;

public class CharacterAnimationInterface : MonoBehaviour {

	public Action EndAttackEvent;

	public void EndAttack(){
		
		if(EndAttackEvent != null)
			EndAttackEvent.Invoke();
	}
}
