using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnFinish : Area3D
{
	public static List<SpawnFinish> Locations = new List<SpawnFinish>();

	bool End = false;

	public override void _Ready()
	{
		Locations.Add(this);
	}
	public void SetAsFinish(){
		FinishMarker pack = GD.Load<PackedScene>("res://entities/finish_marker/finish_marker.tscn").Instantiate<FinishMarker>();
		pack.targetPosition = this.GlobalPosition;
		game_manager.instance.Player.player.GetNode("Torzo/Head").AddChild(pack);

		End = true;
	}
	public void VzpominkaEntered(Node3D x){
		if(game_manager.instance.Player.player == (movment)x)
			if(End)
				game_manager.instance.NextSicle();
	}
	public void VzpominkaExited(Node3D obj){
		if(obj.HasMethod("Invincible"))
			obj.Call("Invincible",false);
	}
	public Vector3 GetRandomPosition(Random rand){
		if(HasNode("Spawns")) {
			int randomIndex = rand.Next(0, this.GetNode("Spawns").GetChildCount());
			return this.GetNode("Spawns").GetChild<Marker3D>(randomIndex).GlobalPosition;
		}
		return this.GlobalPosition;
	}
}
