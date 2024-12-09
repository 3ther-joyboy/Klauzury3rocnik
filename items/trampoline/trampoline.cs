using Godot;
using System;

public partial class trampoline : Item
{
	[Export]
	float Jump = 1;
	[Export]
	PackedScene trampolineScene = GD.Load<PackedScene>("res://items/trampoline/TrampolineObject.tscn");

	public trampoline(){}
	//?? its alredy set
	public trampoline(trampoline obj):base(obj){
		this.Jump = obj.Jump;
		this.trampolineScene = obj.trampolineScene ;
	}
	public override PackedScene GetAsScene(){
		return GD.Load<PackedScene>("res://items/trampoline/trampoline.tscn");
	}
	private void Stats(bool x){
		movment player = this.GetNode<movment>("../..");
		player.JumpVelocity += (x?1:-1)*Jump;
	}
	public override void PassivesAdd(){
		Stats(true);
	}
	public override void PassivesRemove(){
		Stats(false);
	}
	public override Item Copy(){
		return new trampoline(this);
	}
	public override void Use(usableObject info){
		TrampolineObj tr = trampolineScene.Instantiate<TrampolineObj>();
		Vector3 dir = new Vector3(0,0,-1);
		dir = dir.Rotated(new Vector3(1,0,0),info.Rotation.X);
		dir = dir.Rotated(new Vector3(0,1,0),info.Rotation.Y);
		tr.Position = info.Position + dir*.5f;
		tr.SetAxisVelocity(dir * info.Force);
		info.executableNode.AddChild(tr);
		KYS();
	}
}
