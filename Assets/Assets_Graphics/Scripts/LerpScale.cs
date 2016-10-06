using UnityEngine;

public class LerpScale : MonoBehaviour {

	public Vector2 DefaultScale = new Vector2(1, 1);
	
	public float LerpStep = 1f;
	
	void Update () {
	
		transform.localScale = Vector2.Lerp(transform.localScale, DefaultScale, LerpStep * Time.deltaTime);
	}
}
