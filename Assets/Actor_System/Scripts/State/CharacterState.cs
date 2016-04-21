using UnityEngine;

public abstract class CharacterState : MonoBehaviour {

	public int Priority = 0;
	
	protected bool _isFacingRight { get{ return _facingDirection == 1; }}	
	protected CharacterController2D _controller;
	protected int _facingDirection;
	
	void OnGUI(){
	
		GUI.Label(new Rect(0, 15, 300, 50), "Velocity: " + _controller.Velocity + " (" + _controller.Velocity.magnitude + ")");
		GUI.Label(new Rect(0, 30, 300, 50), "State: " + _controller.State);
	}
	
	void Awake(){
				
		_controller = GetComponent<CharacterController2D>();
		_facingDirection = (int)Mathf.Sign(transform.localScale.x);
	}
	
	protected virtual void Update(){
		
		_facingDirection = (int)Mathf.Sign(transform.localScale.x);
	}
	
    protected void Flip()
    {
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		_facingDirection = (int)Mathf.Sign(transform.localScale.x);
    }
	
	public virtual void OnActivate(){}
	
	public abstract bool IsRelevant();
}
