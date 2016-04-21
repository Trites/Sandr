using UnityEngine;
using System.Collections;

public class StateHandler : MonoBehaviour {

	private CharacterState[] states;
	private CharacterState activeState;
	
	void Awake(){
		
		states = GetComponents<CharacterState>();
		
		foreach(CharacterState state in states){

			state.enabled = false;	
		}
		activeState = states[0];
		activeState.enabled = true;
		UpdateActiveState();
	}
	
	private void UpdateActiveState(){
		
		CharacterState prevState = activeState;
		foreach(CharacterState state in states){
			
			if(state.IsRelevant()){
				if(state.Priority > activeState.Priority){
						
					activeState = state;
				}
			}
		}
		
		if(activeState != prevState){
			
			prevState.enabled = false;
			activeState.enabled = true;
		}
	}
	
	void FixedUpdate(){
		
		UpdateActiveState();
	}
}
