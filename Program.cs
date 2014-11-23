using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using ConsoleExtender;
using System.Drawing;

namespace ConsoleGraphics
{
    class Program
    {
        //special characters:█ ▄ ▀ ■
        public bool VSYNC_ON = true;
        public static bool renderWireFr = true;
        //    public bool FRAMERATE_ON = true;
        public const short RENDER_WIDTH = 300;
        public const short RENDER_HEIGHT = 100;
        public const short MAX_FPS = 1000;
        public const float pi = (float)3.1415926535;
        static char[, ,] numerals = new char[9, 8, 10];

        static void init(short width, short height, string title)
        {
            Console.Title = title;
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            ConsoleHelper.SetConsoleFont(0);
            Console.SetBufferSize(width, height + 15);
            Console.SetWindowSize(width, height + 15);
            Console.Clear();//clear colors from user preset.
            Console.SetCursorPosition(0, height);
            Console.Write(new String('▄', RENDER_WIDTH));
            Console.ForegroundColor = ConsoleColor.White;

            Bitmap image1 = (Bitmap)Image.FromFile(@"charset.bmp", true);//load character set (digits 1->9..)
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        if (image1.GetPixel(j + 9 * i, k).B == 0)
                        {
                            numerals[i, j, k] = 'x';
                        }
                    }
                }
            }
            Thread cin = new Thread(getInput);
            cin.Start();
            Thread soundPlayer = new Thread(playSound);
            soundPlayer.Start();
        }

        static Mesh loadObj(string loc)
        {
            StreamReader r = new StreamReader(loc + ".obj");
            List<Mesh.point3> loadedVerts = new List<Mesh.point3>();
            List<Mesh.triangle> loadedFaces = new List<Mesh.triangle>();

            int vertCount = 0;
            int faceCount = 0;

            while (!r.EndOfStream)
            {
                string e1 = r.ReadLine();
                if (e1 != "")
                {
                    if (e1[0] == 'v')//vertex
                    {
                        vertCount++;
                        string temp = e1.Substring(3, e1.Length - 3);
                        string[] coords = temp.Split(' ');
                        Mesh.point3 p = new Mesh.point3(float.Parse(coords[0]) * 100, float.Parse(coords[1]) * 100, -float.Parse(coords[2]));
                        //if z is positive it's off screen, z=0 we clip
                        loadedVerts.Add(p);
                    }
                    else if (e1[0] == 'f')//edge
                    {
                        faceCount++;
                        string temp = e1.Substring(2, e1.Length - 2);
                        string[] coords = temp.Split(' ');
                        Mesh.triangle f = new Mesh.triangle(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
                        loadedFaces.Add(f);
                    }
                }
            }
            return (new Mesh(loadedVerts.ToArray(), loadedFaces.ToArray()));
        }

        static Mesh someShape = loadObj("box");

        public static void Main(string[] args)
        {
            ASCIIRender.bt();
            init(RENDER_WIDTH, RENDER_HEIGHT, "render");
            FrameBuffer buffer = new FrameBuffer();
            Rasterizer rast = new Rasterizer();
            someShape.translate(new Mesh.point3(0, 10, 0));
            //someShape.rotate((float)0.25);

            buffer.drawFrame(rast.renderSolid(someShape));
            Console.ReadLine();

            for (double i = 0; i < 1000; i += 0.01)
            {
                //  buffer.drawFrame(drawLine(new byte[RENDER_HEIGHT * RENDER_WIDTH], a1, a2));
                if (renderWireFr)
                    buffer.drawFrame(rast.renderSolid(someShape));
                else
                    buffer.drawFrame(rast.renderWire(someShape));

                someShape.translate(new Mesh.point3(0, -(float)Math.Sin(i)/50, 0));
                someShape.rotate((float)0.01);
            }
            Console.ReadLine();
        }

        static void getInput()
        {
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey();
                renderWireFr = !renderWireFr;
            } while (cki.Key != ConsoleKey.Escape);
        }

        static void playSound()
        {
            while (true)
            {
                Console.Beep(446, 500);
                Console.Beep(440, 500);
                Console.Beep(440, 500);
                Console.Beep(349, 350);
                Console.Beep(523, 150);
                Console.Beep(440, 500);
                Console.Beep(349, 350);
                Console.Beep(523, 150);
                Console.Beep(440, 1000);
                Console.Beep(659, 500);
                Console.Beep(659, 500);
                Console.Beep(659, 500);
                Console.Beep(698, 350);
                Console.Beep(523, 150);
                Console.Beep(415, 500);
                Console.Beep(349, 350);
                Console.Beep(523, 150);
                Console.Beep(440, 1000);
            }
        }

        public static void printch(char[] charId, int x, int y)
        {
            Console.SetCursorPosition(0, RENDER_HEIGHT + 4);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(0, RENDER_HEIGHT + 4);
            Console.Write(charId);
        }
    }
}
