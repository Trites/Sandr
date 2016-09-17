using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StateHandler))]
public class StateHandlerEditor : Editor {

	public override void OnInspectorGUI(){
		
		StateHandler handler = target as StateHandler;
		
		if(handler.ActiveState != null)
			EditorGUILayout.LabelField("State: ", handler.ActiveState.ToString());
	}
}
