using GarageDI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDI.Entities
{
    internal class Rocket : Vehicle
    {
        [Include]
        public int Range { get; set; }
    }
}
