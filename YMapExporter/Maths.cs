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
    }
}
