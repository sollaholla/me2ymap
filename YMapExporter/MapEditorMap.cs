using System.Collections.Generic;
using System.Xml.Serialization;
using SlimDX;

namespace YMapExporter
{
    public enum MapObjectTypes
    {
        Prop,
        Vehicle,
        Ped,
        Marker
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

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        public int Hash { get; set; }

        public bool Dynamic { get; set; }

        public Quaternion Quaternion { get; set; }

        public bool Door { get; set; }
    }

    public class MapMetaData
    {
        public string Creator { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}