using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KontrastTesnenFilter
{
    public partial class Form1 : Form
    {
        Bitmap[] CuttedImage;

        static Stopwatch st;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Load("dog.jpg");
            Before.Load("dog.jpg");
            st = new Stopwatch();
            textBox10.Text = "2";

        }

        public Bitmap Filter(double[,] filt, Image im, int postitionInCuttedArray = 0)
        {
            int Width = 4 * im.Width;
            int Height = im.Height;
            Bitmap bmp = new Bitmap(im);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValue = new byte[bytes];
            Marshal.Copy(ptr, rgbValue, 0, bytes);
            byte[,] mas = new byte[Height, Width];

            int n = 0;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    mas[i, j] = rgbValue[n];
                    n++;
                }

            n = 0;
            int k, temp;
            for (int i = 1; i < Height - 1; i++)
                for (int j = 4; j < Width - 4; j++)
                {
                    if (n > 3)
                        n = 0;
                    else
                    {
                        k = i * Width + j;
                        temp = Convert.ToInt32(filt[0, 0] * mas[i - 1, j - 4] + filt[0, 1] * mas[i - 1, j] + filt[0, 2] * mas[i - 1, j + 4] + filt[1, 0] * mas[i, j - 4] + filt[1, 1] * mas[i, j] + filt[1, 2] * mas[i, j + 4] + filt[2, 0] * mas[i + 1, j - 4] + filt[2, 1] * mas[i + 1, j] + filt[2, 2] * mas[i + 1, j + 4]);
                        if (temp > 255)
                        { temp = 255; }
                        if (temp < 0)
                        { temp = 0; }
                        rgbValue[k] = (byte)temp;
                        n++;
                    }
                }

            Marshal.Copy(rgbValue, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            CuttedImage[postitionInCuttedArray] = bmp;
            return bmp;
        }

        public void Pruit()
        {
            double[,] filt = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            double[,] filtY = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            filt[0, 0] = -1;
            filt[0, 1] = -1;
            filt[0, 2] = -1;
            filt[1, 0] = 0;
            filt[1, 1] = 0;
            filt[1, 2] = 0;
            filt[2, 0] = 1;
            filt[2, 1] = 1;
            filt[2, 2] = 1;
            filtY[0, 0] = -1;
            filtY[0, 1] = 0;
            filtY[0, 2] = 1;
            filtY[1, 0] = -1;
            filtY[1, 1] = 0;
            filtY[1, 2] = 1;
            filtY[2, 0] = -1;
            filtY[2, 1] = 0;
            filtY[2, 2] = 1;

            int Width = 4 * pictureBox1.Image.Width;
            int Height = pictureBox1.Image.Height;
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(pictureBox1.Image);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            Rectangle rect2 = new Rectangle(0, 0, bmp2.Width, bmp2.Height);

            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            BitmapData bmpData2 = bmp2.LockBits(rect2, ImageLockMode.ReadWrite, bmp2.PixelFormat);


            IntPtr ptr = bmpData.Scan0;
            IntPtr ptr2 = bmpData2.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            int bytes2 = Math.Abs(bmpData2.Stride) * bmp2.Height;

            byte[] rgbValue = new byte[bytes];
            byte[] rgbValue2 = new byte[bytes2];

            Marshal.Copy(ptr, rgbValue, 0, bytes);
            Marshal.Copy(ptr2, rgbValue2, 0, bytes2);

            byte[,] mas = new byte[Height, Width];
            byte[,] mas2 = new byte[Height, Width];

            int n = 0;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    mas[i, j] = rgbValue[n];
                    mas2[i, j] = rgbValue2[n];
                    n++;
                }

            n = 0;
            int k, temp, temp2;
            for (int i = 1; i < Height - 1; i++)
                for (int j = 4; j < Width - 4; j++)
                {
                    if (n > 3)
                        n = 0;
                    else
                    {
                        k = i * Width + j;
                        temp = Convert.ToInt32(filt[0, 0] * mas[i - 1, j - 4] + filt[0, 1] * mas[i - 1, j] + filt[0, 2] * mas[i - 1, j + 4] + filt[1, 0] * mas[i, j - 4] + filt[1, 1] * mas[i, j] + filt[1, 2] * mas[i, j + 4] + filt[2, 0] * mas[i + 1, j - 4] + filt[2, 1] * mas[i + 1, j] + filt[2, 2] * mas[i + 1, j + 4]);
                        temp2 = Convert.ToInt32(filtY[0, 0] * mas2[i - 1, j - 4] + filtY[0, 1] * mas2[i - 1, j] + filtY[0, 2] * mas2[i - 1, j + 4] + filtY[1, 0] * mas2[i, j - 4] + filtY[1, 1] * mas2[i, j] + filtY[1, 2] * mas2[i, j + 4] + filtY[2, 0] * mas2[i + 1, j - 4] + filtY[2, 1] * mas2[i + 1, j] + filtY[2, 2] * mas2[i + 1, j + 4]);
                        if (temp > 255)
                        { temp = 255; }
                        if (temp < 0)
                        { temp = 0; }
                        rgbValue[k] = (byte)temp;
                        if (temp2 > 255)
                        { temp2 = 255; }
                        if (temp2 < 0)
                        { temp2 = 0; }
                        rgbValue2[k] = (byte)temp2;
                        n++;
                    }
                }

            for (int i = 0; i < rgbValue.Length; i++)
            {
                rgbValue[i] = (byte)Math.Sqrt(Math.Pow((double)rgbValue[i], 2) + Math.Pow((double)rgbValue2[i], 2));
            }


            Marshal.Copy(rgbValue, 0, ptr, bytes);
            Marshal.Copy(rgbValue2, 0, ptr2, bytes2);
            bmp.UnlockBits(bmpData);
            bmp2.UnlockBits(bmpData2);

            pictureBox1.Image = bmp;
        }

        public void Laplas()
        {

            double[,] filt = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            double[,] filtY = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            filt[0, 0] = 0;
            filt[0, 1] = 1;
            filt[0, 2] = 0;
            filt[1, 0] = 1;
            filt[1, 1] = -4;
            filt[1, 2] = 1;
            filt[2, 0] = 0;
            filt[2, 1] = 1;
            filt[2, 2] = 0;
            filtY[0, 0] = 1;
            filtY[0, 1] = 0;
            filtY[0, 2] = 1;
            filtY[1, 0] = 0;
            filtY[1, 1] = -4;
            filtY[1, 2] = 0;
            filtY[2, 0] = 1;
            filtY[2, 1] = 0;
            filtY[2, 2] = 1;
            int Width = 4 * pictureBox1.Image.Width;
            int Height = pictureBox1.Image.Height;
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(pictureBox1.Image);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            Rectangle rect2 = new Rectangle(0, 0, bmp2.Width, bmp2.Height);

            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            BitmapData bmpData2 = bmp2.LockBits(rect2, ImageLockMode.ReadWrite, bmp2.PixelFormat);


            IntPtr ptr = bmpData.Scan0;
            IntPtr ptr2 = bmpData2.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            int bytes2 = Math.Abs(bmpData2.Stride) * bmp2.Height;

            byte[] rgbValue = new byte[bytes];
            byte[] rgbValue2 = new byte[bytes2];

            Marshal.Copy(ptr, rgbValue, 0, bytes);
            Marshal.Copy(ptr2, rgbValue2, 0, bytes2);

            byte[,] mas = new byte[Height, Width];
            byte[,] mas2 = new byte[Height, Width];

            int n = 0;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    mas[i, j] = rgbValue[n];
                    mas2[i, j] = rgbValue2[n];
                    n++;
                }

            n = 0;
            int k, temp, temp2;
            for (int i = 1; i < Height - 1; i++)
                for (int j = 4; j < Width - 4; j++)
                {
                    if (n > 3)
                        n = 0;
                    else
                    {
                        k = i * Width + j;
                        temp = Convert.ToInt32(filt[0, 0] * mas[i - 1, j - 4] + filt[0, 1] * mas[i - 1, j] + filt[0, 2] * mas[i - 1, j + 4] + filt[1, 0] * mas[i, j - 4] + filt[1, 1] * mas[i, j] + filt[1, 2] * mas[i, j + 4] + filt[2, 0] * mas[i + 1, j - 4] + filt[2, 1] * mas[i + 1, j] + filt[2, 2] * mas[i + 1, j + 4]);
                        temp2 = Convert.ToInt32(filtY[0, 0] * mas2[i - 1, j - 4] + filtY[0, 1] * mas2[i - 1, j] + filtY[0, 2] * mas2[i - 1, j + 4] + filtY[1, 0] * mas2[i, j - 4] + filtY[1, 1] * mas2[i, j] + filtY[1, 2] * mas2[i, j + 4] + filtY[2, 0] * mas2[i + 1, j - 4] + filtY[2, 1] * mas2[i + 1, j] + filtY[2, 2] * mas2[i + 1, j + 4]);
                        if (temp > 255)
                        { temp = 255; }
                        if (temp < 0)
                        { temp = 0; }
                        rgbValue[k] = (byte)temp;
                        if (temp2 > 255)
                        { temp2 = 255; }
                        if (temp2 < 0)
                        { temp2 = 0; }
                        rgbValue2[k] = (byte)temp2;
                        n++;
                    }
                }

            for (int i = 0; i < rgbValue.Length; i++)
            {
                rgbValue[i] = (byte)Math.Sqrt(Math.Pow((double)rgbValue[i], 2) + Math.Pow((double)rgbValue2[i], 2));
            }


            Marshal.Copy(rgbValue, 0, ptr, bytes);
            Marshal.Copy(rgbValue2, 0, ptr2, bytes2);
            bmp.UnlockBits(bmpData);
            bmp2.UnlockBits(bmpData2);

            pictureBox1.Image = bmp;
        }

        public void TesnenFilter(double[,] filt)
        {

            int Width = 4 * pictureBox1.Image.Width;
            int Height = pictureBox1.Image.Height;

            Bitmap bmp = new Bitmap(pictureBox1.Image);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValue = new byte[bytes];
            Marshal.Copy(ptr, rgbValue, 0, bytes);
            byte[,] mas = new byte[Height, Width];

            int n = 0;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    mas[i, j] = rgbValue[n];
                    n++;
                }
            double avg = 0; n = 0;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    if (n < 3)
                    {
                        avg += (double)mas[i, j];
                        n++;
                    }
                    else
                    {
                        avg /= 3;
                        mas[i, j - 1] = (byte)avg;
                        mas[i, j - 2] = (byte)avg;
                        mas[i, j - 3] = (byte)avg;
                        avg = 0;
                        n = 0;
                    }
                }

            n = 0;
            int k;
            for (int i = 1; i < Height - 1; i++)
            {
                for (int j = 4; j < Width - 4; j++)
                {
                    if (n > 3)
                    { n = 0; }
                    {
                        double temp = 0;
                        k = i * Width + j;
                        temp = Convert.ToDouble(filt[0, 0] * mas[i - 1, j - 4] + filt[0, 1] * mas[i - 1, j] + filt[0, 2] * mas[i - 1, j + 4] + filt[1, 0] * mas[i, j - 4] + filt[1, 1] * mas[i, j] + filt[1, 2] * mas[i, j + 4] + filt[2, 0] * mas[i + 1, j - 4] + filt[2, 1] * mas[i + 1, j] + filt[2, 2] * mas[i + 1, j + 4]);
                        temp = temp + 128;
                        if (temp > 255) { temp = 255; }
                        if (temp < 0) { temp = 0; }
                        rgbValue[k] = (byte)temp;
                        n++;
                    }


                }
            }
            Marshal.Copy(rgbValue, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            pictureBox1.Image = bmp;
        }

        public void KuwaharaBlur(Bitmap Image, int Size)
        {

            System.Drawing.Bitmap TempBitmap = (Bitmap)Image.Clone();
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
            int[] ApetureMinX = { -(Size / 2), 0, -(Size / 2), 0 };
            int[] ApetureMaxX = { 0, (Size / 2), 0, (Size / 2) };
            int[] ApetureMinY = { -(Size / 2), -(Size / 2), 0, 0 };
            int[] ApetureMaxY = { 0, 0, (Size / 2), (Size / 2) };
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int[] RValues = { 0, 0, 0, 0 };
                    int[] GValues = { 0, 0, 0, 0 };
                    int[] BValues = { 0, 0, 0, 0 };
                    int[] NumPixels = { 0, 0, 0, 0 };
                    int[] MaxRValue = { 0, 0, 0, 0 };
                    int[] MaxGValue = { 0, 0, 0, 0 };
                    int[] MaxBValue = { 0, 0, 0, 0 };
                    int[] MinRValue = { 255, 255, 255, 255 };
                    int[] MinGValue = { 255, 255, 255, 255 };
                    int[] MinBValue = { 255, 255, 255, 255 };
                    for (int i = 0; i < 4; ++i)
                    {
                        for (int x2 = ApetureMinX[i]; x2 < ApetureMaxX[i]; ++x2)
                        {
                            int TempX = x + x2;
                            if (TempX >= 0 && TempX < NewBitmap.Width)
                            {
                                for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
                                {
                                    int TempY = y + y2;
                                    if (TempY >= 0 && TempY < NewBitmap.Height)
                                    {
                                        Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                        RValues[i] += TempColor.R;
                                        GValues[i] += TempColor.G;
                                        BValues[i] += TempColor.B;
                                        if (TempColor.R > MaxRValue[i])
                                        {
                                            MaxRValue[i] = TempColor.R;
                                        }
                                        else if (TempColor.R < MinRValue[i])
                                        {
                                            MinRValue[i] = TempColor.R;
                                        }

                                        if (TempColor.G > MaxGValue[i])
                                        {
                                            MaxGValue[i] = TempColor.G;
                                        }
                                        else if (TempColor.G < MinGValue[i])
                                        {
                                            MinGValue[i] = TempColor.G;
                                        }

                                        if (TempColor.B > MaxBValue[i])
                                        {
                                            MaxBValue[i] = TempColor.B;
                                        }
                                        else if (TempColor.B < MinBValue[i])
                                        {
                                            MinBValue[i] = TempColor.B;
                                        }
                                        ++NumPixels[i];
                                    }
                                }
                            }
                        }
                    }
                    int j = 0;
                    int MinDifference = 10000;
                    for (int i = 0; i < 4; ++i)
                    {
                        int CurrentDifference = (MaxRValue[i] - MinRValue[i]) + (MaxGValue[i] - MinGValue[i]) + (MaxBValue[i] - MinBValue[i]);
                        if (CurrentDifference < MinDifference && NumPixels[i] > 0)
                        {
                            j = i;
                            MinDifference = CurrentDifference;
                        }
                    }

                    Color MeanPixel = Color.FromArgb(RValues[j] / NumPixels[j],
                        GValues[j] / NumPixels[j],
                        BValues[j] / NumPixels[j]);
                    NewBitmap.SetPixel(x, y, MeanPixel);
                }
            }
            pictureBox1.Image = NewBitmap;
        }

        private void MedianFilter()
        {
            Bitmap original = new Bitmap(pictureBox1.Image);
            int width = original.Width;
            int height = original.Height;
            Bitmap edited = new Bitmap(width, height);
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    Color[] window = new Color[9];
                    int[] windowR = new int[9];
                    int[] windowG = new int[9];
                    int[] windowB = new int[9];
                    int count = 0;
                    for (int i = x - 1; i < x + 2; i++)
                    {
                        for (int j = y - 1; j < y + 2; j++)
                        {
                            window[count] = original.GetPixel(i, j);
                            windowR[count] = window[count].R;
                            windowG[count] = window[count].G;
                            windowB[count] = window[count].B;
                            count++;
                        }
                    }
                    Array.Sort(windowR);
                    Array.Sort(windowG);
                    Array.Sort(windowB);
                    int r = windowR[9 / 2];
                    int g = windowG[9 / 2];
                    int b = windowB[9 / 2];
                    Color color = Color.FromArgb(r, g, b);
                    edited.SetPixel(x, y, color);

                }
            }
            pictureBox1.Image = edited;
        }



        private void button1_Click(object sender, EventArgs e)
        {

            double[,] filter = new double[3, 3];
            int streamCount = int.Parse(textBox10.Text);

            if (radioButton1.Checked)//Гаусс
            {
                filter[0, 0] = 0.0625;
                filter[0, 1] = 0.125;
                filter[0, 2] = 0.0625;
                filter[1, 0] = 0.125;
                filter[1, 1] = 0.25;
                filter[1, 2] = 0.125;
                filter[2, 0] = 0.0625;
                filter[2, 1] = 0.125;
                filter[2, 2] = 0.0625;

                pictureBox1.Refresh();
              
                st.Start();
                Filter(filter, pictureBox1.Image);
                st.Stop();
                label3.Text = st.Elapsed.ToString();
                st.Reset();
                st.Start();

                //for (int i = 0; i < streamCount; i++) // ну типо потоки запускаем
                //{
                //    int t = i;
                //    (new Thread(new ThreadStart(()=>Filter(filter, CuttedImage[t], t)))).Start();
                //}

                Task[] TaskArray = new Task[streamCount];
                for (int i = 0; i < streamCount; i++)
                {
                    int t = i;
                    TaskArray[i] = new Task(() => Filter(filter, CuttedImage[t], t));
                    TaskArray[i].Start();
                }
                Task.WaitAll(TaskArray);
                st.Stop();

                label4.Text = st.Elapsed.ToString();
                st.Reset();
                pictureBox5.Image = mergePartImmage(CuttedImage);
            }
            if (radioButton2.Checked)//Тиснение
            {
                filter[0, 0] = 0;
                filter[0, 1] = 1;
                filter[0, 2] = 0;
                filter[1, 0] = 1;
                filter[1, 1] = 0;
                filter[1, 2] = -1;
                filter[2, 0] = 0;
                filter[2, 1] = -1;
                filter[2, 2] = 0;

                pictureBox1.Refresh();
                TesnenFilter(filter);
            }
            if (radioButton3.Checked)//Прюитт
            {
                pictureBox1.Refresh();
                Pruit();
            }
            if (radioButton4.Checked)
            {
                pictureBox1.Refresh();
                MedianFilter();
            }
            if (radioButton5.Checked)
            {
                filter[0, 0] = Convert.ToDouble(textBox1.Text);
                filter[0, 1] = Convert.ToDouble(textBox2.Text);
                filter[0, 2] = Convert.ToDouble(textBox3.Text);
                filter[1, 0] = Convert.ToDouble(textBox4.Text);
                filter[1, 1] = Convert.ToDouble(textBox5.Text);
                filter[1, 2] = Convert.ToDouble(textBox6.Text);
                filter[2, 0] = Convert.ToDouble(textBox7.Text);
                filter[2, 1] = Convert.ToDouble(textBox8.Text);
                filter[2, 2] = Convert.ToDouble(textBox9.Text);
                pictureBox1.Refresh();
                Filter(filter, pictureBox1.Image);

            }

            if (radioButton6.Checked)
            {
                pictureBox1.Refresh();
                KuwaharaBlur(new Bitmap(pictureBox1.Image), 5);
            }

            if (radioButton7.Checked)
            {
                pictureBox1.Refresh();
                filter[0, 0] = 0;
                filter[0, 1] = -1;
                filter[0, 2] = 0;
                filter[1, 0] = -1;
                filter[1, 1] = 5;
                filter[1, 2] = -1;
                filter[2, 0] = 0;
                filter[2, 1] = -1;
                filter[2, 2] = 0;
                Filter(filter, pictureBox1.Image);
            }
            if (radioButton8.Checked)
            {
                pictureBox1.Refresh();
                Laplas();
            }
            if (radioButton9.Checked)
            {

                pictureBox1.Refresh();
                MedianFilter();
                filter[0, 0] = 0;
                filter[0, 1] = -1;
                filter[0, 2] = 0;
                filter[1, 0] = -1;
                filter[1, 1] = 5;
                filter[1, 2] = -1;
                filter[2, 0] = 0;
                filter[2, 1] = -1;
                filter[2, 2] = 0;
                Filter(filter, pictureBox1.Image);
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            double[,] filter = new double[3, 3];
            filter[0, 0] = Convert.ToDouble(textBox1.Text);
            filter[0, 1] = Convert.ToDouble(textBox2.Text);
            filter[0, 2] = Convert.ToDouble(textBox3.Text);
            filter[1, 0] = Convert.ToDouble(textBox4.Text);
            filter[1, 1] = Convert.ToDouble(textBox5.Text);
            filter[1, 2] = Convert.ToDouble(textBox6.Text);
            filter[2, 0] = Convert.ToDouble(textBox7.Text);
            filter[2, 1] = Convert.ToDouble(textBox8.Text);
            filter[2, 2] = Convert.ToDouble(textBox9.Text);
            pictureBox1.Refresh();
            Filter(filter, pictureBox1.Image);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            pictureBox1.Image = (Bitmap)Before.Image.Clone();
        }



        public Rectangle[] generateRectangle(int count, int width, int heigth)
        {
            Rectangle[] rect = new Rectangle[count];

            double shift = heigth / count;

            int newHeigth = (int)Math.Floor(shift);

            for (int i = 0; i < count; i++)
            {
                rect[i] = new Rectangle(new Point(0, i * newHeigth), new Size(width, heigth / count));
            }

            return rect;
        }

        public Bitmap[] CutImage(Bitmap src, Rectangle[] rect)
        {
            Bitmap[] resBmp = new Bitmap[rect.Length];

            Graphics[] gr = new Graphics[rect.Length];

            for (int i = 0; i < rect.Length; i++)
            {
                resBmp[i] = new Bitmap(src.Width, src.Height);
                gr[i] = Graphics.FromImage(resBmp[i]);
            }
            for (int i = 0; i < rect.Length; i++)
            {
                gr[i].DrawImage(src, 0, 0, rect[i], GraphicsUnit.Pixel); //перерисовываем с источника по координатам
            }
            return resBmp;
        }

        public Image mergePartImmage(Bitmap[] cutImmage)
        {
            double shift = pictureBox1.Height / cutImmage.Length;

            int newHeigth = (int)Math.Floor(shift);
            // Создаем новый image нужного размера (это будет объединенный image)
            Image img = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // Делаем этот image нашим контекстом, куда будем рисовать
            Graphics g = Graphics.FromImage(img);
            // Create rectangle for region.
            Rectangle fillRect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            // Create solid brush.
            SolidBrush blueBrush = new SolidBrush(Color.Blue);
            // Create region for fill.
            Region fillRegion = new Region(fillRect);

          //  g.FillRegion(blueBrush, fillRegion);
            // рисуем существующие маленькие image в созданный нами большой image
            for (int i = 0; i < cutImmage.Length; i++)
            {
                g.DrawImage(cutImmage[i], new Point(0, i* newHeigth));
            }
            
            // Записываем обобщенный image в файл (или можно этот image назначить PictureBox... Неважно, он у нас уже есть)
            img.Save("output.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            return img;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Image temp = pictureBox1.Image;// берем картинку или Image.FromFile("D:\\123.png");
            Bitmap src = new Bitmap(temp, pictureBox1.Width, pictureBox1.Height);
            // Задаем нужную область вырезания (отсчет с верхнего левого угла)
            Rectangle[] r = generateRectangle(int.Parse(textBox10.Text), pictureBox1.Width, pictureBox1.Height);
            CuttedImage = CutImage(src, r);
            // результат изображение передаем на форму 
        /*    pictureBox2.Image = CuttedImage[0];
            pictureBox3.Image = CuttedImage[1];

            pictureBox4.Image = mergePartImmage(CuttedImage);*/
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
