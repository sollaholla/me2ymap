using Maths;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace YMapExporter
{
    public class SpoonerPlacements
    {
        public object Note { get; set; }

        public object AudioFile { get; set; }

        public object ClearDatabase { get; set; }

        public object ClearMarkers { get; set; }

        [XmlElement("IPLsToLoad")]
        public object IpLsToLoad { get; set; }

        [XmlElement("IPLsToRemove")]
        public object IpLsToRemove { get; set; }

        public object InteriorsToEnable { get; set; }

        public object InteriorsToCap { get; set; }

        public object WeatherToSet { get; set; }

        public object StartTaskSequencesOnLoad { get; set; }

        public object ImgLoadingCoords { get; set; }

        public object ReferenceCoords { get; set; }

        [XmlElement("Placement")]
        public List<Placement> Placements { get; set; }
    }

    public class Placement
    {
        public string ModelHash { get; set; }

        public int Type { get; set; }

        public bool Dynamic { get; set; }

        public bool FrozenPos { get; set; }

        public string HashName { get; set; }

        public int InitialHandle { get; set; }

        public object ObjectProperties { get; set; }

        public int OpacityLevel { get; set; }

        public int LodDistance { get; set; }

        public bool IsVisible { get; set; }

        public int MaxHealth { get; set; }

        public int Health { get; set; }

        public bool HasGravity { get; set; }

        public bool IsOnFire { get; set; }

        public bool IsInvincible { get; set; }

        public bool IsBulletProof { get; set; }

        public bool IsCollisionProof { get; set; }

        public bool IsExplosionProof { get; set; }

        public bool IsFireProof { get; set; }

        public bool IsMeleeProof { get; set; }

        public bool IsOnlyDamagedByPlayer { get; set; }

        public PositionRotation PositionRotation { get; set; }

        public object Attachment { get; set; }

        public object PedProperties { get; set; }

        public object VehicleProperties { get; set; }
    }

    public class PositionRotation
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float Pitch { get; set; }

        public float Roll { get; set; }

        public float Yaw { get; set; }

        public Vector3 GetPosition()
        {
            return new Vector3(X, Y, Z);
        }

        public Quaternion GetQuaternion()
        {
            return Quaternion.Euler(Roll, Pitch, Yaw);
        }
    }
}
