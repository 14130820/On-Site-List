using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.Models
{
    public enum Status : byte
    {
        NONE = 0,

        CHECKIN = 1,
        IN = 2,
        OUT = 3,
        CHECKOUT = 4,

        INS = 1 | 2,
        OUTS = 3 | 4
    }
}
