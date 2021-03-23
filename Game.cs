using System;
using System.Threading;
using System.IO;
using ConsoleGraphics.Maths;
using ConsoleGraphics.Render;
using ConsoleGraphics.Utils;

namespace ConsoleGraphics
{
    public static class Game
    {
        //special characters:█ ▄ ▀ ■
        //public bool FRAMERATE_ON = true;

        public static bool USE_VSYNC = false;
        public static ThreeState renderState = new ThreeState(0);
        public const short RENDER_WIDTH = 300;
        public const short RENDER_HEIGHT = 100;
        public const short MAX_FPS = 1000;
        public const float PI = (float)3.1415926535;

        //static char[, ,] numerals = new char[9, 8, 10];
        public static char[] levels;

        public static volatile Mesh ActiveMesh = null;
        public static Light light1 = new Light(0, 0, 0, 700);

        public static void Main(string[] args)
        {
            try
            {
                ActiveMesh = Mesh.LoadMeshFromFile(@"Resources\Meshes\link.obj");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to load mesh: {e.Message}");
                Console.Read();
                return;
            }

            //BrightnessInit.go();
            //BrightnessInit.sortstuff();
            GameInit(RENDER_WIDTH, RENDER_HEIGHT, "render");
            ActiveMesh.Translate(new Vector3(-800, -300, -8));
            //someShape.rotate((float)0.25);

            FrameBuffer.DrawFrame(Rasterizer.RenderSolid(ActiveMesh));
            //Console.ReadLine();
            //someShape.rotate((float)(3.1415));

            // render loop
            while (true)
            {
                //light1.intensity = (int)((double)800 * Math.Sin(4*i));
                //someShape.rotate(0.005, 0);
                //someShape.rotate(0.005, 1);
                //someShape.rotate(0.005, 2);
                //someShape.translate(new Mesh.point3((float)Math.Sin(i), 0, 0));
                //  buffer.drawFrame(drawLine(new byte[RENDER_HEIGHT * RENDER_WIDTH], a1, a2));
                if (renderState.x == 0)
                    FrameBuffer.DrawFrame(Rasterizer.RenderSolid(ActiveMesh));
                else if (renderState.x == 1)
                    FrameBuffer.DrawFrame(Rasterizer.RenderVertices(ActiveMesh));
                else if (renderState.x == 2)
                    FrameBuffer.DrawFrame(Rasterizer.RenderWire(ActiveMesh));

                //light1.coords.x = 1000 * (float)Math.Sin(i);
                // light1.coords.y = 1000 * (float)Math.Sin(i);
                //light1.coords.z = 100 * (float)Math.Sin(i);
                //someShape.rotate((float)0.005, 0);
                //someShape.rotate((float)0.005, 1);
            }
        }

        private static void GameInit(short width, short height, string title)
        {
            Console.Title = title;
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;

            SetConsoleSize(width, height);

            //clear colors from user preset.
            Console.Clear();
            Console.SetCursorPosition(0, height);
            Console.Write(new String('▄', RENDER_WIDTH));
            Console.ForegroundColor = ConsoleColor.Cyan;

            StreamReader levelsReader = new StreamReader("Resources\\levels.txt");

            levels = new char[256];
            int index = 0;
            while (!levelsReader.EndOfStream && index < 256)
            {
                string e1 = levelsReader.ReadLine();
                levels[index] = Char.Parse(e1);
                index++;
            }

            Thread inputThread = new Thread(MainGetInput);
            inputThread.Start();
            //Thread soundPlayer = new Thread(playSound);
            //soundPlayer.Start();
        }

        private static void SetConsoleSize(short width, short height)
        {
            // loop forever. only return when the window size and buffer size is set
            while (true)
            {
                try
                {
                    Console.SetWindowSize(width, height + 15);
                    Console.SetBufferSize(width, height + 15);
                    // exit the function allowing everything else to initialise
                    return;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Decrease your font size and press enter");
                    Console.WriteLine("You can resize the font using 'CTRL + MouseWheel'");
                    Console.ReadLine();
                }
            }
        }

        private static byte[,] LoadTexture(string loc)
        {
            byte[,] loadedTexture = new byte[2, 2];
            return (loadedTexture);
        }

        public static void printch(char[] charId, int x, int y)
        {
            Console.SetCursorPosition(0, RENDER_HEIGHT + 4);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(0, RENDER_HEIGHT + 4);
            Console.Write(charId);
        }

        private static void MainGetInput()
        {
            //Console.TreatControlCAsInput = true;

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                    return;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Spacebar:
                        renderState.changeState();
                        break;
                    case ConsoleKey.W:
                        ActiveMesh.Translate(0, 0, 0.1f);
                        break;
                    case ConsoleKey.A:
                        ActiveMesh.Translate(-10.0f, 0, 0);
                        break;
                    case ConsoleKey.S:
                        ActiveMesh.Translate(0, 0, -0.1f);
                        break;
                    case ConsoleKey.D:
                        ActiveMesh.Translate(10.0f, 0, 0);
                        break;
                    case ConsoleKey.R:
                        ActiveMesh.Rotate(0.01, 0);
                        break;
                    case ConsoleKey.T:
                        ActiveMesh.Rotate(0.01, 1);
                        break;
                    case ConsoleKey.Y:
                        ActiveMesh.Rotate(0.01, 2);
                        break;
                    case ConsoleKey.Q:
                        ActiveMesh.Translate(0, -10.0f, 0);
                        break;
                    case ConsoleKey.E:
                        ActiveMesh.Translate(0, 10.0f, 0);
                        break;
                }
            }
        }
    }
}
