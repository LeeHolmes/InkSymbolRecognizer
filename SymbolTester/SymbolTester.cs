using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Microsoft.Ink;

namespace SymbolTester
{
	/// <summary>
	/// Summary description for SymbolTester.
	/// </summary>
	public class SymbolTester : System.Windows.Forms.Form
	{
        private Microsoft.Ink.InkPicture inkPicture1;
        private System.Windows.Forms.Button recognizeButton;
        private System.Windows.Forms.Button clearButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SymbolTester()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

        [STAThread]
        static void Main() 
        {
            Application.Run(new SymbolTester());
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.inkPicture1 = new Microsoft.Ink.InkPicture();
            this.recognizeButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inkPicture1
            // 
            this.inkPicture1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.inkPicture1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inkPicture1.Location = new System.Drawing.Point(16, 8);
            this.inkPicture1.MarginX = -2147483648;
            this.inkPicture1.MarginY = -2147483648;
            this.inkPicture1.Name = "inkPicture1";
            this.inkPicture1.Size = new System.Drawing.Size(264, 216);
            this.inkPicture1.TabIndex = 0;
            // 
            // recognizeButton
            // 
            this.recognizeButton.Location = new System.Drawing.Point(200, 232);
            this.recognizeButton.Name = "recognizeButton";
            this.recognizeButton.TabIndex = 1;
            this.recognizeButton.Text = "Recognize";
            this.recognizeButton.Click += new System.EventHandler(this.recognizeButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(112, 232);
            this.clearButton.Name = "clearButton";
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Clear";
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // SymbolTester
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.recognizeButton);
            this.Controls.Add(this.inkPicture1);
            this.Name = "SymbolTester";
            this.Text = "Symbol Recognition Tester";
            this.ResumeLayout(false);

        }
		#endregion

        private void recognizeButton_Click(object sender, System.EventArgs e)
        {
            Ink currentInk = inkPicture1.Ink;
            string result = Microsoft.Ink.SymbolRecognizers.SymbolRecognizer.Recognize(currentInk);
            MessageBox.Show(result);
        }

        private void clearButton_Click(object sender, System.EventArgs e)
        {
            inkPicture1.InkEnabled = false;
            Ink newInk = new Ink();
            inkPicture1.Ink = newInk;
            inkPicture1.InkEnabled = true;
            this.Refresh();
        }
	}
}
