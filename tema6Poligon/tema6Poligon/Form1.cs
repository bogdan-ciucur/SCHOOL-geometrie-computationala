using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tema6Poligon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            if (PointList.Count <= 1)
            {
                return;
            }
            
            List<Point> Points = Compute(PointList);
            
            
            
            e.Graphics.DrawPolygon(pen, Points.ToArray());

            for (int i = 0; i < PointList.Count; i++)
            {

                var p = PointList[i];
                e.Graphics.DrawEllipse(Pens.Black, p.X, p.Y, 1, 1);
            }

            
            
            //triangularea
            
            for (int i = 2; i < Points.Count-1; i++)
            {
                e.Graphics.DrawLine(pen2, Points[0], Points[i]);
            }
            
            
            
            

        }

        
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Point pct = new Point(e.X, e.Y);
            PointList.Add(pct);
            Refresh();
            
        }
        
        
        public List<Point> Compute(IReadOnlyList<Point> points)
        {
            var hull = new List<Point>();

            for (var p = 0; p < points.Count; p++)
            {
                for (var q = 0; q < points.Count; q++)
                {
                    if (p == q)
                    {
                        continue;
                    }

                    var ok = true;

                    for (var r = 0; r < points.Count; r++)
                    {
                        if (r == p || r == q)
                        {
                            continue;
                        }

                        if (Orientation(points[r], points[p], points[q]) == -1)
                        {
                            ok = false;
                        }
                    }

                    if (ok)
                    {
                        if (!hull.Contains(points[q]))
                        {
                            hull.Add(points[q]);
                        }

                        if (!hull.Contains(points[p]))
                        {
                            hull.Add(points[p]);
                        }
                    }
                }
            }

            return OrderTrigonometrically(hull);
        }

        private static List<Point> OrderTrigonometrically(List<Point> points)
        {
            var centerPoint = GetCenterPoint(points);
            return points.OrderBy(point => Math.Atan2(point.Y - centerPoint.Y, point.X - centerPoint.X)).ToList();
        }

        
        
        public static Point GetCenterPoint(List<Point> points)
        {
            var x = 0;
            var y = 0;

            foreach (var point in points)
            {
                x += point.X;
                y += point.Y;
            }

            return new Point(x / points.Count, y / points.Count);
        }

        public static int Orientation(Point r, Point q, Point p)
        {
            var val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;
            return val > 0 ? 1 : -1;
        }
        
        
        
        private static readonly Pen pen = new Pen(Color.Black, 1);
        private static readonly Pen pen2 = new Pen(Color.Red, 1);

        private static readonly List<Point> PointList = new List<Point>();

        



    }
}