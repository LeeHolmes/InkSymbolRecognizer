using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Microsoft.Ink;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SymbolTrainer
{
	/// <summary>
	/// Summary description for SymbolTrainer.
	/// </summary>
    public class SymbolTrainer : System.Windows.Forms.Form
    {
        ArrayList al = new ArrayList();
        
        private System.Windows.Forms.TabControl tabControl1;

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button generateButton;
        
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public SymbolTrainer()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            tabControl1.Layout += new LayoutEventHandler(tabControl1_Layout);
            this.KeyUp += new KeyEventHandler(SymbolTrainer_KeyUp);
            this.KeyPreview = true;

            AddPage();
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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main() 
        {
            Application.Run(new SymbolTrainer());
        }

        private void AddPage(byte[] inkData, string text)
        {
            System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
            System.Windows.Forms.TabPage newPage = new System.Windows.Forms.TabPage();
            System.Windows.Forms.GroupBox groupBox = new System.Windows.Forms.GroupBox();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            Microsoft.Ink.InkPicture inkPicture = new Microsoft.Ink.InkPicture();
            System.Windows.Forms.GroupBox groupBox2 = new System.Windows.Forms.GroupBox();

            this.tabControl1.SuspendLayout();
            tabPage.SuspendLayout();
            groupBox.SuspendLayout();

            // Create the tab page
            tabPage.Controls.Add(groupBox2);
            tabPage.Controls.Add(inkPicture);
            tabPage.Controls.Add(groupBox);
            tabPage.Location = new System.Drawing.Point(4, 22);
            tabPage.Name = "tabPage1";
            tabPage.Size = new System.Drawing.Size(288, 334);
            tabPage.TabIndex = 0;
            tabPage.Text = CutText(text);
            
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("Remove", new EventHandler(tabPage_Remove)));
            tabPage.ContextMenu = contextMenu;

            // Group box to hold the text
            groupBox2.Controls.Add(textBox);
            groupBox2.Location = new System.Drawing.Point(0, 264);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(288, 56);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Text";

            // Text box to hold the text
            textBox.Location = new System.Drawing.Point(8, 24);
            textBox.Name = "textBox1";
            textBox.Size = new System.Drawing.Size(272, 20);
            textBox.TabIndex = 0;
            textBox.Text = text;
            textBox.KeyUp += new KeyEventHandler(textBox_KeyUp);

            // Ink Picture to hold the drawing
            inkPicture.BackColor = System.Drawing.SystemColors.ControlLightLight;
            inkPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            inkPicture.Location = new System.Drawing.Point(8, 32);
            inkPicture.MarginX = -2147483648;
            inkPicture.MarginY = -2147483648;
            inkPicture.Name = "inkPicture1";
            inkPicture.Size = new System.Drawing.Size(272, 216);
            inkPicture.TabIndex = 6;
            if(inkData.Length > 0)
                inkPicture.Ink.Load(inkData);

            // Group box to hold the drawing
            groupBox.Location = new System.Drawing.Point(0, 8);
            groupBox.Name = "groupBox1";
            groupBox.Size = new System.Drawing.Size(288, 248);
            groupBox.TabIndex = 5;
            groupBox.TabStop = false;
            groupBox.Text = "Symbol";

            // "New" page
            newPage.Location = new System.Drawing.Point(4, 22);
            newPage.Name = "newPage";
            newPage.Size = new System.Drawing.Size(288, 334);
            newPage.TabIndex = 1;
            newPage.Text = "New";

            // Remove the old "new" page
            if(tabControl1.Controls.Count >= 1)
                tabControl1.Controls.RemoveAt(tabControl1.Controls.Count - 1);

            // Add the new tab, and the new "new" page
            tabControl1.Controls.Add(tabPage);
            tabControl1.Controls.Add(newPage);

            this.tabControl1.ResumeLayout(false);
            tabPage.ResumeLayout(false);
            groupBox.ResumeLayout(false);
            this.Refresh();
        }

        private void AddPage()
        {
            AddPage(new byte[] {}, "Enter Text");
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(0, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(296, 360);
            this.tabControl1.TabIndex = 0;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(8, 384);
            this.saveButton.Name = "saveButton";
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(112, 384);
            this.loadButton.Name = "loadButton";
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "Load";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(208, 384);
            this.generateButton.Name = "generateButton";
            this.generateButton.TabIndex = 3;
            this.generateButton.Text = "Export";
            this.generateButton.Click += new EventHandler(generateButton_Click);
            // 
            // SymbolTrainer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 422);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SymbolTrainer";
            this.Text = "Symbol Trainer";
            this.ResumeLayout(false);

        }
        #endregion

        private void tabControl1_Layout(object sender, LayoutEventArgs e)
        {
            if(tabControl1.SelectedIndex == (tabControl1.TabCount - 1))
            {
                AddPage();
                tabControl1.SelectedIndex = (tabControl1.TabCount - 2);
            }
        }

        private string CutText(string inText)
        {
            if(inText.Length > 5)
                return inText.Substring(0,5) + "...";
            else
                return inText;
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            // A key has been pressed -- update the tab title
            tabControl1.TabPages[tabControl1.SelectedIndex].Text = 
                CutText(tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0].Controls[0].Text);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ArrayList tabs = new ArrayList();

            for(int counter = 0; counter < (tabControl1.TabPages.Count - 1); counter++)
            {
                byte[] inkData = ((InkPicture) tabControl1.TabPages[counter].Controls[1]).Ink.Save();
                TabData savedTab = new TabData(inkData, tabControl1.TabPages[counter].Controls[0].Controls[0].Text);
                tabs.Add(savedTab);
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Symbol Trainer files (*.str)|*.str|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.RestoreDirectory = true;
            
            if(saveDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveDialog.FileName;
                Stream outStream = File.OpenWrite(filename);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(outStream, tabs);
                outStream.Close();
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Symbol Trainer files (*.str)|*.str|All files (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;
            
            if(openDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openDialog.FileName;
                Stream inStream = File.OpenRead(filename);
                BinaryFormatter formatter = new BinaryFormatter();
            
                ArrayList loadedTabs = (ArrayList) formatter.Deserialize(inStream);
                inStream.Close();

                this.tabControl1.SuspendLayout();
                tabControl1.Controls.Clear();
                this.tabControl1.ResumeLayout(false);

                foreach(TabData currentTab in loadedTabs)
                    AddPage(currentTab.InkData, currentTab.TextPane);

                this.Text = "Symbol Trainer - " + filename;
            }
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            DLLInfo dllInfo = new DLLInfo();
            if(dllInfo.ShowDialog() == DialogResult.OK)
            {
                InkAnalyzer analyzer = new InkAnalyzer();

                for(int currentPage = 0; currentPage < (tabControl1.TabPages.Count - 1); currentPage++)
                {
                    Ink paneInk = ((InkPicture) tabControl1.TabPages[currentPage].Controls[1]).Ink;
                    string relatedText = tabControl1.TabPages[currentPage].Controls[0].Controls[0].Text;

                    double[] analysisVector = InkAnalyzer.ComputeStatistics(paneInk);
                    analyzer.AddItem(analysisVector, relatedText);
                }

                string results = analyzer.Generate(dllInfo.className.Text, dllInfo.savePath.Text, dllInfo.dllPath.Text);
                if(results.Length > 0)
                    MessageBox.Show(results);
                else
                    MessageBox.Show("Exported " + (tabControl1.TabPages.Count - 1) + " symbols.");
            }
        }

        private void tabPage_Remove(object sender, EventArgs e)
        {
            tabControl1.SuspendLayout();
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            tabControl1.ResumeLayout(false);

            if(tabControl1.TabPages.Count == 1)
                AddPage();

            this.Refresh();
        }

        private void SymbolTrainer_KeyUp(object sender, KeyEventArgs e)
        {
            if((e.KeyCode == Keys.F4) && (e.Modifiers == Keys.Control))
                tabPage_Remove(sender, e);
        }
    }
}
