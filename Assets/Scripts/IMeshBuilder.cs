namespace Assets.Scripts
{
    public interface IMeshBuilder
    {
        MeshData BuildMesh(float[,] data, int size);
    }
}
