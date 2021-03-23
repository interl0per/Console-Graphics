/* Rasterization - all the grunt work!
 * Determine pixle 'colors'..
 */

using System;
using ConsoleGraphics.Maths;

namespace ConsoleGraphics.Render
{
    public class Rasterizer
    {
        public static int[,] zBuffer = new int[Game.RENDER_WIDTH, Game.RENDER_HEIGHT];

        private static Triangle TargetFace;

        public static byte[,] RenderVertices(Mesh someShape)
        {
            byte[,] image = new byte[Game.RENDER_WIDTH, Game.RENDER_HEIGHT];

            for (int x = 0; x < Game.RENDER_WIDTH; x++)
                for (int y = 0; y < Game.RENDER_HEIGHT; y++)
                    image[x, y] = 32;

            foreach (Triangle f in someShape.Faces)
            {
                // each vertex
                for (int i = 0; i < 3; i++)
                {
                    double depth = someShape.Vertices[f.VertexIds[i]].Z;
                    int P_x1 = (int)(someShape.Vertices[f.VertexIds[i]].X / depth);
                    int P_y1 = (int)(someShape.Vertices[f.VertexIds[i]].Y / depth);

                    //the vert is in bounds with the screen
                    if (P_y1 < Game.RENDER_HEIGHT - 1 && P_y1 >= 0 && P_x1 < Game.RENDER_WIDTH - 1 && P_x1 >= 0)
                        image[P_x1, P_y1] = 177;
                }
            }
            return (image);
        }

        public static byte[,] RenderWire(Mesh someShape)
        {
            byte[,] image = new byte[Game.RENDER_WIDTH, Game.RENDER_HEIGHT];//

            for (int x = 0; x < Game.RENDER_WIDTH; x++)
                for (int y = 0; y < Game.RENDER_HEIGHT; y++)
                    image[x, y] = 32;

            foreach (Triangle f in someShape.Faces)
            {
                for (int i = 0; i < 3; i++)//each vertex
                {
                    double depth = someShape.Vertices[f.VertexIds[i]].Z;
                    int P_x1 = (int)((Game.RENDER_WIDTH / 2) + someShape.Vertices[f.VertexIds[i]].X / depth);
                    int P_y1 = (int)(((Game.RENDER_HEIGHT - 15) / 2) + someShape.Vertices[f.VertexIds[i]].Y / depth);

                    //the vert is in bounds with the screen
                    if (P_y1 < Game.RENDER_HEIGHT - 1 && P_y1 >= 0 && P_x1 < Game.RENDER_WIDTH - 1 && P_x1 >= 0)
                    {
                        //each of the 2 neighboring verts
                        for (int j = 0; j < 2; j++)
                        {
                            double depth2 = someShape.Vertices[f.VertexIds[j]].Z;
                            int P_x = (int)((Game.RENDER_WIDTH / 2) + someShape.Vertices[f.VertexIds[j]].X / depth2);
                            int P_y = (int)(((Game.RENDER_HEIGHT - 15) / 2) + someShape.Vertices[f.VertexIds[j]].Y / depth2);

                            if (P_y < Game.RENDER_HEIGHT - 1 && P_y >= 0 && P_x < Game.RENDER_WIDTH - 1 && P_x >= 0)
                                image = DrawLine(image, P_x, P_y, P_x1, P_y1, true);
                        }
                    }
                }
            }
            return (image);
        }

