using System;
using System.IO;
using System.Threading;

namespace ConsoleGraphics.Render
{
    /// <summary>
    /// Draws a buffer (an array of bytes) to the console buffer/screen
    /// </summary>
    public static class FrameBuffer
    {
        private static int LastRenderTick;
        private static int numRenderings = 0;
        private const short sampleSize = 100;
        //private string lastFrame = "";

        /// <summary>
        /// STDOUT
        /// </summary>
        public static Stream ConsoleOutput = Console.OpenStandardOutput();

        // defining it static. saves creating a 30000 byte long array every time the game renders
        // probably makes the garbage collector happier too lol
        private static byte[] ViewportBuffer = new byte[Game.RENDER_HEIGHT * Game.RENDER_WIDTH];

        public static void DrawFrame(byte[,] image, int a = 0, int b = 0)
        {
            //use cudafy / multithreading to paint quickly, also invoke writetoconsole?

            for (int x = 0; x < Game.RENDER_WIDTH; x++)
            {
                for (int y = 0; y < Game.RENDER_HEIGHT; y++)
                {
                    ViewportBuffer[x + y * Game.RENDER_WIDTH] = image[x, y];
                }
            }

            Console.SetCursorPosition(a, b);

            int beginRender = Environment.TickCount;
            //.Flush();
            //string iString = bufImg.ToString();
            //byte[] b = Encoding.UTF8.GetBytes(iString);
            //if (string.Compare(lastFrame, iString) != 0)
            //{
                  ConsoleOutput.Write(ViewportBuffer, 0, ViewportBuffer.Length);
            //    lastFrame = iString;
            //}
            int endRender = Environment.TickCount - beginRender;

            VerticalSync(Game.MAX_FPS, endRender, beginRender);

            if (numRenderings == 0)
                LastRenderTick = Environment.TickCount;

            numRenderings++;
        }

        public static void VerticalSync(short targetFrameRate, int delay, int startDrawTime)
        {
            //Synchronize frames and display framerate to the lower right corner of the render window
            int targetDelay = 1000 / targetFrameRate;
            //we're too fast
            if (delay < targetDelay && Game.USE_VSYNC)
            {
                Thread.Sleep(targetDelay - delay);
            }

            if (numRenderings == sampleSize)
            {
                int ticksElapsed = Environment.TickCount - LastRenderTick;
                if (ticksElapsed != 0)
                    Game.printch((sampleSize * 1000 / ticksElapsed).ToString().ToCharArray(), Game.RENDER_WIDTH - 50, Game.RENDER_HEIGHT + 1);
                numRenderings = 0;
            }
        }
    }
}
