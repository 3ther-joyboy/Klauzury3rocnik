using Godot;
using System;

public partial class RockFriend : Item
{
	String rock = "res://items/RockFriend/rockProjectile.tscn";
	Node3D inst;
	public RockFriend(){Redy();}
	public RockFriend(RockFriend rock){Redy();}
	public override PackedScene GetAsScene(){return GD.Load<PackedScene>("res://items/RockFriend/RockSomething.tscn");}
	public override Item Copy(){return new RockFriend();}

	private void Redy(){
		this.ItemName = "MR.Rock";
		this.Description = "Your friend watching your every move";
		this.Image = "res://items/RockFriend/MRRock.png";
		this.Sound = "res://assets/SoundEffects/weapons/357_shot2.wav";
	}

	public override void PassivesAdd(){
		inst = GD.Load<PackedScene>("res://items/RockFriend/RotatingRock.tscn").Instantiate<Node3D>();
		inst.GetNode<AnimationPlayer>("AnimationPlayer").Play("rotating");
		inst.GetNode<AnimationPlayer>("AnimationPlayer").Seek(inst.GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimationLength/(1f * new Random().Next(1,100)));
		this.GetNode("../..").AddChild(inst);
		inst.Position = Vector3.Up*1.5f;
	}

	public override void PassivesRemove(){
		inst.QueueFree();
	}

	public override void Use(usableObject info){

		RigidBody3D rok = GD.Load<PackedScene>(rock).Instantiate<RigidBody3D>();
		Vector3 dir = new Vector3(0,0,-1);
		dir = dir.Rotated(new Vector3(1,0,0),info.Rotation.X);
		dir = dir.Rotated(new Vector3(0,1,0),info.Rotation.Y);
		rok.Position = info.Position + dir*2f;
		rok.SetAxisVelocity(dir * info.Force * 5f);
		info.executableNode.AddChild(rok);
		KYS();
	}
}
