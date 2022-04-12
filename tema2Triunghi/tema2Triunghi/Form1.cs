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

namespace tema2Triunghi
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
            Pen pen = new Pen(Color.Black);
        
            Point[] pcts = new Point[100];
        
            for (int i = 0; i <100; i++)
            {
        
                Point pct = new Point(_rand.Next(500), _rand.Next(300));
        
                e.Graphics.DrawEllipse(pen, pct.X, pct.Y, 1, 1);
        
                pcts[i] = pct;
            }
            
            
            Point a = pcts[0];
            Point b = pcts[0];
            Point c = pcts[0];
            double arie = -1;


            for (int j = 0; j < pcts.Length-2; j++)
            {
                Point p1 = pcts[j];
                for (int k = 0; k < pcts.Length-1; k++)
                {
                    Point p2 = pcts[k];
                    for (int l = 0; l < pcts.Length; l++)
                    {
                        Point p3 = pcts[l];
                        double calculArie = AriaTr(p1, p2, p3);


                        if (calculArie == 0)
                        {
                            continue;
                        }
                        

                        if (arie == -1 || calculArie < arie)
                        {
                            arie = calculArie;
                            a = p1;
                            b = p2;
                            c = p3;
                        }
                    }

                }
            }
            
            g.DrawLine(pen, a, b);
            g.DrawLine(pen, b, c);
            g.DrawLine(pen, c, a);
            
        }
        
        Random _rand = new Random();


        private static double AriaTr(Point a, Point b, Point c)
        {
            return Math.Abs((a.X * b.Y + b.X * c.Y + c.X * a.Y - a.Y * b.X - b.Y * c.X - c.Y * a.X) /2d);
        }
    }
}