        public static byte[,] RenderSolid(Mesh someShape)
        {
            byte[,] image = new byte[Game.RENDER_WIDTH, Game.RENDER_HEIGHT];

            for (int x = 0; x < Game.RENDER_WIDTH; x++)
            {
                for (int y = 0; y < Game.RENDER_HEIGHT; y++)
                {
                    image[x, y] = 32;       //space character is drawn faster than the null character (0)
                    zBuffer[x, y] = -100;   //far clipping plane
                }
            }

            foreach (Triangle f in someShape.Faces)
            {
                TargetFace = f;
                Vector3 a = Vector3.Add(someShape.Vertices[f.VertexIds[0]], new Vector3(-someShape.Vertices[f.VertexIds[2]].X, -someShape.Vertices[f.VertexIds[2]].Y, -someShape.Vertices[f.VertexIds[2]].Z));
                Vector3 b = Vector3.Add(someShape.Vertices[f.VertexIds[0]], new Vector3(-someShape.Vertices[f.VertexIds[1]].X, -someShape.Vertices[f.VertexIds[1]].Y, -someShape.Vertices[f.VertexIds[1]].Z));
                Vector3 surfaceNorm1 = Vector3.Cross(a, b);

                surfaceNorm1 = Vector3.Normalize(surfaceNorm1);

                Vector3 lightNorm1 = Vector3.Add(Game.light1.Coordinates, new Vector3(-someShape.Vertices[f.VertexIds[0]].X, -someShape.Vertices[f.VertexIds[0]].Y, -someShape.Vertices[f.VertexIds[0]].Z));//arbitrary vert
                lightNorm1 = Vector3.Normalize(lightNorm1);
                ////////////////////////////////////
                Vector3 a1 = Vector3.Add(someShape.Vertices[f.VertexIds[1]], new Vector3(-someShape.Vertices[f.VertexIds[2]].X, -someShape.Vertices[f.VertexIds[2]].Y, -someShape.Vertices[f.VertexIds[2]].Z));
                Vector3 b1 = Vector3.Add(someShape.Vertices[f.VertexIds[1]], new Vector3(-someShape.Vertices[f.VertexIds[0]].X, -someShape.Vertices[f.VertexIds[0]].Y, -someShape.Vertices[f.VertexIds[0]].Z));
                Vector3 surfaceNorm2 = Vector3.Cross(a1, b1);

                surfaceNorm2 = Vector3.Normalize(surfaceNorm2);

                Vector3 lightNorm2 = Vector3.Add(Game.light1.Coordinates, new Vector3(-someShape.Vertices[f.VertexIds[1]].X, -someShape.Vertices[f.VertexIds[1]].Y, -someShape.Vertices[f.VertexIds[1]].Z));//arbitrary vert
                lightNorm2 = Vector3.Normalize(lightNorm2);
                //////////////////////////////////////////////
                Vector3 a2 = Vector3.Add(someShape.Vertices[f.VertexIds[2]], new Vector3(-someShape.Vertices[f.VertexIds[0]].X, -someShape.Vertices[f.VertexIds[0]].Y, -someShape.Vertices[f.VertexIds[0]].Z));
                Vector3 b2 = Vector3.Add(someShape.Vertices[f.VertexIds[2]], new Vector3(-someShape.Vertices[f.VertexIds[1]].X, -someShape.Vertices[f.VertexIds[1]].Y, -someShape.Vertices[f.VertexIds[1]].Z));
                Vector3 surfaceNorm3 = Vector3.Cross(a2, b2);

                surfaceNorm3 = Vector3.Normalize(surfaceNorm3);

                Vector3 lightNorm3 = Vector3.Add(Game.light1.Coordinates, new Vector3(-someShape.Vertices[f.VertexIds[2]].X, -someShape.Vertices[f.VertexIds[2]].Y, -someShape.Vertices[f.VertexIds[2]].Z));//arbitrary vert
                lightNorm3 = Vector3.Normalize(lightNorm3);
                ////////////////////////////////////////////

                //perspective projection
                float[] depth = {someShape.Vertices[f.VertexIds[0]].Z,
                                 someShape.Vertices[f.VertexIds[1]].Z,
                                 someShape.Vertices[f.VertexIds[2]].Z};

                if (depth[0] < 0 && depth[1] < 0 && depth[2] < 0)
                {                    //>= one vertex is in screen
                    int x0 = (int)((Game.RENDER_WIDTH / 2) + someShape.Vertices[f.VertexIds[0]].X / depth[0]);
                    int y0 = (int)(((Game.RENDER_HEIGHT - 15) / 2) + someShape.Vertices[f.VertexIds[0]].Y / depth[0]);

                    int x1 = (int)((Game.RENDER_WIDTH / 2) + someShape.Vertices[f.VertexIds[1]].X / depth[1]);
                    int y1 = (int)(((Game.RENDER_HEIGHT - 15) / 2) + someShape.Vertices[f.VertexIds[1]].Y / depth[1]);

                    int x2 = (int)((Game.RENDER_WIDTH / 2) + someShape.Vertices[f.VertexIds[2]].X / depth[2]);
                    int y2 = (int)(((Game.RENDER_HEIGHT - 15) / 2) + someShape.Vertices[f.VertexIds[2]].Y / depth[2]);


                    Vector2[] uvs = new Vector2[]
                    {
                        someShape.UVs[f.UVIds[0]],
                        someShape.UVs[f.UVIds[1]],
                        someShape.UVs[f.UVIds[2]]
                    };

                    Vector3[] projPoints = new Vector3[]
                    {
                        new Vector3(x0, y0, depth[0]),
                        new Vector3(x1, y1, depth[1]),
                        new Vector3(x2, y2, depth[2])
                    };

                    byte[] colors = new byte[] 
                    {
                        (byte)(Game.light1.Intensity * Math.Max(0, Vector3.Dot(surfaceNorm1, lightNorm1))),
                        (byte)(Game.light1.Intensity * Math.Max(0, Vector3.Dot(surfaceNorm2, lightNorm2))),
                        (byte)(Game.light1.Intensity * Math.Max(0, Vector3.Dot(surfaceNorm3, lightNorm3)))
                    };

                    image = DrawTriangle(image, projPoints, uvs, colors);
                }
            }
            return image;
        }

