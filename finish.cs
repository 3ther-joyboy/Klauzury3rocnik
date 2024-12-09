using Godot;
using System;

public partial class finish : Area3D
{
	public void Enter(Node3D x){
		if(game_manager.instance.Player.player == (movment)x){
			game_manager.instance.NextSicle();
		}else{
			((movment) x).KYS();
		}
	}
}
