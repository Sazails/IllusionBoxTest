using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IllusionTest
{
    public partial class IllusionForm : Form
    {
        // Default shape "CUBE"
        private double[][] _vertices = {
        new double[] {-1,-1,-1},
        new double[] {-1,-1,1},
        new double[] {-1,1,-1},
        new double[] {-1,1,1},
        new double[] {1,-1,-1},
        new double[] {1,-1,1},
        new double[] {1,1,-1},
        new double[] {1,1,1} };

        private int[][] _edges = {
        new int[] {0, 1},
        new int[] {1, 3},
        new int[] {3, 2},
        new int[] {2, 0},
        new int[] {4, 5},
        new int[] {5, 7},
        new int[] {7, 6},
        new int[] {6, 4},
        new int[] {0, 4},
        new int[] {1, 5},
        new int[] {2, 6},
        new int[] {3, 7}};

        private double tickRate = 180;
        private SmoothingMode _smoothingMode = SmoothingMode.None;
        
        private Color _backgroundColor = Color.White;
        private Color _shapeColor = Color.Black;

        public IllusionForm()
        {
            InitializeComponent();
            Width = Height = 768;
            StartPosition = FormStartPosition.CenterScreen;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Scale(100, 100, 100);
            RotateShape(Math.PI / 4, Math.Atan(Math.Sqrt(2)));

            Timer timer = new Timer();
            timer.Tick += (s, e) => { RotateShape(Math.PI / tickRate, Math.PI / (tickRate * 3)); Refresh(); };
            timer.Interval = 000017;
            timer.Start();
        }

        private void RotateShape(double angleX, double angleY)
        {
            double sinX = Math.Sin(angleX);
            double cosX = Math.Cos(angleX);

            double sinY = Math.Sin(angleY);
            double cosY = Math.Cos(angleY);

            foreach (double[] vertice in _vertices)
            {
                double x = vertice[0];
                double y = vertice[1];
                double z = vertice[2];

                vertice[0] = x * cosX - z * sinX;
                vertice[2] = z * cosX + x * sinX;

                z = vertice[2];

                vertice[1] = y * cosY - z * sinY;
                vertice[2] = z * cosY + y * sinY;
            }
        }

        private void Scale(int v1, int v2, int v3)
        {
            foreach (double[] vertice in _vertices)
            {
                vertice[0] *= v1;
                vertice[1] *= v2;
                vertice[2] *= v3;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = _smoothingMode;
            g.Clear(_backgroundColor);
            g.TranslateTransform(Width / 2, Height / 2);

            foreach (int[] edge in _edges)
            {
                double[] xy1 = _vertices[edge[0]];
                double[] xy2 = _vertices[edge[1]];
                g.DrawLine(new Pen(_shapeColor), (int)Math.Round(xy1[0]), (int)Math.Round(xy1[1]), (int)Math.Round(xy2[0]), (int)Math.Round(xy2[1]));
            }

            foreach (double[] vertice in _vertices)
                g.FillEllipse(new SolidBrush(_shapeColor), (int)Math.Round(vertice[0]) - 4, (int)Math.Round(vertice[1]) - 4, 8, 8);
        }

        #region Theme Dropdown
        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backgroundColor = Color.White;
            _shapeColor = Color.Black;
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backgroundColor = Color.Black;
            _shapeColor = Color.White;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backgroundColor = Color.DarkGreen;
            _shapeColor = Color.LawnGreen;
        }

        private void purpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backgroundColor = Color.Purple;
            _shapeColor = Color.MediumPurple;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backgroundColor = Color.DarkBlue;
            _shapeColor = Color.Blue;
        }
        #endregion

        private void ShapeSmoothingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShapeSmoothingCheckBox.Checked)
                _smoothingMode = SmoothingMode.HighQuality;
            else
                _smoothingMode = SmoothingMode.None;
        }
    }
}
