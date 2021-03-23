namespace ConsoleGraphics.Maths
{
    /// <summary>
    /// Matrix. N x N matricies
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Row,Column
        /// </summary>
        public float[,] M;

        public Matrix(float[,] vals, int rows, int cols)
        {
            M = new float[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    M[i, j] = vals[i, j];
                }
            }
        }

        public Vector3 Multiply(Vector3 vector)
        {
            Vector3 pNew;
            pNew.X = vector.X * M[0, 0] + vector.Y * M[0, 1] + vector.Z * M[0, 2];
            pNew.Y = vector.X * M[1, 0] + vector.Y * M[1, 1] + vector.Z * M[1, 2];
            pNew.Z = vector.X * M[2, 0] + vector.Y * M[2, 1] + vector.Z * M[2, 2];
            return pNew;
        }
    }
}
