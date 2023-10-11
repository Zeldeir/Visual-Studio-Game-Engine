using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEngine
{
    internal class Physics
    {
        public Physics()
        {

        }
        public static void gravity(PictureBox picture)
        {
            if (picture.Location.Y < 298)
            {
                picture.Location = new System.Drawing.Point(picture.Location.X, picture.Location.Y + 1);
            }
        }

    }
}