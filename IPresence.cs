using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsStatusLight
{
    internal interface IPresence
    {
        string activity { get; set; }
        string availability { get; set; }
    }
}
