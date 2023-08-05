using Godot;
using PluieDeFleche.Engine.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.NPC
{
	internal partial class NpcEntity : CharacterBody3D
	{
		[Export]
		public NpcParameters Parameters { get; set; }

		[Export]
		public NpcData Data { get; set; }

		private static readonly Random _rnd;
		private AnimationTree _animTree;
		private PackedScene _arrowPrefab;

		static NpcEntity()
		{
			_rnd = new();
		}

		public override void _Ready()
		{
			_animTree = GetNode<AnimationTree>("./MDL_Mahaut/AnimationTree");
			_arrowPrefab = ResourceLoader.Load<PackedScene>("res://game_object/weapons/itm_arrow.tscn");
			Data.FireTargetCount = _rnd.Next(Parameters.MinArrow, Parameters.MaxArrow + 1);
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector3 vel;

			vel = Vector3.Zero;

			if(!IsOnFloor())
			{
				vel.Y = -2.0f;
			}

			if(Data.CurrentTarget != null && (Data.CurrentTarget.IsQueuedForDeletion() || Data.CurrentTarget.Get("hit_points").AsInt32() <= 0))
			{
				Data.AvailableTargets.Remove(Data.CurrentTarget);
				Data.CurrentTarget = null;
				Data.NextTargetTimer = 0.0d;
			}

			if(Data.CurrentTarget == null)
			{
				Data.NextTargetTimer += delta;
			}

			if(Data.CurrentTarget == null && Data.NextTargetTimer >= Parameters.NextTargetCoolDown && Data.AvailableTargets.Any())
			{
				Vector3 flatPos;
				List<Node3D> candidates;

				flatPos = GlobalTransform.Origin;
				flatPos.Y = 0.0f;

				candidates = Data.AvailableTargets
					.Where(x =>
					{
						Vector3 flatPos2;
						float dist;

						flatPos2 = x.GlobalTransform.Origin;
						flatPos2.Y = 0.0f;
						dist = Math.Abs(flatPos.Length() - flatPos2.Length());

						return dist >= Parameters.MinRange && dist <= Parameters.MaxRange;
					})
					.ToList();

				if(candidates.Any())
				{
					Data.CurrentTarget = candidates[_rnd.Next(0, candidates.Count)];
				}
			}

			Data.FireTimer += delta;

			if (Data.CurrentTarget != null)
			{
				Vector3 dir;
				Quaternion quat;

				dir = Data.CurrentTarget.GlobalTransform.Origin - GlobalTransform.Origin;
				dir.Y = 0.0f;
				dir = dir.Normalized();

				quat = new Quaternion(Vector3.Forward, dir);
				quat = quat.Normalized();

				GlobalTransform = new Transform3D(new Basis(GlobalTransform.Basis.GetRotationQuaternion().Slerp(quat, 0.5f)), GlobalTransform.Origin);

				if(Data.FireTimer >= Parameters.FireRate)
				{
					ArrowItem arrow;
					Vector3 pos;
					double spreadAngle;

					Data.FireTimer = 0.0f;
					_animTree.Set("parameters/fire_trigger/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);

					arrow = _arrowPrefab.Instantiate<ArrowItem>();
					arrow.Parameters.Damage = Parameters.ArrowDamage;
					GetTree().Root.AddChild(arrow);
					pos = GlobalTransform.Origin;
					pos.Y += 1.2f;
					arrow.GlobalTransform = new Transform3D(new Basis(GlobalTransform.Basis.GetRotationQuaternion()), pos);
					spreadAngle = Math.PI * (Parameters.Spread * (_rnd.NextDouble() - 0.5d) - 0.5d);
					dir = Data.CurrentTarget.GlobalTransform.Origin - GlobalTransform.Origin;
					dir = dir.Normalized();
					//dir += new Vector3((float)Math.Cos(spreadAngle), 0, (float)Math.Sin(spreadAngle));
					//dir = dir.Normalized();
					//dir += Vector3.Down * 0.2f;
					//dir = dir.Normalized();
					arrow.ApplyForce(dir * 300.0f);

					Data.FireCount++;

					if(Data.FireCount >= Data.FireTargetCount)
					{
						Data.FireCount = 0;
						Data.FireTargetCount = _rnd.Next(Parameters.MinArrow, Parameters.MaxArrow + 1);
						Data.CurrentTarget = null;
						Data.NextTargetTimer = 0.0d;
					}
				}
			}

			_animTree.Set("parameters/run_blend/blend_amount", Mathf.Clamp(vel.Length(), 0.0f, 1.0f));
			Velocity = vel;
			MoveAndSlide();
		}
	}
}
