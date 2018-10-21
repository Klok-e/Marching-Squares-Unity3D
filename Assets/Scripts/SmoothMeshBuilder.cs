using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public struct Edge
    {
        public Vector2 _point1;
        public Vector2 _point2;
        public Edge Swap(bool swap)
        {
            if(swap)
                return new Edge()
                {
                    _point1 = _point2,
                    _point2 = _point1,
                };
            else
                return this;
        }
    }

    /// <summary>
    /// Marching squares
    /// </summary>
    public class SmoothMeshBuilder : IMeshBuilder
    {
        public MeshData BuildMesh(float[,] data, int size)
        {
            var mesh = new MeshData()
            {
                _triangles = new List<int>(),
                _vertices = new List<Vector3>(),
            };

            for(int y = 0; y < size - 1; y++)
            {
                for(int x = 0; x < size - 1; x++)
                {
                    var leftDown = data[x, y];
                    var leftUp = data[x, y + 1];
                    var rightDown = data[x + 1, y];
                    var rightUp = data[x + 1, y + 1];

                    CellToMeshAndAddToMesh(mesh, leftUp, leftDown, rightUp, rightDown, new Vector3(x + 0.5f, y + 0.5f, 0), 1);
                }
            }
            return mesh;
        }

        private void CellToMeshAndAddToMesh(MeshData mesh, float leftUp, float leftDown, float rightUp, float rightDown, Vector3 offset, float scale)
        {
            byte config = 0;
            if(leftDown > 0)
            {
                config |= 0b0000_0001;
            }
            if(rightDown > 0)
            {
                config |= 0b0000_0010;
            }
            if(rightUp > 0)
            {
                config |= 0b0000_0100;
            }
            if(leftUp > 0)
            {
                config |= 0b0000_1000;
            }

            var ver = mesh._vertices.Count;
            switch(config)
            {
                case 0:
                    break;
                #region 3 points
                case 1:
                    {
                        var y = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                        var x = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                        mesh._vertices.Add((new Vector3(-0.5f, y, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        break;
                    }
                case 2:
                    {
                        var y = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                        var x = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                        mesh._vertices.Add((new Vector3(x, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, y, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        break;
                    }
                case 4:
                    {
                        var y = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                        var x = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                        mesh._vertices.Add((new Vector3(x, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, y, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        break;
                    }
                case 8:
                    {
                        var y = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                        var x = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                        mesh._vertices.Add((new Vector3(-0.5f, y, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x, 0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        break;
                    }
                #endregion

                #region 4 points
                case 3:
                    {
                        var y1 = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                        var y2 = Find1dCoordKnowingAllValues(0, rightDown, rightUp);

                        mesh._vertices.Add((new Vector3(-0.5f, y1, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, y2, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        break;
                    }
                case 6:
                    {
                        var x1 = Find1dCoordKnowingAllValues(0, leftDown, rightDown);
                        var x2 = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                        mesh._vertices.Add((new Vector3(x1, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x2, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        break;
                    }
                case 9:
                    {
                        var x1 = Find1dCoordKnowingAllValues(0, leftDown, rightDown);
                        var x2 = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                        mesh._vertices.Add((new Vector3(-0.5f, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x2, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x1, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        break;
                    }
                case 12:
                    {
                        var y1 = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                        var y2 = Find1dCoordKnowingAllValues(0, rightDown, rightUp);

                        mesh._vertices.Add((new Vector3(-0.5f, y1, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, y2, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        break;
                    }
                case 15:
                    {
                        mesh._vertices.Add((new Vector3(-0.5f, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        break;
                    }
                #endregion

                #region 5 points
                case 7:
                    {
                        var y = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                        var x = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                        mesh._vertices.Add((new Vector3(-0.5f, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, y, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 4);
                        break;
                    }
                case 11:
                    {
                        var y = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                        var x = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                        mesh._vertices.Add((new Vector3(-0.5f, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, y, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 4);
                        break;
                    }
                case 13:
                    {
                        var y = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                        var x = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                        mesh._vertices.Add((new Vector3(-0.5f, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, y, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 4);
                        break;
                    }
                case 14:
                    {
                        var y = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                        var x = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                        mesh._vertices.Add((new Vector3(-0.5f, y, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 4);
                        break;
                    }
                #endregion

                #region 6 points
                case 5:
                    {
                        var y1 = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                        var x1 = Find1dCoordKnowingAllValues(0, leftUp, rightUp);
                        var y2 = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                        var x2 = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                        mesh._vertices.Add((new Vector3(-0.5f, y1, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x1, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, y2, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x2, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 5);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 5);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 5);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 5);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 4);
                        break;
                    }
                case 10:
                    {
                        var y1 = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                        var x1 = Find1dCoordKnowingAllValues(0, leftUp, rightUp);
                        var y2 = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                        var x2 = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                        mesh._vertices.Add((new Vector3(-0.5f, y1, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(-0.5f, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x1, 0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, y2, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(0.5f, -0.5f, 0) + offset) * scale);
                        mesh._vertices.Add((new Vector3(x2, -0.5f, 0) + offset) * scale);

                        mesh._triangles.Add(ver + 5);
                        mesh._triangles.Add(ver + 0);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 5);
                        mesh._triangles.Add(ver + 1);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 5);
                        mesh._triangles.Add(ver + 2);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 5);
                        mesh._triangles.Add(ver + 3);
                        mesh._triangles.Add(ver + 4);
                        break;
                    }
                    #endregion
            }
            return;

            float Find1dCoordKnowingAllValues(float supposedValueOfSurface, float valueOfFirstPoint, float valueOfSecondPoint)
            {
                var qx = -0.5f + (0.5f - (-0.5f)) * ((supposedValueOfSurface - valueOfFirstPoint) / (valueOfSecondPoint - valueOfFirstPoint));
                return qx;
            }
        }
    }
}
