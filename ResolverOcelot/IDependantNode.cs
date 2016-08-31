using System;
using System.Collections.Generic;

namespace ResolverOcelot
{
    public interface IDependantNode
    {
        IEnumerable<IDependantNode> Dependancies { get; set; }

        Func<bool> Action { get; set; }

    }
}
