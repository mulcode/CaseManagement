using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDomain.Enums
{
    public enum ReturnType
    {
        Served = 1,
        Attempted = 2,
        DismissedBeforeService = 3,
        DeathOfParty = 4
    }
}
