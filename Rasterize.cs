/* Rasterization - all the grunt work!
 * Determine pixle 'colors'..
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleGraphics
{
    class Rasterizer : Program
    {
        public static int[,] zBuffer = new int[RENDER_WIDTH, RENDER_HEIGHT];

        void swap(ref float x, ref float y)
        {
            float temp = x;
            x = y;
            y = temp;
        }

        void swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }

        float clamp(float val, float min, float max)
        {
            return (Math.Max(Math.Min(val, max), min));
        }

        public byte[,] renderVerts(Mesh someShape)
        {
            byte[,] image = new byte[RENDER_WIDTH, RENDER_HEIGHT];//

            for (int x = 0; x < RENDER_WIDTH; x++)
                for (int y = 0; y < RENDER_HEIGHT; y++)
                    image[x, y] = 32;

            foreach (Mesh.triangle f in someShape.faces)
            {
                for (int i = 0; i < 3; i++)//each vertex
                {
                    double depth = someShape.verts[f.vertIDs[i]].z;
                    int P_x1 = (int)(someShape.verts[f.vertIDs[i]].x / depth);
                    int P_y1 = (int)(someShape.verts[f.vertIDs[i]].y / depth);

                    if (P_y1 < RENDER_HEIGHT - 1 && P_y1 >= 0 && P_x1 < RENDER_WIDTH - 1 && P_x1 >= 0)//the vert is in bounds with the screen
                        image[P_x1, P_y1] = 177;
                }
            }
            return (image);
        }

        public byte[,] renderWire(Mesh someShape)
        {
            byte[,] image = new byte[RENDER_WIDTH, RENDER_HEIGHT];//

            for (int x = 0; x < RENDER_WIDTH; x++)
                for (int y = 0; y < RENDER_HEIGHT; y++)
                    image[x, y] = 32;

            foreach (Mesh.triangle f in someShape.faces)
            {
                for (int i = 0; i < 3; i++)//each vertex
                {
                    double depth = someShape.verts[f.vertIDs[i]].z;
                    int P_x1 = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[i]].x / depth);
                    int P_y1 = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[i]].y / depth);

                    if (P_y1 < RENDER_HEIGHT - 1 && P_y1 >= 0 && P_x1 < RENDER_WIDTH - 1 && P_x1 >= 0)//the vert is in bounds with the screen
                        for (int j = 0; j < 2; j++)//each of the 2 neighboring verts
                        {
                            double depth2 = someShape.verts[f.vertIDs[j]].z;
                            int P_x = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[j]].x / depth2);
                            int P_y = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[j]].y / depth2);

                            if (P_y < RENDER_HEIGHT - 1 && P_y >= 0 && P_x < RENDER_WIDTH - 1 && P_x >= 0)
                                image = drawLine(image, P_x, P_y, P_x1, P_y1, true);
                        }
                }
            }
            return (image);
        }
        Mesh.triangle targFace;
        public byte[,] renderSolid()
        {
            byte[,] image = new byte[RENDER_WIDTH, RENDER_HEIGHT];//

            for (int x = 0; x < RENDER_WIDTH; x++)
                for (int y = 0; y < RENDER_HEIGHT; y++)
                {
                    image[x, y] = 32;       //space character is drawn faster than the null character (0)
                    zBuffer[x, y] = -100;   //far clipping plane
                }

            foreach (Mesh.triangle f in someShape.faces)
            {
                targFace = f;
                Mesh.point3 a = Matrix.add(someShape.verts[f.vertIDs[0]], new Mesh.point3(-someShape.verts[f.vertIDs[2]].x, -someShape.verts[f.vertIDs[2]].y, -someShape.verts[f.vertIDs[2]].z));
                Mesh.point3 b = Matrix.add(someShape.verts[f.vertIDs[0]], new Mesh.point3(-someShape.verts[f.vertIDs[1]].x, -someShape.verts[f.vertIDs[1]].y, -someShape.verts[f.vertIDs[1]].z));
                Mesh.point3 surfaceNorm1 = Matrix.cross(a, b);

                surfaceNorm1 = Matrix.normalize(surfaceNorm1);

                Mesh.point3 lightNorm1 = Matrix.add(light1.coords, new Mesh.point3(-someShape.verts[f.vertIDs[0]].x, -someShape.verts[f.vertIDs[0]].y, -someShape.verts[f.vertIDs[0]].z));//arbitrary vert
                lightNorm1 = Matrix.normalize(lightNorm1);
                ////////////////////////////////////
                Mesh.point3 a1 = Matrix.add(someShape.verts[f.vertIDs[1]], new Mesh.point3(-someShape.verts[f.vertIDs[2]].x, -someShape.verts[f.vertIDs[2]].y, -someShape.verts[f.vertIDs[2]].z));
                Mesh.point3 b1 = Matrix.add(someShape.verts[f.vertIDs[1]], new Mesh.point3(-someShape.verts[f.vertIDs[0]].x, -someShape.verts[f.vertIDs[0]].y, -someShape.verts[f.vertIDs[0]].z));
                Mesh.point3 surfaceNorm2 = Matrix.cross(a1, b1);

                surfaceNorm2 = Matrix.normalize(surfaceNorm2);

                Mesh.point3 lightNorm2 = Matrix.add(light1.coords, new Mesh.point3(-someShape.verts[f.vertIDs[1]].x, -someShape.verts[f.vertIDs[1]].y, -someShape.verts[f.vertIDs[1]].z));//arbitrary vert
                lightNorm2 = Matrix.normalize(lightNorm2);
                //////////////////////////////////////////////
                Mesh.point3 a2 = Matrix.add(someShape.verts[f.vertIDs[2]], new Mesh.point3(-someShape.verts[f.vertIDs[0]].x, -someShape.verts[f.vertIDs[0]].y, -someShape.verts[f.vertIDs[0]].z));
                Mesh.point3 b2 = Matrix.add(someShape.verts[f.vertIDs[2]], new Mesh.point3(-someShape.verts[f.vertIDs[1]].x, -someShape.verts[f.vertIDs[1]].y, -someShape.verts[f.vertIDs[1]].z));
                Mesh.point3 surfaceNorm3 = Matrix.cross(a2, b2);

                surfaceNorm3 = Matrix.normalize(surfaceNorm3);

                Mesh.point3 lightNorm3 = Matrix.add(light1.coords, new Mesh.point3(-someShape.verts[f.vertIDs[2]].x, -someShape.verts[f.vertIDs[2]].y, -someShape.verts[f.vertIDs[2]].z));//arbitrary vert
                lightNorm3 = Matrix.normalize(lightNorm3);
                ////////////////////////////////////////////

                //perspective projection
                float[] depth = {someShape.verts[f.vertIDs[0]].z,
                                 someShape.verts[f.vertIDs[1]].z,
                                 someShape.verts[f.vertIDs[2]].z};

                if (depth[0] < 0 && depth[1] < 0 && depth[2] < 0)
                {                    //>= one vertex is in screen
                    int x0 = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[0]].x / depth[0]);
                    int y0 = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[0]].y / depth[0]);

                    int x1 = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[1]].x / depth[1]);
                    int y1 = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[1]].y / depth[1]);

                    int x2 = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[2]].x / depth[2]);
                    int y2 = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[2]].y / depth[2]);


                    Mesh.point2[] uvs = new Mesh.point2[] { someShape.uvVerts[f.uvIds[0]],
                                                            someShape.uvVerts[f.uvIds[1]], 
                                                            someShape.uvVerts[f.uvIds[2]] 
                                                          };
                    Mesh.point3[] projPoints = new Mesh.point3[] {new Mesh.point3(x0, y0, depth[0]),
                                                                  new Mesh.point3(x1, y1, depth[1]),
                                                                  new Mesh.point3(x2, y2, depth[2])};

                    byte[] colors = {(byte)(light1.intensity * Math.Max(0, Matrix.dot(surfaceNorm1, lightNorm1))),
                                     (byte)(light1.intensity * Math.Max(0, Matrix.dot(surfaceNorm2, lightNorm2))),
                                     (byte)(light1.intensity * Math.Max(0, Matrix.dot(surfaceNorm3, lightNorm3)))};

                    image = drawTriangle(image, projPoints, uvs, colors);
                }
            }
            return (image);
        }

        private byte[,] drawTriangle(byte[,] image, Mesh.point3[] points, Mesh.point2[] uvs, byte[] brightness = null)
        {
            /*
             *      A
             *  B------- 
             *          C
             * 
             */
            //sort by lowest y value (highest on screen)
            Mesh.point3[] unsortedPoints = new Mesh.point3[3];
            points.CopyTo(unsortedPoints, 0);

            if (points[1].y < points[0].y)
            {//swap point[1] and point[0]
                swap(ref points[0].y, ref points[1].y);
                swap(ref points[0].x, ref points[1].x);
            }
            if (points[2].y < points[0].y)
            {//swap point[0] and point[2]
                swap(ref points[0].y, ref points[2].y);
                swap(ref points[0].x, ref points[2].x);
            }
            if (points[2].y < points[1].y)
            {//swap point[1] & point[2]
                swap(ref points[1].y, ref points[2].y);
                swap(ref points[1].x, ref points[2].x);
            }

            if (points[0].y == points[1].y)
            {//flat top
                image = fillFlatTop(image, points, unsortedPoints, brightness, uvs);
            }
            else if (points[1].y == points[2].y)
            {//flat bottom
                image = fillFlatBottom(image, points, unsortedPoints, brightness, uvs);
            }
            else
            {//nontrivial
                Mesh.point3 midpoint = new Mesh.point3((int)(Math.Ceiling(points[0].x + ((float)(points[1].y - points[0].y) / (float)(points[2].y - points[0].y)) * (points[2].x - points[0].x))), points[1].y);
                fillFlatBottom(image, new Mesh.point3[] { points[0], points[1], midpoint }, unsortedPoints, brightness, uvs);
                fillFlatTop(image, new Mesh.point3[] { points[1], midpoint, points[2] }, unsortedPoints, brightness, uvs);
            }
            return (image);
        }

        private byte[,] fillFlatTop(byte[,] image, Mesh.point3[] sortedPoints, Mesh.point3[] unsortedPoints, byte[] brightness, Mesh.point2[] uvs = null)
        {
            float startX = sortedPoints[2].x;
            float endX = sortedPoints[2].x;

            float invslope1 = (float)(sortedPoints[2].x - sortedPoints[0].x) / (float)(sortedPoints[2].y - sortedPoints[0].y);
            float invslope2 = (float)(sortedPoints[2].x - sortedPoints[1].x) / (float)(sortedPoints[2].y - sortedPoints[1].y);

            for (int scanlineY = (int)sortedPoints[2].y; scanlineY > sortedPoints[0].y; scanlineY--)
            {
                drawLine(image, (int)startX, scanlineY, (int)endX, scanlineY, false, unsortedPoints, brightness, uvs);
                startX -= invslope1;
                endX -= invslope2;
            }
            return (image);
        }

        private byte[,] fillFlatBottom(byte[,] image, Mesh.point3[] sortedPoints, Mesh.point3[] unsortedPoints, byte[] brightness, Mesh.point2[] uvs = null)
        {
            float invslope1 = (float)(sortedPoints[2].x - sortedPoints[0].x) / (float)(sortedPoints[2].y - sortedPoints[0].y);
            float invslope2 = (float)(sortedPoints[0].x - sortedPoints[1].x) / (float)(sortedPoints[0].y - sortedPoints[1].y);

            float curx1 = sortedPoints[0].x;
            float curx2 = sortedPoints[0].x;

            for (int scanlineY = (int)sortedPoints[0].y; scanlineY <= sortedPoints[1].y; scanlineY++)
            {
                drawLine(image, (int)curx1, scanlineY, (int)curx2, scanlineY, false, unsortedPoints, brightness, uvs);
                curx1 += invslope1;
                curx2 += invslope2;
            }
            return (image);
        }

        public byte[,] drawLine(byte[,] image, /*start*/int x0, int y0, int x1, int y1,/*end*/ bool drawWireFrame, Mesh.point3[] unsortedPoints = null, byte[] color = null, Mesh.point2[] uvs = null)
        {
            float rise = y1 - y0;
            float run = x1 - x0;
            byte txl = 219;
            //if (color != null)
            //{
            //    color[0] = 0;
            //    color[1] = 50;
            //    color[2] = 100;
            //}
            if (run == 0)
            {
                if (drawWireFrame)
                    txl = 124;// |

                if (y0 > y1)
                    swap(ref y0, ref y1);

                while (y0 < y1)//DRAW
                {
                    image[x0, y0] = txl;
                    y0++;
                }
                return (image);
            }

            float m = rise / run;

            if (-1 <= rise / run && rise / run <= 1)
            {

                if (drawWireFrame)
                {
                    if (rise / run > 0.5)
                        txl = 92;// /
                    else if (rise / run < -0.5)
                        txl = 47;// \
                    else
                        txl = 95;// -
                }

                if (x0 > x1)
                {
                    swap(ref x0, ref x1);
                    swap(ref y0, ref y1);
                }

                float y = y0;
                float x = x0;
                float denom = 0;
                if (!drawWireFrame)
                    denom = ((unsortedPoints[1].y - unsortedPoints[2].y) * (unsortedPoints[0].x - unsortedPoints[2].x) + (unsortedPoints[2].x - unsortedPoints[1].x) * (unsortedPoints[0].y - unsortedPoints[2].y));

                while (x <= x1 && y < RENDER_HEIGHT && x < RENDER_WIDTH && y >= 0)
                {
                    if (x > 0)//clipping logic     <    ^
                    {
                        int xtoi = (int)x;
                        int ytoi = (int)y;
                        if (drawWireFrame)
                        {
                            image[xtoi, ytoi] = txl;
                            y += m;
                        }
                        else//HERE'S WHERE WE DRAW TRIANGLES!
                        {
                            float b1 = (((unsortedPoints[1].y - unsortedPoints[2].y) * (x - unsortedPoints[2].x)) + ((unsortedPoints[2].x - unsortedPoints[1].x) * (y - unsortedPoints[2].y))) / denom;
                            float b2 = (((unsortedPoints[2].y - unsortedPoints[0].y) * (x - unsortedPoints[2].x)) + ((unsortedPoints[0].x - unsortedPoints[2].x) * (y - unsortedPoints[2].y))) / denom;
                            float b3 = 1 - b1 - b2;

                            int interpolatedZ = (int)(b1 * unsortedPoints[0].z + b2 * unsortedPoints[1].z + b3 * unsortedPoints[2].z);

                            if (interpolatedZ > zBuffer[xtoi, ytoi])
                            { // use barycentric interpolation to do everything..
                                float interpU = (float)(b1 * uvs[0].x + b2 * uvs[1].x + b3 * uvs[2].x);
                                float interpV = (float)(b1 * uvs[0].y + b2 * uvs[1].y + b3 * uvs[2].y);
                                float interpBright = color[1];// + b2 * (float)color[1] + b3 * (float)color[2]);
                                Material someMat = someShape.matIDs[targFace.matID-1];
                                int xpos = Math.Min(Math.Max((int)(interpU * someMat.SIZE), 0), someMat.SIZE-1);
                                int ypos = Math.Min(Math.Max((int)(interpV * someMat.SIZE), 0), someMat.SIZE-1);

                                byte fa = someMat.bitmapColorsCached[(int)clamp(xpos, 0, someMat.SIZE-1), (int)clamp(ypos, 0, someMat.SIZE-1)];
                                image[xtoi, ytoi] = (byte)(Convert.ToInt32(levels[(int)clamp(fa + interpBright, 0, 255)]));
                                zBuffer[xtoi, ytoi] = interpolatedZ;
                            }
                        }
                    }
                    x++;
                }
            }
            else
            {
                if (run / rise > 0.5)
                    txl = 92;// /
                else if (run / rise < -0.5)
                    txl = 47;// \
                else
                    txl = 124;// -

                if (y0 > y1)
                {
                    swap(ref x0, ref x1);
                    swap(ref y0, ref y1);
                }

                float x = x0;
                float invSlope = 1 / m;

                while (y0 < y1)
                {
                    image[(int)x, y0] = txl;
                    x += invSlope;
                    y0++;
                }
            }
            return (image);
        }
    }
}
