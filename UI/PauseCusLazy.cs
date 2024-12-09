using Godot;
using System;

public partial class PauseCusLazy : Control
{
    public override void _Ready()
	{
		SetVolumeMetersCorrectly();
	}
	public void ExitToMenu(){
		GetNode<hud>("..").Pause();
		Input.MouseMode = Input.MouseModeEnum.Confined;
		game_manager.instance.MainMenu();
	}
	public void Return(){
		GetNode<hud>("..").Pause();
	}
	public void Restart(){
		GetNode<hud>("..").Pause();
		game_manager.instance.seed = new Random().Next();
		game_manager.instance.LoadGame();
	}
	public void SetVolumeMetersCorrectly(){
		Node sliders = GetNode("VolumeSliders");
		for(int i = 0; i < sliders.GetChildCount(); i++)
			sliders.GetChild(i).QueueFree();

		for(int i = 0; i < AudioServer.BusCount; i++){
			audioChanger AudioCH = new audioChanger(AudioServer.GetBusName(i));
			sliders.AddChild(AudioCH);
		}
	}
}
