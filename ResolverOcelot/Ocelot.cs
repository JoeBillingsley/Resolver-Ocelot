using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return CanResolve(node, new List<IDependantNode>());
        }

        private static bool CanResolve(IDependantNode node, IList<IDependantNode> visitedNodes)
        {
            visitedNodes = new List<IDependantNode>(visitedNodes);
            visitedNodes.Add(node);

            foreach (var dependantNode in node.Dependancies)
            {
                if (visitedNodes.Contains(dependantNode) || !CanResolve(dependantNode, visitedNodes))
                    return false;
            }

            return true;
        }

        /**
         * <summary>
         * Attempts to run the action associated with a node. If the node has dependancies, the dependant nodes will be executed before attempting to execute the current node. Each action will only be run once.
         * </summary>
         * <param name="node">The node to begin execution on</param>
         * <returns>True if all nodes and dependant action are succesfully resolved and executed, false if any are unsuccesful.</returns>
         **/
        public static bool Execute(IDependantNode node)
        {
            return Execute(node, new ConcurrentDictionary<IDependantNode, int>());
        }

        /**
         * <summary>
         * Attempts to run the action associated with a node. If the node has dependancies, the dependant nodes will be executed before attempting to execute the current node. 
         * 
         * Each action will be run at a maximum, the value provided in the dictionary for the IDependantNode.
         * </summary>
         * <param name="node">The node to begin execution on</param>
         * <param name="remainingExecutions">Provides the maximum number of times an action should be run for a node</param>
         * <returns>True if all nodes and dependant action are succesfully resolved and executed, false if any are unsuccesful.</returns>
         **/
        public static bool Execute(IDependantNode node, ConcurrentDictionary<IDependantNode, int> remainingExecutions)
        {
            var success = true;

            // TODO: Could the same result be achieved but simplified with asnyc/await?

            Parallel.ForEach(node.Dependancies, dependantNode =>
            {
                if (!remainingExecutions.ContainsKey(dependantNode))
                    remainingExecutions[dependantNode] = 1;

                if (remainingExecutions[dependantNode] > 0)
                {
                    success = success && Execute(dependantNode, remainingExecutions);
                    remainingExecutions[dependantNode]--;
                }
            });

           return success && node.Action();
        }

    }
}
