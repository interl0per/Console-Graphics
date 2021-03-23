using System;

namespace ConsoleGraphics.Maths
{
    /// <summary>
    /// A struct containing an X, Y and Z value, and basic functions for manipulating those values, e.g. the dot product, normalisation, etc
    /// </summary>
    public struct Vector3
    {
        public float X, Y, Z;

        public Vector3(float x, float y, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Adds the given XYZ values to this vector instance's XYZ values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Add(float x, float y, float z)
        {
            this.X += x;
            this.Y += y;
            this.Z += z;
        }

        /// <summary>
        /// Adds the XYZ values of the given vector to this vector instance's XYZ values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Add(Vector3 v)
        {
            Add(v.X, v.Y, v.Z);
        }

        /// <summary>
        /// Returns the dot product between this vector instance and the given vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public float Dot(Vector3 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        /// <summary>
        /// Returns the squared magnitude of this vector instance
        /// </summary>
        /// <returns></returns>
        public float MagnitudeSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Returns the magnitude of this vector instance
        /// </summary>
        /// <returns></returns>
        public float Magnitude()
        {
            return (float)Math.Sqrt(MagnitudeSquared());
        }

        public static Vector3 Normalize(Vector3 a)
        {
            float norm = (float)Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
            return (new Vector3(a.X / norm, a.Y / norm, a.Z / norm));
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            Vector3 resultant;
            resultant.X = a.Y * b.Z - a.Z * b.Y;
            resultant.Y = a.Z * b.X - a.X * b.Z;
            resultant.Z = a.X * b.Y - a.Y * b.X;
            return (resultant);
        }

        /// <summary>
        /// Returns a new vector where the XYZ values of a and b have been added
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 Add(Vector3 a, Vector3 b)
        {
            return (new Vector3((a.X + b.X), (a.Y + b.Y), (a.Z + b.Z)));
        }

        /// <summary>
        /// Returns a new vector which contains the dot product between the a and b vector
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Dot(Vector3 a, Vector3 b)
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }
    }
}
