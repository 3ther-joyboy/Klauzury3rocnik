using Godot;
using System;

public partial class hud : Control
{
	ItemList inventory;
	int currentlySelected = 0;
	Control pausedMenu;
    movment player;
    Label lab;
    ProgressBar timer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<movment>("../../../..");
        lab = GetNode<Label>("speed");
        pausedMenu = GetNode<Control>("Pause");
		inventory = GetNode<ItemList>("inventory");
		timer = GetNode<ProgressBar>("ProgressBar");

		UpdateInventory();
    }

    public override void _Process(double delta)
    {
		if(!GetTree().Paused){
			lab.Text = Math.Round(new Vector2(player.Velocity.X, player.Velocity.Z).Length(), 2) + "";
		}
    }
	public void AddItem(Item item){
		player.GetNode("Items").AddChild(item);
		UpdateInventory();
	}
	public int UseItem(usableObject info) {
		if(inventory.ItemCount > 0){
			int inv = currentlySelected;
			inventory.RemoveItem(currentlySelected);
			player.GetNode("Items").GetChild<Item>(currentlySelected).Use(info);
			MoveInInventory(0);
			return inv;
		} else {
			GD.PushWarning("Uh, Well, Actually, You dont have any items");
			return -1;
		}
	}
	private void MoveInInventory(int i){
		if(inventory.ItemCount > 1){
			if(currentlySelected + i < 0) 
			{ 
				inventory.Select(inventory.ItemCount-1);
				currentlySelected = inventory.ItemCount-1;
			} else if (currentlySelected + i > inventory.ItemCount - 1) {
				inventory.Select(0);
				currentlySelected = 0;
			} else {
				inventory.Select(currentlySelected + i);
				currentlySelected += i;
			}
		}else{
			if(inventory.ItemCount > 0){
				currentlySelected = 0;
				inventory.Select(0);
			}
		}
	}
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("NextItem"))MoveInInventory(1);
        if (@event.IsActionPressed("PreviusItem"))MoveInInventory(-1);
        if (@event.IsActionPressed("Pause"))
			Pause();

    }
	public void Pause()
	{
		bool paused = !GetTree().Paused;
		GetTree().Paused = paused;

		pausedMenu.Visible = paused;

		Input.MouseMode = (!paused) ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Confined;
	}


	public void UpdateInventory(){
		inventory.Clear();

		if(player.GetNode("Items").GetChildCount() > 0) {
			Item[] items = new Item[player.GetNode("Items").GetChildCount()];
			for(int i = 0; i < player.GetNode("Items").GetChildCount(); i++)
				items[i] = player.GetNode("Items").GetChild<Item>(i);

			for(int i = 0; i < items.Length; i++){
				inventory.AddItem(items[i].GetName(),items[i].GetImage());
				inventory.SetItemTooltip(i,items[i].GetDescription());
			}
			inventory.Select(currentlySelected);
		}
	}
}
