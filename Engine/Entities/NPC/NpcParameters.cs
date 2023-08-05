using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.NPC
{
    [GlobalClass]
    internal partial class NpcParameters : Resource
    {
        [Export]
        public float MinRange { get; set; } = 5.0f;

        [Export]
        public float MaxRange { get; set; } = 20.0f;

        [Export]
        public int MinArrow { get; set; } = 2;

        [Export]
        public int MaxArrow { get; set; } = 5;

        [Export]
        public float FireRate { get; set; } = 3.0f;

        [Export]
        public float Spread { get; set; } = 0.05f;
    }
}
