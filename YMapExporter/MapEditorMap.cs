using System.Collections.Generic;
using System.Xml.Serialization;

namespace YMapExporter
{
    public enum MapObjectTypes
    {
        Prop,
        Vehicle
    }

    [XmlRoot("Map")]
    public class MapEditorMap
    {
        public List<MapObject> Objects { get; set; }

        public MapMetaData MetaData { get; set; } = new MapMetaData();
    }

    public class MapObject
    {
        public string Id { get; set; }

        public MapObjectTypes Type { get; set; }

        public GtaVector Position { get; set; }

        public GtaVector Rotation { get; set; }

        public int Hash { get; set; }

        public bool Dynamic { get; set; }

        public GtaQuaternion Quaternion { get; set; }

        public bool Door { get; set; }
    }

    public struct GtaVector
    {
        public GtaVector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Z:{Z}";
        }

        public static implicit operator XVector(GtaVector l)
        {
            return new XVector(l.X, l.Y, l.Z);
        }

        public static implicit operator GtaVector(XVector l)
        {
            return new GtaVector(l.X, l.Y, l.Z);
        }

        public static GtaVector operator +(GtaVector lhs, GtaVector rhs)
        {
            return new GtaVector(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        public static GtaVector operator -(GtaVector lhs, GtaVector rhs)
        {
            return new GtaVector(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        public static GtaVector operator /(GtaVector lhs, float rhs)
        {
            return new GtaVector(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
        }

        public static GtaVector operator *(GtaVector lhs, float rhs)
        {
            return new GtaVector(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
        }
    }

    public struct GtaQuaternion
    {
        public GtaQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float W { get; set; }

        public static GtaQuaternion Invert(GtaQuaternion quaternion)
        {
            var q = quaternion;
            var lengthSq = 1.0f / (q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);
            q.X = -q.X * lengthSq;
            q.Y = -q.Y * lengthSq;
            q.Z = -q.Z * lengthSq;
            q.W = q.W * lengthSq;
            return q;
        }

        public static implicit operator XQuaternion(GtaQuaternion lhs)
        {
            return new XQuaternion(lhs.X, lhs.Y, lhs.Z, lhs.W);
        }

        public static implicit operator GtaQuaternion(XQuaternion lhs)
        {
            return new GtaQuaternion(lhs.X, lhs.Y, lhs.Z, lhs.W);
        }
    }

    public class MapMetaData
    {
        public string Creator { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}