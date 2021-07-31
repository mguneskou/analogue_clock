using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace analogue_clock
{
    public partial class MainForm : Form
    {
        Graphics g = null;
        Pen p = null;
        Timer t = new Timer();
        Point centre;
        DateTime now;
        int hour = 0, minute = 0, second = 0;
        Hand second_hand = null, minute_hand = null, hour_hand = null;

        public MainForm()
        {
            InitializeComponent();
            t.Tick += T_Tick;
            t.Interval = 1000;
            g = this.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            p = new Pen(Color.White, 2);
            centre = new Point(this.Width / 2, this.Height / 2);
        }

        void Initialize_clock()
        {
            //get the system time
            now = new DateTime();
            now = DateTime.Now;
            hour = now.Hour;
            minute = now.Minute;
            second = now.Second;
            second_hand = new Hand();
            second_hand.length = this.Width / 2 - 25;
            second_hand.angle = 360 / 60 * second;
            minute_hand = new Hand();
            minute_hand.length = this.Width / 2 - 50;
            minute_hand.angle = 360 / 60 * minute;
            hour_hand = new Hand();
            hour_hand.length = this.Width / 2 - 70;
            hour_hand.angle = 360 / 12 * hour;
        }

        int x1, y1, delta_x, delta_y;
        bool holding = false;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            holding = true;
            x1 = e.X;
            y1 = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            holding = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (holding)
            {
                delta_x = e.X - x1;
                delta_y = e.Y - y1;
                this.Left += delta_x;
                this.Top += delta_y;
            }
        }

        void draw_background()
        {
            p.Color = Color.White;
            p.Width = 6;
            g.DrawLine(p, centre.X, 25, centre.X, 50);//12
            g.DrawLine(p, this.Width - 50, centre.Y, this.Width - 25, centre.Y);//3
            g.DrawLine(p, centre.X, this.Height - 50, centre.X, this.Height - 25);//6
            g.DrawLine(p, 25, centre.Y, 50, centre.Y);//9
        }

        private void T_Tick(object sender, EventArgs e)
        {
            now = DateTime.Now;
            hour = now.Hour;
            minute = now.Minute;
            second = now.Second;
            string s, m, h;
            if (second < 10)
            {
                s = "0" + second.ToString();
            }
            else
            {
                s = second.ToString();
            }
            if (minute < 10)
            {
                m = "0" + minute.ToString();
            }
            else
            {
                m = minute.ToString();
            }
            if (hour < 10)
            {
                h = "0" + hour.ToString();
            }
            else
            {
                h = hour.ToString();
            }
            lbl_digital.Text = h + ":" + m + ":" + s;
            lbl_date.Text = now.Day + "." + now.Month + "." + now.Year + " " + now.DayOfWeek;
            second_hand.angle = ((2 * Math.PI / 60) * second) - (Math.PI / 2);
            minute_hand.angle = ((2 * Math.PI / 60) * minute) - (Math.PI / 2);
            hour_hand.angle = ((2 * Math.PI / 12) * hour) - (Math.PI / 2);
            g.Clear(Color.Black);
            draw_background();
            //draw second hand
            p.Width = 2;
            p.Color = Color.Red;
            g.DrawLine(p, (float)centre.X, (float)centre.Y, (float)centre.X + (float)(Math.Cos(second_hand.angle) * second_hand.length), (float)centre.Y + (float)(Math.Sin(second_hand.angle) * second_hand.length));
            //draw minute hand
            p.Color = Color.Green;
            g.DrawLine(p, (float)centre.X, (float)centre.Y, (float)centre.X + (float)(Math.Cos(minute_hand.angle) * minute_hand.length), (float)centre.Y + (float)(Math.Sin(minute_hand.angle) * minute_hand.length));
            //draw hour hand
            p.Color = Color.White;
            g.DrawLine(p, (float)centre.X, (float)centre.Y, (float)centre.X + (float)(Math.Cos(hour_hand.angle) * hour_hand.length), (float)centre.Y + (float)(Math.Sin(hour_hand.angle) * hour_hand.length));
            //g.Flush();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize_clock();
            t.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                t.Stop();
                Application.Exit();
            }
        }
    }

    internal class Hand
    {
        internal int length;
        internal double angle;
    }
}