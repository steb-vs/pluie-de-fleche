using Godot;
using PluieDeFleche.Engine.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Weapons
{
    internal partial class ArrowArea : Area3D
    {
        public bool HasHit { get; set; }

        public override void _Ready()
        {
            AreaEntered += ArrowArea_AreaEntered;
        }

        private void ArrowArea_AreaEntered(Area3D area)
        {
            HasHit = area.GetParent()?.GDTypeIs("chb_malemoniak") == true;

            if(HasHit)
            {
                area.GetParent().Call("death");
            }
        }
    }
}
