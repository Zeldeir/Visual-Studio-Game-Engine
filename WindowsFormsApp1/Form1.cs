using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace SimpleEngine
{


    public partial class Form1 : Form
    {

        public static List<PictureBox> pictures = new List<PictureBox>();
        public static PictureBox player;
        public PictureBox selectedPicture = null;
        public static bool isPlayer = false;
        public static PictureBox flag = null;
        private Point mouseOffset;
        public static bool win = false;
        private bool hasDisplayedWinMessage = false;
        private System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();


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

        void GameLoop(object sender, EventArgs e)
        {
            foreach (PictureBox pic in pictures)
            {
                if(pic.Name == "player")  
                {   
                    Physics.gravity(pic);              
                    System.Console.WriteLine("Update");
                        
                    if (player != null && flag != null && player.Bounds.IntersectsWith(flag.Bounds) && !win)               
                    {
                        // You won! Display a pop-up message.
                        MessageBox.Show("You won!");
                        win = true;
                        Application.Exit();
                        if (!hasDisplayedWinMessage)
                        {
                            // Set the hasDisplayedWinMessage flag to true to prevent additional win message windows.
                            hasDisplayedWinMessage = true;

                            // Display the win message.
                            MessageBox.Show("You won!");
                        }
                    }
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

        //function to add a player to the window
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
            pic.Load("../../Images/player.png");
            pic.Name = "player";
            pictures.Add(pic);
            panel1.Controls.Add(pic);
            listBox1.Items.Add("Player");
            isPlayer = true;
            player = pic;

            // select the last item
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        //function to add a sound to the window and allow the user to turn it on and off with the space bar
        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Sound Files|*.wav;*.mp3"; // Filter to allow only WAV and MP3 files.
                openFileDialog.Title = "Select a Sound File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Set the SoundPlayer's sound location to the selected file.
                    soundPlayer.SoundLocation = openFileDialog.FileName;

                    // Play the selected sound.
                    soundPlayer.Play();

                    // Add a key press event handler to the form.
                    this.KeyDown += new KeyEventHandler(Sound_KeyDown);

                }
            }
        }

        //function to handle the space bar event
        private void Sound_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                // If the sound is playing, stop it.
                if (soundPlayer.IsLoadCompleted)
                {
                    soundPlayer.Stop();
                }
            }
        }


        //function to add a flag to the window
        private void button4_Click(object sender, EventArgs e)
        {
            if (flag != null)
            {
                MessageBox.Show("You already have a flag");
                return;
            }

            int sizeFlag = 50;
            int x = 200;
            int y = 200;

            listBox1.ClearSelected();

            PictureBox pic = new PictureBox();
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Size = new Size(sizeFlag, sizeFlag);
            pic.Location = new Point(x, y);
            pic.Load("../../Images/flag.png"); // Load the flag sprite.
            pic.Name = "Flag";
            pictures.Add(pic);
            panel1.Controls.Add(pic);
            listBox1.Items.Add("Flag");
            flag = pic;
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        //function to choose an image and add it to the window
        //make the image resizable
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;..."; // Filter to allow only image files.
            openFileDialog1.Title = "Select an Image File";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                //make the image resizable
                PictureBox pic = new PictureBox();
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new System.Drawing.Size(50, 50);
                pic.Location = new System.Drawing.Point(0, 0);
                pic.Load(openFileDialog1.FileName);
                pictures.Add(pic);
                panel1.Controls.Add(pic);
                listBox1.Items.Add("Image");
                pic.Name = "Image";
                //allow the user to resize the image with the double click and mouse wheel
                pic.MouseDoubleClick += new MouseEventHandler(pictureBox1_DoubleClick);
                pic.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
                
            }
        }

        //function to handle the resizing of the image after a double click on it
        private void pictureBox1_DoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Size += new System.Drawing.Size(10, 10);
        }

        //function to handle the resizing of the image after a mouse wheel event on it
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Size -= new System.Drawing.Size(10, 10);
        }

        //function to select a picturebox from the listbox selection and allow the user to move it with the mouse
        //event handlers for the mouse down, mouse move, and mouse up events
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // clear selection
            if (selectedPicture != null)
            {
                selectedPicture.BorderStyle = BorderStyle.None;
                //remove handlers
                selectedPicture.MouseDown -= new MouseEventHandler(pictureBox_MouseDown);
                selectedPicture.MouseMove -= new MouseEventHandler(pictureBox_MouseMove);
                selectedPicture.MouseUp -= new MouseEventHandler(pictureBox_MouseUp);
                
            }
            // select the picture
            if (listBox1.SelectedIndex == -1)
            {
                return;
            }
            selectedPicture = pictures[listBox1.SelectedIndex];
            selectedPicture.BorderStyle = BorderStyle.FixedSingle;
            //handlers to move the picture
            selectedPicture.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
            selectedPicture.MouseMove += new MouseEventHandler(pictureBox_MouseMove);
            selectedPicture.MouseUp += new MouseEventHandler(pictureBox_MouseUp);
            
        }

        // Handle mouse down on picture
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox pic = (PictureBox)sender;
                pic.Tag = new Point(e.X, e.Y);
            }

            // if right click choose a new image
            if (e.Button == MouseButtons.Right)
            {
                PictureBox pic = (PictureBox)sender;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
                openFileDialog1.Title = "Select an Image File";
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName != "")
                    pic.ImageLocation = openFileDialog1.FileName;
            }
        }

        // Handle mouse move on picture
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox pic = (PictureBox)sender;
                Point mousePos = PointToClient(Control.MousePosition);
                Point newLocationOffset = (Point)pic.Tag;
                pic.Location = new Point(mousePos.X - newLocationOffset.X, mousePos.Y - newLocationOffset.Y);
            }
        }

        // Handle mouse up on picture
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox pic = (PictureBox)sender;
                pic.Tag = null;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                // Move the player left (decrease X coordinate).
                player.Location = new Point(player.Location.X - 5, player.Location.Y);
            }
            else if (e.KeyCode == Keys.D)
            {
                // Move the player right (increase X coordinate).
                player.Location = new Point(player.Location.X + 5, player.Location.Y);
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Z)
            {
                player.Location = new Point(player.Location.X, player.Location.Y - 50);
            }
        }
    }
}