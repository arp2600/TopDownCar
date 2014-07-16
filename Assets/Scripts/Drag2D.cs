using UnityEngine;
using System.Collections;

// Apply a drag force proportional to the squared velocity of a rigidbody2D
public class Drag2D : MonoBehaviour 
{
	public float drag_k = 1; // The coefficient of drag

	void FixedUpdate () 
	{
		Vector2 drag_force = -rigidbody2D.velocity.normalized * Mathf.Pow(rigidbody2D.velocity.magnitude, 2) * drag_k;
		rigidbody2D.AddForce(drag_force);
	}
}
