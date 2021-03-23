/* Mesh - all the things that define a mesh. triangles, edges, verticies... later UVs...
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using ConsoleGraphics.Maths;

namespace ConsoleGraphics.Render
{
    public class Mesh
    {
        public Vector3[] Vertices;
        public Triangle[] Faces;
        public Vector2[] UVs;
        public Material[] Materials;
        public int VerticesCount;
        public int FacesCount;

        public Mesh(Vector3[] meshVerts, Triangle[] meshFaces, Vector2[] meshUvs, Material[] mats = null)
        {
            Vertices = meshVerts;
            VerticesCount = Vertices.Length;
            Faces = meshFaces;
            FacesCount = Faces.Length;
            UVs = meshUvs;
            Materials = mats;
        }

        public static Mesh LoadMeshFromFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            List<Vector3> loadedVerts = new List<Vector3>();
            List<Vector2> loadedUvs = new List<Vector2>();
            List<Triangle> loadedFaces = new List<Triangle>();
            //List<Triangle> loadedUvFaces = new List<Triangle>();
            List<Material> mtls = new List<Material>();
            int vertCount = 0;
            int faceCount = 0;

            while (!reader.EndOfStream)
            {
                string e1 = reader.ReadLine();
                if (e1 != "")
                {
                    if (e1[0] == 'v' && e1[1] == 't')//texture verticies
                    {
                        string temp = e1.Substring(3, e1.Length - 3);
                        string[] coords = temp.Split(' ');
                        Vector2 p = new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
                        loadedUvs.Add(p);
                    }
                    else if (e1[0] == 'v')//vertex
                    {
                        vertCount++;
                        string temp = e1.Substring(3, e1.Length - 3);
                        string[] coords = temp.Split(' ');
                        Vector3 p = new Vector3(float.Parse(coords[0]) * 100, float.Parse(coords[1]) * 100, float.Parse(coords[2]));
                        //if z is positive it's off screen, z=0 we clip
                        loadedVerts.Add(p);
                    }
                    else if (e1[0] == 'u')
                    {//use this mtl for rest of faces faces material
                        Material m = new Material(e1.Substring(7, e1.Length - 7));
                        mtls.Add(m);
                    }
                    else if (e1[0] == 'f')//edge
                    {
                        faceCount++;
                        string temp = e1.Substring(2, e1.Length - 2);
                        string[] coords = temp.Split(' ', '/');
                        Triangle f = new Triangle(int.Parse(coords[0]) - 1, int.Parse(coords[2]) - 1, int.Parse(coords[4]) - 1, /*UV ID's*/ int.Parse(coords[1]) - 1, int.Parse(coords[3]) - 1, int.Parse(coords[5]) - 1, mtls.Count);
                        // Triangle uvf = new Triangle(int.Parse(coords[1]), int.Parse(coords[3]), int.Parse(coords[5]));
                        loadedFaces.Add(f);
                        // loadedUvFaces.Add(uvf);
                    }
                }
            }
            return (new Mesh(loadedVerts.ToArray(), loadedFaces.ToArray(), loadedUvs.ToArray(), mtls.ToArray()));
        }

        public void Translate(Vector3 translation)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Add(translation);
            }
        }

        public void Translate(float x, float y, float z)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Add(x, y, z);
            }
        }

        public void Rotate(double angle, byte dir)
        {
            //Rotate about centerpoint of the shape: translate the center of our shape to the origin, rotate, translate back.
            float cosAngle = (float)Math.Cos(angle);
            float sinAngle = (float)Math.Sin(angle);
            Vector3 startCoords = Vertices[0];
            Translate(new Vector3(-startCoords.X, -startCoords.Y, -startCoords.Z));
            #region rotations
            /*        Rotate about X Axis            {{1,0, 0},
                                                     {0,  cosAngle,sinAngle*100}, 
                                                     {0,  -sinAngle/100, cosAngle}}, 3, 3);
             
             
             *        Rotate about Y Axis            {{cosAngle,0,-sinAngle*100},
                                                     {0,  1, 0}, 
                                                     {sinAngle,  0, cosAngle}}, 3, 3);
                      Rotate about Z axis
             *                                       {{cosAngle,-sinAngle,0},
                                                     {sinAngle,  cosAngle,0}, 
                                                     {0,         0,       1}},3,3);
             */
            #endregion
            int x = Environment.TickCount;

            Matrix rotationMatrix;
            if (dir == 0)
                rotationMatrix = new Matrix(
                    new float[,]
                    {
                        {1,   0,              0},
                        {0,   cosAngle,       sinAngle * 100},
                        {0,  -sinAngle / 100, cosAngle}
                    }, 3, 3);
            else if (dir == 1)
                rotationMatrix = new Matrix(
                    new float[,]
                    {
                        {cosAngle,       0, -sinAngle * 100},
                        {0,              1, 0},
                        {sinAngle / 100, 0, cosAngle}
                    }, 3, 3);
            else
                rotationMatrix = new Matrix(
                    new float[,]
                    {
                        {cosAngle, -sinAngle, 0},
                        {sinAngle,  cosAngle, 0},
                        {0,         0,        1}
                    }, 3, 3);

            x = Environment.TickCount - x;

            for (int i = 0; i < VerticesCount; i++)
            {
                Vertices[i] = rotationMatrix.Multiply(Vertices[i]);
            }

            Translate(new Vector3(startCoords.X, startCoords.Y, startCoords.Z));
        }
    }
}
