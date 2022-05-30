using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Temp9
{
    public partial class Application : Form
    {
        private readonly Graphics graphics;
        private readonly List<Point> points = new List<Point>();
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
            graphics.DrawEllipse(pen, e.Location.X, e.Location.Y, 2, 2);
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

            for (var i = 0; i < points.Count; i++)
            {
                if (!Reflex(i)) continue;
                
                if (i == 0)
                {
                    int j;
                    if (points.Last().Y > points[i].Y && points[i + 1].Y > points[i].Y)
                        if (FirstAbove(i) != -1)
                        {
                            j = FirstAbove(i);
                            graphics.DrawLine(Pens.Aqua, points[i], points[j]);
                        }

                    if (points.Last().Y >= points[i].Y || points[i + 1].Y >= points[i].Y) continue;
                    if (FirstUnder(i) == -1) continue;

                    j = FirstUnder(i);
                    graphics.DrawLine(Pens.Aqua, points[i], points[j]);
                }
                else if (i == points.Count - 1)
                {
                    int j;
                    if (points[i - 1].Y > points[i].Y && points.First().Y > points[i].Y)
                        if (FirstAbove(i) != -1)
                        {
                            j = FirstAbove(i);
                            graphics.DrawLine(Pens.Aqua, points[i], points[j]);
                        }

                    if (points[i - 1].Y >= points[i].Y || points.First().Y >= points[i].Y) continue;
                    if (FirstUnder(i) == -1) continue;

                    j = FirstUnder(i);
                    graphics.DrawLine(Pens.Aqua, points[i], points[j]);
                }
                else
                {
                    int j;
                    if (points[i - 1].Y > points[i].Y && points[i + 1].Y > points[i].Y)
                        if (FirstAbove(i) != -1)
                        {
                            j = FirstAbove(i);
                            graphics.DrawLine(Pens.Aqua, points[i], points[j]);
                        }

                    if (points[i - 1].Y >= points[i].Y || points[i + 1].Y >= points[i].Y) continue;
                    if (FirstUnder(i) == -1) continue;

                    j = FirstUnder(i);
                    graphics.DrawLine(Pens.Aqua, points[i], points[j]);
                }
            }
        }
        
      
        
        private int FirstUnder(int i)
        {
            for (var k = points[i].Y + 1; k < Height; k++)
            {
                for (var h = 0; h < points.Count; h++)
                {
                    if (points[h].Y == k && IsDiagonal(i, h)) return h;
                }
            }

            return -1;
        }

        private int FirstAbove(int i)
        {
            for (var k = points[i].Y - 1; k >= 0; k--)
            {
                for (var h = 0; h < points.Count; h++)
                {
                    if (points[h].Y == k && IsDiagonal(i, h)) return h;
                }
            }

            return -1;
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
        
        private static bool Intersects(Point firstStart, Point firstEnd, Point secondStart, Point secondEnd)
        {
            return Sarrus(secondEnd, secondStart, firstStart) * Sarrus(secondEnd, secondStart, firstEnd) <= 0 &&
                   Sarrus(firstEnd, firstStart, secondStart) * Sarrus(firstEnd, firstStart, secondEnd) <= 0;
        }

        private static double Sarrus(Point p1, Point p2, Point p3)
        {
            return p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y;
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
