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
		private Node _malemoniak;
		private bool _hasHit;
		private bool _isAttached;
		private ArrowArea _area;

		public override void _Ready()
		{
			BodyEntered += ArrowItem_BodyEntered;
			_area = GetNode<ArrowArea>("./Area3D");

		}

		private void ArrowItem_BodyEntered(Node body)
		{
			_hasHit = true;

			if(body.GDTypeIs("chb_malemoniak") == true && _area.HasHit)
			{
				_malemoniak = body;
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			if(_hasHit)
			{
				Freeze = true;

				if (!_isAttached && _malemoniak != null)
				{
					Transform3D oldTr;

					_malemoniak.Call("take_damage", 2);
					oldTr = GlobalTransform;
					GetParent().RemoveChild(this);
					_malemoniak.AddChild(this);
					GlobalTransform = oldTr;
					_isAttached = true;
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
