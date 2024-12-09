using Godot;
using System;

public partial class KillArea : Area3D
{
	public void OnBodyEntered(Node3D obj){
		if(obj.GetType() == typeof(movment)){
			((movment)obj).KYS();
		}
	}
}
