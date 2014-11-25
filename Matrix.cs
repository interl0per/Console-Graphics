/*
 *                                                  N x N matricies
 *      Define & perform basic operations on matricies.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics
{
    class Matrix : Program
    {
        public float[,] m_i;//row,col

        public Matrix(float[,] vals, int rows, int cols)
        {
            m_i = new float[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    m_i[i, j] = vals[i, j];
                }
            }
        }

        public static float magnitude(Mesh.point3 a)
        {
            return((float)Math.Sqrt(dot(a,a)));
        }
        public static Mesh.point3 normalize(Mesh.point3 a)
        {
            float norm = (float)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
            return (new Mesh.point3(a.x/norm, a.y/norm, a.z/norm));
        }
        public static Mesh.point3 cross(Mesh.point3 a, Mesh.point3 b)
        {
            Mesh.point3 resultant;
            resultant.x = a.y * b.z - a.z * b.y;
            resultant.y = a.z * b.x - a.x * b.z;
            resultant.z = a.x * b.y - a.y * b.x;
            return (resultant);
        }

        public static Mesh.point3 add(Mesh.point3 a, Mesh.point3 b)
        {
            return (new Mesh.point3((a.x + b.x), (a.y + b.y), (a.z + b.z)));
        }

        public static float dot(Mesh.point3 a, Mesh.point3 b)
        {
            return (a.x * b.x + a.y * b.y + a.z * b.z);
        }

        public Mesh.point3 multiply(Mesh.point3 vector)
        {
            Mesh.point3 pNew;
            pNew.x = vector.x * m_i[0, 0] + vector.y * m_i[0, 1] + vector.z * m_i[0, 2];
            pNew.y = vector.x * m_i[1, 0] + vector.y * m_i[1, 1] + vector.z * m_i[1, 2];
            pNew.z = vector.x * m_i[2, 0] + vector.y * m_i[2, 1] + vector.z * m_i[2, 2];
            return (pNew);
        }
    }
}
