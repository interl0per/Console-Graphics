/* Rasterization - all the grunt work!
 * Determine pixle 'colors'..
 * 
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

        public byte[,] renderWire(Mesh someShape)
        {
            int z = Environment.TickCount;
            byte[,] image = new byte[RENDER_WIDTH, RENDER_HEIGHT];//

            for (int x = 0; x < RENDER_WIDTH; x++)
                for (int y = 0; y < RENDER_HEIGHT; y++)
                    image[x, y] = 32;

            foreach (Mesh.triangle f in someShape.faces)
            {
                for (int i = 0; i < 3; i++)//each vertex
                {
                    double depth = someShape.verts[f.vertIDs[i] - 1].z;
                    int P_x1 = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[i] - 1].x / -depth);
                    int P_y1 = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[i] - 1].y / -depth);

                    if (P_y1 < RENDER_HEIGHT - 1 && P_y1 >= 0 && P_x1 < RENDER_WIDTH - 1 && P_x1 >= 0)//the vert is in bounds with the screen
                    {
                        for (int j = 0; j < 2; j++)//each of the 2 neighboring verts
                        {
                            double depth2 = someShape.verts[f.vertIDs[j] - 1].z;
                            int P_x = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[j] - 1].x / -depth2);
                            int P_y = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[j] - 1].y / -depth2);

                            if (P_y < RENDER_HEIGHT - 1 && P_y >= 0 && P_x < RENDER_WIDTH - 1 && P_x >= 0)
                                image = drawLine(image, P_x, P_y, P_x1, P_y1, true);
                        }
                    }
                }
            }
            z = Environment.TickCount - z;
            return (image);
        }

        public byte[,] renderSolid(Mesh someShape)
        {
            int z = Environment.TickCount;
            byte[,] image = new byte[RENDER_WIDTH, RENDER_HEIGHT];//

            for (int x = 0; x < RENDER_WIDTH; x++)
                for (int y = 0; y < RENDER_HEIGHT; y++)
                    image[x, y] = 32;

            foreach (Mesh.triangle f in someShape.faces)
            {
                Mesh.point3 surfaceNorm = Matrix.cross(someShape.verts[f.vertIDs[0] - 1], someShape.verts[f.vertIDs[1] - 1]);
                Mesh.point3 lightNorm = Matrix.add(new Mesh.point3(0, 10, 0), someShape.verts[f.vertIDs[0] - 1]);//arbitrary thing
                float lighting = Matrix.dot(surfaceNorm, lightNorm);

                double depth1 = someShape.verts[f.vertIDs[0] - 1].z;
                int x0 = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[0] - 1].x / -depth1);
                int y0 = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[0] - 1].y / -depth1);

                double depth2 = someShape.verts[f.vertIDs[1] - 1].z;
                int x1 = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[1] - 1].x / -depth2);
                int y1 = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[1] - 1].y / -depth2);

                double depth3 = someShape.verts[f.vertIDs[2] - 1].z;
                int x2 = (int)((RENDER_WIDTH / 2) + someShape.verts[f.vertIDs[2] - 1].x / -depth3);
                int y2 = (int)(((RENDER_HEIGHT - 15) / 2) + someShape.verts[f.vertIDs[2] - 1].y / -depth3);

                if ((y0 < RENDER_HEIGHT && y0 >= 0 && x0 < RENDER_WIDTH && x0 > 0) || (y1 < RENDER_HEIGHT && y1 >= 0 && x1 < RENDER_WIDTH && x1 >= 0) || (y2 < RENDER_HEIGHT && y2 >= 0 && x2 < RENDER_WIDTH && x2 >= 0))
                {   //>= one vertex is in screen
                    //byte col = (byte)(Matrix.dot(Matrix.cross(someShape.verts[f.vertIDs[0] - 1], someShape.verts[f.vertIDs[1] - 1]), new Mesh.point3(-200, 10, 3)));
                    image = drawTriangle(image, new Mesh.point3((float)x0, (float)y0, (float)depth1), new Mesh.point3((float)x1, (float)y1, (float)depth2), new Mesh.point3((float)x2, (float)y2, (float)depth3));
                }
            }
            z = Environment.TickCount - z;
            return (image);
        }

        private byte[,] drawTriangle(byte[,] image, Mesh.point3 p1, Mesh.point3 p2, Mesh.point3 p3, byte color = 0)
        {
            //want to apply a texture with p1, p2, p3 as verts.

            //sort by lowest y value (highest on screen)
            Mesh.point3 a = p1;
            Mesh.point3 b = p2;
            Mesh.point3 c = p3;

            if (p2.y < p1.y)
            {
                int tempy = (int)p1.y;
                p1.y = p2.y;
                p2.y = tempy;
                int tempx = (int)p1.x;
                p1.x = p2.x;
                p2.x = tempx;
            }
            if (p3.y < p1.y)
            {
                int tempy = (int)p1.y;
                p1.y = p3.y;
                p3.y = tempy;
                int tempx = (int)p1.x;
                p1.x = p3.x;
                p3.x = tempx;
            }
            if (p3.y < p2.y)
            {
                int tempy = (int)p2.y;
                p2.y = p3.y;
                p3.y = tempy;
                int tempx = (int)p2.x;
                p2.x = p3.x;
                p3.x = tempx;
            }

            if (p1.y == p2.y)
            {//flat top
                image = fillFlatTop(image, p1, p2, p3, a, b, c, color);
            }
            else if (p2.y == p3.y)
            {//flat bottom
                image = fillFlatBottom(image, p1, p2, p3, a, b, c, color);
            }
            else
            {//nontrivial
                Mesh.point3 midpoint = new Mesh.point3((int)(Math.Ceiling(p1.x + ((float)(p2.y - p1.y) / (float)(p3.y - p1.y)) * (p3.x - p1.x))), p2.y);
                fillFlatBottom(image, p1, p2, midpoint, a, b, c, color);
                fillFlatTop(image, p2, midpoint, p3, a, b, c, color);
            }
            return (image);
        }

        private byte[,] fillFlatTop(byte[,] image, Mesh.point3 p1, Mesh.point3 p2, Mesh.point3 p3, Mesh.point3 a, Mesh.point3 b, Mesh.point3 c, byte color)
        {
            float invslope1 = (float)(p3.x - p1.x) / (float)(p3.y - p1.y);
            float invslope2 = (float)(p3.x - p2.x) / (float)(p3.y - p2.y);

            float curx1 = p3.x;
            float curx2 = p3.x;

            for (int scanlineY = (int)p3.y; scanlineY > p1.y; scanlineY--)
            {
                curx1 -= invslope1;
                curx2 -= invslope2;
                drawLine(image, (int)curx1, scanlineY, (int)curx2, scanlineY, false, a, b, c, color);
            }
            return (image);
        }

        private byte[,] fillFlatBottom(byte[,] image, Mesh.point3 p1, Mesh.point3 p2, Mesh.point3 p3, Mesh.point3 a, Mesh.point3 b, Mesh.point3 c, byte color)
        {
            float invslope1 = (float)(p3.x - p1.x) / (float)(p3.y - p1.y);
            float invslope2 = (float)(p1.x - p2.x) / (float)(p1.y - p2.y);

            float curx1 = p1.x;
            float curx2 = p1.x;

            for (int scanlineY = (int)p1.y; scanlineY <= p2.y; scanlineY++)
            {
                drawLine(image, (int)curx1, scanlineY, (int)curx2, scanlineY, false, a, b, c, color);
                curx1 += invslope1;
                curx2 += invslope2;
            }
            return (image);
        }

        public byte[,] drawLine(byte[,] image, int x0, int y0, int x1, int y1, bool drawWireFrame, Mesh.point3 a = new Mesh.point3(), Mesh.point3 b = new Mesh.point3(), Mesh.point3 c = new Mesh.point3(), byte color = 0)
        {
            float rise = y1 - y0;
            float run = x1 - x0;
            byte txl = 219;
            float denom = ((b.y - c.y) * (a.x - c.x) + (c.x - b.x) * (a.y - c.y));

            /*
             *      A
             *  B   .   
             *          C
             * 
             */

            if (run == 0)
            {
                if (drawWireFrame)
                    txl = 124;// |
                if (y0 > y1)
                {
                    int tempY1 = y1;
                    y1 = y0;
                    y0 = tempY1;
                }
                for (; y0 < y1; y0++)//DRAW
                {
                    float b1 = (((b.y - c.y) * (x0 - c.x)) + ((c.x - b.x) * (y0 - c.y))) / denom;
                    float b2 = (((c.y - a.y) * (x0 - c.x)) + ((a.x - c.x) * (y0 - c.y))) / denom;
                    float b3 = 1 - b1 - b2;

                    float u = b1;
                    float v = b2;
                    int xpos = Math.Min(Math.Max((int)(u * 128), 0), 127);
                    int ypos = Math.Min(Math.Max((int)(v * 128), 0), 127);

                    byte fa = ASCIIRender.bitmapColorsCached[xpos, ypos];
                    image[x0, y0] = (byte)Convert.ToInt32(ASCIIRender.levels[fa]);
                }
                return (image);
            }
            float m = rise / run;
            if (-1 <= rise / run && rise / run <= 1)
            {
                if (drawWireFrame)
                {
                    if (rise / run > 0.5)
                    {
                        txl = 92;// /
                    }
                    else if (rise / run < -0.5)
                    {
                        txl = 47;// \
                    }
                    else
                    {
                        txl = 95;// -
                    }
                }
                if (x0 > x1)
                {
                    int tx0 = x0;
                    x0 = x1;
                    x1 = tx0;
                    int ty0 = y0;
                    y0 = y1;
                    y1 = ty0;
                }
                float y = y0;
                for (; x0 <= x1 && y < RENDER_HEIGHT && x0 < RENDER_WIDTH && y >= 0; x0++)//DRAW
                {
                    if (x0 > 0)
                    {
                        float b1 = (((b.y - c.y) * (x0 - c.x)) + ((c.x - b.x) * (y - c.y))) / denom;
                        float b2 = (((c.y - a.y) * (x0 - c.x)) + ((a.x - c.x) * (y - c.y))) / denom;
                        float b3 = 1 - b1 - b2;

                        float u = b1;
                        float v = b2;
                        int xpos = Math.Min(Math.Max((int)(u * 128), 0), 127);
                        int ypos = Math.Min(Math.Max((int)(v * 128), 0), 127);

                        byte fa = ASCIIRender.bitmapColorsCached[xpos, ypos];
                        image[x0, (int)y] = (byte)Convert.ToInt32(ASCIIRender.levels[fa]);
                    }
                    y += m;
                }
            }
            //else
            //{
            //    if (drawWireFrame)
            //    {
            //        if (run / rise > 0.5)
            //        {
            //            txl = 92;// /
            //        }
            //        else if (run / rise < -0.5)
            //        {
            //            txl = 47;// \
            //        }
            //        else
            //        {
            //            txl = 124;// -
            //        }
            //    }

            //    if (y0 > y1)
            //    {
            //        int tx0 = x0;
            //        x0 = x1;
            //        x1 = tx0;
            //        int ty0 = y0;
            //        y0 = y1;
            //        y1 = ty0;
            //    }
            //    float x = x0;
            //    float invSlope = 1 / m;
            //    for (; y0 < y1; y0++)//DRAW
            //    {
            //        //float b1 = (((b.y - c.y) * (x - c.x)) + ((c.x - b.x) * (y0 - c.y))) / denom;
            //        //float b2 = (((c.y - a.y) * (x - c.x)) + ((a.x - c.x) * (y0 - c.y))) / denom;
            //        //float b3 = 1 - b1 - b2;

            //        //float u = b1;
            //        //float v = b2;
            //        //int xpos = Math.Min(Math.Max((int)(u * 128), 0), 127);
            //        //int ypos = Math.Min(Math.Max((int)(v * 128), 0), 127);

            //        //byte fa = ASCIIRender.bitmapColorsCached[xpos, ypos];
            //        //image[(int)x, y0] = (byte)Convert.ToInt32(ASCIIRender.levels[fa]);

            //        //x += invSlope;
            //    }
            //}
            return (image);
        }
    }
}
