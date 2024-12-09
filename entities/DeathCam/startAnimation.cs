using Godot;
using System;

public partial class startAnimation : AnimationPlayer
{
	[Export]
	String AnimationName;
	public override void _Ready()
	{
		this.Play(AnimationName);
	}
}
