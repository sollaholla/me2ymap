using System;

namespace YMapExporter
{
    public static class Maths
    {
        public static XQuaternion Euler(float x, float y, float z)
        {
            const float deg2Rad = (float)(Math.PI / 180.0);
            return RotationYawPitchRoll(x * deg2Rad, y * deg2Rad, z * deg2Rad);
        }

        public static XQuaternion Euler(GtaVector euler)
        {
            var eulerRad = euler * (float)(Math.PI / 180.0);
            return RotationYawPitchRoll(eulerRad.X, eulerRad.Y, eulerRad.Z);
        }

        private static XQuaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            var result = new XQuaternion();

            var halfRoll = roll / 2;
            var halfPitch = pitch / 2;
            var halfYaw = yaw / 2;

            var sinRoll = (float)Math.Sin(halfRoll);
            var cosRoll = (float)Math.Cos(halfRoll);

            var sinPitch = (float)Math.Sin(halfPitch);
            var cosPitch = (float)Math.Cos(halfPitch);

            var sinYaw = (float)Math.Sin(halfYaw);
            var cosYaw = (float)Math.Cos(halfYaw);

            result.X = cosYaw * sinPitch * cosRoll + sinYaw * cosPitch * sinRoll;
            result.Y = sinYaw * cosPitch * cosRoll - cosYaw * sinPitch * sinRoll;
            result.Z = cosYaw * cosPitch * sinRoll - sinYaw * sinPitch * cosRoll;
            result.W = cosYaw * cosPitch * cosRoll + sinYaw * sinPitch * sinRoll;

            return result;
        }

        public static GtaQuaternion ToQuaternion(this GtaVector vect)
        {
            vect = new GtaVector()
            {
                X = vect.X.Denormalize() * -1f,
                Y = vect.Y.Denormalize() - 180f,
                Z = vect.Z.Denormalize() - 180f,
            };

            vect = vect.TransformVector(ToRadians);

            var rollOver2 = vect.Z * 0.5f;
            var sinRollOver2 = (float)Math.Sin(rollOver2);
            var cosRollOver2 = (float)Math.Cos(rollOver2);
            var pitchOver2 = vect.Y * 0.5f;
            var sinPitchOver2 = (float)Math.Sin(pitchOver2);
            var cosPitchOver2 = (float)Math.Cos(pitchOver2);
            var yawOver2 = vect.X * 0.5f; // pitch
            var sinYawOver2 = (float)Math.Sin(yawOver2);
            var cosYawOver2 = (float)Math.Cos(yawOver2);
            var result = new GtaQuaternion
            {
                X = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2,
                Y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2,
                Z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2,
                W = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2
            };
            return result;
        }

        public static float Denormalize(this float h)
        {
            return h < 0f ? h + 360f : h;
        }

        public static GtaVector Denormalize(this GtaVector v)
        {
            return new GtaVector(v.X.Denormalize(), v.Y.Denormalize(), v.Z.Denormalize());
        }

        public static GtaVector TransformVector(this GtaVector i, Func<float, float> method)
        {
            return new GtaVector()
            {
                X = method(i.X),
                Y = method(i.Y),
                Z = method(i.Z),
            };
        }

        public static float ToRadians(this float val)
        {
            return (float)(Math.PI / 180) * val;
        }
    }
}
