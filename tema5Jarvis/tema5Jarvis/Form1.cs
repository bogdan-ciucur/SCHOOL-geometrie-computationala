using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tema5Jarvis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);

            Point[] pcts = new Point[100];

            for (int i = 0; i < 100; i++)
            {

                Point pct = new Point(_rand.Next(500), _rand.Next(300));

                e.Graphics.DrawEllipse(pen, pct.X, pct.Y, 1, 1);

                pcts[i] = pct;
            }


            var hullPoints = convexHull(pcts, pcts.Length);
            
            e.Graphics.DrawPolygon(pen, hullPoints.ToArray());

        }


        public static int orientation(Point p, Point q, Point r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;

            return (val > 0) ? 1 : 2;
        }


        public static List<Point> convexHull(Point[] points, int n)
        {
            if (n <= 3)
            {
                return points.ToList();
            }

            ;

            // Initialize Result
            List<Point> hull = new List<Point>();

            // Find the leftmost point
            int left = 0;
            for (int i = 1; i < n; i++)
                if (points[i].X < points[left].X)
                    left = i;

            // Start from leftmost point, keep moving
            int p = left, q;
            do
            {
                // Add current point to result
                hull.Add(points[p]);

                // Search for a point 'q' such that
                // orientation(p, q, x) is counterclockwise
                // for all points 'x'. The idea is to keep
                q = (p + 1) % n;

                for (int i = 0; i < n; i++)
                {
                    if (orientation(points[p], points[i], points[q]) == 2)
                        q = i;
                }

                p = q;

            } while (p != left); 

            return hull;
        }


        Random _rand = new Random();

    }

}