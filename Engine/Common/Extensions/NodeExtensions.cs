using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluieDeFleche.Engine.Common.Extensions
{
    internal static class NodeExtensions
    {
        public static bool GDTypeOf(this Node node, string scriptName)
        {
            return node.GetScript().Obj is GDScript gdScript && gdScript.ResourcePath.EndsWith("/" + scriptName + ".gd");
        }
    }
}
