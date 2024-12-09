using Godot;
using System;

public partial class DeathCam : Camera3D
{
	public Marker3D trackTarget;
	public override void _Ready()
	{
		GetNode<Timer>("Timer").Start();
		GetNode<Label>("Control/Vbux/Label").Text += ": " + game_manager.instance.Vzpominky.Count;
		Input.MouseMode = Input.MouseModeEnum.Confined;
	}

	public override void _Process(double delta)
	{
		this.LookAt(trackTarget.GlobalPosition,Vector3.Up);
	}

	public void UiStart(){
		AnimationPlayer animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animPlayer.Play("menu_popup");
	}

	public void BackToMenu(){
		game_manager.instance.MainMenu();
	}
	public void RestartThis(){
		Restart(game_manager.instance.seed);
	}
	public void RestartRandom(){
		Restart(new Random().Next());
	}
	public void Restart(int seed){
		game_manager.instance.seed = seed;
		game_manager.instance.Flush();
		game_manager.instance.LoadGame();
	}
}
