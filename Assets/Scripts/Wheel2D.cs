using UnityEngine;
using System.Collections;

// A wheel for a top down 2D car
// Attached gameObject must contain a RigidBody2D
public class Wheel2D : MonoBehaviour 
{
	public Vector2 _position; // The position of the wheel relative to the car
	public bool _is_drive = false; // Is the wheel a driving wheel
	public bool _is_steer = false; // Does the wheel steer

	public float _steer_angle = 0; // The max steering angle for the wheel
	public float _slip_velocity = 4; // The lateral velocity at which the wheel will begin to slip
	public float _lateral_weight = 1; // Effects the lateral force from the wheels, lower values cause the car to be more boat like
	public float _drive_force = 0; // The force driving wheels apply

	private Vector2 _relative_position; // The position of the wheel in world space
	private float _rotation; // The rotation of the wheel, takes into account steering
	private bool _is_skiding = false; // Is the wheel skiding

	public GameObject _tire_tracks; // Leave a trail of gameobjects behind the wheel when skidding

	void FixedUpdate () 
	{
		CalculateValues();
		KillLateralVelocity();
		ApplyDriveForce();
		DrawSkid();
	}

	// Calculate the wheel rotation and position for multiple use in the script
	void CalculateValues ()
	{
		// The rotation of the wheel
		_rotation = rigidbody2D.rotation;
		if (_is_steer)
			_rotation -= Input.GetAxis("Horizontal")*_steer_angle;
		// The position in world space
		_relative_position = rigidbody2D.GetRelativePoint(_position);
	}

	// Stop the sideways motion of a wheel
	void KillLateralVelocity ()
	{
		// Get the wheel veloctiy and discard the longitudinal component
		Vector2 wheel_velocity = rigidbody2D.GetRelativePointVelocity(_position);
		wheel_velocity = RotateVector(wheel_velocity, -_rotation);
		wheel_velocity.y = 0;

		// Set the _is_skiding flag if the wheel is skiding
		_is_skiding = (Mathf.Abs(wheel_velocity.x) >= _slip_velocity) ? true : false;

		// Clamp the velocity if it is over the slip threshold
		wheel_velocity.x = Mathf.Clamp(wheel_velocity.x, -_slip_velocity, _slip_velocity);

		// Rotate it back to world space and apply the force
		wheel_velocity = RotateVector(wheel_velocity, _rotation);
		rigidbody2D.AddForceAtPosition(-wheel_velocity * _lateral_weight, _relative_position);
	}

	// Apply driving force to driving wheels
	void ApplyDriveForce ()
	{
		if (!_is_drive)
			return;

		// Create a drive force
		Vector2 drive_force = new Vector2(0, _drive_force * Input.GetAxis("Vertical"));

		// Rotate it to world space and apply the force
		drive_force = RotateVector(drive_force, _rotation);
		rigidbody2D.AddForceAtPosition(drive_force, _relative_position);
	}

	void DrawSkid ()
	{
		// If there is a tire tracks prefab and _is_skiding
		if (_tire_tracks && _is_skiding)
			Instantiate(_tire_tracks, _relative_position, Quaternion.identity);
	}

	// Draw the wheel in the scene view, red wheels are driving wheels, green are not
	public void OnDrawGizmosSelected ()
	{
		if (_is_drive)
			Gizmos.color = Color.red;
		else
			Gizmos.color = Color.green;
		Gizmos.DrawWireSphere((Vector3)rigidbody2D.GetRelativePoint(_position), 0.25f);
	}

	// Rotate a vector v by the angle theta
	Vector2 RotateVector (Vector2 v, float theta)
	{
		float sin = Mathf.Sin(theta * Mathf.Deg2Rad);
		float cos = Mathf.Cos(theta * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;

		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);

		return v;
	}
}
