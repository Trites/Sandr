using UnityEngine;

public abstract class CharacterState : MonoBehaviour {

	public int Priority = 0;
		
	protected CharacterController2D _controller;
	protected bool _isFacingRight;
	
	void Awake(){
				
		_controller = GetComponent<CharacterController2D>();
		_isFacingRight = transform.localScale.x > 0;
	}
	
    protected void Flip()
    {
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		_isFacingRight = transform.localScale.x > 0;
    }
	
	public abstract bool IsRelevant();
}
