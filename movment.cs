using Godot;
using System;

public partial class movment : CharacterBody3D
{
    // Controller
    public Vector2 inputDir = Vector2.Zero;
    public bool DuckState = false;

    public bool ducked = false;

    [Export]
    public float Speed = 15.0f;
	
    [Export]
    public float MaxSpeed = 10f;
    [Export]
    public float JumpVelocity = 4.5f;

	public String steps = "res://assets/SoundEffects/player/pl_step";
	public int stepsNumber = 4;
	public String JumpSound = "res://assets/SoundEffects/player/pl_jump";
	public int JumpSoundNumber = 2;
	public String SoundFIleExtension = ".wav";

    [Export]
    public float friction = 3f;
    [Export]
    public float airControl = .9f;

	[Export]
    public int MaxJumps = 1;
    public int Jumps = 0;


    [Export]
    public float throwForce = 5.0f;

    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
	{
		Invincible(true);
	}
	public void Invincible(bool inv){
		if(inv) {
			this.CollisionMask = 1;
			this.CollisionLayer = 4;
		} else {
			this.CollisionMask = 5;
			this.CollisionLayer = 6;
		}

	}
	public void PlayStepSound(){
		GetNode<AudioStreamPlayer3D>("walking").Stream = GD.Load<AudioStreamWav>(steps + new Random().Next(1,stepsNumber) + SoundFIleExtension);
		GetNode<AudioStreamPlayer3D>("walking").Play();
	}
    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
		if(IsOnFloor() && Velocity.DistanceTo(Vector3.Zero) > .1f && GetNode<Timer>("walking/Timer").IsStopped())
			GetNode<Timer>("walking/Timer").Start();

        if (!IsOnFloor() || (IsOnFloor() && GetFloorAngle() > Math.PI/4)){
            velocity.Y -= gravity * (float)(delta) * (ducked ? 2 : 1);
			if(this.GlobalPosition.Y < -100)
				KYS();
		}

        Vector3 direction = (new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
			
            if (IsOnFloor() && !ducked){
                velocity += direction * Speed * (float)(delta) * friction;

			}

            if (!IsOnFloor() && ((velocity + direction * Speed * .01f * airControl).Length() < velocity.Length() || (new Vector3(1f,0,1f) * velocity).Length() < 1))
                velocity += direction * Speed * (float)(delta) * airControl;

        }

        if (IsOnFloor())
        {
			Jumps = MaxJumps;

            velocity -= new Vector3(1f, 0, 1f) * velocity * (friction) * (float)(delta) * (ducked ? 1 : 2);

            if (ducked && this.GetFloorAngle() != 0)
                velocity += new Vector3(1f, 0, 1f) * this.GetFloorNormal() * gravity * 2 * (float)(delta) * GetFloorAngle();
        }


        Velocity = velocity;
		MoveAndSlide();
    }
    private void StartsJumpTimer()
    {
		GetNode<AudioStreamPlayer3D>("jumping").Stream = GD.Load<AudioStreamWav>(JumpSound + new Random().Next(1,JumpSoundNumber) + SoundFIleExtension);
		GetNode<AudioStreamPlayer3D>("jumping").Play();
        GetNode<Timer>("WallJumpRange/Jump wait time").Start();
    }
    // tryes to jump or wall jump near a wall
    public void Jump()
    {
        if (GetNode<Timer>("WallJumpRange/Jump wait time").IsStopped())
        {

            if (IsOnFloor())
            {
                if (Velocity.Z < JumpVelocity * 1.5)
                {
					Jumps--;
                    StartsJumpTimer();
                    Velocity += new Vector3(0, JumpVelocity, 0);
                }
            }
            else
            {
                Area3D wallArea = GetNode<Area3D>("WallJumpRange");


                if (!ducked && wallArea.HasOverlappingBodies())
                {
					Jumps = MaxJumps;

                    StartsJumpTimer();
                    ShapeCast3D ray = wallArea.GetNode<ShapeCast3D>("WallCast");

                    float smallestAngle = 0f;
                    float smallestDistance = 1.5f;

                    float resolution = 36f;
                    for (int i = 0; i < resolution; i++)
                    {
                        ray.TargetPosition = new Vector3(0f, 0f, -smallestDistance).Rotated(new Vector3(0f, 1f, 0f), (2 * Mathf.Pi / resolution) * i);
                        ray.ForceShapecastUpdate();

                        if (ray.IsColliding())
                        {
                            float distance = ray.GetCollisionPoint(0).DistanceTo(ray.GlobalPosition);
                            if (smallestDistance > distance)
                            {
                                smallestDistance = distance;
                                smallestAngle = (2 * Mathf.Pi / resolution) * i;
                            }
                        }
                    }
                    Velocity += new Vector3(0, 0, JumpVelocity).Rotated(new Vector3(0f, 1f, 0f), smallestAngle + this.Rotation.Y);
                    Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);
                } else if (Jumps > 1) {
					Jumps--;
                    StartsJumpTimer();
                    Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);
				}
            }
        }
    }

    // tryes to duck (true) or un-duck (false)
    public void Duck(bool dk)
    {
        if (ducked != dk)
        {
            if (dk)
            {
                ducked = dk;
                GetNode<CollisionShape3D>("Bottom").Disabled = true;
                if (IsOnFloor())
                    this.GlobalPosition -= new Vector3(0, 1f, 0);

            }
            else
            {
                if (ducked && !GetNode<ShapeCast3D>("FreeHeadSpace").IsColliding())
                {
                    ducked = dk;
                    GetNode<CollisionShape3D>("Bottom").Disabled = false;

                    if (IsOnFloor())
                        this.GlobalPosition += new Vector3(0, 1f, 0);
                }
            }
        }
    }
    public int UseItem()
    {
		usableObject info = new usableObject(this);
		return GetNode<hud>("Torzo/Head/Camera3D/Control").UseItem(info);
    }

    // tryes to interact with thing thats is raycasted on
    public void Interact()
    {
        RayCast3D ray = GetNode<RayCast3D>("Head/cast");
        if (ray.IsColliding() && ray.GetCollider() is IInteractible)
            ((IInteractible)ray.GetCollider()).Interact();
    }
	public void EnteredHitbox(Node3D obj){
		if(obj.GetType() == this.GetType() && this != obj){
			((movment)obj).KYS();
		}
	}
    public void KYS()
    {
		if(this != game_manager.instance.Player.player)
			Rakdoll();
		else {
			DeathCam ded = GD.Load<PackedScene>("res://entities/DeathCam/death_cam.tscn").Instantiate<DeathCam>();

			ded.trackTarget = Rakdoll();
			ded.Position = this.GlobalPosition + Vector3.Up*0.5f + Vector3.Left*2f;

			game_manager.instance.CurrentScene.AddChild(ded);
			game_manager.instance.CurrentScene.GetNode<PlayerController>("PlayerController").GetPlayerReady(false);
		}
		this.Free();
    }
	private Marker3D Rakdoll(){
		RigidBody3D ragdoll = GD.Load<PackedScene>("res://entities/DeathCam/ragdoll.tscn").Instantiate<RigidBody3D>();
		ragdoll.ApplyCentralImpulse(this.Velocity+Vector3.Up);
		ragdoll.Position = this.GlobalPosition;
		game_manager.instance.CurrentScene.AddChild(ragdoll);
		return ragdoll.GetNode<Marker3D>("Marker3D");
	}
}
