using UnityEngine;

public class PlayerInput : MonoBehaviour {

	private const KeyCode JOYSTICK_JUMP = KeyCode.Joystick1Button0;	
	private const KeyCode JOYSTICK_DROP = KeyCode.Joystick1Button1;	
	private const KeyCode JOYSTICK_AIM = KeyCode.Joystick1Button4;
	private const KeyCode JOYSTICK_ATTACK = KeyCode.Joystick1Button5;

	public float Horizontal { get; private set; }
	public float Vertical { get; private set; }	
	public Vector2 RightStick { get; private set; }
	public bool Jump { get; private set; }
	public bool Drop { get; private set; }
	public bool Aim{ get; private set; }
	public bool Attack{ get; private set; }
	
	
	
	protected void Update () {
	
		Horizontal = Input.GetAxisRaw("Horizontal");
		Vertical = Input.GetAxis("Vertical");
		RightStick = new Vector2(Input.GetAxis("Right_Horizontal"), Input.GetAxis("Right_Vertical"));
		Jump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(JOYSTICK_JUMP);	
		Drop = Input.GetKeyDown(JOYSTICK_DROP);
		Aim = Input.GetKey(JOYSTICK_AIM);
		Attack = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(JOYSTICK_ATTACK);
	}
}
