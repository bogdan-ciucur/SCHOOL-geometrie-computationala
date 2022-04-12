using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace tema1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // Form1_Paint();
            InitializeComponent();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);

            Point[] pcts = new Point[100];
            
            for (int i = 0; i < 100; i++)
            {

                Point  pct = new Point(_rand.Next(700),_rand.Next(300));
                
                e.Graphics.DrawEllipse(pen, pct.X, pct.Y, 1, 1);

                pcts[i] = pct;

                
                



            }
            
            
            Point minX = pcts[0];
            Point minY = pcts[0];
            Point maxX = pcts[0]; 
            Point maxY = pcts[0];

            for (int j = 1; j < 100; j++)
            {
                Point point = pcts[j];
                    
                if (minX.X > point.X)
                {
                    minX = point;
                }

                if (maxX.X < point.X)
                {
                    maxX = point;
                }

                if (minY.Y > point.Y)
                {
                    minY = point;
                }

                if (maxY.Y < point.Y)
                {
                    maxY= point;
                }


            }

            Rectangle drept = new Rectangle(minX.X - 1, minY.Y - 1, maxX.X - minX.X + 2, maxY.Y - minY.Y + 2);
       //     Rectangle drept = new Rectangle(minX.X - `, minY.Y, minX.Y, maxY.Y);
            e.Graphics.DrawRectangle(pen, drept);
            

        }

        
        
        Random _rand = new Random();   
        
        

    }
}