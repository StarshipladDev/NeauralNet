using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuralNet;
namespace NeuralNet
{
    public partial class NeuralNetForm : Form
    {
        public NeuralNetForm()
        {
            InitializeComponent();
        }
        public void ClickFunction(object sender, EventArgs e)
        {

            DateTime startTime = DateTime.Now;
            String reply = funkyTextBox.Text;
            //TEST MAYBE IAMGES
            reply = "Maybe/Image" + (reply) + ".png";
            try
            {
                NeuralNet.TestData(reply, NeuralNet.testLayer,1-NeuralNet.highestDifferenceWhenRight);
            }
            catch (Exception z)
            {
                NeuralNet.DebugWrite("Main->Testing() ->", "Error while reading image", false);
            }
            //END MODULAR TEST SECTION
            Console.WriteLine("Done, it took " + (DateTime.Now.Subtract(startTime)));
            this.Invalidate();
        }

        public void LoadFunction(object sender, EventArgs e)
        {
            NeuralNet.RunNeuralNet();
        }
        public void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (NeuralNet.lastWasApple)
            {
                g.DrawImage(Image.FromFile("Tick.png"), new PointF(0, 200));
                this.textBoxWithSunglassesAndOutput.Text = NeuralNet.imageBeingSearched + " was an apple!";

            }
            else
            {
                g.DrawImage(Image.FromFile("Cross.png"), new PointF(0, 200));
                this.textBoxWithSunglassesAndOutput.Text = NeuralNet.imageBeingSearched + " wasn't an apple.";
            }
            g.DrawImage(ResizeImage((Bitmap)Image.FromFile(NeuralNet.imageBeingSearched), new Size(200,200)), new PointF(200, 200));

        }
        public static Bitmap ResizeImage(Bitmap imageBase,Size size)
        {
            return new Bitmap(imageBase, size);
        }
    }

}
