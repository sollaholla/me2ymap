using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace YMapExporter
{
    public partial class YMapExporter : Form
    {
        private YMap _currentYMap = new YMap();

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
                Filter = @"Map Editor files (*.xml)|*.xml|YMap files (*.ymap.xml)|*.ymap.xml"
            };

            if (o.ShowDialog() != DialogResult.OK)
                return;

            if (o.FileName.EndsWith(".ymap.xml"))
            {
                using (var reader = XmlReader.Create(o.FileName))
                {
                    var s = new XmlSerializer(typeof(YMap));
                    _currentYMap = (YMap) s.Deserialize(reader);
                    reader.Close();
                    propertyGrid1.SelectedObject = _currentYMap;
                }
            }
            else if (o.FileName.EndsWith(".xml"))
            {
                MapEditorMap map = null;

                using (var reader = XmlReader.Create(o.FileName))
                {
                    try
                    {
                        var s = new XmlSerializer(typeof(MapEditorMap));
                        map = (MapEditorMap) s.Deserialize(reader);
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                if (map == null)
                {
                    MessageBox.Show(@"Failed to read map editor file.");
                    return;
                }

                AddObjectsToYMap(map);
            }
            else
            {
                MessageBox.Show(@"This is not a valid type!");
            }
        }

        private void AddObjectsToYMap(MapEditorMap map)
        {
            _currentYMap = new YMap();

            // Create the entities.
            var accum = new GtaVector();
            foreach (var mapObject in map.Objects)
            {
                if (mapObject.Type != MapObjectTypes.Prop) continue;
                var name = HashMap.GetName(mapObject.Hash);
                if (string.IsNullOrEmpty(name)) name = "0x" + mapObject.Hash.ToString("x");

                var rot = mapObject.Quaternion;
                if (rot.W < 0) rot.W = -rot.W;
                else
                {
                    rot.X = -rot.X;
                    rot.Z = -rot.Z;
                    rot.Y = -rot.Y;
                }

                var ent = new Entity {
                    Position = mapObject.Position,
                    Rotation = rot,
                    ArchetypeName = name
                };
                accum += mapObject.Position;
                _currentYMap.Entities.Add(ent);
            }
            accum /= map.Objects.Count;

            // Get the extents from the position accumulator.
            _currentYMap.StreamingExtenstsMin = new XVector(accum.X - 10000, accum.Y - 10000, accum.Z - 1000);
            _currentYMap.StreamingExtenstsMax = new XVector(accum.X + 10000, accum.Y + 10000, accum.Z + 5000);
            _currentYMap.EntitiesExtenstsMin = new XVector(accum.X - 10000, accum.Y - 10000, accum.Z - 1000);
            _currentYMap.EntitiesExtenstsMax = new XVector(accum.X + 10000, accum.Y + 10000, accum.Z + 5000);

            // Set some default values.
            _currentYMap.MetaDataBlock.Author = _currentYMap.MetaDataBlock.Owner = map.MetaData.Creator;
            _currentYMap.MetaDataBlock.Version = new XValue<int>(0);
            _currentYMap.Flags = new XValue<int>(0);

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
    }
}