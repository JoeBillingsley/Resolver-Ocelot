using System;
using System.Collections.Generic;

namespace ResolverOcelot
{
    public class DependantNode : IDependantNode
    {
        public Func<bool> Action { get; set; }

        public IEnumerable<IDependantNode> Dependancies { get; set; }

        public DependantNode(params IDependantNode[] dependancies)
        {
            Dependancies = dependancies;
        }

    }
}
