using ConsoleGraphics.Maths;

namespace ConsoleGraphics.Render
{
    public struct Light
    {
        public Vector3 Coordinates;
        public float Intensity;

        public Light(float x, float y, float z, float intensity)
        {
            Coordinates = new Vector3(x, y, z);
            Intensity = intensity;
        }
    }
}
