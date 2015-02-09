using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ConsoleGraphics
{
    class Material
    //convert textures
    {
        public byte[,] bitmapColorsCached;
        public int SIZE;

        public Material(string fileName)
        {
            Bitmap sourceBmp = (Bitmap)Image.FromFile(fileName, true);//load character set (digits 1->9..)
            bitmapColorsCached = new byte[sourceBmp.Width, sourceBmp.Height];
            SIZE = sourceBmp.Width;
            for (int i = 0; i < sourceBmp.Width; i++)
                for (int j = 0; j < sourceBmp.Height; j++)
                {
                    Color c = sourceBmp.GetPixel(i, j);
                    bitmapColorsCached[i,j] = (byte)((c.R + c.B + c.G) / 3);
                }
        }
    }
}
