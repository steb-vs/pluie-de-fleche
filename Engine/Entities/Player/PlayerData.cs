using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Player
{
    internal partial class PlayerData : Node
    {
        [Export]
        public Vector3 Velocity { get; set; }

        [Export]
        public float InvMass { get; set; }

        [Export]
        public Vector2 Rotation { get; set; }
    }
}
