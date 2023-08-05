using Godot;
using PluieDeFleche.Engine.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PluieDeFleche.Engine.Entities.NPC
{
    internal partial class NpcMalemoniakDetector : Area3D
    {
        private NpcData _data;

        public override void _Ready()
        {
            _data = GetNode<NpcEntity>("..").Data;
            BodyEntered += NpcMalemoniakDetector_BodyEntered;
            BodyExited += NpcMalemoniakDetector_BodyExited; ;
        }

        private void NpcMalemoniakDetector_BodyExited(Node3D body)
        {
            if (!body.GDTypeIs("chb_malemoniak"))
            {
                return;
            }

            _data.AvailableTargets.Remove(body);
            
            if(_data.CurrentTarget == body)
            {
                _data.CurrentTarget = null;
                _data.NextTargetTimer = 0.0d;
            }
        }

        private void NpcMalemoniakDetector_BodyEntered(Node3D body)
        {
            if (!body.GDTypeIs("chb_malemoniak"))
            {
                return;
            }

            _data.AvailableTargets.Add(body);
        }
    }
}
