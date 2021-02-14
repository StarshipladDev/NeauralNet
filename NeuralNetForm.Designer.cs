using System.Windows.Forms;

namespace NeuralNet
{
    partial class NeuralNetForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Button button;
        private System.Windows.Forms.TextBox funkyTextBox;
        private System.Windows.Forms.TextBox textBoxWithSunglassesAndOutput;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Neural Net";
            funkyTextBox = new TextBox();
            funkyTextBox.Size = new System.Drawing.Size(400, 50);
            funkyTextBox.Location = new System.Drawing.Point(100, 100);
            funkyTextBox.Text ="Please enter the number of the image you would like to test:";

            textBoxWithSunglassesAndOutput = new TextBox();
            textBoxWithSunglassesAndOutput.Size = new System.Drawing.Size(400, 50);
            textBoxWithSunglassesAndOutput.Location = new System.Drawing.Point(100, 150);
            button = new Button();

            button.Text = "Test Apple";
            button.Size = new System.Drawing.Size(100, 100);
            button.Location = new System.Drawing.Point(500, 100);
            button.Click += this.ClickFunction;

            this.Controls.Add(button);
            this.Controls.Add(textBoxWithSunglassesAndOutput);
            this.Controls.Add(funkyTextBox);
            this.Load += this.LoadFunction;
            this.Paint += this.OnPaint;


        }

        #endregion
    }
}

