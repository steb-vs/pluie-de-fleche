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
		[Export]
		public ArrowParameters Parameters { get; set; }

		private const float LIFESPAN = 10.0f;
		private double _airTimer;
		private double _stuckTimer;
		private bool _hasHit;
		private ArrowArea _area;
		private AudioStreamPlayer3D _audio;
		private AudioStream _hit;

		public override void _Ready()
		{
			BodyEntered += ArrowItem_BodyEntered;
			_area = GetNode<ArrowArea>("./Area3D");
			_audio = GetNode<AudioStreamPlayer3D>("./ASP_Arrow");
			_hit = ResourceLoader.Load<AudioStream>("res://sounds/longbow/hit3.wav");
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
				_audio.Stream = _hit;
				_audio.Play();

				if (_area.Malemonaik != null)
				{
					Transform3D oldTr;

					_area.Malemonaik.Call("take_damage", Parameters.Damage);
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
