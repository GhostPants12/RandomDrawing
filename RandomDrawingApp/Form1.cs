using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomDrawingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int imageWidth;
            int imageHeight;
            if(!int.TryParse(this.width.Text, out imageWidth) || !int.TryParse(this.height.Text, out imageHeight))
            {
                MessageBox.Show("Wrong resolution format (not a number).", "Error", MessageBoxButtons.OK);
                return;
            }

            using(var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                using(var bitmap = new Bitmap(imageWidth, imageHeight))
                {
                    if (!this.Message.Text.Equals(string.Empty))
                    {
                        DrawMessageBitmap(this.Message.Text, bitmap);
                    }
                    else 
                    {
                        DrawRandomBitmap(bitmap);
                    }

                    if(result == DialogResult.OK)
                    {
                        bitmap.Save(@fbd.SelectedPath + "\\random.png", ImageFormat.Png);
                    }
                }
            }
        }

        private void DrawRandomBitmap(Bitmap bitmap)
        {
            Brush[] brushArr = { Brushes.Yellow, Brushes.Blue, Brushes.Green, Brushes.Red};
            Random rnd = new Random();
            int buf;
            using(Graphics g = Graphics.FromImage(bitmap))
            {
                for(int x = 0; x<bitmap.Width; x++)
                {
                    for(int y =0; y<bitmap.Height;y++)
                    {
                        buf = rnd.Next(0,4);
                        g.FillRectangle(brushArr[buf], x, y, 1, 1);
                    }
                }
            }
        }

        private void DrawMessageBitmap(string message, Bitmap bitmap)
        {
            if(bitmap.Height*bitmap.Width < message.Length)
            {
                MessageBox.Show("Resolution is too low.", "Error", MessageBoxButtons.OK);
                return;
            }

            int index = 0;
            List<SolidBrush> brushes = new List<SolidBrush>();
            Random rnd = new Random();
            int randomResult, unicodeIndex, modPart, divPart;
            foreach (char character in message)
            {
                randomResult = rnd.Next(0,255);
                unicodeIndex = character;
                modPart = unicodeIndex % 255;
                divPart = (unicodeIndex/255)%255;
                if(divPart == 0)
                {
                    divPart = Math.Abs(randomResult-modPart);
                }
                if(randomResult<127)
                {
                    brushes.Add(new SolidBrush(Color.FromArgb(modPart, divPart, randomResult)));
                }
                else
                {
                    brushes.Add(new SolidBrush(Color.FromArgb(divPart, modPart, randomResult)));
                }
            }

            using(Graphics g = Graphics.FromImage(bitmap))
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    for(int y = 0; y < bitmap.Height; y++)
                    {
                        if(index < brushes.Count)
                        {
                            g.FillRectangle(brushes[index], x, y, 1, 1);
                            index++;
                        }
                        else
                        {
                            g.FillRectangle(Brushes.White, x, y, 1, 1);
                            index=0;
                        }
                    }
                }
            }
        }
    }
}
