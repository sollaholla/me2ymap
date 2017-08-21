using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace YMapExporter
{
    public enum EntityTypes
    {
        CEntityDef,
        CMloInstanceDef
    }

    public enum LodLevels
    {
        LODTYPES_DEPTH_LOD,
        LODTYPES_DEPTH_SLOD1,
        LODTYPES_DEPTH_ORPHANHD,
        LODTYPES_DEPTH_HD
    }

    public enum PriorityLevels
    {
        PRI_REQUIRED
    }

    [XmlRoot("CMapData")]
    public class YMap
    {
        [XmlElement("name", IsNullable = true, Namespace = "")]
        [Category("Info")]
        public string Name { get; set; }

        [XmlElement("parent", IsNullable = true, Namespace = "")]
        [Category("Info")]
        [DisplayName("Parent YMap")]
        public string Parent { get; set; }

        [XmlElement("flags")]
        [Category("Info")]
        public XValue<int> Flags { get; set; }

        [XmlElement("contentFlags")]
        [Category("Info")]
        [DisplayName("Content Flags")]
        public XValue<int> ContentFlags { get; set; } = new XValue<int>(65);

        [XmlElement("streamingExtentsMin")]
        public XVector StreamingExtenstsMin { get; set; } = new XVector();

        [XmlElement("streamingExtentsMax")]
        public XVector StreamingExtenstsMax { get; set; } = new XVector();

        [XmlElement("entitiesExtentsMin")]
        public XVector EntitiesExtenstsMin { get; set; } = new XVector();

        [XmlElement("entitiesExtentsMax")]
        public XVector EntitiesExtenstsMax { get; set; } = new XVector();

        [XmlArrayItem(ElementName = "Item", Type = typeof(Entity))]
        [XmlArray("entities")]
        [Category("Collections")]
        public List<Entity> Entities { get; set; } = new List<Entity>();

        [XmlElement("containerLods")]
        [Browsable(false)]
        public object ContainerLods { get; set; } = new object();

        [XmlElement("boxOccluders")]
        [Browsable(false)]
        public object BoxOccluders { get; set; } = new object();

        [XmlElement("occludeModels")]
        [Browsable(false)]
        public object OccludeModels { get; set; } = new object();

        [XmlElement("physicsDictionaries")]
        [Browsable(false)]
        public object PhysicsDictionaries { get; set; } = new object();

        [XmlElement("instancedData")]
        [Browsable(false)]
        public object InstancedData { get; set; } = new object();

        [XmlArrayItem(ElementName = "Item", Type = typeof(TimeCycleModifier))]
        [XmlArray("timeCycleModifiers")]
        [Category("Collections")]
        [DisplayName("TimeCycle Modifiers")]
        public List<TimeCycleModifier> TimeCycleModifiers { get; set; } = new List<TimeCycleModifier>();

        [XmlArrayItem(ElementName = "Item", Type = typeof(CarGenerator))]
        [XmlArray("carGenerators")]
        [Category("Collections")]
        [DisplayName("Vehicle Generators")]
        public List<CarGenerator> CarGenerators { get; set; } = new List<CarGenerator>();

        [Browsable(false)]
        public object LodLightsSoa { get; set; } = new object();

        [Browsable(false)]
        public object DistantLodLightsSoa { get; set; } = new object();

        [XmlElement("block")]
        [Category("MetaData")]
        [DisplayName("Meta Data")]
        public MetaDataBlock MetaDataBlock { get; set; } = new MetaDataBlock();

        //[XmlIgnore]
        //[Category("Ytyp Support")]
        //public XVector CustomOrigin { get; set; } = new XVector();
    }

    [TypeConverter(typeof(XValueConverter))]
    public struct XValue<T> where T : struct
    {
        public XValue(T value)
        {
            Value = value;
        }

        [XmlAttribute("value")]
        public T Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class XValueConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string)) return base.ConvertFrom(context, culture, value);
            if (context.PropertyDescriptor == null) return base.ConvertFrom(context, culture, value);
            var t = context.PropertyDescriptor.PropertyType;
            if (t == typeof(XValue<int>) && int.TryParse((string) value, out int intValue))
                return new XValue<int>(intValue);
            if (t == typeof(XValue<float>) && float.TryParse((string) value, out float floatValue))
                return new XValue<float>(floatValue);
            if (t == typeof(XValue<long>) && long.TryParse((string) value, out long int64Value))
                return new XValue<long>(int64Value);

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof(string))
                return value.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Category("Render")]
    public class XVector
    {
        public XVector()
        {
        }

        public XVector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [Browsable(false)]
        public bool IsEmpty => X == 0 && Y == 0 && Z == 0;

        [XmlAttribute("x")]
        [RefreshProperties(RefreshProperties.All)]
        public float X { get; set; }

        [XmlAttribute("y")]
        [RefreshProperties(RefreshProperties.All)]
        public float Y { get; set; }

        [XmlAttribute("z")]
        [RefreshProperties(RefreshProperties.All)]
        public float Z { get; set; }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Z:{Z}";
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Category("Render")]
    public class XQuaternion
    {
        public XQuaternion()
        {
        }

        public XQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        [XmlAttribute("x")]
        [RefreshProperties(RefreshProperties.All)]
        public float X { get; set; }

        [XmlAttribute("y")]
        [RefreshProperties(RefreshProperties.All)]
        public float Y { get; set; }

        [XmlAttribute("z")]
        [RefreshProperties(RefreshProperties.All)]
        public float Z { get; set; }

        [XmlAttribute("w")]
        [RefreshProperties(RefreshProperties.All)]
        public float W { get; set; }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Z:{Z} W:{W}";
        }
    }

    [Category("Collections")]
    [XmlType(TypeName = "Item")]
    public class Entity
    {
        [XmlElement("archetypeName")]
        [Category("Info")]
        public string ArchetypeName { get; set; }

        [XmlAttribute("type")]
        [Category("Info")]
        public EntityTypes Type { get; set; }

        [XmlElement("flags")]
        [Category("Info")]
        public XValue<int> Flags { get; set; } = new XValue<int>(1572865);

        [XmlElement("guid")]
        [Category("Info")]
        public XValue<long> Guid { get; set; }

        [XmlElement("position")]
        [Category("Info")]
        public XVector Position { get; set; } = new XVector();

        [XmlElement("rotation")]
        [Category("Info")]
        public XQuaternion Rotation { get; set; } = new XQuaternion();

        [XmlElement("scaleXY")]
        [Category("Render")]
        public XValue<float> ScaleXy { get; set; } = new XValue<float>(1);

        [XmlElement("scaleZ")]
        [Category("Render")]
        public XValue<float> ScaleZ { get; set; } = new XValue<float>(1);

        [XmlElement("parentIndex")]
        public XValue<int> ParentIndex { get; set; } = new XValue<int>(-1);

        [XmlElement("lodDist")]
        [Category("Render")]
        public XValue<float> LodDistance { get; set; } = new XValue<float>(500);

        [XmlElement("childLodDist")]
        [Category("Render")]
        public XValue<float> ChildLodDistance { get; set; } = new XValue<float>(500);

        [XmlElement("lodLevel")]
        [Category("Render")]
        public LodLevels LodLevel { get; set; } = LodLevels.LODTYPES_DEPTH_HD;

        [XmlElement("numChildren")]
        public XValue<int> NumChildren { get; set; }

        [XmlElement("priorityLevel")]
        public PriorityLevels PriorityLevel { get; set; }

        [XmlElement("extensions")]
        [Browsable(false)]
        public object Extensions { get; set; }

        [XmlElement("ambientOcclusionMultiplier")]
        [Category("Render")]
        public XValue<int> AmbientOcclusionMultiplier { get; set; } = new XValue<int>(255);

        [XmlElement("artificialAmbientOcclusion")]
        [Category("Render")]
        public XValue<int> ArtificialAmbientOcclusion { get; set; } = new XValue<int>(255);

        [XmlElement("tintValue")]
        [Category("Render")]
        public XValue<int> TintValue { get; set; }

        public override string ToString()
        {
            return ArchetypeName ?? base.ToString();
        }
    }

    [Category("Collections")]
    public class TimeCycleModifier
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("minExtents")]
        public XVector MinExtents { get; set; } = new XVector();

        [XmlElement("maxExtents")]
        public XVector MaxExtents { get; set; } = new XVector();

        [XmlElement("percentage")]
        public XValue<float> Percentage { get; set; }

        [XmlElement("range")]
        public XValue<float> Range { get; set; }

        [XmlElement("startHour")]
        public XValue<int> StartHour { get; set; } = new XValue<int>(0);

        [XmlElement("endHour")]
        public XValue<int> EndHour { get; set; } = new XValue<int>(23);
    }

    [Category("Collections")]
    public class CarGenerator
    {
        [XmlElement("bodyColorRemap3")]
        public XValue<int> BodyColorRemap3 { get; set; } = new XValue<int>(-1);

        [XmlElement("bodyColorRemap4")]
        public XValue<int> BodyColorRemap4 { get; set; } = new XValue<int>(-1);

        [XmlElement("popGroup")] public object PopGroup = new object();

        [XmlElement("position")]
        public XVector Position { get; set; } = new XVector();

        [XmlElement("orientX")]
        public XValue<float> OrientationX { get; set; }

        [XmlElement("orientY")]
        public XValue<float> OrientationY { get; set; }

        [XmlElement("perpendicularLength")]
        public XValue<float> PerpendicularLength { get; set; }

        [XmlElement("carModel")]
        public string CarModel { get; set; }

        [XmlElement("flags")]
        public XValue<int> Flags { get; set; }

        [XmlElement("bodyColorRemap1")]
        public XValue<int> BodyColorRemap1 { get; set; } = new XValue<int>(-1);

        [XmlElement("bodyColorRemap2")]
        public XValue<int> BodyColorRemap2 { get; set; } = new XValue<int>(-1);


        [XmlElement("livery")]
        public XValue<int> Livery { get; set; } = new XValue<int>(-1);
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MetaDataBlock
    {
        private const string DateTimeFormat = "dd MMMM yyyy H:mm";

        [XmlElement("version")]
        public XValue<int> Version { get; set; }

        [XmlElement("flags")]
        public XValue<int> Flags { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("exportedBy")]
        public string Author { get; set; }

        [XmlElement("owner")]
        public string Owner { get; set; }

        [XmlIgnore]
        public DateTime Time { get; set; } = DateTime.Now;

        [XmlElement("time")]
        [Browsable(false)]
        public string TimeString
        {
            get => Time.ToString(DateTimeFormat);
            set => Time = XmlConvert.ToDateTime(value, DateTimeFormat);
        }

        public override string ToString()
        {
            return Author ?? "No author specified...";
        }
    }
}