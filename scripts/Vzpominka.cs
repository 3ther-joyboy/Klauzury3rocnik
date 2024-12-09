using Godot;
using System;
using System.Collections.Generic;

public class Vzpominka
{
    public PackedScene playerModel = GD.Load<PackedScene>("res://entities/player.tscn");
    public movment player;

    public PackedScene[] itemy = new PackedScene[0];
    public Vector3 StartingPosition = Vector3.Zero;
    public List<RecordFrame> Recording = new List<RecordFrame>();
    public List<RecordFrame> RecordingCopy;



	public Vzpominka(){
	}
	public Vzpominka(Vzpominka inst){
		itemy = inst.itemy;
		StartingPosition = inst.StartingPosition;
		Recording = inst.Recording;
		if(Recording.Count > 0)
			RecordingCopy = new List<RecordFrame>(Recording);

	}
	public void SetToCurrentItems(){
		Node inv = player.GetNode("Items");
		itemy = new PackedScene[inv.GetChildCount()];
		for(int i = 0; i < inv.GetChildCount(); i++)
			itemy[i] = inv.GetChild<Item>(i).GetAsScene();
	}
	public RecordFrame[] GetRecording(){
		RecordFrame[] hold = Recording.ToArray();
		RecordFrame[] hold2 = new RecordFrame[hold.Length];
		for(int i = 0; i < hold.Length; i++)
			hold2[hold.Length-i-1] = hold[i];
		return hold;
	}
	public static Vzpominka DebugPlayerVzpominka(){
		Vzpominka obj = new Vzpominka();
		obj.itemy = new PackedScene[5];
		obj.itemy[0] = new trampoline().GetAsScene();
		obj.itemy[1] = new trampoline().GetAsScene();
		obj.itemy[2] = new trampoline().GetAsScene();
		obj.itemy[3] = new trampoline().GetAsScene();
		obj.itemy[4] = new trampoline().GetAsScene();
		return obj;
	}
	public void SpawnVzpominku(Node rootNode)
	{
		player = playerModel.Instantiate<movment>();

		player.Position = StartingPosition;

		Node inv = player.GetNode("Items");

		rootNode.AddChild(player);
		foreach (PackedScene item in itemy)
			inv.AddChild(item.Instantiate<Item>());
		player.GetNode<hud>("Torzo/Head/Camera3D/Control").UpdateInventory();

		SetToCurrentItems();

		RecordingCopy = new List<RecordFrame>(Recording);
	}
	public void NextFrame()
	{

		if(player != null && RecordingCopy.Count > 0){
			RecordFrame rn = RecordingCopy[0];
			RecordingCopy.RemoveAt(0);

			/*Marker3D head = player.GetNode<Marker3D>("Torzo/Head");*/
			/*head.Rotation = rn.looking;*/

			player.inputDir = rn.dir;

			if (rn.jump)
				player.Jump();

			player.Duck(rn.duck);

			if (rn.use){
				usableObject obj = new usableObject(player);
				obj.Rotation = rn.looking;
				player.GetNode<hud>("Torzo/Head/Camera3D/Control").UseItem(obj);
			}
		}
    }
}
