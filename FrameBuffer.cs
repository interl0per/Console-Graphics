/*
 *                                                  Print image to screen. 
 * 
 *  Just 'draw' the byte array to the buffer.
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleGraphics
{
    class FrameBuffer : Program
    {
        private static int lastTime;
        private static int numRenderings = 0;
        private const short sampleSize = 100;
        private string lastFrame = "";
        Stream s = Console.OpenStandardOutput();
        
        private void vSync(short targetFrameRate, int delay, int startDrawTime)
        {
            //Synchronize frames and display framerate to the lower right corner of the render window
            int targetDelay = 1000 / targetFrameRate;
            if (delay < targetDelay && VSYNC_ON)//we're too fast
            {
                System.Threading.Thread.Sleep(targetDelay - delay);
            }

            if (numRenderings == sampleSize)
            {
                int y = Environment.TickCount - lastTime;
                if (y != 0)
                    printch((sampleSize * 1000 / y).ToString().ToCharArray(), RENDER_WIDTH - 50, RENDER_HEIGHT + 1);
                numRenderings = 0;
            }
        }

        public void drawFrame(byte[,] image, int a = 0, int b = 0)
        {
            //use cudafy / multithreading to paint quickly, also invoke writetoconsole?
            byte[] bufImg = new byte[RENDER_HEIGHT * RENDER_WIDTH];

            for (int x = 0; x < RENDER_WIDTH; x++)
                for (int y = 0; y < RENDER_HEIGHT; y++)
                    bufImg[x+y*RENDER_WIDTH] = image[x,y];
            int beginRender = Environment.TickCount;

            Console.SetCursorPosition(a, b);
            //.Flush();
            //string iString = bufImg.ToString();
            //byte[] b = Encoding.UTF8.GetBytes(iString);
            //if (string.Compare(lastFrame, iString) != 0)
            //{
                s.Write(bufImg, 0, bufImg.Length);
            //    lastFrame = iString;
            //}
            int endRender = Environment.TickCount - beginRender;

            vSync(MAX_FPS, endRender, beginRender);

            if (numRenderings == 0)
                lastTime = Environment.TickCount;

            numRenderings++;
        }
    }
}