        private static byte[,] DrawTriangle(byte[,] image, Vector3[] points, Vector2[] uvs, byte[] brightness = null)
        {
            /*
             *      A
             *  B------- 
             *          C
             * 
             */
            //sort by lowest y value (highest on screen)
            Vector3[] unsortedPoints = new Vector3[3];
            points.CopyTo(unsortedPoints, 0);

            if (points[1].Y < points[0].Y)
            {//swap point[1] and point[0]
                MathHelper.Swap(ref points[0].Y, ref points[1].Y);
                MathHelper.Swap(ref points[0].X, ref points[1].X);
            }
            if (points[2].Y < points[0].Y)
            {//swap point[0] and point[2]
                MathHelper.Swap(ref points[0].Y, ref points[2].Y);
                MathHelper.Swap(ref points[0].X, ref points[2].X);
            }
            if (points[2].Y < points[1].Y)
            {//swap point[1] & point[2]
                MathHelper.Swap(ref points[1].Y, ref points[2].Y);
                MathHelper.Swap(ref points[1].X, ref points[2].X);
            }

            if (points[0].Y == points[1].Y)
            {//flat top
                image = FillFlatTop(image, points, unsortedPoints, brightness, uvs);
            }
            else if (points[1].Y == points[2].Y)
            {//flat bottom
                image = FillFlatBottom(image, points, unsortedPoints, brightness, uvs);
            }
            else
            {//nontrivial
                Vector3 midpoint = new Vector3((int)(Math.Ceiling(points[0].X + ((float)(points[1].Y - points[0].Y) / (float)(points[2].Y - points[0].Y)) * (points[2].X - points[0].X))), points[1].Y);
                FillFlatBottom(image, new Vector3[] { points[0], points[1], midpoint }, unsortedPoints, brightness, uvs);
                FillFlatTop(image, new Vector3[] { points[1], midpoint, points[2] }, unsortedPoints, brightness, uvs);
            }
            return (image);
        }

        private static byte[,] FillFlatTop(byte[,] image, Vector3[] sortedPoints, Vector3[] unsortedPoints, byte[] brightness, Vector2[] uvs = null)
        {
            float startX = sortedPoints[2].X;
            float endX = sortedPoints[2].X;

            float invslope1 = (float)(sortedPoints[2].X - sortedPoints[0].X) / (float)(sortedPoints[2].Y - sortedPoints[0].Y);
            float invslope2 = (float)(sortedPoints[2].X - sortedPoints[1].X) / (float)(sortedPoints[2].Y - sortedPoints[1].Y);

            for (int scanlineY = (int)sortedPoints[2].Y; scanlineY > sortedPoints[0].Y; scanlineY--)
            {
                DrawLine(image, (int)startX, scanlineY, (int)endX, scanlineY, false, unsortedPoints, brightness, uvs);
                startX -= invslope1;
                endX -= invslope2;
            }
            return (image);
        }

