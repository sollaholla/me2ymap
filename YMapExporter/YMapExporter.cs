using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Maths;

namespace YMapExporter
{
    public partial class YMapExporter : Form
    {
        private YMap _currentYMap = new YMap();
        private Vector3 _ymapCenter;

        public YMapExporter()
        {
            InitializeComponent();
        }

        private void YMapExporter_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = _currentYMap;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var o = new OpenFileDialog
            {
                Filter = @"Map Editor files (*.xml)|*.xml|Menyoo Spooner files (*.xml)|*.xml|YMap files (*.ymap.xml)|*.ymap.xml"
            };

            if (o.ShowDialog() != DialogResult.OK)
                return;

            if (o.FileName.EndsWith(".ymap.xml"))
            {
                using (var reader = XmlReader.Create(o.FileName))
                {
                    var s = new XmlSerializer(typeof(YMap));
                    _currentYMap = (YMap)s.Deserialize(reader);
                    reader.Close();
                    propertyGrid1.SelectedObject = _currentYMap;
                }
            }
            else if (o.FileName.EndsWith(".xml"))
            {
                MapEditorMap map = null;
                SpoonerPlacements spoonerMap = null;

                var reader = XmlReader.Create(o.FileName);
                try
                {
                    var s = new XmlSerializer(typeof(MapEditorMap));
                    map = (MapEditorMap)s.Deserialize(reader);
                }
                catch
                {
                    try
                    {
                        var s = new XmlSerializer(typeof(SpoonerPlacements));
                        spoonerMap = (SpoonerPlacements)s.Deserialize(reader);
                    }
                    catch (Exception ex)
                    {
                        // ignored
                        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
                reader.Close();

                if (map == null && spoonerMap == null)
                {
                    MessageBox.Show(@"Failed to read file.");
                    return;
                }

                if (map != null)
                    AddObjectsToYMap(map);
                else AddObjectsToYMap(spoonerMap);
            }
            else
            {
                MessageBox.Show(@"This is not a valid type!");
            }
        }

        private void AddObjectsToYMap(MapEditorMap map)
        {
            _currentYMap = new YMap();

            foreach (var mapObject in map.Objects)
            {
                var name = HashMap.GetName(mapObject.Hash);
                if (string.IsNullOrEmpty(name)) name = "0x" + mapObject.Hash.ToString("x");

                if (mapObject.Type == MapObjectTypes.Vehicle)
                {
                    const float len = 1.5f; // TODO: FIXME
                    var v = new Vector3(0, len, 0);
                    var t = mapObject.Quaternion * v;
                    var orientX = t.X;
                    var orientY = t.Y;

                    var car = new CarGenerator
                    {
                        Position =  new XmlVector3(mapObject.Position.X, mapObject.Position.Y, mapObject.Position.Z),
                        OrientationX = new XmlValue<float>(orientX),
                        OrientationY = new XmlValue<float>(orientY),
                        PerpendicularLength = new XmlValue<float>(len),
                        CarModel = name
                    };
                    _currentYMap.CarGenerators.Add(car);
                }

                if (mapObject.Type != MapObjectTypes.Prop) continue;

                var rot = mapObject.Quaternion;
                if (rot.W < 0) rot.W = -rot.W;
                else rot.Conjugate();

                var ent = new Entity
                {
                    Position = new XmlVector3(mapObject.Position.X, mapObject.Position.Y, mapObject.Position.Z),
                    Rotation = new XmlQuaternion(rot.X, rot.Y, rot.Z, rot.W),
                    ArchetypeName = name
                };
                if (!mapObject.Dynamic)
                    ent.Flags = new XmlValue<int>(32);
                _currentYMap.Entities.Add(ent);
            }

            CalcExtents();

            // Set some default values.
            _currentYMap.MetaDataBlock.Author = _currentYMap.MetaDataBlock.Owner = map.MetaData.Creator;
            _currentYMap.MetaDataBlock.Version = new XmlValue<int>(0);
            _currentYMap.Flags = new XmlValue<int>(0);

            // Set the current y map.
            propertyGrid1.SelectedObject = _currentYMap;
        }

        private void AddObjectsToYMap(SpoonerPlacements map)
        {
            _currentYMap = new YMap();

            foreach (var placement in map.Placements)
            {
                var name = placement.HashName;
                if (string.IsNullOrEmpty(name)) name = placement.ModelHash;

                var pos = placement.PositionRotation.GetPosition();

                if (placement.Type == 2)
                {
                    const float len = 1.5f; // TODO: FIXME
                    var v = new Vector3(0, len, 0);
                    var t = placement.PositionRotation.GetQuaternion() * v;
                    var orientX = t.X;
                    var orientY = t.Y;

                    var car = new CarGenerator
                    {
                        Position = new XmlVector3(pos.X, pos.Y, pos.Z),
                        OrientationX = new XmlValue<float>(orientX),
                        OrientationY = new XmlValue<float>(orientY),
                        PerpendicularLength = new XmlValue<float>(len),
                        CarModel = name
                    };
                    _currentYMap.CarGenerators.Add(car);
                }

                if (placement.Type != 3) continue;
                var rot = placement.PositionRotation.GetQuaternion();
                if (rot.W < 0) rot.W = -rot.W;
                else rot.Conjugate();

                var ent = new Entity
                {
                    Position = new XmlVector3(pos.X, pos.Y, pos.Z),
                    Rotation = new XmlQuaternion(rot.X, rot.Y, rot.Z, rot.W),
                    ArchetypeName = name
                };
                if (!placement.Dynamic)
                    ent.Flags = new XmlValue<int>(32);
                _currentYMap.Entities.Add(ent);
            }

            CalcExtents();

            // Set some default values.
            _currentYMap.MetaDataBlock.Version = new XmlValue<int>(0);
            _currentYMap.Flags = new XmlValue<int>(0);

            // Set the current y map.
            propertyGrid1.SelectedObject = _currentYMap;
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var s = new SaveFileDialog
            {
                Filter = @"YMap files (*.ymap.xml)|*.ymap.xml"
            };

            if (s.ShowDialog() != DialogResult.OK)
                return;

            using (var w = XmlWriter.Create(s.FileName,
                new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(),
                    Indent = true
                }))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var ser = new XmlSerializer(typeof(YMap));
                w.WriteStartDocument(false);
                ser.Serialize(w, _currentYMap, ns);
            }
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentYMap = new YMap();
            propertyGrid1.SelectedObject = _currentYMap;
        }

