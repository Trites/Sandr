using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class FollowPlayerGround : FollowPlayer {
	
    protected override void MoveTowardsTarget()
    {
        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, DirectionToTarget.x * MaxSpeed, Time.deltaTime * 5f));
    }

    protected override Vector2 SelectExploreDirection()
    {
        return new Vector2(Random.value - 0.5f, 0).normalized;
    }
}
