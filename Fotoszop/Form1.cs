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

namespace Fotoszop
{
    public partial class Form1 : Form
    {
        Image file;
        Bitmap newBitmap1;
        Bitmap newBitmap2;
        Bitmap newBitmap3;
        Bitmap newBitmap4;
        Bitmap bitmapWyr;
        float contrast = 1;
        float gamma = 1;
        Boolean czyJestZdjecie1 = false;
        Boolean czyJestZdjecie2 = false;
        Boolean czyJestZdjecie3 = false;
        Boolean robHistogram = false;
        Boolean wyrownanieHistogramu = false;
        int[] tabRed = new int[256];
        int[] tabGreen = new int[256];
        int[] tabBlue = new int[256];
        int[] newRed = new int[256];
        int[] newGreen = new int[256];
        int[] newBlue = new int[256];
        int[] histCaly = new int[256];
        int[] histKum = new int[256];
        int[] histKumRed = new int[256];
        int[] histKumGreen = new int[256];
        int[] histKumBlue = new int[256];





        public Form1()
        {
            InitializeComponent();
        }
        public int MyProperty { get; set; }

        private void buttonPicture1_Click(object sender, EventArgs e)
        {
            String imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    pictureBox1.ImageLocation = imageLocation;
                    newBitmap1 = new Bitmap(imageLocation);
                    czyJestZdjecie1 = true;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonPicture2_Click(object sender, EventArgs e)
        {
            String imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All files(*.*)|*.*";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    pictureBox2.ImageLocation = imageLocation;
                    newBitmap2 = new Bitmap(imageLocation);
                    pictureBox4.ImageLocation = imageLocation;
                    newBitmap4 = new Bitmap(imageLocation);
                    czyJestZdjecie2 = true;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonPicture3_Click(object sender, EventArgs e)
        {
            String imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    pictureBox3.ImageLocation = imageLocation;
                    newBitmap3 = new Bitmap(imageLocation);
                    czyJestZdjecie3 = true;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd!", "Wybierz plik z rozszerzeniem np. jpg", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            if (!czyJestZdjecie1)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                labelBright.Text = trackBarBrightness.Value.ToString();

                pictureBox1.Image = AdjustBrightness(newBitmap1, trackBarBrightness.Value);
            }

        }

        public static Bitmap AdjustBrightness(Bitmap Image, int Value)
        {
            Bitmap TempBitmap = Image;
            float FinalValue = (float)Value / 255.0f;
            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix =
            {
                new float[] {1,0,0,0,0},

                new float[] {0,1,0,0,0},

                new float[] {0,0,1,0,0},

                new float[] {0,0,0,1,0},

                new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
            };
            System.Drawing.Imaging.ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);
            System.Drawing.Imaging.ImageAttributes Attributes = new ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);
            Attributes.Dispose();
            NewGraphics.Dispose();
            return NewBitmap;
        }

