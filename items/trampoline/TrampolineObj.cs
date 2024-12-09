using Godot;
using System;

public partial class TrampolineObj : RigidBody3D
{
	[Export]
	float MaxVelocity = 10f;
	[Export]
	float Multiplayer = .5f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){}

	public void OnEnter(Node3D obj){
		movment ob = (movment)obj;
		Vector3 dir = Vector3.Up;

		// i bet there is better way of doing this
		dir = dir.Rotated(new Vector3(1,0,0),this.Rotation.X);
		dir = dir.Rotated(new Vector3(0,1,0),this.Rotation.Y);
		dir = dir.Rotated(new Vector3(0,0,1),this.Rotation.Z);

		if((dir + ob.Velocity.Project(dir)).Length() < ob.Velocity.Project(dir).Length()){
			ob.Velocity = ob.Velocity.Bounce(dir);
			if(ob.Velocity.Project(dir).Length() < MaxVelocity)
				ob.Velocity += ob.Velocity.Project(dir)*Multiplayer;
		}
	}
}
