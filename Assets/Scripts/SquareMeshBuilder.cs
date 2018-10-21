using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SquareMeshBuilder : IMeshBuilder
    {
        public MeshData BuildMesh(float[,] data, int size)
        {
            var mesh = new MeshData
            {
                _vertices = new List<Vector3>(),
                _triangles = new List<int>()
            };

            for(int y = 0; y < size; y++)
            {
                for(int x = 0; x < size; x++)
                {
                    if(data[x, y] > 0f)
                    {
                        var count = mesh._vertices.Count;

                        mesh._vertices.Add(new Vector3(-0.5f, 0.5f, 0) + new Vector3(x, y, 0));
                        mesh._vertices.Add(new Vector3(0.5f, 0.5f, 0) + new Vector3(x, y, 0));
                        mesh._vertices.Add(new Vector3(0.5f, -0.5f, 0) + new Vector3(x, y, 0));
                        mesh._vertices.Add(new Vector3(-0.5f, -0.5f, 0) + new Vector3(x, y, 0));

                        mesh._triangles.Add(count + 0);
                        mesh._triangles.Add(count + 1);
                        mesh._triangles.Add(count + 2);
                        mesh._triangles.Add(count + 2);
                        mesh._triangles.Add(count + 3);
                        mesh._triangles.Add(count + 0);
                    }
                }
            }
            return mesh;
        }
    }
}
