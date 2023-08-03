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
		private PackedScene _arrowPrefab;
		private double _holdTime;

		public override void _Ready()
		{
			_arrowPrefab = ResourceLoader.Load<PackedScene>("res://game_objects/weapons/itm_arrow.tscn");
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

                arrow = _arrowPrefab.Instantiate<ArrowItem>();
				GetTree().Root.AddChild(arrow);
				arrow.GlobalTransform = GlobalTransform;
				arrow.ApplyForce(Vector3.Forward * GlobalTransform.Basis.Inverse() * ((float)_holdTime + 1.0f) * 50.0f);
                _holdTime = 0.0f;
            }
        }
	}
}
