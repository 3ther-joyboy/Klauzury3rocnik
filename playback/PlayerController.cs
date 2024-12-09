using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerController : Node
{
    [Export(PropertyHint.Range,"0,0.5,0.05")]
    public static float MouseSensitivity = .05f;

	[Export]
	public movment player;
	public Vzpominka replay = new Vzpominka();

	private RecordFrame current = new RecordFrame();

    public override void _Ready()
	{
        Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	public void GetPlayerReady(bool TF){
		if(player != null){
			player.GetNode<Camera3D>("Torzo/Head/Camera3D").Current = TF;
			AudioListener3D ears = player.GetNode<AudioListener3D>("Torzo/Head/ears");
			if (TF)
				ears.MakeCurrent();
			else
				ears.ClearCurrent();

			player.GetNode<Control>("Torzo/Head/Camera3D/Control").Visible = TF;

			player.GetNode<Area3D>("Hitbox").Monitoring = !TF;


			player.GetNode<ProgressBar>("Torzo/Head/Camera3D/Control/timer").MaxValue = GetNode<Timer>("Timer").WaitTime;
		}
	}

    public override void _Process(double delta)
	{
		if(player != null)
			player.GetNode<ProgressBar>("Torzo/Head/Camera3D/Control/timer").Value = GetNode<Timer>("Timer").TimeLeft;
	}
	public override void _PhysicsProcess(double delta)
	{
		game_manager.instance.NextStep();	
		if(player != null){
			current = new RecordFrame();

			player.inputDir = Input.GetVector("Left", "Right", "Forwad", "Back").Rotated(-player.GetNode<Marker3D>("Torzo").Rotation.Y);
			current.dir = player.inputDir;

			if (Input.IsActionJustPressed("Jump")){
				player.Jump();
				current.jump = true;
			}
			
			player.Duck(Input.IsActionPressed("Duck"));
			current.duck = Input.IsActionPressed("Duck");

			if(Input.IsActionJustPressed("Use")){
				current.used = player.UseItem();
				current.use = true;
				current.looking = player.GetNode<Node3D>("Torzo/Head/Camera3D").GlobalRotation;
			}

			replay.Recording.Add(new RecordFrame(current));
		}
	}

	public override void _Input(InputEvent @event)
	{
		if(player != null)
		{
			Marker3D head = player.GetNode<Marker3D>("Torzo/Head");
			if (@event is InputEventMouseMotion mouseEvent) {
				Vector2 rotationAmount = mouseEvent.Relative * MouseSensitivity;
				player.GetNode<Marker3D>("Torzo").RotateY(Mathf.DegToRad(rotationAmount.X) * -1);
				head.Rotation -= new Vector3(Mathf.DegToRad(rotationAmount.Y), 0, 0);
				head.Rotation = new Vector3(Mathf.Clamp(head.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(90)),0,head.Rotation.Z);
			}
		}
	}
	public void KYS(){
		if(player != null)
			player.KYS();

	}
}
