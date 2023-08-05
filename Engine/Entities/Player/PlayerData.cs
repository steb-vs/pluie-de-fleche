using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Player
{
    [GlobalClass]
    internal partial class PlayerData : Resource
    {
        [Export]
        public Vector3 Velocity { get; set; }

        [Export]
        public float InvMass { get; set; }

        [Export]
        public Vector2 RawRotation { get; set; }

        [Export]
        public Vector2 OldRawRotation { get; set; }

        [Export]
        public Vector2 Rotation { get; set; }

        [Export]
        public Vector2 OldRotation { get; set; }
    }
}
