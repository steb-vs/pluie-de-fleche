using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Player
{
    internal partial class PlayerParameters : Node
    {
        [Export]
        public float Speed { get; set; } = 200.0f;

        [Export]
        public float Gravity { get; set; } = 9.81f;

        [Export]
        public float Friction { get; set; } = 2.0f;

        [Export]
        public float Mass { get; set; } = 70.0f;

        [Export]
        public float JumpForce { get; set; } = 150.0f;

        [Export]
        public float MouseSensivityX { get; set; } = 1.0f;

        [Export]
        public float MouseSensivityY { get; set; } = 1.0f;
    }
}
