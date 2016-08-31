using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ResolverOcelot.Tests
{
    [TestClass]
    public class OcelotTests
    {
        [TestMethod]
        public void CanResolveNoDependancies()
        {
            var edge = new DependantNode();

            var resolved = Ocelot.CanResolve(edge);

            Assert.IsTrue(resolved);
        }

        [TestMethod]
        public void CanResolveLineDependancies()
        {
            var edge = new DependantNode();
            var node = new DependantNode { Dependancies = new List<IDependantNode> { edge } };
            var root = new DependantNode { Dependancies = new List<IDependantNode> { node } };

            var resolved = Ocelot.CanResolve(root);

            Assert.IsTrue(resolved);
        }

        [TestMethod]
        public void CanResolveSplitDependancies()
        {
            var nodeA = new DependantNode();
            var nodeB = new DependantNode();
            var root = new DependantNode { Dependancies = new List<IDependantNode> { nodeA, nodeB } };

            var resolved = Ocelot.CanResolve(root);

            Assert.IsTrue(resolved);
        }

        [TestMethod]
        public void CanResolveDiamondDependancies()
        {
            var edge = new DependantNode();
            var nodeA = new DependantNode (edge);
            var nodeB = new DependantNode (edge); 
            var root = new DependantNode (nodeA, nodeB);

            var resolved = Ocelot.CanResolve(root);

            Assert.IsTrue(resolved);
        }

        [TestMethod]
        public void CantResolveSimpleCircularDependancies()
        {
            var edge = new DependantNode();
            var root = new DependantNode(edge);
            edge.Dependancies = new List<IDependantNode> { root };

            var resolved = Ocelot.CanResolve(root);

            Assert.IsFalse(resolved);
        }

        [TestMethod]
        public void CantResolveComplexCircularDependancies()
        {
            var edge = new DependantNode();
            var root = new DependantNode(edge);
            var nodeA = new DependantNode(root);

            edge.Dependancies = new List<IDependantNode> { nodeA };

            var resolved = Ocelot.CanResolve(root);

            Assert.IsFalse(resolved);
        }

        // TODO: Unit test that actions are called and in the correct order
    }
}
