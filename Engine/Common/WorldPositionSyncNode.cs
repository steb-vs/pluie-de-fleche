using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Common
{
    internal partial class WorldPositionSyncNode : Node3D
    {
        [Export]
        public NodePath SyncTo { get; set; }

        private Node3D _syncNode;

        public override void _Ready()
        {
            _syncNode = GetNode<Node3D>(SyncTo);
        }

        public override void _Process(double delta)
        {
            GlobalTransform = _syncNode.GlobalTransform;
        }
    }
}
