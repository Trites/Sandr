using UnityEngine;

public class Util {

    public static Vector2 getBounceVelocity(Vector2 centerA, Vector2 centerB, Vector2 velA, Vector2 velB, float massA, float massB){

		Vector2 line = (centerB - centerA).normalized;

		float a1 = Vector2.Dot(velA, line);
		float a2 = Vector2.Dot(velB, line);

		float p = (2f * (a2-a1))/(massA + massB);

		Vector2 dv = line*p*massB;

		return velA + dv;
    }
}
