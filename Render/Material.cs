using System.Drawing;

namespace ConsoleGraphics.Render
{
    //convert textures
    public class Material
    {
        public byte[,] BitmapColorsCached;
        public int Size;

        public Material(string fileName)
        {
            //load character set (digits 1->9..)
            Bitmap sourceBmp = (Bitmap)Image.FromFile(fileName, true);
            BitmapColorsCached = new byte[sourceBmp.Width, sourceBmp.Height];
            Size = sourceBmp.Width;
            for (int w = 0; w < sourceBmp.Width; w++)
            {
                for (int h = 0; h < sourceBmp.Height; h++)
                {
                    Color c = sourceBmp.GetPixel(w, h);
                    BitmapColorsCached[w, h] = (byte)((c.R + c.B + c.G) / 3);
                }
            }
        }
    }
}
