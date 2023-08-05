using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Entities.Weapons
{
    [GlobalClass]
    internal partial class ArrowParameters : Resource
    {
        [Export]
        public int Damage { get; set; } = 2;
    }
}
