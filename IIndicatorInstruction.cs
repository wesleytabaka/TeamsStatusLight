using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsStatusLight
{
    internal interface IIndicatorInstruction
    {
        int r {  get; set; }
        int g { get; set; }
        int b { get; set; }
        Effect effect { get; set; }
        int effectRate { get; set; }
        Transition transition { get; set; }
        int transitionDuration { get; set; }
    }
}
