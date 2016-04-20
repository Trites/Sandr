using UnityEngine;

public class FPSLog : MonoBehaviour {


	public float DisplayRefreshTime = 1f;
	public float FPSLowLimit = 10f;

	private float _fps;
	private float _displayFps;
	private float _timer;

	void OnGUI(){
		
		GUI.Label(new Rect(0, 0, 300, 50), "FPS: " + (int)(_displayFps));
	}
	
	public void Awake(){
		
		_displayFps = 0f;
		_timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
	
		_timer += Time.deltaTime;
		_fps = 1f/Time.deltaTime;
		
		if(_timer >= DisplayRefreshTime){
			
			_displayFps = _fps;
			_timer = 0;
		}
		
		//if(_fps < FPSLowLimit)
		//	Debug.LogWarning("Low fps: " + (int)(_fps));
	}
}
