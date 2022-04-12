using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tema3Cerc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // throw new System.NotImplementedException();

            Graphics g = e.Graphics;
            var pen = new Pen(Color.Black);
            
            
            //se creeaza o lista de puncte
            List<Point> pcts = new List<Point>();

            for (int i = 0; i < 100; i++)
            {

                Point pct = new Point(_rand.Next(200,600), _rand.Next(200, 250));

                e.Graphics.DrawEllipse(pen, pct.X, pct.Y, 1, 1);

                pcts.Add(pct);
            }
            
            
            Solve(e.Graphics, pcts, pen);
        }
        
        
        
        //funcntie care apeleaza celelalte functii
        public void Solve(Graphics graphics, List<Point> points, Pen pen)
        {
            
            
            var circle = GetSmallestEnclosingCircle(points);
            DrawCircle(graphics, pen, circle);
            /* vvv Aici se deseneaza centrul cercului dar nu e obligatoriu vvv */
            DrawPoint(graphics, pen, circle.Item1);
        }

        
        //functia care gaseste cercul cel mai mic
        
        
        
        public Tuple<Point, float> GetSmallestEnclosingCircle(List<Point> points)
        {
            
            //daca nu sunt puncte, atunci se da un cerc gol
            if (points.Count < 1)
            {
                return new Tuple<Point, float>(Point.Empty, 0);
            }
            // daca exista un punct, se da un cerc de raza 0, care e punctul
            if (points.Count < 2)
            {
                return new Tuple<Point, float>(points[0], 0);
            }
            
            
            // se gaseste punctul central
            var centerPoint = GetCenterPoint(points);
            
            //se gaseste primul punct generat
            //gaseste cel mai departat punct de centru
            var maxPoint = points.First();
            foreach (var point in points.Where(point => DistanceSquaredTo(point, centerPoint) > DistanceSquaredTo(maxPoint,centerPoint)))
            {
                maxPoint = point;
            }

            return new Tuple<Point, float>(centerPoint, (float) Math.Sqrt(DistanceSquaredTo(maxPoint,centerPoint)));
        }

        public Point GetCenterPoint(List<Point> points)
        {
            //se face media din x si din y 
            
            var x = 0;
            var y = 0;
            
            foreach (var point in points)
            {
                x += point.X;
                y += point.Y;
            }

            return new Point(x / points.Count, y / points.Count);
        }


        public static void DrawCircle(Graphics graphics, Pen pen, Tuple<Point, float> circle)
        {
            if (circle.Item1.IsEmpty || circle.Item2 == 0)
            {
                return;
            }

            graphics.DrawEllipse(
                pen,
                circle.Item1.X - circle.Item2,
                circle.Item1.Y - circle.Item2,
                circle.Item2 * 2,
                circle.Item2 * 2
            );
        }

        Random _rand = new Random();

        
        public static void DrawPoint(Graphics graphics, Pen pen, Point point)
        {
            if (point.IsEmpty)
            {
                return;
            }

            graphics.DrawEllipse(pen, point.X - 1.5f, point.Y - 1.5f, 3f, 3f);
            graphics.FillEllipse(new SolidBrush(pen.Color), point.X - 1.5f, point.Y - 1.5f, 3f, 3f);
        }

        
        
        public static double DistanceSquaredTo(Point a, Point b)
        {
            return Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2);
        }
        
        
    }

}
