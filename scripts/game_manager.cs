using Godot;
using System;
using System.Collections.Generic;

public partial class game_manager : Node
{
	public static game_manager instance;

	public List<Vzpominka> Vzpominky;
	public int seed;
	public Vzpominka Player;

	public PackedScene CurrentMap;
	public Node CurrentScene;


	// this is stoopid
	public Item[] items = new Item[]{
		new trampoline(),
		new energy_drink(),
		new MultipleJump(),
		new RockFriend(),
	};

    public override void _Ready()
	{
		game_manager.instance = this;

		CurrentScene = GetTree().Root.GetChild(1);
	}

	public override void _Input(InputEvent @event)
	{
		if(Input.IsKeyPressed(Godot.Key.F1)) {
			Flush();
			seed = 1;
			CurrentMap = GD.Load<PackedScene>("res://worlds/testWorld.tscn");

			Player = Vzpominka.DebugPlayerVzpominka();
			LoadGame();
		}
		if(Input.IsKeyPressed(Godot.Key.F2))
			SpawnItem();
		if(Input.IsKeyPressed(Godot.Key.F3))
			seed = new Random().Next(0,Int16.MaxValue);
	}
	public void SpawnItem(){
		Random rand = new Random(seed);
		Vector3 one = CurrentScene.GetNode("ItemSpawnLocation").GetChild<Marker3D>(0).GlobalPosition;
		Vector3 two = CurrentScene.GetNode("ItemSpawnLocation").GetChild<Marker3D>(1).GlobalPosition;

		for(int i = 0; i < Vzpominky.Count; i++){
			Vector3 spawn = new Vector3(
			(one.X < two.X)? rand.Next((int)one.X,(int)two.X):rand.Next((int)two.X,(int)one.X),
			(one.Y < two.Y)? rand.Next((int)one.Y,(int)two.Y):rand.Next((int)two.Y,(int)one.Y),
			(one.Z < two.Z)? rand.Next((int)one.Z,(int)two.Z):rand.Next((int)two.Z,(int)one.Z)
			);

			items[rand.Next(0,items.Length)].Copy().SpawnItemCollectable(spawn);
		}
	}

	public void NextSicle(){
		Player.Recording = CurrentScene.GetNode<PlayerController>("PlayerController").replay.Recording;
		Vzpominky.Add(new Vzpominka(Player));
		Player.SetToCurrentItems();
		
		Player = new Vzpominka(Player);
		LoadGame();
		SpawnItem();
	}
	public void Flush(){
		Player = new Vzpominka();
		Vzpominky = new List<Vzpominka>();
	}
	public void SpawnPlayer(){
		SpawnFinish[] Locations = SpawnFinish.Locations.ToArray();
		int numberOfSpawns = Locations.Length;


		if(numberOfSpawns >= 2){
			Random rand = new Random(seed + Vzpominky.Count);

			int start = rand.Next(numberOfSpawns);

			Player.StartingPosition = Locations[start].GetRandomPosition(rand);

			int end = rand.Next(numberOfSpawns);
			while(start == end)
				end = rand.Next(numberOfSpawns);


			Player.SpawnVzpominku(CurrentScene);

			CurrentScene.GetNode<PlayerController>("PlayerController").player = Player.player;
			CurrentScene.GetNode<PlayerController>("PlayerController").GetPlayerReady(true);


			Locations[end].SetAsFinish();
		}else
			GD.PushWarning("More Spawns Neaded, currently: " + numberOfSpawns);

	}
	public void MainMenu(){
		CurrentScene.QueueFree();
		CurrentScene = GD.Load<PackedScene>("res://UI/mainMenu.tscn").Instantiate();
		GetTree().Root.AddChild(CurrentScene);
	}
	public void LoadGame(){
		LoadScene();
		SpawnPlayer();
		for(int i = 0; i < Vzpominky.Count; i++)
			Vzpominky[i].SpawnVzpominku(CurrentScene);
	}
	public void NextStep(){
		for(int i = 0; i < Vzpominky.Count; i++)
			Vzpominky[i].NextFrame();
	}
	public void LoadScene(){
		SpawnFinish.Locations = new List<SpawnFinish>();

		CurrentScene.QueueFree();
		CurrentScene = CurrentMap.Instantiate();
		GetTree().Root.AddChild(CurrentScene);
	}
}
