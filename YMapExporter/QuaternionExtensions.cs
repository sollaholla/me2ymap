using System;
using SlimDX;

namespace YMapExporter
{
    /// <summary>
    ///     Source: Codewalker (c) 2017
    /// </summary>
    public static class QuaternionExtensions
    {
        public static Vector3 Multiply(this Quaternion a, Vector3 b)
        {
            var axx = a.X * 2.0f;
            var ayy = a.Y * 2.0f;
            var azz = a.Z * 2.0f;
            var awxx = a.W * axx;
            var awyy = a.W * ayy;
            var awzz = a.W * azz;
            var axxx = a.X * axx;
            var axyy = a.X * ayy;
            var axzz = a.X * azz;
            var ayyy = a.Y * ayy;
            var ayzz = a.Y * azz;
            var azzz = a.Z * azz;
            return new Vector3(b.X * (1.0f - ayyy - azzz) + b.Y * (axyy - awzz) + b.Z * (axzz + awyy),
                b.X * (axyy + awzz) + b.Y * (1.0f - axxx - azzz) + b.Z * (ayzz - awxx),
                b.X * (axzz - awyy) + b.Y * (ayzz + awxx) + b.Z * (1.0f - axxx - ayyy));
        }

        //public static Quaternion ToQuaternion(this Vector3 vect)
        //{
        //    vect = new Vector3
        //    {
        //        X = vect.X.Denormalize() * -1f,
        //        Y = vect.Y.Denormalize() - 180f,
        //        Z = vect.Z.Denormalize() - 180f
        //    };

        //    vect = vect.TransformVector(ToRadians);

        //    var rollOver2 = vect.Z * 0.5f;
        //    var sinRollOver2 = (float) Math.Sin(rollOver2);
        //    var cosRollOver2 = (float) Math.Cos(rollOver2);
        //    var pitchOver2 = vect.Y * 0.5f;
        //    var sinPitchOver2 = (float) Math.Sin(pitchOver2);
        //    var cosPitchOver2 = (float) Math.Cos(pitchOver2);
        //    var yawOver2 = vect.X * 0.5f; // pitch
        //    var sinYawOver2 = (float) Math.Sin(yawOver2);
        //    var cosYawOver2 = (float) Math.Cos(yawOver2);
        //    var result = new Quaternion
        //    {
        //        X = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2,
        //        Y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2,
        //        Z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2,
        //        W = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2
        //    };
        //    return result;
        //}

        //public static float Denormalize(this float h)
        //{
        //    return h < 0f ? h + 360f : h;
        //}

        //public static Vector3 Denormalize(this Vector3 v)
        //{
        //    return new Vector3(v.X.Denormalize(), v.Y.Denormalize(), v.Z.Denormalize());
        //}

        //public static Vector3 TransformVector(this Vector3 i, Func<float, float> method)
        //{
        //    return new Vector3
        //    {
        //        X = method(i.X),
        //        Y = method(i.Y),
        //        Z = method(i.Z)
        //    };
        //}

        //public static float ToRadians(this float val)
        //{
        //    return (float) (Math.PI / 180) * val;
        //}
        
        public static Quaternion Euler (this Vector3 euler)
        {
            var eulerRad = euler * (float)(Math.PI / 180.0);
            return RotationYawPitchRoll(eulerRad.X, eulerRad.Y, eulerRad.Z);
        }

        public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Quaternion result;

            var halfRoll = roll * 0.5f;
            var sinRoll = (float)(Math.Sin(halfRoll));
            var cosRoll = (float)(Math.Cos(halfRoll));
            var halfPitch = pitch * 0.5f;
            var sinPitch = (float)(Math.Sin(halfPitch));
            var cosPitch = (float)(Math.Cos(halfPitch));
            var halfYaw = yaw * 0.5f;
            var sinYaw = (float)(Math.Sin(halfYaw));
            var cosYaw = (float)(Math.Cos(halfYaw));

            result.X = (cosYaw* sinPitch * cosRoll) + (sinYaw* cosPitch * sinRoll);
            result.Y = (sinYaw* cosPitch * cosRoll) - (cosYaw* sinPitch * sinRoll);
            result.Z = (cosYaw* cosPitch * sinRoll) - (sinYaw* sinPitch * cosRoll);
            result.W = (cosYaw* cosPitch * cosRoll) + (sinYaw* sinPitch * sinRoll);

            return result;
        }
}
}