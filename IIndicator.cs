﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsStatusLight
{
    internal interface IIndicator
    {
        void SetIndicator(IIndicatorInstruction instruction);
    }
}
