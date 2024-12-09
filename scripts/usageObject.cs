using Godot;
using System;

public partial class usableObject {
	public float Force;
	public Node executableNode;
	public Vector3 Position;
	public Vector3 Velocity;
	public Vector3 Rotation;

	public usableObject(movment a){
		Force = a.throwForce;
		Velocity = a.Velocity;
		Rotation = a.GetNode<Node3D>("Torzo/Head/Camera3D").GlobalRotation;
		Position = a.GetNode<Node3D>("Torzo/Head/Camera3D").GlobalPosition;
		executableNode = a.GetParent();
	}
}
