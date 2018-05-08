using System;
using Xunit;

namespace MyBlockchain.P2p.UnitTests
{
    public class NodeIdTest
    {
        [Fact]
        public void GetClosestDistance_Ok()
        {
            var n1 = new NodeId("5825da3d2275be487d9987f3e792a3d889005d43");
            var n2 = new NodeId("5825da3d2275be487d9987f3e792a3d889005d43");
            var distance = NodeId.GetDistance(n1, n2);
            Assert.Equal("0000000000000000000000000000000000000000", distance.ToString());
        }

        [Fact]
        public void GetDistance_Test1()
        {
            // d(x,y) = d(y,x)
            var x = new NodeId("5825da3d2275be487d9987f3e792a3d889005d43");
            var y = new NodeId("da39a3ee5e6b4b0d3255bfef95601890afd80709");

            var d1 = NodeId.GetDistance(x, y);
            var d2 = NodeId.GetDistance(y, x);
            Assert.Equal(d1.ToString(), d2.ToString());
        }

        [Fact]
        public void GetDistance_Test2()
        {
            // d(x,y) + d(y,z) >= d(x,z)
            var x = new NodeId("5825da3d2275be487d9987f3e792a3d889005d43");
            var y = new NodeId("da39a3ee5e6b4b0d3255bfef95601890afd80709");
            var z = new NodeId("38a400e4aebf80b93fcd9c18bc7ebbd5e8b1a93f");

            var d1 = NodeId.GetDistance(x, y);
            var d2 = NodeId.GetDistance(y, z);
            var d3 = NodeId.GetDistance(x, z);
            Assert.True((d1 ^ d2) >= d3);
        }

        [Fact]
        public void XorOperatorCheck()
        {
            var x = new NodeId("5825da3d2275be487d9987f3e792a3d889005d43");
            var y = new NodeId("da39a3ee5e6b4b0d3255bfef95601890afd80709");
            var xor = (x ^ y);
            Assert.Equal("821c79d37c1ef5454fcc381c72f2bb4826d85a4a", xor.ToString());
        }

        [Fact]
        public void ImportExistingNodeId()
        {
            var nodeId = new NodeId("5825da3d2275be487d9987f3e792a3d889005d43");
            Assert.Equal("5825da3d2275be487d9987f3e792a3d889005d43", nodeId.ToString());
        }

        [Fact]
        public void GenerateNewNodeId()
        {
            var nodeId = new NodeId();
            Assert.Equal<int>((int)NodeId.DefaultNodeLength / 8, nodeId.NodeBytes.Length);
        }
    }
}
