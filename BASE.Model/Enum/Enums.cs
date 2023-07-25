using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Model;

public enum LanguageEnum
{
    vi = 1,
    en = 2,
}

public enum EntityStatus
{
    Visible = 1,
    Invisible = 0,
    UnDefined = 0,
    UnPublic = -1,
    UnActive_OldAgent = -3,
    UnActive_OldAgent_FakeRating = -4
}
