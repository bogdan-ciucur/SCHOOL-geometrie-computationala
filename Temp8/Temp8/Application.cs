using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Temp8
{

    public partial class Application : Form
    {
        private readonly Graphics graphics;
        private readonly List<Point> points = new List<Point>();
        private readonly List<Tuple<Point, Point, Point>> triangles = new List<Tuple<Point, Point, Point>>();
        private readonly Pen pen = new Pen(Color.Black, 3);
        
        private bool closed;
        
        public Application()
        {
            InitializeComponent();
            graphics = CreateGraphics();
        }

        private void OnFormClick(object sender, MouseEventArgs e)
        {
            if (closed) return;
            
            points.Add(e.Location);
            graphics.DrawString(
                (points.Count + 1).ToString(),
                new Font(FontFamily.GenericSansSerif, 10),
                new SolidBrush(Color.Lime),
                e.Location.X + 15,
                e.Location.Y - 15);
            
            graphics.DrawEllipse(pen, e.Location.X, e.Location.Y, 1, 1);
            if (points.Count > 1)
            {
                graphics.DrawLine(pen, points[points.Count - 2], points.Last());
            }
        }

        private void OnSolveButtonClick(object sender, EventArgs e)
        {
            if (closed) return;
            if (points.Count < 3) return;
            
            closed = true;

            graphics.DrawLine(pen, points.Last(), points.First());
            if (points.Count == 3)
            {
                areaLabel.Text =
                    $@"Area: {Convert.ToString(Area(points.First(), points[1], points[2]), CultureInfo.InvariantCulture)}";
                return;
            }

            double area = 0;

            while (points.Count > 3)
            {
                for (var i = 0; i < points.Count; i++)
                {
                    double triangleArea;
                    if (i != points.Count - 1 && i != points.Count - 2 && IsDiagonal(i, i + 2))
                    {
                        triangleArea = Area(points[i], points[i + 1], points[i + 2]);
                        area += triangleArea;
                        graphics.DrawLine(Pens.MediumPurple, points[i], points[i + 2]);
                        triangles.Add(new Tuple<Point, Point, Point>(points[i], points[i + 1], points[i + 2]));
                        Thread.Sleep(100);
                        points.Remove(points[i + 1]);
                        break;
                    }

                    if (i == points.Count - 1 && IsDiagonal(i, 1))
                    {
                        triangleArea = Area(points[i], points.First(), points[1]);
                        area += triangleArea;
                        graphics.DrawLine(Pens.Red, points[i], points[1]);
                        triangles.Add(new Tuple<Point, Point, Point>(points[i], points.First(), points[1]));
                        Thread.Sleep(100);
                        points.Remove(points.First());
                        break;
                    }

                    if (i != points.Count - 2 || !IsDiagonal(i, 0)) continue;

                    triangleArea = Area(points[i], points[i - 1], points.First());
                    area += triangleArea;
                    graphics.DrawLine(Pens.Red, points[i], points.First());
                    triangles.Add(new Tuple<Point, Point, Point>(points[i], points[i - 1], points.First()));
                    Thread.Sleep(100);
                    points.Remove(points[i + 1]);
                    break;
                }
            }

            triangles.Add(new Tuple<Point, Point, Point>(points.Last(), points[points.Count - 2], points.First()));
            areaLabel.Text = $@"Area: {Convert.ToString(area, CultureInfo.InvariantCulture)}";
        }
       
        

        private void OnTricolorButtonClick(object sender, EventArgs e)
        {
            var pens = new[] { new Pen(Color.Aqua, 3), new Pen(Color.Purple, 3), new Pen(Color.GreenYellow, 3) };
            var markedVertices = new List<Tuple<Point, int>>();
            for (var i = triangles.Count - 1; i >= 0; i--)
            {
                var firstMarked = IsMarked(triangles[i].Item1, markedVertices);
                var secondMarked = IsMarked(triangles[i].Item2, markedVertices);
                var thirdMarked = IsMarked(triangles[i].Item3, markedVertices);

                switch (firstMarked)
                {
                    case -1 when secondMarked == -1 && thirdMarked == -1:
                        markedVertices.Add(new Tuple<Point, int>(triangles[i].Item1, 0));
                        graphics.DrawEllipse(pens[0], triangles[i].Item1.X - 8, triangles[i].Item1.Y - 8, 16, 16);
                        markedVertices.Add(new Tuple<Point, int>(triangles[i].Item2, 1));
                        graphics.DrawEllipse(pens[1], triangles[i].Item2.X - 8, triangles[i].Item2.Y - 8, 16, 16);
                        markedVertices.Add(new Tuple<Point, int>(triangles[i].Item3, 2));
                        graphics.DrawEllipse(pens[2], triangles[i].Item3.X - 8, triangles[i].Item3.Y - 8, 16, 16);
                        break;
                    case -1:
                        markedVertices.Add(new Tuple<Point, int>(triangles[i].Item1, MissingColor(secondMarked, thirdMarked)));
                        graphics.DrawEllipse(pens[MissingColor(secondMarked, thirdMarked)], triangles[i].Item1.X - 8, triangles[i].Item1.Y - 8, 16, 16);
                        break;
                    default:
                    {
                        if (secondMarked == -1)
                        {
                            markedVertices.Add(new Tuple<Point, int>(triangles[i].Item2, MissingColor(firstMarked, thirdMarked)));
                            graphics.DrawEllipse(pens[MissingColor(firstMarked, thirdMarked)], triangles[i].Item2.X - 8, triangles[i].Item2.Y - 8, 16, 16);
                        }
                        else if (thirdMarked == -1)
                        {
                            markedVertices.Add(new Tuple<Point, int>(triangles[i].Item3, MissingColor(firstMarked, secondMarked)));
                            graphics.DrawEllipse(pens[MissingColor(firstMarked, secondMarked)], triangles[i].Item3.X - 8, triangles[i].Item3.Y - 8, 16, 16);
                        }

                        break;
                    }
                }
            }
        }

        private bool IsDiagonal(int i, int j)
        {
            var intersected = false;
            for (var k = 0; k < points.Count - 1; k++)
            {
                if (i == k || i == k + 1 || j == k || j == k + 1 ||
                    !Intersects(points[i], points[j], points[k], points[k + 1])) continue;
                
                intersected = true;
                break;
            }
            
            if (i != points.Count - 1 && i != 0 && j != points.Count - 1 && j != 0 &&
                Intersects(points[i], points[j], points.Last(), points.First())) intersected = true;
            
            return !intersected && IsInsidePolygon(i, j);
        }

        private bool IsInsidePolygon(int pi, int pj)
        {
            var previousPointI = pi > 0 ? pi - 1 : points.Count - 1;
            var nextPointI = pi < points.Count - 1 ? pi + 1 : 0;
            return (Convex(pi) && LeftTurn(pi, pj, nextPointI) && LeftTurn(pi, previousPointI, pj)) ||
                   (Reflex(pi) && !(RightTurn(pi, pj, nextPointI) && RightTurn(pi, previousPointI, pj)));
        }

        private bool Convex(int p)
        {
            var previousPoint = p > 0 ? p - 1 : points.Count - 1;
            var nextPoint = p < points.Count - 1 ? p + 1 : 0;
            
            return RightTurn(previousPoint, p, nextPoint);
        }

        private bool Reflex(int p)
        {
            var previousPoint = p > 0 ? p - 1 : points.Count - 1;
            var nextPoint = p < points.Count - 1 ? p + 1 : 0;
            
            return LeftTurn(previousPoint, p, nextPoint);
        }

        private bool LeftTurn(int p1, int p2, int p3)
        {
            return Sarrus(points[p1], points[p2], points[p3]) < 0;
        }

        private bool RightTurn(int p1, int p2, int p3)
        {
            return Sarrus(points[p1], points[p2], points[p3]) > 0;
        }

        private static int IsMarked(Point point, IEnumerable<Tuple<Point, int>> markedVertices)
        {
            foreach (var markedVertex in markedVertices)
            {
                if (markedVertex.Item1 == point) return markedVertex.Item2;
            }

            return -1;
        }
        
        private static double Area(Point first, Point second, Point third)
        {
            return Math.Abs(0.5 * Sarrus(first, second, third));
        }
        
        private static bool Intersects(Point firstStart, Point firstEnd, Point secondStart, Point secondEnd)
        {
            return Sarrus(secondEnd, secondStart, firstStart) * Sarrus(secondEnd, secondStart, firstEnd) <= 0 &&
                   Sarrus(firstEnd, firstStart, secondStart) * Sarrus(firstEnd, firstStart, secondEnd) <= 0;
        }

        private static double Sarrus(Point first, Point second, Point third)
        {
            return first.X * second.Y + second.X * third.Y + third.X * first.Y - third.X * second.Y - second.X * first.Y - first.X * third.Y;
        }

        private static int MissingColor(int a, int b)
        {
            switch (a)
            {
                case 0 when b == 1:
                case 1 when b == 0:
                    return 2;
                case 0 when b == 2:
                case 2 when b == 0:
                    return 1;
                case 1 when b == 2:
                case 2 when b == 1:
                    return 0;
                default:
                    return -1;
            }
        }
        
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr h, int attr, int[] attrValue, int attrSize);

        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }
    }
}
