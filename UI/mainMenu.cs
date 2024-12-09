using Godot;
using System;

public partial class mainMenu : Control
{
	String[] maps = new String[]{
		"res://worlds/ActuallLvl.tscn",
		/*"res://worlds/dalsiMapa.tscn"*/
	};
	public void StartGame(){
		String seed = GetNode<LineEdit>("MenuContainer/Seed").Text;
		try{
			if(seed.Length > 0)
				game_manager.instance.seed = seed.ToInt();
			else
				game_manager.instance.seed = new Random().Next();

			game_manager.instance.CurrentMap = GD.Load<PackedScene>(maps[new Random(game_manager.instance.seed).Next(0,maps.Length)]);
			game_manager.instance.Flush();
			game_manager.instance.LoadGame();
		} catch(Exception e){}
	}
	public void Quit(){
		GetTree().Quit();
	}

}
