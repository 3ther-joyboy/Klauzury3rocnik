using Godot;
using System;

public partial class SpawnRandom : Node
{
	private const int chance = 3;
	public override void _Ready()
	{
		Random rand = new Random(game_manager.instance.seed);
		int e = this.GetChildCount();
		for (int i = 0; i < e; i++) {
			if(rand.Next()%chance != 0)
				GetChild(rand.Next(0,this.GetChildCount()-1)).QueueFree();
		}
	}
}