        private void CalculateExtentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CalcExtents();
        }

        private void CalcExtents()
        {
            if (!_currentYMap.Entities.Any() && !_currentYMap.CarGenerators.Any())
                return;

            _ymapCenter = new Vector3();

            foreach (var entity in _currentYMap.Entities)
                _ymapCenter += new Vector3(entity.Position.X, entity.Position.Y, entity.Position.Z);
            foreach (var carGenerator in _currentYMap.CarGenerators)
                _ymapCenter += new Vector3(carGenerator.Position.X, carGenerator.Position.Y, carGenerator.Position.Z);

            _ymapCenter /= _currentYMap.Entities.Count + _currentYMap.CarGenerators.Count;
            _currentYMap.StreamingExtenstsMin = new XmlVector3(_ymapCenter.X - 10000, _ymapCenter.Y - 10000, _ymapCenter.Z - 1000);
            _currentYMap.StreamingExtenstsMax = new XmlVector3(_ymapCenter.X + 10000, _ymapCenter.Y + 10000, _ymapCenter.Z + 5000);
            _currentYMap.EntitiesExtenstsMin = new XmlVector3(_ymapCenter.X - 10000, _ymapCenter.Y - 10000, _ymapCenter.Z - 1000);
            _currentYMap.EntitiesExtenstsMax = new XmlVector3(_ymapCenter.X + 10000, _ymapCenter.Y + 10000, _ymapCenter.Z + 5000);
            propertyGrid1.SelectedObject = _currentYMap;
        }
    }
}