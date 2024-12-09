using Godot;
using System;

public class RecordFrame
{
	public bool duck = false;

	public bool jump = false;

	public Vector2 dir = Vector2.Zero;

	public bool use = false;
	public int used = 0;
	public Vector3 looking = Vector3.Zero;

	public RecordFrame(){}
	public RecordFrame(RecordFrame x){
		duck = x.duck ;

		jump = x.jump ;

		dir = x.dir ;

		use = x.use ;
		used = x.used ;
		looking = x.looking ;

	}
	public RecordFrame(bool _duck, bool _jump, Vector2 _dir, bool _use, int _used, Vector3 _looking){
		duck = _duck ;

		jump = _jump ;

		dir = _dir ;

		use = _use ;
		used = _used ;
		looking = _looking ;
	}
}
