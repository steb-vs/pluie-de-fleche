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
		private Vector2 _rot;

		private const float PRECISION = 4096.0f;
		private const float LIMIT = 1000.0f;

		public override void _Ready()
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;

			_data = GetNode<PlayerData>("./NOD_Data");
			_parameters = GetNode<PlayerParameters>("./NOD_Parameters");
			_cam = GetNode<Camera3D>("./CAM_Player");

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

			_data.Rotation = new Vector2(_rot.X / PRECISION * (Mathf.Pi * 2.0f), _rot.Y / PRECISION * (Mathf.Pi * 2.0f));

			quat = new Quaternion(Vector3.Up, -_data.Rotation.X);
			Transform = new Transform3D(new Basis(quat), Transform.Origin);

			quat = new Quaternion(Vector3.Right, -_data.Rotation.Y);
			_cam.Transform = new Transform3D(new Basis(quat), _cam.Transform.Origin);
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector3 acc;
			Vector3 vel;
			Vector3 flatVel;

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

			acc *= Transform.Basis.Inverse();

			if(!IsOnFloor())
			{
				acc.Y -= _parameters.Gravity;
			}

			vel = Velocity;
			vel += acc * _data.InvMass;
			flatVel = new Vector3(vel.X, 0, vel.Z);

			if (flatVel.Length() > _parameters.Speed)
			{
				flatVel = flatVel.Normalized() * _parameters.Speed;
				vel.X = flatVel.X;
				vel.Z = flatVel.Z;
			}

			if(IsOnFloor())
			{
				vel -= flatVel * (1.0f - 1.0f / _parameters.Friction);

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
		}
	}
}
