using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Weapons
{
	internal partial class ArrowItem : RigidBody3D
	{
        public override void _Ready()
        {
            BodyEntered += ArrowItem_BodyEntered;
        }

        private void ArrowItem_BodyEntered(Node body)
        {
            SetPhysicsProcess(false);
        }

        public override void _PhysicsProcess(double delta)
        {
            LookAt(GlobalTransform.Origin + LinearVelocity.Normalized());
        }
    }
}
