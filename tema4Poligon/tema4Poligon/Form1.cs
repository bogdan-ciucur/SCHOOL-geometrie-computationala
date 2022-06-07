using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tema4Poligon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            graphics = CreateGraphics();
            
        }

        // private void Form1_Paint(object sender, PaintEventArgs e)
        // {
        //
        //      // Graphics g = e.Graphics;
        //
        //     
        //     for (int i = 0; i < 100; i++)
        //     {
        //         Point pct = new Point(rand.Next(500), rand.Next(300));
        //
        //         graphics.DrawEllipse(pen, pct.X, pct.Y, 1, 1);
        //
        //         points[i] = pct;
        //     }
        // }
        
        private void SomeMethod()
        {

            for (int i = 0; i < 100; i++)
            {
                Point pct = new Point(rand.Next(500), rand.Next(300));
            
                graphics.DrawEllipse(pen, pct.X, pct.Y, 1, 1);
            
                points[i] = pct;
            }

            Compute(points);
        }
        
        
        
        
        
         public List<Point> Compute(IReadOnlyList<Point> points)
        {
            
            
            
            var hull = new HashSet<Point>();

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

                        // if (points[r].Orientation(points[p], points[q]) == -1)
                        // {
                        //     ok = false;
                        // }
                        
                        
                        if (Orientation(points[p], points[q], points[r]) == -1)
                        {
                            ok = false;
                        }
                    }

                    if (!ok)
                    {
                        continue;
                    }

                    hull.Add(points[q]);
                    hull.Add(points[p]);
                }
            }

            var ordered = OrderTrigonometrically(hull);
            graphics.DrawPolygon(pen, ordered.ToArray());

            return ordered;
            
            
            
        }

        private static List<Point> OrderTrigonometrically(ICollection<Point> points)
        {
            var centerPoint = GetCenterPoint(points);
            return points.OrderBy(point => Math.Atan2(point.Y - centerPoint.Y, point.X - centerPoint.X)).ToList();
            
            
        }
        
        public static Point GetCenterPoint( ICollection<Point> points)
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
        
        public static int Orientation( Point r, Point q, Point p)
        {
            var val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;
            return val > 0 ? 1 : -1;
        }


        Graphics graphics;
        
        Pen pen = new Pen(Color.Black);

        Point[] points = new Point[100];

        Random rand = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            
            SomeMethod();
            
        }
    }
}