        private void buttonGrey_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie1)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for (int i = 0; i < newBitmap1.Width; i++)
                {
                    for (int j = 0; j < newBitmap1.Height; j++)
                    {
                        Color orginalColor = newBitmap1.GetPixel(i, j);
                        int greyScale = (int)((orginalColor.R * .3) + (orginalColor.G * .59) + (orginalColor.B * .11));

                        Color newColor = Color.FromArgb(greyScale, greyScale, greyScale);

                        newBitmap1.SetPixel(i, j, newColor);
                    }
                }
                pictureBox1.Image = newBitmap1;

            }

        }

        private void labelContr_Click(object sender, EventArgs e)
        {
            labelContr.Text = trackBarBrightness.Value.ToString();

        }

        private void trackBarContr_Scroll(object sender, EventArgs e)
        {
            if (!czyJestZdjecie1)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                labelContr.Text = trackBarContr.Value.ToString();

                contrast = 0.04f * trackBarContr.Value;

                Bitmap bm = new Bitmap(newBitmap1.Width, newBitmap1.Height);

                Graphics g = Graphics.FromImage(bm);
                ImageAttributes ia = new ImageAttributes();

                ColorMatrix cm = new ColorMatrix(new float[][]
                {
                new float []{contrast,0f,0f,0f,0f},
                new float []{0f,contrast,0f,0f,0f},
                new float []{0f,0f,contrast,0f,0f},
                new float []{0f,0f,0f,1f,0f},
                new float []{0.001f,0.001f,0.001f,0f,1f},
                });
                ia.SetColorMatrix(cm);

                g.DrawImage(newBitmap1, new Rectangle(0, 0, newBitmap1.Width, newBitmap1.Height), 0, 0, newBitmap1.Width, newBitmap1.Height, GraphicsUnit.Pixel, ia);
                g.Dispose();
                ia.Dispose();
                pictureBox1.Image = bm;
            }


        }

        private void buttonInvert_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie1)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for (int i = 0; i < newBitmap1.Width; i++)
                {
                    for (int j = 0; j < newBitmap1.Height; j++)
                    {
                        Color pixel = newBitmap1.GetPixel(i, j);
                        int red = pixel.R;
                        int green = pixel.G;
                        int blue = pixel.B;

                        newBitmap1.SetPixel(i, j, Color.FromArgb(255 - red, 255 - green, 255 - blue));
                    }
                }
                pictureBox1.Image = newBitmap1;
            }
        }

        private void buttonSuma_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Suma(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }

            }


        }
        private Color Suma(Color x, Color y)
        {
            int newRed = (int)(x.R + y.R);
            if (newRed > 255)
                newRed = 255;

            int newGreen = (int)(x.G + y.G);
            if (newGreen > 255)
                newGreen = 255;

            int newBlue = (int)(x.B + y.B);
            if (newBlue > 255)
                newBlue = 255;

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonRoznica_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Roznica(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Roznica(Color x, Color y)
        {
            int newRed = Math.Abs(x.R - y.R);
            if (newRed < 0)
                newRed = 0;

            int newGreen = Math.Abs(x.G - y.G);
            if (newGreen < 0)
                newGreen = 0;

            int newBlue = Math.Abs(x.B - y.B);
            if (newBlue < 0)
                newBlue = 0;

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonOdejmowanie_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Odejmowanie(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }

        private Color Odejmowanie(Color x, Color y)
        {
            int newRed = Math.Abs(x.R + y.R - 255);
            if (newRed < 0)
                newRed = 0;

            int newGreen = Math.Abs(x.G + y.G - 255);
            if (newGreen < 0)
                newGreen = 0;

            int newBlue = Math.Abs(x.B + y.B - 255);
            if (newBlue < 0)
                newBlue = 0;

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonMnozenieOdw_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, MnozOdwr(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color MnozOdwr(Color x, Color y)
        {
            int newRed = (int)(255 - ((255 - x.R) * (255 - y.R)) / 255);
            if (newRed < 0)
                newRed = 0;
            else if (newRed > 255)
                newRed = 255;

            int newGreen = (int)(255 - ((255 - x.G) * (255 - y.G)) / 255);
            if (newGreen < 0)
                newGreen = 0;
            else if (newGreen > 255)
                newGreen = 255;

            int newBlue = (int)(255 - ((255 - x.B) * (255 - y.B)) / 255);
            if (newBlue < 0)
                newBlue = 0;
            else if (newBlue > 255)
                newBlue = 255;

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonMnozenie_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3  || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Mnozenie(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Mnozenie(Color x, Color y)
        {
            int newRed = x.R * y.R / 255;

            int newGreen = x.G * y.G / 255;

            int newBlue = x.B * y.B / 255;

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonNegacja_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Negacja(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }      
        }
        private Color Negacja(Color x, Color y)
        {
            int newRed = 255 - Math.Abs(255 - x.R - y.R);

            int newGreen = 255 - Math.Abs(255 - x.G - y.G);

            int newBlue = 255 - Math.Abs(255 - x.B - y.B);

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonCiemniejszy_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Ciemniej(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Ciemniej(Color x, Color y)
        {
            int newRed = 0;
            int newGreen = 0;
            int newBlue = 0;
            if (x.R < y.R)
                newRed = x.R;
            else
                newRed = y.R;

            if (x.G < y.G)
                newGreen = x.G;
            else
                newGreen = y.G;

            if (x.B < y.B)
                newBlue = x.B;
            else
                newBlue = y.B;

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonJasniejszy_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Jasniej(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Jasniej(Color x, Color y)
        {
            int newRed = 0;
            int newGreen = 0;
            int newBlue = 0;
            if (x.R > y.R)
                newRed = x.R;
            else
                newRed = y.R;

            if (x.G > y.G)
                newGreen = x.G;
            else
                newGreen = y.G;

            if (x.B > y.B)
                newBlue = x.B;
            else
                newBlue = y.B;

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonWylaczenie_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Wylacz(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Wylacz(Color x, Color y)
        {
            int newRed = x.R + y.R - 2 * (x.R * y.R) / 255;
            if (newRed < 0)
                newRed = 0;
            else if (newRed > 255)
                newRed = 255;

            int newGreen = x.G + y.G - 2 * (x.G * y.G) / 255;
            if (newGreen < 0)
                newGreen = 0;
            else if (newGreen > 255)
                newGreen = 255;

            int newBlue = x.B + y.B - 2 * (x.B * y.B) / 255;
            if (newBlue < 0)
                newBlue = 0;
            else if (newBlue > 255)
                newBlue = 255;

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonNakładka_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Nakladka(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Nakladka(Color x, Color y)
        {
            int newRed = 0;
            if (x.R < (int)(255 / 2))
                newRed = 2 * y.R * x.R / 255;
            else
                newRed = 255 - 2 * ((255 - y.R) * (255 - x.R) / 255);

            int newGreen = 0;
            if (x.G < (int)(255 / 2))
                newGreen = 2 * y.G * x.G / 255;
            else
                newGreen = 255 - 2 * ((255 - y.G) * (255 - x.G) / 255);

            int newBlue = 0;
            if (x.B < (int)(255 / 2))
                newBlue = 2 * y.B * x.B / 255;
            else
                newBlue = 255 - 2 * ((255 - y.B) * (255 - x.B) / 255);

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonOstry_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Ostre(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Ostre(Color x, Color y)
        {
            int newRed = 0;
            if (y.R < (int)(255 / 2))
                newRed = 2 * y.R * x.R / 255;
            else
                newRed = 255 - 2 * ((255 - y.R) * (255 - x.R) / 255);

            int newGreen = 0;
            if (y.G < (int)(255 / 2))
                newGreen = 2 * y.G * x.G / 255;
            else
                newGreen = 255 - 2 * ((255 - y.G) * (255 - x.G) / 255);

            int newBlue = 0;
            if (y.B < (int)(255 / 2))
                newBlue = 2 * y.B * x.B / 255;
            else
                newBlue = 255 - 2 * ((255 - y.B) * (255 - x.B) / 255);

            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        private void buttonLagodny_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Lagodne(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Lagodne(Color x, Color y)
        {
            double red1 = (double)x.R / 255;
            double green1 = (double)x.G / 255;
            double blue1 = (double)x.B / 255;

            double red2 = (double)y.R / 255;
            double green2 = (double)y.B / 255;
            double blue2 = (double)y.G / 255;

            double red3 = 0;
            if (red2 < 0.5)
            {
                red3 = 2 * red1 * red2 + ((Math.Pow(red1, 2) * (1 - 2 * red2)));
            }
            else
            {
                red3 = (Math.Sqrt(red1) * (2 * red2 - 1)) + ((2 * red1) * (1 - red2));
            }
            red3 = red3 * 255;

            double green3 = 0;
            if (green2 < 0.5)
            {
                green3 = 2 * green1 * green2 + ((Math.Pow(green1, 2) * (1 - 2 * green2)));
            }
            else
            {
                green3 = (Math.Sqrt(green1) * (2 * green2 - 1)) + ((2 * green1) * (1 - green2));
            }
            green3 = green3 * 255;

            double blue3 = 0;
            if (blue2 < 0.5)
            {
                blue3 = 2 * blue1 * blue2 + ((Math.Pow(blue1, 2) * (1 - 2 * blue2)));
            }
            else
            {
                blue3 = (Math.Sqrt(blue1) * (2 * blue2 - 1)) + ((2 * blue1) * (1 - blue2));
            }
            blue3 = blue3 * 255;


            Color newColor = Color.FromArgb((int)red3, (int)green3, (int)blue3);

            return newColor;

        }

        private void buttonRozcienczenie_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Rozciencz(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Rozciencz(Color x, Color y)
        {
            double red1 = (double)x.R / 255;
            double green1 = (double)x.G / 255;
            double blue1 = (double)x.B / 255;

            double red2 = (double)y.R / 255;
            double green2 = (double)y.B / 255;
            double blue2 = (double)y.G / 255;


            double red3 = 0;

            if (red2 == 1)
                red2 = 0.0039;
            red3 = (red1 / (1 - red2)) * 255;
            if (red3 > 255)
                red3 = 255;
            else if (red3 < 0)
                red3 = 0;

            double green3 = 0;

            if (green2 == 1)
                green2 = 0.0039;
            green3 = (green1 / (1 - green2)) * 255;
            if (green3 > 255)
                green3 = 255;
            else if (green3 < 0)
                green3 = 0;

            double blue3 = 0;

            if (blue2 == 1)
                blue2 = 0.0039;
            blue3 = (blue1 / (1 - blue2)) * 255;
            if (blue3 > 255)
                blue3 = 255;
            else if (blue3 < 0)
                blue3 = 0;



            Color newColor = Color.FromArgb((int)red3, (int)green3, (int)blue3);

            return newColor;

        }

        private void buttonWypalanie_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Wypalanie(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Wypalanie(Color x, Color y)
        {
            double red1 = (double)x.R / 255;
            double green1 = (double)x.G / 255;
            double blue1 = (double)x.B / 255;

            double red2 = (double)y.R / 255;
            double green2 = (double)y.B / 255;
            double blue2 = (double)y.G / 255;


            double red3 = 0;

            if (red2 == 0)
                red2 = 0.0039;
            red3 = (1 - (1 - red1) / red2) * 255;

            if (red3 > 255)
                red3 = 255;
            else if (red3 < 0)
                red3 = 1;

            double green3 = 0;

            if (green2 == 0)
                green2 = 0.0039;
            green3 = (1 - (1 - green1) / green2) * 255;

            if (green3 > 255)
                green3 = 255;
            else if (green3 < 0)
                green3 = 1;

            double blue3 = 0;

            if (blue2 == 0)
                blue2 = 0.0039;
            blue3 = (1 - (1 - blue1) / blue2) * 255;
            if (blue3 > 255)
                blue3 = 255;
            else if (blue3 < 0)
                blue3 = 1;




            Color newColor = Color.FromArgb((int)red3, (int)green3, (int)blue3);

            return newColor;

        }

        private void buttonReflect_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        newBitmap4.SetPixel(x, y, Reflect(newBitmap2.GetPixel(x, y), newBitmap3.GetPixel(x, y)));
                        pictureBox4.Image = newBitmap4;
                    }
                }
            }
        }
        private Color Reflect(Color x, Color y)
        {
            double red1 = (double)x.R / 255;
            double green1 = (double)x.G / 255;
            double blue1 = (double)x.B / 255;

            double red2 = (double)y.R / 255;
            double green2 = (double)y.B / 255;
            double blue2 = (double)y.G / 255;


            double red3 = 0;

            if (red2 == 0)
                red2 = 0.0039;
            red3 = ((red1 * red1) / (1 - red2)) * 255;

            if (red3 > 255)
                red3 = 255;
            else if (red3 < 0)
                red3 = 1;

            double green3 = 0;

            if (green2 == 0)
                green2 = 0.0039;
            green3 = ((green1 * green1) / (1 - green2)) * 255;

            if (green3 > 255)
                green3 = 255;
            else if (green3 < 0)
                green3 = 1;

            double blue3 = 0;

            if (blue2 == 0)
                blue2 = 0.0039;
            blue3 = ((blue1 * blue1) / (1 - blue2)) * 255;
            if (blue3 > 255)
                blue3 = 255;
            else if (blue3 < 0)
                blue3 = 1;

            Color newColor = Color.FromArgb((int)red3, (int)green3, (int)blue3);

            return newColor;

        }

        private void buttonPrzezroczystosc_Click(object sender, EventArgs e)
        {

        }

        private void trackBarOpacity_Scroll(object sender, EventArgs e)
        {
            if (!czyJestZdjecie3 || !czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                labelOpacity.Text = trackBarOpacity.Value.ToString();
                int w = 0;
                int h = 0;

                if (newBitmap4.Width > newBitmap3.Width)
                    w = newBitmap3.Width;
                else
                    w = newBitmap4.Width;

                if (newBitmap4.Height > newBitmap3.Height)
                    h = newBitmap3.Height;
                else
                    h = newBitmap4.Height;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        Color cA = newBitmap3.GetPixel(x, y);
                        Color cB = newBitmap4.GetPixel(x, y);

                        double red1 = (double)cA.R / 255;
                        double green1 = (double)cA.G / 255;
                        double blue1 = (double)cA.B / 255;

                        double red2 = (double)cB.R / 255;
                        double green2 = (double)cB.G / 255;
                        double blue2 = (double)cB.B / 255;

                        double red3 = ((1 - (double)(trackBarOpacity.Value) / 10) * red2) + (((double)(trackBarOpacity.Value) / 10) * red1);
                        double green3 = ((1 - (double)(trackBarOpacity.Value) / 10) * green2) + (((double)(trackBarOpacity.Value) / 10) * green1);
                        double blue3 = ((1 - (double)(trackBarOpacity.Value) / 10) * blue2) + (((double)(trackBarOpacity.Value) / 10) * blue1);

                        red3 = red3 * 255;
                        green3 = green3 * 255;
                        blue3 = blue3 * 255;

                        if (red3 > 255)
                            red3 = 255;
                        else if (red3 < 0)
                            red3 = 0;
                        if (green3 > 255)
                            green3 = 255;
                        else if (green3 < 0)
                            green3 = 0;
                        if (blue3 > 255)
                            blue3 = 255;
                        else if (blue3 < 0)
                            blue3 = 0;

                        Color cC = Color.FromArgb(((int)red3), ((int)green3), ((int)blue3));
                        newBitmap4.SetPixel(x, y, cC);
                    }
                }

                pictureBox4.Image = newBitmap4;
            }
        }

        private void buttonSavePic1_Click(object sender, EventArgs e)
        {
            if (!czyJestZdjecie1)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "ImageFiles(*.jpg, *.png, *.tiff, *.bmp, *.gif) | *.jpg; *.png; *.tiff; *.bmp; *.gif";
                saveFile.Title = "Save an image";
                saveFile.AddExtension = true;
                saveFile.DefaultExt = "jpeg";
                saveFile.RestoreDirectory = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string fName = saveFile.FileName;
                    pictureBox1.Image.Save(fName, ImageFormat.Bmp);
                }
            }
        }
    
        private void trackBarGamma_Scroll(object sender, EventArgs e)
        {

            if (!czyJestZdjecie1)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                labelGamma.Text = trackBarGamma.Value.ToString();

                gamma = 0.04f * trackBarGamma.Value;
                Bitmap bm = new Bitmap(newBitmap1.Width, newBitmap1.Height);

                Graphics g = Graphics.FromImage(bm);
                ImageAttributes ia = new ImageAttributes();
                ia.SetGamma(gamma);

                g.DrawImage(newBitmap1, new Rectangle(0, 0, newBitmap1.Width, newBitmap1.Height), 0, 0, newBitmap1.Width, newBitmap1.Height, GraphicsUnit.Pixel, ia);
                g.Dispose();
                ia.Dispose();
                pictureBox1.Image = bm;
            }




        }

        private void buttonSavePic2_Click(object sender, EventArgs e)
        {
            if(!czyJestZdjecie2)
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            else
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "ImageFiles(*.jpg, *.png, *.tiff, *.bmp, *.gif) | *.jpg; *.png; *.tiff; *.bmp; *.gif";
                saveFile.Title = "Save an image";
                saveFile.AddExtension = true;
                saveFile.DefaultExt = "jpeg";
                saveFile.RestoreDirectory = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string fName = saveFile.FileName;
                    pictureBox4.Image.Save(fName, ImageFormat.Jpeg);
                }

            }

            
        }

        private void Histogram()
        {
            if(czyJestZdjecie1)
            {
                Array.Clear(tabRed, 0, tabRed.Length);
                Array.Clear(tabGreen, 0, tabRed.Length);
                Array.Clear(tabBlue, 0, tabRed.Length);

                Color picColor;

                Pen red = new Pen(Color.Red);

                for (int x = 0; x < newBitmap1.Width; x++)
                {
                    for (int y = 0; y < newBitmap1.Height; y++)
                    {
                        picColor = newBitmap1.GetPixel(x, y);
                        tabRed[picColor.R]++;
                        tabGreen[picColor.G]++;
                        tabBlue[picColor.B]++;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    histCaly[i] = tabRed[i] + tabGreen[i] + tabBlue[i];
                }
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }        
        }

        private void panelRed_Paint(object sender, PaintEventArgs e)
        {
            if (czyJestZdjecie1 && robHistogram)
            {
                Graphics g = e.Graphics;
                int width = panelRed.Width;
                int height = panelRed.Height;

                for (int i = 0; i <= 255; i++)
                {
                    double r = tabRed[i];
                    r = r / (newBitmap1.Width * newBitmap1.Height);
                    r = r * 4000;
                    g.DrawLine(new Pen(Color.Red), i, height, i, height - (int)r);
                }
            }          
        }

        private void buttonHistogram_Click(object sender, EventArgs e)
        {
            robHistogram = true;
            Histogram();
            panelRed.Invalidate();
            panelGreen.Invalidate();
            panelBlue.Invalidate();                    
        }

        private void panelGreen_Paint(object sender, PaintEventArgs e)
        {
            if (czyJestZdjecie1)
            {
                Graphics g = e.Graphics;
                int width = panelGreen.Width;
                int height = panelGreen.Height;

                for (int i = 0; i <= 255; i++)
                {
                    double gr = tabGreen[i];
                    gr = gr / (newBitmap1.Width * newBitmap1.Height);
                    gr = gr * 4000;
                    g.DrawLine(new Pen(Color.Green), i, height, i, height - (int)gr);
                }
            }
        }

        private void panelBlue_Paint(object sender, PaintEventArgs e)
        {
            if (czyJestZdjecie1)
            {
                Graphics g = e.Graphics;
                int width = panelGreen.Width;
                int height = panelGreen.Height;

                for (int i = 0; i <= 255; i++)
                {
                    double b = tabBlue[i];
                    b = b/ (newBitmap1.Width * newBitmap1.Height);
                    b = b * 4000;
                    g.DrawLine(new Pen(Color.Blue), i, height, i, height - (int)b);
                }
            }
        }

        private void buttonMaska_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Bitmap b1 = (Bitmap)pictureBox1.Image;

            int[,] maska = new int[3, 3];

            maska[0, 0] = (int)numericUpDownMask1.Value;
            maska[0, 1] = (int)numericUpDownMask2.Value;
            maska[0, 2] = (int)numericUpDownMask3.Value;

            maska[1, 0] = (int)numericUpDownMask4.Value;
            maska[1, 1] = (int)numericUpDownMask5.Value;
            maska[1, 2] = (int)numericUpDownMask6.Value;

            maska[2, 0] = (int)numericUpDownMask7.Value;
            maska[2, 1] = (int)numericUpDownMask8.Value;
            maska[2, 2] = (int)numericUpDownMask9.Value;

            int norm = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    norm += maska[i, j];
                }
            }

            Color k;

            int R, G, B;

            for (int x = 1; x < newBitmap1.Width - 1; x++)
            {
                for (int y = 1; y < newBitmap1.Height - 1; y++)
                {
                    R = G = B = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            k = b1.GetPixel(x + i - 1, y + j - 1);
                            R += k.R * maska[i, j];
                            G += k.G * maska[i, j];
                            B += k.B * maska[i, j];
                        }
                    }
                    if (norm != 0)
                    {
                        R /= norm;
                        G /= norm;
                        B /= norm;
                    }

                    if (R < 0)
                        R = 0;
                    if (R > 255)
                        R = 255;

                    if (G < 0)
                        G = 0;
                    if (G > 255)
                        G = 255;

                    if (B < 0)
                        B = 0;
                    if (B > 255)
                        B = 255;

                    b1.SetPixel(x, y, Color.FromArgb(R, G, B));

                }
            }

            pictureBox1.Invalidate();
            Cursor = Cursors.Default;
        }

        private void buttonWyrownajHist_Click(object sender, EventArgs e)
        {
            if(robHistogram)
            {
                wyrownanieHistogramu = true;
                histogramKum();
                panelKulm.Invalidate();
                Array.Clear(tabRed, 0, tabRed.Length);
                Array.Clear(tabGreen, 0, tabGreen.Length);
                Array.Clear(tabBlue, 0, tabBlue.Length);
                wyrownajRed();
                wyrownajGreen();
                wyrownajBlue();
                panelRed.Invalidate();
                panelGreen.Invalidate();
                panelBlue.Invalidate();
            }
            else
            {
                MessageBox.Show("Nie załadowałeś histogramu", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }       
        }
        private void histogramKum()
        {
            histKum[0] = histCaly[0];
            for (int i = 1; i < histCaly.Length; i++)
            {
                histKum[i] = histCaly[i] + histKum[i - 1];
            }
            histKumRed[0] = tabRed[0];
            for (int i = 1; i < tabRed.Length; i++)
            {
                newRed[i] = tabRed[i] + newRed[i - 1];
            }
            histKumGreen[0] = tabGreen[0];
            for (int i = 1; i < tabGreen.Length; i++)
            {
                newGreen[i] = tabGreen[i] + newGreen[i - 1];
            }
            histKumBlue[0] = tabBlue[0];
            for (int i = 1; i < tabBlue.Length; i++)
            {
                newBlue[i] = tabBlue[i] + newBlue[i - 1];
            }
            wyrownajZdjecie();

        }
        private void wyrownajZdjecie()
        {
            double temp = 255 / (double)(newBitmap1.Width * newBitmap1.Height);       
            for (int x = 0; x < newBitmap1.Width; x++)
            {
                for (int y = 0; y < newBitmap1.Height; y++)
                {

                    Color k = newBitmap1.GetPixel(x, y);

                    int r = k.R;
                    int g = k.G;
                    int b = k.B;

                    double r1 = newRed[r] * 255;
                    double g1 = newGreen[g] * 255;
                    double b1 = newBlue[b] * 255;
                    double r2 = r1 / (newBitmap1.Width * newBitmap1.Height);
                    double g2 = g1 / (newBitmap1.Width * newBitmap1.Height);
                    double b2 = b1 / (newBitmap1.Width * newBitmap1.Height);

                    if (r2 > 255)
                    {
                        r2 = 255;
                    }
                    if (g2 > 255)
                    {
                        g2 = 255;
                    }
                    if (b2 > 255)
                    {
                        b2 = 255;
                    }
                    newBitmap1.SetPixel(x, y, Color.FromArgb((int)r2, (int)g2, (int)b2));
                }
            }
            pictureBox1.Image = newBitmap1;
        }
        private void wyrownajGreen()
        {
            Color k;
            double d = 0;
            double sum = 0;
            Array.Clear(newGreen, 0, newRed.Length);
            for (int x = 0; x < newBitmap1.Width; x++)
            {
                for (int y = 0; y < newBitmap1.Height; y++)
                {
                    k = newBitmap1.GetPixel(x, y);
                    tabGreen[k.G]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (newGreen[i] != 0)
                {
                    d = tabGreen[i];
                }
            }
            for (int i = 0; i < 256; i++)
            {
                sum += tabGreen[i];
                newGreen[i] = (int)(((sum - d) / ((newBitmap1.Width * newBitmap1.Height) - d)) * 255);
            }
        }
        private void wyrownajBlue()
        {
            Color k;
            double d = 0;
            double sum = 0;
            Array.Clear(newBlue, 0, newBlue.Length);
            for (int x = 0; x < newBitmap1.Width; x++)
            {
                for (int y = 0; y < newBitmap1.Height; y++)
                {
                    k = newBitmap1.GetPixel(x, y);
                    tabBlue[k.B]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (newBlue[i] != 0)
                {
                    d = tabBlue[i];
                }
            }
            for (int i = 0; i < 256; i++)
            {
                sum += tabBlue[i];
                newBlue[i] = (int)(((sum - d) / ((newBitmap1.Width * newBitmap1.Height) - d)) * 255);
            }
        }
        private void wyrownajRed()
        {
            Color k;
            double d = 0;
            double sum = 0;
            Array.Clear(newRed, 0, newRed.Length);
            for (int x = 0; x < newBitmap1.Width; x++)
            {
                for (int y = 0; y < newBitmap1.Height; y++)
                {
                    k = newBitmap1.GetPixel(x, y);
                    tabRed[k.R]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (newRed[i] != 0)
                {
                    d = tabRed[i];
                }
            }
            for (int i = 0; i < 256; i++)
            {
                sum += tabRed[i];
                newRed[i] = (int)(((sum - d) / ((newBitmap1.Width * newBitmap1.Height) - d)) * 255);
            }
        }

        private void panelKulm_Paint(object sender, PaintEventArgs e)
        {
            if(wyrownanieHistogramu)
            {
                int x = 0;
                Graphics g = e.Graphics;
                for (int i = 0; i < 256; i++)
                {
                    double b = histCaly[i];
                    b = b / (newBitmap1.Width * newBitmap1.Height);
                    b = b * 2200;
                    g.DrawLine(new Pen(Color.Black), x, panelKulm.Height, x, panelKulm.Height - (int)b);
                    x++;
                }              
            }         
        }

        private void buttonFiltr_Click(object sender, EventArgs e)
        {
            if(czyJestZdjecie1)
            {
                Rozmyj();
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void Rozmyj()
        {
            double[,] M = new double[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            Bitmap bitmapRozmyta;
            bitmapRozmyta = (Bitmap)newBitmap1.Clone();
            double r1 = 0;
            double g1 = 0;
            double b1 = 0;

            for (int x = 0; x < newBitmap1.Width; x++)
            {
                for (int y = 0; y < newBitmap1.Height; y++)
                {
                    r1 = 0;
                    g1 = 0;
                    b1 = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            int y1 = y + k;
                            int x1 = x + l;
                            
                            if (y1 < 0)
                                y1 = 0;

                            if (y1 >= newBitmap1.Height)
                                y1 = newBitmap1.Height - 1;
                            
                            if (x1 < 0)
                                x1 = 0;
                           
                            if (x1 >= newBitmap1.Width)
                                x1 = newBitmap1.Width - 1;
                            
                            Color p = bitmapRozmyta.GetPixel(x1, y1);
                            double r2 = (double)p.R;
                            double g2 = (double)p.G;
                            double b2 = (double)p.B;
                            r1 += r2 * M[k + 1, l + 1];
                            g1 += g2 * M[k + 1, l + 1];
                            b1 += b2 * M[k + 1, l + 1];
                        }
                    }

                    double r3 = r1 / 9;
                    double g3 = g1 / 9;
                    double b3 = b1 / 9;

                    if (r3 > 255)
                    {
                        r3 = 255;
                    }
                    if (g3 > 255)
                    {
                        g3 = 255;
                    }
                    if (b3 > 255)
                    {
                        b3 = 255;
                    }

                    newBitmap1.SetPixel(x, y, Color.FromArgb((int)r3, (int)g3, (int)b3));
                }
            }
            pictureBox1.Image = newBitmap1;

        }

        private void buttonRobertsPion_Click(object sender, EventArgs e)
        {
            if (czyJestZdjecie1)
            {
                int[,] maska = new int[3, 3];
                Array.Clear(maska, 0, maska.Length);

                maska[0, 0] = 0;
                maska[0, 1] = 0;
                maska[0, 2] = 0;

                maska[1, 0] = 0;
                maska[1, 1] = 1;
                maska[1, 2] = -1;

                maska[2, 0] = 0;
                maska[2, 1] = 0;
                maska[2, 2] = 0;

                wykonajFiltr(maska);
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void wykonajFiltr(int [,] maska)
        {
            Bitmap b1 = (Bitmap)pictureBox1.Image;
            int norm = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    norm += maska[i, j];
                }
            }
            Color k;
            int R, G, B;

            for (int x = 1; x < newBitmap1.Width - 1; x++)
            {
                for (int y = 1; y < newBitmap1.Height - 1; y++)
                {
                    R = G = B = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            k = newBitmap1.GetPixel(x + i - 1, y + j - 1);
                            R += k.R * maska[i, j];
                            G += k.G * maska[i, j];
                            B += k.B * maska[i, j];
                        }
                    }
                    if (norm != 0)
                    {
                        R /= norm;
                        G /= norm;
                        B /= norm;
                    }

                    if (R < 0)
                        R = 0;
                    if (R > 255)
                        R = 255;

                    if (G < 0)
                        G = 0;
                    if (G > 255)
                        G = 255;

                    if (B < 0)
                        B = 0;
                    if (B > 255)
                        B = 255;

                    b1.SetPixel(x, y, Color.FromArgb(R, G, B));

                }
            }
            pictureBox1.Image = b1;

            pictureBox1.Invalidate();
            Cursor = Cursors.Default;

        }

        private void buttonRobertsPoz_Click(object sender, EventArgs e)
        {
            if (czyJestZdjecie1)
            {
                int[,] maska = new int[3, 3];
                Array.Clear(maska, 0, maska.Length);

                maska[0, 0] = 0;
                maska[0, 1] = 0;
                maska[0, 2] = 0;

                maska[1, 0] = 0;
                maska[1, 1] = 1;
                maska[1, 2] = 0;

                maska[2, 0] = 0;
                maska[2, 1] = -1;
                maska[2, 2] = 0;

                wykonajFiltr(maska);
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonPrewittPion_Click(object sender, EventArgs e)
        {
            if (czyJestZdjecie1)
            {
                int[,] maska = new int[3, 3];
                Array.Clear(maska, 0, maska.Length);

                maska[0, 0] = 1;
                maska[0, 1] = 1;
                maska[0, 2] = 1;

                maska[1, 0] = 0;
                maska[1, 1] = 0;
                maska[1, 2] = 0;

                maska[2, 0] = -1;
                maska[2, 1] = -1;
                maska[2, 2] = -1;

                wykonajFiltr(maska);
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonPrewittPoz_Click(object sender, EventArgs e)
        {
            if (czyJestZdjecie1)
            {
                int[,] maska = new int[3, 3];
                Array.Clear(maska, 0, maska.Length);

                maska[0, 0] = 1;
                maska[0, 1] = 0;
                maska[0, 2] = -1;

                maska[1, 0] = 1;
                maska[1, 1] = 0;
                maska[1, 2] = -1;

                maska[2, 0] = 1;
                maska[2, 1] = 0;
                maska[2, 2] = -1;

                wykonajFiltr(maska);
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSobelPion_Click(object sender, EventArgs e)
        {
            if (czyJestZdjecie1)
            {
                int[,] maska = new int[3, 3];
                Array.Clear(maska, 0, maska.Length);

                maska[0, 0] = 1;
                maska[0, 1] = 2;
                maska[0, 2] = 1;

                maska[1, 0] = 0;
                maska[1, 1] = 0;
                maska[1, 2] = 0;

                maska[2, 0] = -1;
                maska[2, 1] = -2;
                maska[2, 2] = -1;

                wykonajFiltr(maska);
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSobelPoz_Click(object sender, EventArgs e)
        {
            if (czyJestZdjecie1)
            {
                int[,] maska = new int[3, 3];
                Array.Clear(maska, 0, maska.Length);

                maska[0, 0] = 1;
                maska[0, 1] = 0;
                maska[0, 2] = -1;

                maska[1, 0] = 2;
                maska[1, 1] = 0;
                maska[1, 2] = -2;

                maska[2, 0] = 1;
                maska[2, 1] = 0;
                maska[2, 2] = -1;

                wykonajFiltr(maska);
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonLaPlace_Click(object sender, EventArgs e)
        {
            if (czyJestZdjecie1)
            {
                int[,] maska = new int[3, 3];
                Array.Clear(maska, 0, maska.Length);

                maska[0, 0] = 0;
                maska[0, 1] = -1;
                maska[0, 2] = 0;

                maska[1, 0] = -1;
                maska[1, 1] = 4;
                maska[1, 2] = -1;

                maska[2, 0] = 0;
                maska[2, 1] = -1;
                maska[2, 2] = 0;

                wykonajFiltr(maska);
            }
            else
            {
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonMin_Click(object sender, EventArgs e)
        {
            if(czyJestZdjecie1)
                filtrMin();
            else           
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void filtrMin()
        {
            Bitmap b1 = (Bitmap)pictureBox1.Image;
            Color kol;

            for (int x = 1; x < newBitmap1.Width - 1; x++)
            {
                for (int y = 1; y < newBitmap1.Height - 1; y++)
                {
                    List<int> r = new List<int>();
                    List<int> g = new List<int>();
                    List<int> b = new List<int>();
                    for (int i = x - 1; i < x + 1; i++)
                    {
                        for (int j = y - 1; j < y + 1; j++)
                        {
                            kol = newBitmap1.GetPixel(i, j);
                            r.Add(kol.R);
                            g.Add(kol.G);
                            b.Add(kol.B);
                        }
                    }
                    r.Sort();
                    g.Sort();
                    b.Sort();
                    b1.SetPixel(x, y, Color.FromArgb(r[0], g[0], b[0]));
                }
            }
            pictureBox1.Image = b1;
        }


        private void filtrMax()
        {
            Bitmap b1 = (Bitmap)pictureBox1.Image;
            Color kol;

            for (int x = 1; x < newBitmap1.Width - 1; x++)
            {
                for (int y = 1; y < newBitmap1.Height - 1; y++)
                {
                    List<int> r = new List<int>();
                    List<int> g = new List<int>();
                    List<int> b = new List<int>();
                    for (int i = x - 1; i < x + 1; i++)
                    {
                        for (int j = y - 1; j < y + 1; j++)
                        {
                            kol = newBitmap1.GetPixel(i, j);
                            r.Add(kol.R);
                            g.Add(kol.G);
                            b.Add(kol.B);                          
                        }
                    }
                    r.Sort();
                    g.Sort();
                    b.Sort();
                    r.Reverse();
                    g.Reverse();
                    b.Reverse();
                    b1.SetPixel(x, y, Color.FromArgb(r[0], g[0], b[0]));
                }
            }
            pictureBox1.Image = b1;
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            if(czyJestZdjecie1)
                filtrMax();
            else
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void buttonMedian_Click(object sender, EventArgs e)
        {
            if(czyJestZdjecie1)
                filtrMedian();
            else            
                MessageBox.Show("Nie załadowałeś zdjęcia", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void filtrMedian()
        {
            Bitmap b1 = (Bitmap)pictureBox1.Image;
            Color kol;

            for (int x = 1; x < newBitmap1.Width - 1; x++)
            {
                for (int y = 1; y < newBitmap1.Height - 1; y++)
                {
                    List<int> r = new List<int>();
                    List<int> g = new List<int>();
                    List<int> b = new List<int>();
                    for (int i = x - 1; i < x + 1; i++)
                    {
                        for (int j = y - 1; j < y + 1; j++)
                        {
                            kol = newBitmap1.GetPixel(i, j);
                            r.Add(kol.R);
                            g.Add(kol.G);
                            b.Add(kol.B);
                        }
                    }
                    int medR = Mediana(r);
                    int medG = Mediana(g);
                    int medB = Mediana(b);

                    if (medR > 255)
                        medR = 255;

                    if (medG > 255)
                        medG = 255;

                    if (medB > 255)
                        medB = 255;




                    b1.SetPixel(x, y, Color.FromArgb(medR, medG, medB));
                }
            }
            pictureBox1.Image = b1;
        }

        private int Mediana(List<int> lista)
        {
            int rozmiar = lista.Count();
            int polowa = lista.Count() / 2;
            int tmp = polowa - 1;

            var posortowanaLista = lista.OrderBy(n => n);
            double median;

            if ((rozmiar % 2) == 0)
                median = posortowanaLista.ElementAt(polowa) + (posortowanaLista.ElementAt(tmp) / 2);
            else
                median = posortowanaLista.ElementAt(polowa);
            return (int)median;
        }
    }
    

}

