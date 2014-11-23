/* Mesh - all the things that define a mesh. triangles, edges, verticies... later UVs...
 * 
 * 
 */ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics
{
    class Mesh : Program
    {
        public Mesh(point3[] meshVerts, triangle[] meshFaces= null, point3[] meshUvs = null)
        {
            verts = meshVerts;
            vertCount = verts.Length;
            faces = meshFaces;
            faceCount = faces.Length;
            uvVerts = meshUvs;//1-1 correspondance with verticies
        }

        public struct triangle
        {//collection of 4 (or 3?) points, defines a face
            public int[] vertIDs;
            public triangle(int v1, int v2, int v3)
            {
                vertIDs = new int[4];
                vertIDs[0] = v1;
                vertIDs[1] = v2;
                vertIDs[2] = v3;
            }
        };

        public struct point3
        {
            public float x, y, z;
            public point3(float newX, float newY, float newZ = 0)
            {
                x = newX;
                y = newY;
                z = newZ;
            }
        };

        public point3[] verts;
        public triangle[] faces;
        public point3[] uvVerts;

        public int vertCount;
        public int faceCount;

        public void translate(point3 translation)
        {
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] = Matrix.add(verts[i], translation);
            }
        }

        public void rotate(double angle)
        {
            //Rotate about centerpoint of the shape: translate the center of our shape to the origin, rotate, translate back.
            float cosAngle = (float)Math.Cos(angle);
            float sinAngle = (float)Math.Sin(angle);

            Matrix rotMtrx = new Matrix(new float[,] {{cosAngle,-sinAngle,0},
                                                     {sinAngle,  cosAngle,0}, 
                                                     {0,         0,       1}}, 3, 3);
            /*
             *                                       {{cosAngle,-sinAngle,0},
                                                     {sinAngle,  cosAngle,0}, 
                                                     {0,         0,       1}},3,3);
             * 
             * */
            for (int i = 0; i < vertCount; i++)
            {
                verts[i] = rotMtrx.multiply(verts[i]);
            }
        }
    }
}
