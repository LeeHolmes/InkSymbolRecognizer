using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SymbolTrainer
{
	/// <summary>
	/// Summary description for DLLInfo.
	/// </summary>
	public class DLLInfo : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox className;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox dllPath;
        private System.Windows.Forms.Button browseInk;
        private System.Windows.Forms.Button browseSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox savePath;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DLLInfo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
            this.label1 = new System.Windows.Forms.Label();
            this.className = new System.Windows.Forms.TextBox();
            this.Ok = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dllPath = new System.Windows.Forms.TextBox();
            this.browseInk = new System.Windows.Forms.Button();
            this.browseSave = new System.Windows.Forms.Button();
            this.savePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Class Name:";
            // 
            // className
            // 
            this.className.Location = new System.Drawing.Point(8, 24);
            this.className.Name = "className";
            this.className.Size = new System.Drawing.Size(272, 20);
            this.className.TabIndex = 1;
            this.className.Text = "SymbolRecognizer";
            // 
            // Ok
            // 
            this.Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Ok.Location = new System.Drawing.Point(208, 168);
            this.Ok.Name = "Ok";
            this.Ok.TabIndex = 2;
            this.Ok.Text = "OK";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Path to Microsoft.Ink.dll:";
            // 
            // dllPath
            // 
            this.dllPath.Location = new System.Drawing.Point(8, 72);
            this.dllPath.Name = "dllPath";
            this.dllPath.Size = new System.Drawing.Size(248, 20);
            this.dllPath.TabIndex = 4;
            this.dllPath.Text = "C:\\Program Files\\Microsoft Tablet PC Platform SDK\\Include\\Microsoft.Ink.dll";
            // 
            // browseInk
            // 
            this.browseInk.Location = new System.Drawing.Point(260, 71);
            this.browseInk.Name = "browseInk";
            this.browseInk.Size = new System.Drawing.Size(24, 23);
            this.browseInk.TabIndex = 5;
            this.browseInk.Text = "...";
            this.browseInk.Click += new System.EventHandler(this.browseInk_Click);
            // 
            // browseSave
            // 
            this.browseSave.Location = new System.Drawing.Point(260, 119);
            this.browseSave.Name = "browseSave";
            this.browseSave.Size = new System.Drawing.Size(24, 23);
            this.browseSave.TabIndex = 8;
            this.browseSave.Text = "...";
            this.browseSave.Click += new System.EventHandler(this.browseSave_Click);
            // 
            // savePath
            // 
            this.savePath.Location = new System.Drawing.Point(8, 120);
            this.savePath.Name = "savePath";
            this.savePath.Size = new System.Drawing.Size(248, 20);
            this.savePath.TabIndex = 7;
            this.savePath.Text = "C:\\SymbolRecognizer.dll";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Path to save:";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(112, 168);
            this.button1.Name = "button1";
            this.button1.TabIndex = 9;
            this.button1.Text = "Cancel";
            // 
            // DLLInfo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 198);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.browseSave);
            this.Controls.Add(this.savePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.browseInk);
            this.Controls.Add(this.dllPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.className);
            this.Controls.Add(this.label1);
            this.Name = "DLLInfo";
            this.Text = "Configure your DLL";
            this.ResumeLayout(false);

        }
		#endregion

        private void browseInk_Click(object sender, System.EventArgs e)
        {
            // Get path to ink DLL
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Please browse to the Microsoft.Ink.dll";
            openDialog.Filter = "Dynamic Link Libraries (*.dll)|*.dll|All files (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;
            
            if(openDialog.ShowDialog() == DialogResult.OK)
                dllPath.Text = openDialog.FileName;
        }

        private void browseSave_Click(object sender, System.EventArgs e)
        {
            // Recognize
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Dynamic Link Libraries (*.dll)|*.dll|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.RestoreDirectory = true;
            
            if(saveDialog.ShowDialog() == DialogResult.OK)
                savePath.Text = saveDialog.FileName;
        }
	}
}
