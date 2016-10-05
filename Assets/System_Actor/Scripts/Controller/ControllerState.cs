
public class ControllerState {
	
	public bool IsCollidingRight { get; set; }
	public bool IsCollidingLeft { get; set; }
	public bool IsCollidingUp { get; set; }
	public bool IsCollidingDown { get; set; }
	
	public bool IsMovingUpSlope { get; set; }
	public bool IsMovingDownSlope { get; set; }
	
	public float SlopeAngle { get; set; }
	
	public bool IsColliding { get {return IsCollidingLeft || IsCollidingRight || IsCollidingDown || IsCollidingUp;} }
	
	public void Reset(){
		
		IsCollidingRight = false;
		IsCollidingLeft = false;
		IsCollidingUp = false;
		IsCollidingDown = false;
		IsMovingUpSlope = false;
		IsMovingDownSlope = false;
		SlopeAngle = 0;
	}
	
	public override string ToString(){
		
		return string.Format("(controller: r: {0} l: {1} u: {2} d: {3} up-slope: {4} down-slope: {5} angle: {6})",
		IsCollidingRight, IsCollidingLeft, IsCollidingUp, IsCollidingDown, IsMovingUpSlope, IsMovingDownSlope, SlopeAngle);
	}
}
