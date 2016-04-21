using UnityEngine;
using System.Collections;

public class FollowPlayerAir : FollowPlayer {

	protected override void MoveTowardsTarget()
    {
		_controller.SetForce(Vector2.Lerp(_controller.Velocity, DirectionToTarget * MaxSpeed, Time.deltaTime * 5f));
    }

    protected override Vector2 SelectExploreDirection()
    {
        return new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized;
    }
}
