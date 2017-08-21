using SlimDX;

namespace YMapExporter
{
    /// <summary>
    /// Source: Codewalker (c) 2017
    /// </summary>
    public static class QuaternionExtension
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

        public static Matrix ToMatrix(this Quaternion q)
        {
            var xx = q.X * q.X;
            var yy = q.Y * q.Y;
            var zz = q.Z * q.Z;
            var xy = q.X * q.Y;
            var zw = q.Z * q.W;
            var zx = q.Z * q.X;
            var yw = q.Y * q.W;
            var yz = q.Y * q.Z;
            var xw = q.X * q.W;
            var result = new Matrix
            {
                M11 = 1.0f - 2.0f * (yy + zz),
                M12 = 2.0f * (xy + zw),
                M13 = 2.0f * (zx - yw),
                M14 = 0.0f,
                M21 = 2.0f * (xy - zw),
                M22 = 1.0f - 2.0f * (zz + xx),
                M23 = 2.0f * (yz + xw),
                M24 = 0.0f,
                M31 = 2.0f * (zx + yw),
                M32 = 2.0f * (yz - xw),
                M33 = 1.0f - 2.0f * (yy + xx),
                M34 = 0.0f,
                M41 = 0.0f,
                M42 = 0.0f,
                M43 = 0.0f,
                M44 = 1.0f
            };
            return result;
        }

    }
}
