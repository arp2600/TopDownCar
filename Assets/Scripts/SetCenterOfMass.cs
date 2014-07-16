using UnityEngine;
using System.Collections;

// Set the center of mass of a rigidbody2D
public class SetCenterOfMass : MonoBehaviour 
{
	public Vector2 _com; // The center of mass of the rigidbody

	// Updated everytime a value is changed in the inspector
	void OnValidate ()
	{
		rigidbody2D.centerOfMass = _com;
	}

	// Draw the center of mass as a blue wire sphere
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(rigidbody2D.GetRelativePoint(rigidbody2D.centerOfMass), 0.25f);
	}
}
