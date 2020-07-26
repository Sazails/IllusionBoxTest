using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IllusionTest
{
    public partial class illusion_form : Form
    {
        double[][] vertices = { new double[] {-1,-1,-1}, new double[] {-1,-1,1}, new double[] {-1,1,-1},
        new double[] {-1,1,1}, new double[] {1,-1,-1}, new double[] {1,-1,1},
        new double[] {1,1,-1}, new double[] {1,1,1} };

        int[][] edges = { new int[] {0, 1}, new int[] {1, 3}, new int[] {3, 2}, new int[] {2, 0},
        new int[] {4, 5}, new int[] {5, 7}, new int[] {7, 6}, new int[] {6, 4}, new int[] {0, 4},
        new int[] {1, 5}, new int[] {2, 6}, new int[] {3, 7}};

        public illusion_form()
        {
            InitializeComponent();
            Width = Height - 512;
            StartPosition = FormStartPosition.CenterScreen;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Scale(100,100,100);
            RotateCube(Math.PI / 4, Math.Atan(Math.Sqrt(2)));

            var timer = new Timer();
            timer.Tick += (s, e) => { RotateCube(Math.PI / 180, 0); Refresh(); };
            timer.Interval = 000017;
            timer.Start();
        }

        private void RotateCube(double angleX, double angleY)
        {
            double sinX = Math.Sin(angleX);
            double cosX = Math.Cos(angleX);

            double sinY = Math.Sin(angleY);
            double cosY = Math.Cos(angleY);

            foreach (double[] vertice in vertices)
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
            foreach(double[] vertice in vertices)
            {
                vertice[0] *= v1;
                vertice[1] *= v2;
                vertice[2] *= v3;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);
            g.TranslateTransform(Width / 2, Height / 2);

            foreach(int[] edge in edges)
            {
                double[] xy1 = vertices[edge[0]];
                double[] xy2 = vertices[edge[1]];
                g.DrawLine(Pens.Black, (int)Math.Round(xy1[0]), (int)Math.Round(xy1[1]), (int)Math.Round(xy2[0]), (int)Math.Round(xy2[1]));
            }

            foreach (double[] vertice in vertices)
            {
                g.FillEllipse(Brushes.Black, (int)Math.Round(vertice[0]) - 4, (int)Math.Round(vertice[1]) - 4, 8, 8);
            }
        }
    }
}
