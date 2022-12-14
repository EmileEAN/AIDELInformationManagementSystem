using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinForms.UI
{
    public static class ImageDisplayer
    {
        private static readonly Size m_margin = new Size { Width = 5, Height = 5 };

        public static void Display(Stream _stream)
        {
            Image image = Image.FromStream(_stream);
            Display(image);
        }
        public static void Display(Image _image)
        {
            using (Form form = new Form())
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = _image.Size + m_margin;

                PictureBox pb = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    Image = _image
                };

                form.Controls.Add(pb);
                form.ShowDialog();
            }
        }
    }
}
