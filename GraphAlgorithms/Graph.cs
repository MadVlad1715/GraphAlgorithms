using System;

namespace GraphAlgorithms
{
    public class Graph
    {
        public double?[,] adjMatrix { get; private set; }
        public int vertexCount { get; private set; }

        public double? this[int i, int j]
        {
            get
            {
                return adjMatrix[i, j];
            }
            set
            {
                if (i == j && value == null)
                    value = 0;

                adjMatrix[i, j] = value;
            }
        }

        public Graph(int vertexCount)
        {
            this.vertexCount = vertexCount;
            adjMatrix = new double?[this.vertexCount, this.vertexCount];
            for (int v = 0; v < this.vertexCount; v++)
                adjMatrix[v, v] = 0;
        }

        public Graph(double?[,] adjMatrix)
            : this(adjMatrix.GetLength(0))
        {
            for (int v = 0; v < vertexCount; v++)
                for (int u = 0; u < vertexCount; u++)
                    if (adjMatrix[v, u] != null)
                        this.adjMatrix[v, u] = adjMatrix[v, u];
        }

        public double?[] bellmanFord(int srcVertex) => bellmanFord(adjMatrix, srcVertex);

        public static double?[] bellmanFord(double?[,] adjMatrix, int srcVertex)
        {
            int vertexCount = adjMatrix.GetLength(0);

            if (vertexCount <= 0) return new double?[0];

            double?[] dist = new double?[vertexCount];
            for (int i = 0; i < vertexCount; i++)
                dist[i] = double.PositiveInfinity;

            dist[srcVertex] = 0;

            for (int i = 0; i < vertexCount - 1; i++)
                for (int u = 0; u < vertexCount; u++)
                    for (int v = 0; v < vertexCount; v++)
                        if (adjMatrix[u, v] != null && dist[u] + adjMatrix[u, v] < dist[v])
                            dist[v] = dist[u] + adjMatrix[u, v];

            for (int u = 0; u < vertexCount; u++)
                for (int v = 0; v < vertexCount; v++)
                    if (adjMatrix[u, v] != null && dist[u] + adjMatrix[u, v] < dist[v])
                        throw new Exception("Graph contains negative weight cycle");

            for (int i = 0; i < vertexCount; i++)
                if (dist[i] == double.PositiveInfinity)
                    dist[i] = null;

            dist[srcVertex] = adjMatrix[srcVertex, srcVertex];

            return dist;
        }

        public double?[] dijkstras(int srcVertex) => dijkstras(adjMatrix, srcVertex, out _);
        public double?[] dijkstras(int srcVertex, out int?[] path) => dijkstras(adjMatrix, srcVertex, out path);
        public static double?[] dijkstras(double?[,] adjMatrix, int srcVertex) => dijkstras(adjMatrix, srcVertex, out _);

        public static double?[] dijkstras(double?[,] adjMatrix, int srcVertex, out int?[] path)
        {
            int vertexCount = adjMatrix.GetLength(0);

            path = new int?[vertexCount];

            if (vertexCount <= 0) return new double?[0];

            double?[] dist = new double?[vertexCount];
            for (int i = 0; i < vertexCount; i++)
                dist[i] = double.PositiveInfinity;

            dist[srcVertex] = 0;

            bool[] usedVertices = new bool[vertexCount];

            while (true)
            {
                int u = -1;
                for (int i = 0; i < vertexCount; i++)
                    if (!usedVertices[i] && (u == -1 || dist[i] < dist[u]))
                        u = i;

                if (u == -1) break;

                usedVertices[u] = true;

                for (int v = 0; v < vertexCount; v++)
                    if (adjMatrix[u, v] != null)
                    {
                        if (adjMatrix[u, v] < 0)
                            throw new Exception("Graph contains negative edge");
                        else if (dist[u] + adjMatrix[u, v] < dist[v])
                        {
                            dist[v] = dist[u] + adjMatrix[u, v];
                            path[v] = u;
                        }
                    }
            }

            for (int i = 0; i < vertexCount; i++)
                if (dist[i] == double.PositiveInfinity)
                    dist[i] = null;

            dist[srcVertex] = adjMatrix[srcVertex, srcVertex];

            return dist;
        }

        public double?[,] johnsons() => johnsons(this.adjMatrix);

        public static double?[,] johnsons(double?[,] adjMatrix)
        {
            int vertexCount = adjMatrix.GetLength(0);

            if (vertexCount <= 0) return new double?[0, 0];

            int extVertexCount = vertexCount + 1;
            double?[,] extAdjMatrix = new double?[extVertexCount, extVertexCount];

            for (int u = 0; u < vertexCount; u++)
                for (int v = 0; v < vertexCount; v++)
                    extAdjMatrix[u, v] = adjMatrix[u, v];

            for (int i = 0; i < extVertexCount; i++)
                extAdjMatrix[extVertexCount - 1, i] = 0;

            var dist = bellmanFord(extAdjMatrix, extVertexCount - 1);

            double?[,] newAdjMatrix = new double?[vertexCount, vertexCount];

            for (int u = 0; u < vertexCount; u++)
                for (int v = 0; v < vertexCount; v++)
                    if (extAdjMatrix[u, v] != null)
                        newAdjMatrix[u, v] = extAdjMatrix[u, v] + dist[u] - dist[v];

            double?[,] distMatrix = new double?[vertexCount, vertexCount];

            for (int v = 0; v < vertexCount; v++)
            {
                dijkstras(newAdjMatrix, v, out int?[] path);

                for (int u = 0; u < vertexCount; u++)
                {
                    if (path[u] == null) continue;

                    int vertex = u;
                    distMatrix[v, vertex] = 0;

                    do
                    {
                        distMatrix[v, u] += adjMatrix[path[vertex].Value, vertex];
                        vertex = path[vertex].Value;
                    } while (path[vertex] != null);
                }

                distMatrix[v, v] = adjMatrix[v, v];
            }

            return distMatrix;
        }
    }
}