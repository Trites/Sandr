﻿using System;
using UnityEngine;

[Serializable]
public class ControllerParameters{

	public enum JumpBehaviour{
		
		CanJumpOnGround,
		CanJumpAnywhere,
		CantJump
	}

	public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);
	
	[Range(0, 90)]
	public float SlopeLimit = 30f;
	
	public float Gravity = -25f;
	
	public JumpBehaviour JumpRestrictions;
	
	public float JumpFrequency = 0.25f;
	
	public float JumpMagnitude = 12f;
}
