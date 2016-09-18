using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
/*
 * Finds all Collider2D components in the scene and draws yellow lines for debugging purpose.
 * To use, add the script to any gameObject in the scene you want to debug. Check in/out the
 * boxes in the script under Inspector to enable/disable specific types of colliders.
 * OBS! does not support rotation in X and Y.
 * Author: David Lindqist
 */
public class Collider2DDebugger : MonoBehaviour {

	public bool drawBoxColliders = true;
	public bool drawCircleColliders = true;
	public bool drawPolygonColliders = true;
	public bool drawEdgeColliders = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	/*
	 * Finds all collider2D components in the scene and call their draw functions.
	 */
	private void OnDrawGizmos(){
		Collider2D[] colliders = FindObjectsOfType (typeof(Collider2D)) as Collider2D[];

		foreach (Collider2D col in colliders) {
			if (col.GetType().IsAssignableFrom (typeof(CircleCollider2D)) && drawCircleColliders) {
					DrawCircleCollider2D ((CircleCollider2D)col);
			} else if (col.GetType ().IsAssignableFrom (typeof(PolygonCollider2D)) && drawPolygonColliders) {
					DrawPolygonCollider2D ((PolygonCollider2D)col);
			} else if (col.GetType ().IsAssignableFrom (typeof(BoxCollider2D)) && drawBoxColliders) {
					DrawBoxCollider2D ((BoxCollider2D)col);
			} else if (col.GetType ().IsAssignableFrom (typeof(EdgeCollider2D)) && drawEdgeColliders) {
				DrawEdgeCollider2D ((EdgeCollider2D)col);
			}
		}
	}

	/*
	 * Draws the lines for the PolygonCollider2D "col".
	 */
	private void DrawPolygonCollider2D(PolygonCollider2D col){

        Gizmos.color = col.enabled ? Color.yellow : Color.gray;
        
		Vector3 pos1 = new Vector3(0,0,0);
		Vector3 pos2= new Vector3(0,0,0);
		Vector3 tempV = new Vector3 (0,0,0);
		for (int i = 0; i < col.GetTotalPointCount()-1; i++) {
			tempV.Set(col.points[i].x, col.points[i].y, 0);
			pos1 = GetPositionWithTransform(col, tempV);
			tempV.Set(col.points[i+1].x, col.points[i+1].y, 0);
			pos2 = GetPositionWithTransform(col, tempV);
			Gizmos.DrawLine(pos1, pos2);
		}
		pos1 = GetPositionWithTransform(col, col.points[0]);
		Gizmos.DrawLine(pos2, pos1);
	}

	/*
	 * Draws a circle for the CircleCollider2D "col".
	 */
	private void DrawCircleCollider2D(CircleCollider2D col){
		UnityEditor.Handles.color = Color.yellow;
		UnityEditor.Handles.DrawWireDisc((Vector2)col.transform.position + col.offset ,Vector3.back, col.radius);
	}

	/*
	 * Draws the lines for the BoxCollider2D "col".
	 */
	private void DrawBoxCollider2D(BoxCollider2D col){
        Gizmos.color = col.enabled ? Color.yellow : Color.gray;
        
		Vector3 v1 = new Vector3(0,0,0);
		Vector3 v2 = new Vector3(0,0,0);
		Vector3 v3 = new Vector3(0,0,0);
		Vector3 v4 = new Vector3(0,0,0);
		float leftX = col.offset.x - (col.size.x / 2);
		float rightX = col.offset.x + (col.size.x / 2);
		float upperY = col.offset.y + (col.size.y / 2);
		float lowerY = col.offset.y - (col.size.y / 2);

		/* 
		 * v1----v2
		 * |	  |
		 * |	  |
		 * |	  |
		 * v4----v3
		 */

		v1.x = leftX;
		v1.y = upperY;
		v1 = GetPositionWithTransform (col, v1);

		v2.x = rightX;
		v2.y = upperY;
		v2 = GetPositionWithTransform (col, v2);

		v3.x = rightX;
		v3.y = lowerY;
		v3 = GetPositionWithTransform(col, v3);

		v4.x = leftX;
		v4.y = lowerY;
		v4 = GetPositionWithTransform (col, v4);

		Gizmos.DrawLine (v1,v2);
		Gizmos.DrawLine (v2,v3);
		Gizmos.DrawLine (v3,v4);
		Gizmos.DrawLine (v4,v1);
	}

	/*
	 * Draws the lines for the EdgeCollider2D "col".
	 */
	private void DrawEdgeCollider2D(EdgeCollider2D col){
        Gizmos.color = col.enabled ? Color.yellow : Color.gray;
        
		Vector3 pos = new Vector3(0,0,0);
		Vector3 nextPos= new Vector3(0,0,0);
		Vector3 tempV = new Vector3 (0,0,0);
		for (int i = 0; i < col.pointCount - 1; i++) {
			tempV.Set (col.points[i].x, col.points[i].y, 0);
			pos = GetPositionWithTransform(col, tempV);
			tempV.Set (col.points[i+1].x, col.points[i+1].y, 0);
			nextPos = GetPositionWithTransform(col, tempV);
			Gizmos.DrawLine(pos, nextPos);
		}
	}

	/*
	 * Returns the vector v1 in respect off the collider "col"s transformation
	 */
	private Vector3 GetPositionWithTransform(Collider2D col, Vector3 v){
		Vector3 newV = new Vector3 (0,0,0);


		//transform scale added
		newV.x = v.x * col.transform.lossyScale.x;
		newV.y = v.y * col.transform.lossyScale.y;

		//transform rotation Z added (parents already included by unity)
		newV = RotateZ (newV, col.transform.rotation.eulerAngles.z);

		//transform rotation X added (parents already included by unity)
		newV = RotateX (newV, col.transform.rotation.eulerAngles.x);

		//transform rotation Y added (parents already included by unity)
		newV = RotateY (newV, col.transform.rotation.eulerAngles.y);



		//transform position added (parents already included by unity)
		newV.x = newV.x + col.transform.position.x;
		newV.y = newV.y + col.transform.position.y;
		newV.z = newV.z + col.transform.position.z;



		return newV;
	}

	/*
	 * Rotates a vector3 "v" with given "degrees" around Z-axis
	 */
	private Vector3 RotateZ(Vector3 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
		
		float tempx = v.x;
		float tempy = v.y;
		v.x = (cos * tempx) - (sin * tempy);
		v.y = (sin * tempx) + (cos * tempy);
		return v;
	}

	/*
	 * Rotates a vector3 "v" with given "degrees" around Y-axis
	 */
	private Vector3 RotateY(Vector3 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
		
		float tempx = v.z;
		float tempy = v.x;
		v.z = (cos * tempx) - (sin * tempy);
		v.x = (sin * tempx) + (cos * tempy);
		return v;
	}

	/*
	 * Rotates a vector3 "v" with given "degrees" around X-axis
	 */
	private Vector3 RotateX(Vector3 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
		
		float tempx = v.y;
		float tempy = v.z;
		v.y = (cos * tempx) - (sin * tempy);
		v.z = (sin * tempx) + (cos * tempy);
		return v;
	}
	
}
#endif