        private static byte[,] FillFlatBottom(
            byte[,] image, 
            Vector3[] sortedPoints, 
            Vector3[] unsortedPoints, 
            byte[] brightness, 
            Vector2[] uvs = null)
        {
            float invslope1 = (float)(sortedPoints[2].X - sortedPoints[0].X) / (float)(sortedPoints[2].Y - sortedPoints[0].Y);
            float invslope2 = (float)(sortedPoints[0].X - sortedPoints[1].X) / (float)(sortedPoints[0].Y - sortedPoints[1].Y);

            float curx1 = sortedPoints[0].X;
            float curx2 = sortedPoints[0].X;

            for (int scanlineY = (int)sortedPoints[0].Y; scanlineY <= sortedPoints[1].Y; scanlineY++)
            {
                DrawLine(image, (int)curx1, scanlineY, (int)curx2, scanlineY, false, unsortedPoints, brightness, uvs);
                curx1 += invslope1;
                curx2 += invslope2;
            }
            return (image);
        }

        public static byte[,] DrawLine(
            byte[,]     image, 
            // start
            int x0, int y0, 
            int x1, int y1,
            // end
            bool        drawWireFrame, 
            Vector3[]   unsortedPoints = null, 
            byte[]      color = null, 
            Vector2[]   uvs = null)
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
                    MathHelper.Swap(ref y0, ref y1);

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
                    MathHelper.Swap(ref x0, ref x1);
                    MathHelper.Swap(ref y0, ref y1);
                }

                float y = y0;
                float x = x0;
                float denom = 0;
                if (!drawWireFrame)
                    denom = ((unsortedPoints[1].Y - unsortedPoints[2].Y) * (unsortedPoints[0].X - unsortedPoints[2].X) + (unsortedPoints[2].X - unsortedPoints[1].X) * (unsortedPoints[0].Y - unsortedPoints[2].Y));

                while (x <= x1 && y < Game.RENDER_HEIGHT && x < Game.RENDER_WIDTH && y >= 0)
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
                        else
                        {
                            //HERE'S WHERE WE DRAW TRIANGLES!
                            float b1 = (((unsortedPoints[1].Y - unsortedPoints[2].Y) * (x - unsortedPoints[2].X)) + ((unsortedPoints[2].X - unsortedPoints[1].X) * (y - unsortedPoints[2].Y))) / denom;
                            float b2 = (((unsortedPoints[2].Y - unsortedPoints[0].Y) * (x - unsortedPoints[2].X)) + ((unsortedPoints[0].X - unsortedPoints[2].X) * (y - unsortedPoints[2].Y))) / denom;
                            float b3 = 1 - b1 - b2;

                            int interpolatedZ = (int)(b1 * unsortedPoints[0].Z + b2 * unsortedPoints[1].Z + b3 * unsortedPoints[2].Z);

                            if (interpolatedZ > zBuffer[xtoi, ytoi])
                            { 
                                // use barycentric interpolation to do everything..
                                float interpU = (float)(b1 * uvs[0].X + b2 * uvs[1].X + b3 * uvs[2].X);
                                float interpV = (float)(b1 * uvs[0].Y + b2 * uvs[1].Y + b3 * uvs[2].Y);
                                float interpBright = color[1];// + b2 * (float)color[1] + b3 * (float)color[2]);
                                Material material = Game.ActiveMesh.Materials[TargetFace.MaterialId - 1];

                                int xpos = Math.Min(Math.Max((int)(interpU * material.Size), 0), material.Size - 1);
                                int ypos = Math.Min(Math.Max((int)(interpV * material.Size), 0), material.Size - 1);

                                byte fa = material.BitmapColorsCached[(int)MathHelper.Clamp(xpos, 0, material.Size - 1), (int)MathHelper.Clamp(ypos, 0, material.Size - 1)];
                                image[xtoi, ytoi] = (byte)(Convert.ToInt32(Game.levels[(int)MathHelper.Clamp(fa + interpBright, 0, 255)]));
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
                    MathHelper.Swap(ref x0, ref x1);
                    MathHelper.Swap(ref y0, ref y1);
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
