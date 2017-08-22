/**
 * Copyright (C) 2007-2010 SlimDX Group
 *
 * Permission is hereby granted, free  of charge, to any person obtaining a copy of this software  and
 * associated  documentation  files (the  "Software"), to deal  in the Software  without  restriction,
 * including  without  limitation  the  rights  to use,  copy,  modify,  merge,  publish,  distribute,
 * sublicense, and/or sell  copies of the  Software,  and to permit  persons to whom  the Software  is
 * furnished to do so, subject to the following conditions:
 *
 * The  above  copyright  notice  and this  permission  notice shall  be included  in  all  copies  or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS",  WITHOUT WARRANTY OF  ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
 * NOT  LIMITED  TO  THE  WARRANTIES  OF  MERCHANTABILITY,  FITNESS  FOR  A   PARTICULAR  PURPOSE  AND
 * NONINFRINGEMENT.  IN  NO  EVENT SHALL THE  AUTHORS  OR COPYRIGHT HOLDERS  BE LIABLE FOR  ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,  OUT
 * OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Globalization;

namespace Maths
{
    public struct Vector3
    {
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 Normalized {
            get {

                Vector3 result = new Vector3();
                float length = Length();
                if (length == 0)
                    return result;
                return result;
            }
        }

        public float Length()
        {
            return (float)(Math.Sqrt((X * X) + (Y * Y) + (Z * Z)));
        }

        public float LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z);
        }

        public void Normalize()
        {
            float length = Length();
            if (length == 0)
                return;

            length = 1.0f / length;
            X *= length;
            Y *= length;
            Z *= length;
        }

        public static Vector3 Add(Vector3 left, Vector3 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        public static Vector3 Subtract(Vector3 left, Vector3 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        public static Vector3 Multiply(Vector3 value, float scale)
        {
            value.X *= scale;
            value.Y *= scale;
            value.Z *= scale;
            return value;
        }

        public static Vector3 Divide(Vector3 value, float scale)
        {
            value.X /= scale;
            value.Y /= scale;
            value.Z /= scale;
            return value;
        }

        public static Vector3 Negate(Vector3 value)
        {
            return new Vector3(-value.X, -value.Y, -value.Z);
        }

        public static Vector3 Lerp(Vector3 start, Vector3 end, float factor)
        {
            Vector3 vector = new Vector3();

            vector.X = start.X + ((end.X - start.X) * factor);
            vector.Y = start.Y + ((end.Y - start.Y) * factor);
            vector.Z = start.Z + ((end.Z - start.Z) * factor);

            return vector;
        }

        public static float Dot(Vector3 left, Vector3 right)
        {
            return (left.X * right.X + left.Y * right.Y + left.Z * right.Z);
        }

        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            Vector3 result = new Vector3();
            result.X = left.Y * right.Z - left.Z * right.Y;
            result.Y = left.Z * right.X - left.X * right.Z;
            result.Z = left.X * right.Y - left.Y * right.X;
            return result;
        }

        public static Vector3 Project(Vector3 vector, Vector3 onNormal)
        {
            return onNormal * Dot(vector, onNormal) / Dot(onNormal, onNormal);
        }

        public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
        {
            return (vector - Project(vector, planeNormal));
        }

        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            Vector3 result = new Vector3();
            float dubdot = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);

            result.X = vector.X - (dubdot * normal.X);
            result.Y = vector.Y - (dubdot * normal.Y);
            result.Z = vector.Z - (dubdot * normal.Z);

            return result;
        }

        public static Vector3 Normalize(Vector3 vector)
        {
            vector.Normalize();
            return vector;
        }

        public static Vector3 Minimize(Vector3 left, Vector3 right)
        {
            Vector3 vector = new Vector3();
            vector.X = (left.X < right.X) ? left.X : right.X;
            vector.Y = (left.Y < right.Y) ? left.Y : right.Y;
            vector.Z = (left.Z < right.Z) ? left.Z : right.Z;
            return vector;
        }

        public static Vector3 Maximize(Vector3 left, Vector3 right)
        {
            Vector3 vector = new Vector3();
            vector.X = (left.X > right.X) ? left.X : right.X;
            vector.Y = (left.Y > right.Y) ? left.Y : right.Y;
            vector.Z = (left.Z > right.Z) ? left.Z : right.Z;
            return vector;
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return Add(left, right);
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return Subtract(left, right);
        }

        public static Vector3 operator -(Vector3 value)
        {
            return Negate(value);
        }
        public static Vector3 operator *(Vector3 value, float scale)
        {
            return Multiply(value, scale);
        }

        public static Vector3 operator *(float scale, Vector3 vec)
        {
            return Multiply(vec, scale);
        }

        public static Vector3 operator /(Vector3 value, float scale)
        {
            return Divide(value, scale);
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "X:{0} Y:{1} Z:{2}", X.ToString(), Y.ToString(), Z.ToString());
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2;
        }

        public override bool Equals(object value)
        {
            if (value == null || value.GetType() != GetType())
                return false;

            return Equals((Vector3)(value));
        }

        public bool Equals(Vector3 value)
        {
            return (X == value.X && Y == value.Y && Z == value.Z);
        }

        public static bool Equals(Vector3 value1, Vector3 value2)
        {
            return (value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z);
        }
    }
}
