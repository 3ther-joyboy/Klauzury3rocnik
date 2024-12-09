using Godot;
using System;

public partial class audioChanger : VBoxContainer
{

	private String Name;
	private HSlider NewSlider;


	public audioChanger(){}
	public audioChanger(String name){
		this.Name = name;
	}
	public override void _Ready()
	{
		NewSlider = new HSlider();
		Label NewLableName = new Label();
		NewLableName.Text = Name;

		NewSlider.MaxValue = 1.3f;
		NewSlider.Step = .1f;
		NewSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex(Name)));
		NewSlider.ValueChanged += (change) => SetAudioTo((float)change);


		AddChild(NewSlider);
		AddChild(NewLableName);
	}
	// YESS
	public void SetAudioTo(float change){
		GD.Print(Name + ": " + change);
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Name),Mathf.LinearToDb(change));
	}
}
