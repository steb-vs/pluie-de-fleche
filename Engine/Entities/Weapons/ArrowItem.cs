using Godot;
using PluieDeFleche.Engine.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Weapons
{
	internal partial class ArrowItem : RigidBody3D
	{
		private const float LIFESPAN = 10.0f;
		private double _airTimer;
		private double _stuckTimer;
		private bool _hasHit;
		private ArrowArea _area;

		public override void _Ready()
		{
			BodyEntered += ArrowItem_BodyEntered;
			_area = GetNode<ArrowArea>("./Area3D");
		}

		private void ArrowItem_BodyEntered(Node body)
		{
			_hasHit = true;
		}

		public override void _PhysicsProcess(double delta)
		{
			if(!Freeze && (_hasHit || _area.Malemonaik != null))
			{
				Freeze = true;

				if (_area.Malemonaik != null)
				{
					Transform3D oldTr;

					_area.Malemonaik.Call("taking_damage", 5);
					oldTr = GlobalTransform;
					GetParent().RemoveChild(this);
					_area.Malemonaik.AddChild(this);
					GlobalTransform = oldTr;
				}
			}

			if(Freeze)
			{
				_stuckTimer += delta;

				if (_stuckTimer >= LIFESPAN)
				{
					QueueFree();
				}

				return;
			}

			_airTimer += delta;

			if(_airTimer >= LIFESPAN)
			{
				QueueFree();
			}

			LookAt(GlobalTransform.Origin + LinearVelocity.Normalized());
		}
	}
}
