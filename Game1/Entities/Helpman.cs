using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class Helpman : GameElement
    {
        public Helpman(string name) : base(name)
        {
            // Always assume that the person is alive.
            IsAlive = true;
        }
        public bool IsAlive { get; set; }
    }
}
