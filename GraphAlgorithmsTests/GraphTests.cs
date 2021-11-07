using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphAlgorithms;

namespace GraphAlgorithmsTests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void BellmanFord_WithValidMatrix_DistReturned()
        {
            double?[,] adjMatrix = new double?[,]
            {
                {null, -2, 7, 5, null },
                {null, null, 8, 6, null },
                {null, 3, null, -4, null },
                {-1, null, null, null, null },
                {null, null, null, null, null }
            };

            int sourceVertex = 2;

            double?[] expectedDist = { -5, -7, 0, -4, null };

            int expectedDistLength = 5;

            Graph graph = new(adjMatrix);


            var actualDist = graph.bellmanFord(sourceVertex);

            Assert.AreEqual(expectedDistLength, actualDist.Length, "BellmanFord algorithm return array with wrong length");

            bool isEqual = true;
            for (int i = 0; i < actualDist.Length; i++)
                if (actualDist[i] != expectedDist[i])
                {
                    isEqual = false;
                    break;
                }

            Assert.IsTrue(isEqual, "BellmanFord algorithm return wrong distances");
        }

        [TestMethod]
        public void BellmanFord_WithNegativeWeightCycleMatrix_ThrowsException()
        {
            double?[,] adjMatrix = new double?[,]
            {
                {null, 2, null},
                {null, null, 5},
                {-10, null, null}
            };

            Graph graph = new(adjMatrix);

            Assert.ThrowsException<Exception>(() => graph.bellmanFord(1), "BellmanFord algorithm doesn't throw an exception on a negative weight cycle");
        }

        [TestMethod]
        public void Dijkstras_WithValidMatrix_DistReturned()
        {
            double?[,] adjMatrix = new double?[,]
            {
                {null, 2, 7, 5, null },
                {null, null, 8, 6, null },
                {null, 3, null, 4, null },
                {1, null, null, null, null },
                {null, null, null, null, null }
            };

            int sourceVertex = 3;

            double?[] expectedDist = { 1, 3, 8, 0, null };

            int expectedDistLength = 5;

            Graph graph = new(adjMatrix);

            var actualDist = graph.dijkstras(sourceVertex);

            Assert.AreEqual(expectedDistLength, actualDist.Length, "Dijkstra's algorithm return array with wrong length");

            bool isEqual = true;
            for (int i = 0; i < actualDist.Length; i++)
                if (actualDist[i] != expectedDist[i])
                {
                    isEqual = false;
                    break;
                }

            Assert.IsTrue(isEqual, "Dijkstra's algorithm return wrong distances");
        }

        [TestMethod]
        public void Dijkstras_WithNegativeWeightMatrix_ThrowsException()
        {
            double?[,] adjMatrix = new double?[,]
            {
                {null, 6, null},
                {-2, null, 7},
                {3, null, null}
            };

            int sourceVertex = 0;

            Graph graph = new(adjMatrix);

            Assert.ThrowsException<Exception>(() => graph.dijkstras(sourceVertex), "Dijkstra's algorithm doesn't throw an exception on a negative weight of edge");
        }

        [TestMethod]
        public void Johnsons_WithValidMatrix_DistReturned()
        {
            double?[,] adjMatrix = new double?[,]
            {
                {null, -2, 7, 5, null },
                {null, null, 8, 6, null },
                {null, 3, 6, -4, null },
                {-1, null, null, null, null },
                {null, null, null, null, null }
            };

            double?[,] expectedDist =
            {
                {0, -2, 6, 2, null },
                {3, 0, 8, 4, null },
                {-5, -7, 6, -4, null },
                {-1, -3, 5, 0, null },
                {null, null, null, null, 0 }
            };

            int expectedMatrixSize = 5;

            Graph graph = new(adjMatrix);

            var actualDist = graph.johnsons();

            Assert.AreEqual(expectedMatrixSize, actualDist.GetLength(0), "Johnson's algorithm return matrix with wrong size");
            Assert.AreEqual(expectedMatrixSize, actualDist.GetLength(1), "Johnson's algorithm return matrix with wrong size");

            bool isEqual = true;
            for (int u = 0; u < actualDist.GetLength(0); u++)
            {
                for (int v = 0; v < actualDist.GetLength(1); v++)
                    if (actualDist[u, v] != expectedDist[u, v])
                    {
                        isEqual = false;
                        break;
                    }

                if (!isEqual) break;
            }

            Assert.IsTrue(isEqual, "Johnson's algorithm return wrong distances");
        }

        [TestMethod]
        public void Johnsons_WithNegativeWeightCycleMatrix_ThrowsException()
        {
            double?[,] adjMatrix = new double?[,]
            {
                {null, -2, 7, 5, null },
                {null, null, 6, 6, null },
                {null, 3, null, -4, null },
                {-1, null, null, null, null },
                {null, null, null, null, null }
            };

            Graph graph = new(adjMatrix);

            Assert.ThrowsException<Exception>(() => graph.johnsons(), "Johnson's algorithm doesn't throws an exception on a negative weight cycle");
        }
    }
}
