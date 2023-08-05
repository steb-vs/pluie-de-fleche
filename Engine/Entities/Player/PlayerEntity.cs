using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Player
{
	internal partial class PlayerEntity : CharacterBody3D
	{
		[Export]
		private PlayerData Data { get; set; }

		[Export]
		private PlayerParameters Parameters { get; set; }

		private Camera3D _cam;
		private Node3D _bow;
		private Vector2 _rot;
		private AudioStreamPlayer _audio;
		private double _footstepTimer;
		private List<AudioStream> _footsteps;
		private readonly Random _rnd;

		private const float PRECISION = 4096.0f;
		private const float LIMIT = 1000.0f;

		public PlayerEntity()
		{
			_rnd = new();
		}

		public override void _Ready()
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;

			_cam = GetNode<Camera3D>("./CAM_Player");
			_bow = GetNode<Node3D>("./SVC_Hud/SVP_Hud/N3D_HudOrigin/CAM_Hud/N3D_WeaponOrigin/WPN_Bow");
			_audio = GetNode<AudioStreamPlayer>("./ASP_Footsteps");
			_footsteps = new List<AudioStream>();

			for(int i = 3; i <= 8; i++)
			{
				_footsteps.Add(ResourceLoader.Load<AudioStream>($"res://sounds/footsteps/dirt{i}.wav"));
			}

			Data.InvMass = 1.0f / Parameters.Mass;
		}

		public override void _Input(InputEvent @event)
		{
			if(@event is InputEventMouseMotion mm)
			{
				_rot += new Vector2(mm.Relative.X * Parameters.MouseSensivityX, mm.Relative.Y * Parameters.MouseSensivityY);

				_rot.X %= PRECISION;
				_rot.Y = Mathf.Clamp(_rot.Y, -LIMIT, LIMIT);
			}
		}

		public override void _Process(double delta)
		{
			Quaternion quat;
			float dRotX;
			float dRotY;

			Data.OldRawRotation = Data.RawRotation;
			Data.OldRotation = Data.Rotation;

			Data.RawRotation = _rot;
			Data.Rotation = new Vector2(_rot.X / PRECISION * (Mathf.Pi * 2.0f), _rot.Y / PRECISION * (Mathf.Pi * 2.0f));

			quat = new Quaternion(Vector3.Up, -Data.Rotation.X);
			Transform = new Transform3D(new Basis(quat), Transform.Origin);

			quat = new Quaternion(Vector3.Right, -Data.Rotation.Y);
			_cam.Transform = new Transform3D(new Basis(quat), _cam.Transform.Origin);

			dRotX = Data.OldRawRotation.X - Data.RawRotation.X;

			if (Math.Abs(dRotX) > PRECISION * 0.5f)
			{
				dRotX -= PRECISION * Math.Sign(dRotX) * 1.0f;
			}

			dRotX *= 0.002f;

			dRotY = Data.RawRotation.Y - Data.OldRawRotation.Y;
			dRotY *= 0.005f;

			_bow.Transform = new Transform3D(
				_bow.Transform.Basis,
				new Vector3(
					Mathf.Lerp(_bow.Transform.Origin.X, dRotX, (float)(delta * 5.0d)),
					Mathf.Lerp(_bow.Transform.Origin.Y, dRotY - Data.Velocity.Y * 0.02f, (float)(delta * 5.0d)),
					0));
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector3 acc;
			Vector3 vel;
			Vector3 flatVel;
			Vector3 flatAcc;

			acc = Vector3.Zero;

			if (IsOnFloor())
			{
				if(Input.IsActionPressed("forward"))
				{
					acc += Vector3.Forward;
				}
				if (Input.IsActionPressed("backward"))
				{
					acc += Vector3.Back;
				}
				if (Input.IsActionPressed("left"))
				{
					acc += Vector3.Left;
				}
				if (Input.IsActionPressed("right"))
				{
					acc += Vector3.Right;
				}

				acc *= Parameters.Speed;

				if (Input.IsActionJustPressed("jump"))
				{
					acc.Y += Parameters.JumpForce;
				}
			}

			acc = Transform.Basis * acc;
			flatAcc = new Vector3(acc.X, 0, acc.Z);

			if (flatAcc.Length() > Parameters.Speed)
			{
				flatAcc = flatAcc.Normalized() * Parameters.Speed;
				acc.X = flatAcc.X;
				acc.Z = flatAcc.Z;
			}

			if (!IsOnFloor())
			{
				acc.Y -= Parameters.Gravity * Parameters.Mass;
			}

			vel = Velocity;
			flatVel = new Vector3(vel.X, 0, vel.Z);
			_footstepTimer += flatVel.Length();

			if (_footstepTimer > 180.0d)
			{
				_footstepTimer = 0.0d;
				_audio.Stream = _footsteps[_rnd.Next(0, _footsteps.Count)];
				_audio.Play();
			}

			if (IsOnFloor())
			{
				acc -= flatVel * Parameters.Friction;
			}

			vel += acc * Data.InvMass;

			if(IsOnFloor())
			{
				if (Math.Abs(vel.X) < 0.001f)
				{
					vel.X = 0.0f;
				}

				if (Math.Abs(vel.Z) < 0.001f)
				{
					vel.Z = 0.0f;
				}
			}

			Velocity = vel;
			MoveAndSlide();
			Data.Velocity = Velocity;
		}
	}
}
