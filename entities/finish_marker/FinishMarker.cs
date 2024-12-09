using Godot;
using System;

public partial class FinishMarker : Marker3D
{
	public Vector3 targetPosition;
    public override void _Ready()
	{
		this.LookAt(targetPosition);
		GetNode<Marker3D>("../..").Rotation = new Vector3(0,this.GlobalRotation.Y,0);
	}
	public override void _Process(double delta)
	{
		this.LookAt(targetPosition);
	}
}
