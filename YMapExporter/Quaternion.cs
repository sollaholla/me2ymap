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
    public struct Quaternion
    {
        const float DegToRad = 0.01745329251f;

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion(Vector3 value, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = w;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
        public static Quaternion Identity {
            get {
                return new Quaternion();
            }
        }

        public Vector3 Axis {
            get {

                Vector3 axis = new Vector3();
                float length = Length();

                if (length != 0.0f)
                {
                    float invlength = 1.0f / length;
                    axis.X = X * invlength;
                    axis.Y = Y * invlength;
                    axis.Z = Z * invlength;
                }
                else
                {
                    axis.X = 1.0f;
                    axis.Y = 0.0f;
                    axis.Z = 0.0f;
                }

                return axis;
            }
        }

        public float Angle {
            get {
                return (Math.Abs(W) <= 1.0f) ? 2.0f * (float)(Math.Acos(W)) : 0.0f;
            }
        }

        public float Length()
        {
            return (float)(Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W)));
        }

        public float LengthSquared()
        {
            return (float)((X * X) + (Y * Y) + (Z * Z) + (W * W));
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
            W *= length;
        }

        public void Conjugate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        public void Invert()
        {
            float lengthSq = 1.0f / ((X * X) + (Y * Y) + (Z * Z) + (W * W));
            X = -X * lengthSq;
            Y = -Y * lengthSq;
            Z = -Z * lengthSq;
            W = W * lengthSq;
        }

        public static Quaternion Add(Quaternion left, Quaternion right)
        {
            Quaternion result = new Quaternion();
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
            return result;
        }

        public static Quaternion Divide(Quaternion left, Quaternion right)
        {
            Quaternion result = new Quaternion();
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;
            return result;
        }

        public static float Dot(Quaternion left, Quaternion right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        public static Quaternion Invert(Quaternion quaternion)
        {
            Quaternion result = new Quaternion();
            float lengthSq = 1.0f / ((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.W * quaternion.W));

            result.X = -quaternion.X * lengthSq;
            result.Y = -quaternion.Y * lengthSq;
            result.Z = -quaternion.Z * lengthSq;
            result.W = quaternion.W * lengthSq;

            return result;
        }

        public static Quaternion Lerp(Quaternion left, Quaternion right, float amount)
        {
            Quaternion result = new Quaternion();
            float inverse = 1.0f - amount;
            float dot = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);

            if (dot >= 0.0f)
            {
                result.X = (inverse * left.X) + (amount * right.X);
                result.Y = (inverse * left.Y) + (amount * right.Y);
                result.Z = (inverse * left.Z) + (amount * right.Z);
                result.W = (inverse * left.W) + (amount * right.W);
            }
            else
            {
                result.X = (inverse * left.X) - (amount * right.X);
                result.Y = (inverse * left.Y) - (amount * right.Y);
                result.Z = (inverse * left.Z) - (amount * right.Z);
                result.W = (inverse * left.W) - (amount * right.W);
            }

            float invLength = 1.0f / result.Length();

            result.X *= invLength;
            result.Y *= invLength;
            result.Z *= invLength;
            result.W *= invLength;

            return result;
        }

        public static Quaternion Slerp(Quaternion start, Quaternion end, float amount)
        {
            Quaternion result = new Quaternion();
            const float kEpsilon = (float)(1.192093E-07);
            float opposite;
            float inverse;
            float dot = Quaternion.Dot(start, end);

            if (Math.Abs(dot) > (1.0f - kEpsilon))
            {
                inverse = 1.0f - amount;
                opposite = amount * Math.Sign(dot);
            }
            else
            {
                float acos = (float)Math.Acos(Math.Abs(dot));
                float invSin = (float)(1.0 / Math.Sin(acos));

                inverse = (float)(Math.Sin((1.0f - amount) * acos) * invSin);
                opposite = (float)(Math.Sin(amount * acos) * invSin * Math.Sign(dot));
            }

            result.X = (inverse * start.X) + (opposite * end.X);
            result.Y = (inverse * start.Y) + (opposite * end.Y);
            result.Z = (inverse * start.Z) + (opposite * end.Z);
            result.W = (inverse * start.W) + (opposite * end.W);

            return result;
        }

        public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            if (a.LengthSquared() == 0.0f)
            {
                if (b.LengthSquared() == 0.0f)
                {
                    return Identity;
                }
                return b;
            }
            else if (b.LengthSquared() == 0.0f)
            {
                return a;
            }

            float cosHalfAngle = a.W * b.W + Vector3.Dot(a.Axis, b.Axis);

            if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
            {
                return a;
            }
            else if (cosHalfAngle < 0.0f)
            {
                b.X = -b.X;
                b.Y = -b.Y;
                b.Z = -b.Z;
                b.W = -b.W;
                cosHalfAngle = -cosHalfAngle;
            }

            float blendA;
            float blendB;
            if (cosHalfAngle < 0.99f)
            {
                float halfAngle = (float)Math.Acos(cosHalfAngle);
                float sinHalfAngle = (float)Math.Sin(halfAngle);
                float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
                blendA = (float)Math.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
                blendB = (float)Math.Sin(halfAngle * t) * oneOverSinHalfAngle;
            }
            else
            {
                blendA = 1.0f - t;
                blendB = t;
            }

            Quaternion result = new Quaternion(blendA * a.Axis + blendB * b.Axis, blendA * a.W + blendB * b.W);
            if (result.LengthSquared() > 0.0f)
                return Normalize(result);
            else
                return Identity;
        }

        public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            float NormAB = (float)(Math.Sqrt(fromDirection.LengthSquared() * fromDirection.LengthSquared()));

            float w = NormAB + Vector3.Dot(fromDirection, toDirection);
            Quaternion Result;

            if (w >= 1e-6f * NormAB)
            {
                Result = new Quaternion(Vector3.Cross(fromDirection, toDirection), w);
            }
            else
            {
                w = 0.0f;
                Result = Math.Abs(fromDirection.X) > Math.Abs(fromDirection.Y)
                    ? new Quaternion(-fromDirection.Z, 0.0f, fromDirection.X, w)
                    : new Quaternion(0.0f, -fromDirection.Z, fromDirection.Y, w);
            }

            Result.Normalize();
            return Result;
        }

        public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
        {
            float angle = Quaternion.AngleBetween(from, to);
            if (angle == 0.0f)
            {
                return to;
            }
            float t = Math.Min(1.0f, maxDegreesDelta / angle);
            return SlerpUnclamped(from, to, t);
        }

        public static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            Quaternion quaternion = new Quaternion();
            float lx = left.X;
            float ly = left.Y;
            float lz = left.Z;
            float lw = left.W;
            float rx = right.X;
            float ry = right.Y;
            float rz = right.Z;
            float rw = right.W;

            quaternion.X = (lx * rw + rx * lw) + (ly * rz) - (lz * ry);
            quaternion.Y = (ly * rw + ry * lw) + (lz * rx) - (lx * rz);
            quaternion.Z = (lz * rw + rz * lw) + (lx * ry) - (ly * rx);
            quaternion.W = (lw * rw) - (lx * rx + ly * ry + lz * rz);

            return quaternion;
        }

        public static Quaternion Multiply(Quaternion quaternion, float scale)
        {
            Quaternion result = new Quaternion();
            result.X = quaternion.X * scale;
            result.Y = quaternion.Y * scale;
            result.Z = quaternion.Z * scale;
            result.W = quaternion.W * scale;
            return result;
        }

        public static Quaternion Negate(Quaternion quat)
        {
            Quaternion result = new Quaternion();
            result.X = -quat.X;
            result.Y = -quat.Y;
            result.Z = -quat.Z;
            result.W = -quat.W;
            return result;
        }

        public static Quaternion Normalize(Quaternion quat)
        {
            quat.Normalize();
            return quat;
        }

        public static float AngleBetween(Quaternion a, Quaternion b)
        {
            float dot = Dot(a, b);
            return (float)((Math.Acos(Math.Min(Math.Abs(dot), 1.0f)) * 2.0 * (180.0f / Math.PI)));
        }

        public static Quaternion Euler(float x, float y, float z)
        {
            return RotationYawPitchRoll(x * DegToRad, y * DegToRad, z * DegToRad);
        }

        public static Quaternion Euler(Vector3 euler)
        {
            Vector3 eulerRad = euler * DegToRad;
            return RotationYawPitchRoll(eulerRad.X, eulerRad.Y, eulerRad.Z);
        }

        public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Quaternion result = new Quaternion();

            float halfRoll = roll * 0.5f;
            float sinRoll = (float)(Math.Sin((double)(halfRoll)));
            float cosRoll = (float)(Math.Cos((double)(halfRoll)));
            float halfPitch = pitch * 0.5f;
            float sinPitch = (float)(Math.Sin((double)(halfPitch)));
            float cosPitch = (float)(Math.Cos((double)(halfPitch)));
            float halfYaw = yaw * 0.5f;
            float sinYaw = (float)(Math.Sin((double)(halfYaw)));
            float cosYaw = (float)(Math.Cos((double)(halfYaw)));

            result.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            result.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.Z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            result.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);

            return result;
        }

        public static Quaternion RotationAxis(Vector3 axis, float angle)
        {
            Quaternion result = new Quaternion();

            axis = Vector3.Normalize(axis);

            float half = angle * 0.5f;
            float sin = (float)(Math.Sin((double)(half)));
            float cos = (float)(Math.Cos((double)(half)));

            result.X = axis.X * sin;
            result.Y = axis.Y * sin;
            result.Z = axis.Z * sin;
            result.W = cos;

            return result;
        }

        public static Quaternion Subtract(Quaternion left, Quaternion right)
        {
            Quaternion result = new Quaternion();
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
            return result;
        }

        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            Quaternion quaternion = new Quaternion();
            float lx = left.X;
            float ly = left.Y;
            float lz = left.Z;
            float lw = left.W;
            float rx = right.X;
            float ry = right.Y;
            float rz = right.Z;
            float rw = right.W;

            quaternion.X = (lx * rw + rx * lw) + (ly * rz) - (lz * ry);
            quaternion.Y = (ly * rw + ry * lw) + (lz * rx) - (lx * rz);
            quaternion.Z = (lz * rw + rz * lw) + (lx * ry) - (ly * rx);
            quaternion.W = (lw * rw) - (lx * rx + ly * ry + lz * rz);

            return quaternion;
        }

        public static Vector3 operator *(Quaternion rotation, Vector3 point)
        {
            Vector3 q = new Vector3(rotation.X, rotation.Y, rotation.Z);
            Vector3 t = 2.0f * Vector3.Cross(q, point);
            Vector3 result = point + (rotation.W * t) + Vector3.Cross(q, t);
            return result;
        }

        public static Quaternion operator *(Quaternion quaternion, float scale)
        {
            Quaternion result = new Quaternion();
            result.X = quaternion.X * scale;
            result.Y = quaternion.Y * scale;
            result.Z = quaternion.Z * scale;
            result.W = quaternion.W * scale;
            return result;
        }

        public static Quaternion operator *(float scale, Quaternion quaternion)
        {
            Quaternion result = new Quaternion();
            result.X = quaternion.X * scale;
            result.Y = quaternion.Y * scale;
            result.Z = quaternion.Z * scale;
            result.W = quaternion.W * scale;
            return result;
        }

        public static Quaternion operator /(Quaternion lhs, float rhs)
        {
            Quaternion result = new Quaternion();
            float invRhs = 1.0f / rhs;
            result.X = lhs.X * invRhs;
            result.Y = lhs.Y * invRhs;
            result.Z = lhs.Z * invRhs;
            result.W = lhs.W * invRhs;
            return result;
        }

        public static Quaternion operator +(Quaternion lhs, Quaternion rhs)
        {
            Quaternion result = new Quaternion();
            result.X = lhs.X + rhs.X;
            result.Y = lhs.Y + rhs.Y;
            result.Z = lhs.Z + rhs.Z;
            result.W = lhs.W + rhs.W;
            return result;
        }

        public static Quaternion operator -(Quaternion lhs, Quaternion rhs)
        {
            Quaternion result = new Quaternion();
            result.X = lhs.X - rhs.X;
            result.Y = lhs.Y - rhs.Y;
            result.Z = lhs.Z - rhs.Z;
            result.W = lhs.W - rhs.W;
            return result;
        }

        public static Quaternion operator -(Quaternion quaternion)
        {
            Quaternion result = new Quaternion();
            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;
            return result;
        }

        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return Quaternion.Equals(left, right);
        }

        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return !Quaternion.Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(), Y.ToString(), Z.ToString(), W.ToString());
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2 ^ W.GetHashCode();
        }

        public override bool Equals(object value)
        {
            if (value == null || value.GetType() != GetType())
                return false;

            return Equals((Quaternion)(value));
        }

        public bool Equals(Quaternion value)
        {
            return (X == value.X && Y == value.Y && Z == value.Z && W == value.W);
        }

        public static bool Equals(Quaternion value1, Quaternion value2)
        {
            return (value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W);
        }
    }
}