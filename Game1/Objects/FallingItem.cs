using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public class FallingItem : GameElement
    {
        public FallingItem(string name) : base(name)
        {
            // Always assume that the item is used in emergencies.
            IsEmergencyItem = true;
        }
        public bool IsEmergencyItem { get; set; }
    }
}
