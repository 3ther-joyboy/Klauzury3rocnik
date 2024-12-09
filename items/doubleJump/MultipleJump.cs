
using Godot;
using System;

public partial class MultipleJump : Item
{
	int AdditionalJumps = 1;
	float JumpForce = 10;

	public MultipleJump(){}
	public MultipleJump(MultipleJump obj){
		this.AdditionalJumps = obj.AdditionalJumps;
	}

	public override MultipleJump Copy(){
		return new MultipleJump(this);
	}
	// this is dum
	public override PackedScene GetAsScene(){
		return GD.Load<PackedScene>("res://items/doubleJump/doubleJump.tscn");
	}

	private void Stats(bool x){
		movment player = this.GetNode<movment>("../..");
		player.MaxJumps += (x?1:-1)*AdditionalJumps;
	}
	public override void PassivesAdd(){
		Stats(true);
	}
	public override void PassivesRemove(){
		Stats(false);
	}

	public override void Use(usableObject info){
		this.Sound = "res://assets/SoundEffects/player/pl_ladder1.wav";
		this.GetNode<movment>("../..").Velocity += Vector3.Up*JumpForce;
		KYS();
	}
}
