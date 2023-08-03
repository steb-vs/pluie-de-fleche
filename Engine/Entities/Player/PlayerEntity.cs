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
		private PlayerData _data;
		private PlayerParameters _parameters;
		private Camera3D _cam;
		private Node3D _bow;
		private Vector2 _rot;

		private const float PRECISION = 4096.0f;
		private const float LIMIT = 1000.0f;

		public override void _Ready()
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;

			_data = GetNode<PlayerData>("./NOD_Data");
			_parameters = GetNode<PlayerParameters>("./NOD_Parameters");
			_cam = GetNode<Camera3D>("./CAM_Player");
			_bow = GetNode<Node3D>("./SVC_Hud/SVP_Hud/N3D_HudOrigin/CAM_Hud/N3D_WeaponOrigin/WPN_Bow");

			_data.InvMass = 1.0f / _parameters.Mass;
		}

		public override void _Input(InputEvent @event)
		{
			if(@event is InputEventMouseMotion mm)
			{
				_rot += new Vector2(mm.Relative.X * _parameters.MouseSensivityX, mm.Relative.Y * _parameters.MouseSensivityY);

				_rot.X %= PRECISION;
				_rot.Y = Mathf.Clamp(_rot.Y, -LIMIT, LIMIT);
			}
		}

		public override void _Process(double delta)
		{
			Quaternion quat;
			float dRotX;
			float dRotY;

			_data.OldRawRotation = _data.RawRotation;
			_data.OldRotation = _data.Rotation;

			_data.RawRotation = _rot;
			_data.Rotation = new Vector2(_rot.X / PRECISION * (Mathf.Pi * 2.0f), _rot.Y / PRECISION * (Mathf.Pi * 2.0f));

			quat = new Quaternion(Vector3.Up, -_data.Rotation.X);
			Transform = new Transform3D(new Basis(quat), Transform.Origin);

			quat = new Quaternion(Vector3.Right, -_data.Rotation.Y);
			_cam.Transform = new Transform3D(new Basis(quat), _cam.Transform.Origin);

			dRotX = _data.OldRawRotation.X - _data.RawRotation.X;

			if (Math.Abs(dRotX) > PRECISION * 0.5f)
			{
				dRotX -= PRECISION * Math.Sign(dRotX) * 1.0f;
			}

			dRotX *= 0.002f;

			dRotY = _data.RawRotation.Y - _data.OldRawRotation.Y;
			dRotY *= 0.005f;

			_bow.Transform = new Transform3D(
				_bow.Transform.Basis,
				new Vector3(
					Mathf.Lerp(_bow.Transform.Origin.X, dRotX, (float)(delta * 5.0d)),
					Mathf.Lerp(_bow.Transform.Origin.Y, dRotY - _data.Velocity.Y * 0.02f, (float)(delta * 5.0d)),
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

				acc *= _parameters.Speed;

				if (Input.IsActionJustPressed("jump"))
				{
					acc.Y += _parameters.JumpForce;
				}
			}

			acc = Transform.Basis * acc;
			flatAcc = new Vector3(acc.X, 0, acc.Z);

			if (flatAcc.Length() > _parameters.Speed)
			{
				flatAcc = flatAcc.Normalized() * _parameters.Speed;
				acc.X = flatAcc.X;
				acc.Z = flatAcc.Z;
			}

			if (!IsOnFloor())
			{
				acc.Y -= _parameters.Gravity * _parameters.Mass;
			}

			vel = Velocity;
			flatVel = new Vector3(vel.X, 0, vel.Z);

			if (IsOnFloor())
			{
				acc -= flatVel * _parameters.Friction;
			}

			vel += acc * _data.InvMass;

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
			_data.Velocity = Velocity;
		}
	}
}
