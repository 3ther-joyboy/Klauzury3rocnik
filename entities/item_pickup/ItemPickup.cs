using Godot;
using System;

public partial class ItemPickup : Area3D
{
	private static Random rng = new Random();
	public Item item;
	public ItemPickup(){}
    public override void _Ready()
	{
		AnimationPlayer animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animPlayer.Play("spinning");
		animPlayer.Seek(animPlayer.CurrentAnimationLength/(1f * rng.Next(1,100)));

		GetNode<RayCast3D>("RayCast3D").ForceRaycastUpdate();
		this.Position = GetNode<RayCast3D>("RayCast3D").GetCollisionPoint() + Vector3.Up;
	}

	public void SetInstance(Item obj){
		item = obj;
	}
	public void OnEnter(Node3D obj){
		if(obj.GetType()+"" == "movment"){
			obj.GetNode<hud>("Torzo/Head/Camera3D/Control").AddItem(item);
			obj.GetNode<AudioStreamPlayer3D>("itemSounds").Stream = GD.Load<AudioStreamWav>("res://assets/SoundEffects/button9.wav");
			obj.GetNode<AudioStreamPlayer3D>("itemSounds").Play();
			this.QueueFree();
		}
	}
	public void SetIcon(Texture2D txture){
		GetNode<Sprite3D>("Icon").Texture = txture;
	}
}
