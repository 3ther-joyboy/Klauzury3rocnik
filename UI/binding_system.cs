using Godot;
using System;
using System.Text.RegularExpressions;

public partial class binding_system : ItemList
{
    [Export(PropertyHint.File)]
    String[] images;
    [Export]
    String[] inputTypes;

	bool redyToBind = false;
	bool deleteRedady = false;
	int indexToBind = 0;	
	Timer tim;

    public override void _Ready()
    {
        Flush();
		tim = new Timer();
		tim.OneShot = true;
		tim.WaitTime = 0.2;
		this.AddChild(tim);
    }

    public void Flush()
    {
        this.Clear();
        if (images.Length == inputTypes.Length)
        {
            for (int i = 0; i < images.Length; i++)
            {
				if(inputTypes[i] == "Un-bind"){
					this.AddItem(inputTypes[i], GD.Load<Texture2D>(images[i]));
				}else{
					String str = "";
					foreach (InputEvent e in InputMap.ActionGetEvents(inputTypes[i]))
						str += e.AsText() + ", ";

					str = Regex.Replace(str, " \\(........\\)", string.Empty);
					this.AddItem(str, GD.Load<Texture2D>(images[i]));
				}
            }
        }
        else
            GD.PrintErr("špatny počet argumentu v \"binding_system\", musite dat stejny počet");
    }

	public override void _Input(InputEvent @event)
	{
		if(redyToBind && tim.IsStopped()){
			InputMap.ActionAddEvent(inputTypes[indexToBind],@event);
			Flush();
			redyToBind = false;
		}
	}
    public void OnClick(int index)
	{
		if(inputTypes[index] == "Un-bind"){
			deleteRedady = true;
		}else
		if(deleteRedady){
			InputMap.ActionEraseEvents(inputTypes[index]);
			deleteRedady = false;
			Flush();
		}else{
			indexToBind = index;
			redyToBind = true;
			tim.Start();
		}
		
	}

}
