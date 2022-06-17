using DwtReader.Components;
using DwtReader.Core;
using DwtReader.Data;
using DwtReader.Decoders;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

namespace DwtViewer
{
    public partial class Form1 : Form
    {
        DwtFile _dwtFile;
        public Form1()
        {
            InitializeComponent();
            _dwtFile = ImportDwtFile(@"D:\_DISK_E_\FQW\Diplom\Data\" + "а4книга.dwg");
        }

        private void PictureBoxClick(object sender, EventArgs e)
        {
            Graphics graphics = _pictureBox.CreateGraphics();
            Pen pen = new Pen(Color.Black, 3);
            foreach (var entity in _dwtFile.Objects) {
                if (entity.type == ObjectType.Line) {
                    var line = entity.entity as Line;
                    if (line.Invisible == 1)
                        continue;
                    //if (line.Owner.reference != 0x2B6D)
                        //continue;
                    graphics.DrawLine(pen, Norm(line.StartX), Reflect(line.StartY), Norm(line.EndX), Reflect(line.EndY));
                }
                if (entity.type == ObjectType.LwPolyLine) {
                    var polyline = entity.entity as LwPolyLine;
                    //if (polyline.Owner.reference != 0x2B6D)
                        //continue;
                    if (polyline.Invisible == 1)
                        continue;
                    for (int i = 0; i < polyline.Points.Count - 1; i++) {
                        var point1 = polyline.Points[i];
                        var point2 = polyline.Points[i + 1];
                        graphics.DrawLine(pen, Norm(point1.x), Reflect(point1.y), Norm(point2.x), Reflect(point2.y));
                    }
                }
                /*if (entity.type == ObjectType.Text) {
                    var text = entity.entity as Text;
                    if (text.Invisible == 1)
                        continue;
                    //if (text.Owner.reference != 0x2B6D)
                        //continue;
                    if (text.RotationAngle > 0)
                        continue;
                    Font font = new Font("Arial", (int)text.Height);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    graphics.DrawString(text.Value, font, brush, Norm(text.Insertion.x), Reflect(text.Insertion.y));
                }*/
            }
        }

        private int Norm(double value)
        {
            return 500 + (int)value;
        }

        private int Reflect(double value)
        {
            return 1000 - Norm(value);
        }

        private DwtFile ImportDwtFile(string dwtPath)
        {
            DwtFile dwtFile = null;
            try {
                var stream  = new DwtStream(File.Open(dwtPath, FileMode.Open));
                var version = DetermineVersion(stream);
                var factory = new DwtDecoderFactory();
                var decoder = factory.Create(version);

                if (decoder == null)
                    stream.Close();

                dwtFile = decoder?.Decode(stream);
            }
            catch (Exception) {
            }
            return dwtFile;
        }

        private DwtVersion DetermineVersion(DwtStream stream)
        {
            stream.SetPosition(0);
            var version = new string(stream.GetChars(6));
            try {
                return (DwtVersion)Enum.Parse(typeof(DwtVersion), version);
            }
            catch (Exception) {
                return DwtVersion.None;
            }
        }

        private bool IsValidDwtFileName(string dwtPath)
        {
            if (dwtPath is null)
                return false;

            if (Path.GetFileName(dwtPath)
                    .EndsWith(".dwg", StringComparison.CurrentCultureIgnoreCase) == true) {
                return true;
            }

            if (Path.GetFileName(dwtPath)
                    .EndsWith(".dwt", StringComparison.CurrentCultureIgnoreCase) == true)
                return true;

            return false;
        }
    }
}