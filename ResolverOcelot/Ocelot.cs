using System.Collections.Generic;
using System.Linq;

namespace ResolverOcelot
{
    public static class Ocelot
    {

        /**
         * <summary>
         * Explores the dependancy graph of a given node to determine if it contains circular dependancies that would prevent it from being resolved. 
         * </summary>
         * <param name="node">The node to evaluate dependancies for</param>
         * <returns>True if it is possible to resolve all of the dependancies of a given node, false otherwise.</returns>
         **/
        public static bool CanResolve(IDependantNode node)
        {
            return ResolveHelper(node, new List<IDependantNode>());
        }

        private static bool ResolveHelper(IDependantNode node, IList<IDependantNode> visitedNodes)
        {
            visitedNodes = new List<IDependantNode>(visitedNodes);
            visitedNodes.Add(node);

            foreach (var dependantNode in node.Dependancies)
            {
                if (visitedNodes.Contains(dependantNode) || !ResolveHelper(dependantNode, visitedNodes))
                    return false;
            }

            return true;
        }

        /**
         * <summary>
         * Attempts to run the action associated with a node. If the node has dependancies, the dependant nodes will be executed before attempting to execute the current node.
         * </summary>
         * <param name="node">The node to begin execution on</param>
         * <returns>True if all nodes and dependant action are succesfully resolved and executed, false if any are unsuccesful.</returns>
         **/
        public static bool Execute(IDependantNode node)
        {
            if (!node.Dependancies.Any()) return node.Action();
            
            // TODO: Parallelise this. Diamond dependancy problem.
            // Just need to ensure that an action cannot be called twice at the same time somehow. Clever locking required thats not obvious at 10 in the evening.

            var failed = false;
            foreach (var dependantNode in node.Dependancies)
            {
                if (!Ocelot.Execute(dependantNode))
                    failed = true;
            }

            return !failed ? node.Action() : false;
        }

    }
}
