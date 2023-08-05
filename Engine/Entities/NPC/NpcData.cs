using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.NPC
{
    [GlobalClass]
    internal partial class NpcData : Resource
    {
        [Export]
        public Node3D CurrentTarget { get; set; }

        [Export]
        public Godot.Collections.Array<Node3D> AvailableTargets { get; set; }

        [Export]
        public int FireCount { get; set; }

        [Export]
        public int FireTargetCount { get; set; }

        [Export]
        public double FireTimer { get; set; }

        [Export]
        public double NextTargetTimer { get; set; }
    }
}
