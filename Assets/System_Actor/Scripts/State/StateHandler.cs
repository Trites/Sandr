using UnityEngine;

public class StateHandler : MonoBehaviour {

	private CharacterState[] states;
	private CharacterState activeState;
	
	public CharacterState ActiveState { get {return activeState; }}
	
	void Start(){
		
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
		CharacterState nextState = (activeState.IsRelevant() ? activeState : null);
		
		foreach(CharacterState state in states){
			
			if(state.IsRelevant()){
				if(nextState == null || state.Priority > nextState.Priority){
						
					nextState = state;
				}
			}
		}
		
		if(nextState != prevState){
			
			prevState.enabled = false;
			
			if(nextState != null){
				Debug.Log("State transition: " + prevState + " -> " + nextState);
				nextState.enabled = true;
				nextState.OnActivate();
			}
		}
		
		activeState = nextState;
	}
	
	void LateUpdate(){
		
		UpdateActiveState();
	}
}
