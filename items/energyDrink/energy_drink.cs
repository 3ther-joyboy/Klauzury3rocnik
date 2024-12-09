using Godot;
using System;

public partial class energy_drink : Item
{
	[Export]
	float Speed = 1;
	[Export]
	float Dash = 20;

	public energy_drink(){
		this.Image = "res://items/energyDrink/drink.png";
	}
	public energy_drink(energy_drink obj):base(obj){
		this.Image = "res://items/energyDrink/drink.png";
		this.Speed = obj.Speed;
		this.Dash = obj.Dash;
	}

	public override void Use(usableObject info){
		Vector3 dir = new Vector3(0,0,-1);
		dir = dir.Rotated(new Vector3(1,0,0),info.Rotation.X);
		dir = dir.Rotated(new Vector3(0,1,0),info.Rotation.Y);

		player.Velocity += dir*Dash;

		this.Sound = "res://assets/SoundEffects/weapons/rocketfire1.wav";
		KYS();
	}

	public override Item Copy(){
		return new energy_drink(this);
	}

	public override PackedScene GetAsScene(){
		return GD.Load<PackedScene>("res://items/energyDrink/energy_drink.tscn");
	}
	private void Stats(bool a){
		player.Speed += (a?1:-1)*Speed;
	}
	public override void PassivesAdd(){
		Stats(true);
	}
	public override void PassivesRemove(){
		Stats(false);
	}

}
