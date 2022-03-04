using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GotOver
{
    public partial class Form1 : Form
    {
        public int LayerCount = 5;
        public int NeuronsPerLayer = 10;
        [DllImport("User32.dll")]
        public static extern long SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hwnd, StringBuilder ss, int count);
        private string ActiveWindowTitle()
        {
            const int nChar = 256;
            StringBuilder ss = new StringBuilder(nChar);
            IntPtr handle = IntPtr.Zero;
            handle = GetForegroundWindow();
            if (GetWindowText(handle, ss, nChar) > 0) return ss.ToString();
            else return "";
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public class Neuron
        {
            public int layer;
            public float val;
            public float weight;
            Random rand = new Random();
            public Neuron(int layerNum)
            {
                val = 0;
                weight = (float)rand.NextDouble();
                layer = layerNum;
            }
            public void Activation()
            {

            }
        }
        public List<List<Neuron>> Layers = new List<List<Neuron>>();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Task work = new Task(() => Bot());
            work.Start();
            SetupLayers();
        }
        public void SetupLayers()
        {
            for (int i = 0; i < LayerCount; i++)
            {
                List<Neuron> temp = new List<Neuron>();
                if (i == 0)
                {
                    for (int j = 0; j < 9216; j++)
                    {
                        temp.Add(new Neuron(Layers.Count()));
                    }

                    Layers.Add(temp);
                }
                else if(i == LayerCount - 1)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        temp.Add(new Neuron(Layers.Count()));
                    }

                    Layers.Add(temp);
                }
                else
                {
                    for (int j = 0; j < NeuronsPerLayer; j++)
                    {
                        temp.Add(new Neuron(Layers.Count()));
                    }

                    Layers.Add(temp);
                }
            }
        }
        public float[] AISays(Bitmap Small)
        {
            float[] result = new float[2];



            return result;
        }
        public void Bot()
        {
            Bitmap Monitor = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap Smaller = new Bitmap(128, 72);
            Graphics g = Graphics.FromImage(Monitor);
            while(true)
            {
                if (ActiveWindowTitle() == "Getting Over It")
                {
                    g.CopyFromScreen(0, 0, 0, 0, Monitor.Size);
                    Smaller = ResizeImage(Monitor, 128, 72);
                    pictureBox1.Image = Smaller;
                    float[] ret = AISays(Smaller);
                    SetCursorPos((int)Math.Ceiling(Screen.PrimaryScreen.Bounds.Width * ret[0]), (int)Math.Ceiling(Screen.PrimaryScreen.Bounds.Height * ret[1]));
                    Thread.Sleep(20);
                }
            }
        }
    }
}
