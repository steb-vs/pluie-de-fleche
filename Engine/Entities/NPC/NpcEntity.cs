using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.NPC
{
	internal partial class NpcEntity : CharacterBody3D
	{
		private bool _walk;
		private double _aiTimer;
		private float _angle;
		private readonly Random _rnd;
		private AnimationTree _animTree;

		public NpcEntity()
		{
			_rnd = new();
		}

		public override void _Ready()
		{
			_animTree = GetNode<AnimationTree>("./MDL_Mahaut/AnimationTree");
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector3 vel;

			_aiTimer += delta;

			if(_aiTimer >= 2.0f)
			{
				_aiTimer = 0.0d;
				_walk = !_walk;

				if(_walk)
				{
					_angle = (float)(Math.PI * 2.0f * _rnd.NextDouble());
				}
			}

			vel = Vector3.Zero;

			if(!IsOnFloor())
			{
				vel.Y = -1.0f;
			}

			if(_walk)
			{
				GlobalTransform = new Transform3D(new Basis(GlobalTransform.Basis.GetRotationQuaternion().Slerp(new Quaternion(Vector3.Up, _angle), 0.5f)), GlobalTransform.Origin);
				vel += GlobalTransform.Basis.GetRotationQuaternion() * Vector3.Forward;
			}

			_animTree.Set("parameters/run_blend/blend_amount", Mathf.Clamp(vel.Length(), 0.0f, 1.0f));
			Velocity = vel;
			MoveAndSlide();
		}
	}
}
