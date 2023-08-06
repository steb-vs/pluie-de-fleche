using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Weapons
{
    [GlobalClass]
    internal partial class BowParameters : Resource
    {
        [Export]
        public int ArrowDamage { get; set; } = 5;
    }
}
