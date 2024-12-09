
using Godot;
using System;

public abstract partial class Item : Node
{
	[ExportGroup("Wallks of text")]
	[Export]
	public String ItemName = "Extremly Op Item";
	[Export]
	public String Description = "You cant live without this";
    [Export(PropertyHint.File)]
	public String Image = "res://assets/icon.svg";

	public String Sound = "res://assets/SoundEffects/weapons/cbar_hit1.wav";

	public Item(){}
	public Item(Item obj){
		this.ItemName = obj.ItemName;
		this.Description = obj.Description;
		this.Image = obj.Image;
	}

	protected movment player;
	protected AudioStreamPlayer3D aud;

	public String GetName(){
		return GetAsScene().Instantiate<Item>().ItemName;
	}
	public String GetDescription(){
		return GetAsScene().Instantiate<Item>().Description;
	}
	public Texture2D GetImage(){
		return GD.Load<Texture2D>(GetAsScene().Instantiate<Item>().Image);
	}

    public override void _Ready()
	{
		player = GetNode<movment>("../..");
		aud = player.GetNode<AudioStreamPlayer3D>("itemSounds");
		PassivesAdd();
	}

	abstract public PackedScene GetAsScene();
	public ItemPickup SpawnItemCollectable(Vector3 position){
		ItemPickup pack = GD.Load<PackedScene>("res://entities/item_pickup/item_pickup.tscn").Instantiate<ItemPickup>();

		pack.Position = position;
		pack.SetIcon(GetImage());
		pack.SetInstance(this);

		game_manager.instance.CurrentScene.AddChild(pack);
		return pack;
	}

	abstract public Item Copy();
	abstract public void Use(usableObject info);
	abstract public void PassivesAdd();
	abstract public void PassivesRemove();
	protected void KYS(){
		aud.Stream = GD.Load<AudioStreamWav>(Sound);
		aud.Play();
		PassivesRemove();
		this.QueueFree();
	}
}
