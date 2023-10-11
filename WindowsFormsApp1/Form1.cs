using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEngine
{


    public partial class Form1 : Form
    {

        public static List<PictureBox> pictures = new List<PictureBox>();
        public static PictureBox player;
        public PictureBox selectedPictureBox = null;
        public static bool isPlayer = false;
        private Point mouseOffset;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();


            int sizeSquare = 50;
            int x = rnd.Next((572 - sizeSquare));
            int y = rnd.Next((378 - sizeSquare));

            PictureBox pic = new PictureBox();
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Size = new System.Drawing.Size(sizeSquare, sizeSquare);
            pic.Location = new System.Drawing.Point(x, y);
            pic.Load("http://i.imgur.com/7ikw7ye.png");
            pictures.Add(pic);
            panel1.Controls.Add(pic);
            listBox1.Items.Add("Square");
            pic.Name = "Square";

        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //private void GameLoop(object sender, EventArgs e)
        //{
        //    if (Form1.pictures.Count != 0)
        //        Physics.gravity(Form1.pictures[0]);
        //}
        void GameLoop(object sender, EventArgs e)
        {
                foreach (PictureBox pic in pictures)
                {
                    if(pic.Name == "player")
                    {
                        Physics.gravity(pic);
                        System.Console.WriteLine("Update");
                    }
                }
            //make the player box collide with the other boxes
            if (player != null)
            {
                foreach (PictureBox pic in pictures)
                {
                    if (pic.Name != "player")
                    {
                        if (player.Bounds.IntersectsWith(pic.Bounds))
                        {
                            player.Location = new System.Drawing.Point(player.Location.X, player.Location.Y - 1);
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isPlayer)
            {
                MessageBox.Show("You already have a player");
                return;
            }

            int sizeSquare = 50;
            int x = 0;
            int y = 0;

            //clear listbox selection
            listBox1.ClearSelected();

            PictureBox pic = new PictureBox();
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Size = new System.Drawing.Size(sizeSquare, sizeSquare);
            pic.Location = new System.Drawing.Point(x, y);
            pic.Load("oui.png");
            pic.Name = "player";
            pictures.Add(pic);
            panel1.Controls.Add(pic);
            listBox1.Items.Add("Player");
            isPlayer = true;
            player = pic;

            // select the last item
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }
        //create a function to select a picturebox from the listbox selection and allow the user to move it with the mouse
        //create event handlers for the mouse down, mouse move, and mouse up events
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                selectedPictureBox = pictures[listBox1.SelectedIndex];
                selectedPictureBox.BorderStyle = BorderStyle.Fixed3D;
                selectedPictureBox.MouseDown += PictureBox_MouseDown;
                selectedPictureBox.MouseMove += PictureBox_MouseMove;
                selectedPictureBox.MouseUp += PictureBox_MouseUp;
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectedPictureBox = (PictureBox)sender;
                mouseOffset = e.Location;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPictureBox != null && e.Button == MouseButtons.Left)
            {
                Point newLocation = panel1.PointToClient(Cursor.Position);
                newLocation.Offset(-mouseOffset.X, -mouseOffset.Y);
                selectedPictureBox.Location = newLocation;
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedPictureBox != null && e.Button == MouseButtons.Left)
            {
                selectedPictureBox = null;
            }
        }

    }
}