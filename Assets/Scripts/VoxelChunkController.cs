using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    public class VoxelChunkController : MonoBehaviour
    {
        public const int _size = 64;
        public const float _maxDataVal = 1f;
        public const float _minDataVal = -1f;

        private float[,] _data;

        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private Mesh _mesh;

        private IMeshBuilder _meshBuilder;

        private void Start()
        {
            _meshBuilder = new SmoothMeshBuilder();

            _data = new float[_size, _size];
            for(int y = 0; y < _size; y++)
            {
                for(int x = 0; x < _size; x++)
                {
                    _data[x, y] = 1f;
                }
            }

            _mesh = new Mesh();
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();

            GenerateMeshFromDataAndPutInMesh();
        }

        private void GenerateMeshFromDataAndPutInMesh()
        {
            var mesh = _meshBuilder.BuildMesh(_data, _size);

            _mesh.Clear();

            _mesh.SetVertices(mesh._vertices);
            _mesh.SetTriangles(mesh._triangles.ToArray(), 0);

            _meshFilter.sharedMesh = _mesh;
            _meshCollider.sharedMesh = _mesh;
        }

        public void SetSphere(Vector2Int center, int radius, float value)
        {
            value = Mathf.Clamp(value, _minDataVal, _maxDataVal);
            for(int y = -radius + 1; y < radius; y++)
            {
                for(int x = -radius + 1; x < radius; x++)
                {
                    if(x * x + y * y < radius * radius)
                    {
                        var posX = x + center.x;
                        var posY = y + center.y;
                        if(posX >= 0 && posY >= 0 && posX < _size && posY < _size)
                        {
                            float mult = 1f - ((x * x + y * y) / (float)(radius * radius));

                            _data[posX, posY] = value * mult;
                        }
                    }
                }
            }
            GenerateMeshFromDataAndPutInMesh();
        }

        public void AddSphere(Vector2Int center, int radius, float valueAtCenter)
        {
            for(int y = -radius + 1; y < radius; y++)
            {
                for(int x = -radius + 1; x < radius; x++)
                {
                    if(x * x + y * y < radius * radius)
                    {
                        var posX = x + center.x;
                        var posY = y + center.y;

                        float mult = 1f - ((x * x + y * y) / (float)(radius * radius));

                        SetVoxel(new Vector2Int(posX, posY), valueAtCenter * mult);
                    }
                }
            }
            GenerateMeshFromDataAndPutInMesh();
        }

        public void SetVoxel(Vector2Int pos, float value)
        {
            if(pos.x >= 0 && pos.y >= 0 && pos.x < _size && pos.y < _size)
            {
                _data[pos.x, pos.y] += value;
                _data[pos.x, pos.y] = Mathf.Clamp(_data[pos.x, pos.y], _minDataVal, _maxDataVal);
            }
        }

        private void OnDrawGizmos()
        {
            if(_data != null)
                for(int y = 0; y < _size; y++)
                {
                    for(int x = 0; x < _size; x++)
                    {
                        var v = _data[x, y];
                        if(v > 0)
                        {
                            Gizmos.DrawWireCube(new Vector3(x, y, 0), Vector3.one * 1f);
                            Gizmos.DrawCube(new Vector3(x, y, 0), Vector3.one * 0.1f);
                        }
                    }
                }
        }
    }
}