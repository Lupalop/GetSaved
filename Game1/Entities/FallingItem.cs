using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class FallingItem : GameElement
    {
        public FallingItem(string name) : base(name)
        {
            // Always assume that the item is used in emergencies.
            IsEmergencyItem = true;
            ItemID = 0;
        }
        public bool IsEmergencyItem { get; set; }
        public int ItemID { get; set; }
    }
}
