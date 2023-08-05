using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Weapons
{
	internal partial class BowWeapon : Node3D
	{
		[Export]
		public BowParameters Parameters { get; set; }

		private PackedScene _arrowPrefab;
		private double _holdTime;
		private AnimationTree _animTree;
		private AudioStreamPlayer _audio;

		public override void _Ready()
		{
			_arrowPrefab = ResourceLoader.Load<PackedScene>("res://game_object/weapons/itm_arrow.tscn");
			_animTree = GetNode<AnimationTree>("./MDL_Bow/AnimationTree");
			_audio = GetNode<AudioStreamPlayer>("./ASP_Bow");
		}

		public override void _PhysicsProcess(double delta)
		{
			if(Input.IsActionPressed("fire"))
			{
				_holdTime += delta;
			}

			if(Input.IsActionJustReleased("fire"))
			{
				ArrowItem arrow;

				_audio.Play();
				arrow = _arrowPrefab.Instantiate<ArrowItem>();
				arrow.Parameters.Damage = Parameters.ArrowDamage;
				GetTree().Root.AddChild(arrow);
				arrow.GlobalTransform = new Transform3D(new Basis(GlobalTransform.Basis.GetRotationQuaternion()), GlobalTransform.Origin);
				arrow.ApplyForce(GlobalTransform.Basis.GetRotationQuaternion() * Vector3.Forward * ((float)Mathf.Clamp(_holdTime, 0.0f, 2.0f) + 1.0f) * 80.0f);
				_holdTime = 0.0f;
			}

			if(_holdTime < 0.1f)
			{
				_animTree.Set("parameters/anim_blend/blend_amount", Mathf.Clamp(-1.0f + _holdTime * 10.0f, -1.0f, 0.0f));
			}
			else
			{
				_animTree.Set("parameters/anim_blend/blend_amount", Mathf.Clamp(_holdTime * 1.2f - 0.1f , 0.0f, 1.0f));
			}
		}
	}
}
