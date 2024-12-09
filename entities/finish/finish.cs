using Godot;
using System;

public partial class finish : Area3D
{
	public void OnEnter(Node3D x){
		if(game_manager.instance.Player.player == x){
			game_manager.instance.Vzpominky.Add(game_manager.instance.Player);
			game_manager.instance.NextSicle();
		}
	}
}
