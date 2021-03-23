namespace ConsoleGraphics.Maths
{
    /// <summary>
    /// A face that contains a indexes which can be used to point to vertices, texture coordinates, and a material
    /// </summary>
    public struct Triangle
    {
        /// <summary>
        /// An array of indexes which point to a vertex somewhere else
        /// </summary>
        public int[] VertexIds;
        /// <summary>
        /// An array of indexes which point to a texture coordinate somewhere else
        /// </summary>
        public int[] UVIds;
        /// <summary>
        /// An index that points to a material somewhere else
        /// </summary>
        public int MaterialId;

        public Triangle(int v1, int v2, int v3, int vt1, int vt2, int vt3, int mid)
        {
            VertexIds = new int[3];
            VertexIds[0] = v1;
            VertexIds[1] = v2;
            VertexIds[2] = v3;
            UVIds = new int[3];
            UVIds[0] = vt1;
            UVIds[1] = vt2;
            UVIds[2] = vt3;
            MaterialId = mid;
        }
    }
}
