namespace PlayRecent
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.cLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBroswe = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.rbDateD = new System.Windows.Forms.RadioButton();
            this.rbDateA = new System.Windows.Forms.RadioButton();
            this.rbSizeA = new System.Windows.Forms.RadioButton();
            this.rbSizeD = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1178, 1444);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.listView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 71);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1178, 1373);
            this.panel3.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cLocation,
            this.cName,
            this.cSize});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1178, 1373);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // cLocation
            // 
            this.cLocation.Text = "Location";
            this.cLocation.Width = 300;
            // 
            // cName
            // 
            this.cName.Text = "Name";
            this.cName.Width = 400;
            // 
            // cSize
            // 
            this.cSize.Text = "Size";
            this.cSize.Width = 100;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbSizeA);
            this.panel2.Controls.Add(this.rbSizeD);
            this.panel2.Controls.Add(this.rbDateA);
            this.panel2.Controls.Add(this.rbDateD);
            this.panel2.Controls.Add(this.btnBroswe);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1178, 71);
            this.panel2.TabIndex = 0;
            // 
            // btnBroswe
            // 
            this.btnBroswe.Location = new System.Drawing.Point(673, 17);
            this.btnBroswe.Name = "btnBroswe";
            this.btnBroswe.Size = new System.Drawing.Size(93, 41);
            this.btnBroswe.TabIndex = 2;
            this.btnBroswe.Text = "OK";
            this.btnBroswe.UseVisualStyleBackColor = true;
            this.btnBroswe.Click += new System.EventHandler(this.btnBroswe_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(664, 26);
            this.textBox1.TabIndex = 1;
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // rbDateD
            // 
            this.rbDateD.AutoSize = true;
            this.rbDateD.Checked = true;
            this.rbDateD.Location = new System.Drawing.Point(871, 3);
            this.rbDateD.Name = "rbDateD";
            this.rbDateD.Size = new System.Drawing.Size(110, 24);
            this.rbDateD.TabIndex = 3;
            this.rbDateD.TabStop = true;
            this.rbDateD.Text = "Date Desc";
            this.rbDateD.UseVisualStyleBackColor = true;
            this.rbDateD.CheckedChanged += new System.EventHandler(this.rbDateD_CheckedChanged);
            // 
            // rbDateA
            // 
            this.rbDateA.AutoSize = true;
            this.rbDateA.Location = new System.Drawing.Point(987, 3);
            this.rbDateA.Name = "rbDateA";
            this.rbDateA.Size = new System.Drawing.Size(100, 24);
            this.rbDateA.TabIndex = 4;
            this.rbDateA.Text = "Date Asc";
            this.rbDateA.UseVisualStyleBackColor = true;
            this.rbDateA.CheckedChanged += new System.EventHandler(this.rbDateA_CheckedChanged);
            // 
            // rbSizeA
            // 
            this.rbSizeA.AutoSize = true;
            this.rbSizeA.Location = new System.Drawing.Point(987, 41);
            this.rbSizeA.Name = "rbSizeA";
            this.rbSizeA.Size = new System.Drawing.Size(96, 24);
            this.rbSizeA.TabIndex = 6;
            this.rbSizeA.Text = "Size Asc";
            this.rbSizeA.UseVisualStyleBackColor = true;
            this.rbSizeA.CheckedChanged += new System.EventHandler(this.rbSizeA_CheckedChanged);
            // 
            // rbSizeD
            // 
            this.rbSizeD.AutoSize = true;
            this.rbSizeD.Location = new System.Drawing.Point(871, 41);
            this.rbSizeD.Name = "rbSizeD";
            this.rbSizeD.Size = new System.Drawing.Size(106, 24);
            this.rbSizeD.TabIndex = 5;
            this.rbSizeD.Text = "Size Desc";
            this.rbSizeD.UseVisualStyleBackColor = true;
            this.rbSizeD.CheckedChanged += new System.EventHandler(this.rbSizeD_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 1444);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBroswe;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader cLocation;
        private System.Windows.Forms.ColumnHeader cName;
        private System.Windows.Forms.ColumnHeader cSize;
        private System.Windows.Forms.RadioButton rbDateA;
        private System.Windows.Forms.RadioButton rbDateD;
        private System.Windows.Forms.RadioButton rbSizeA;
        private System.Windows.Forms.RadioButton rbSizeD;
    }
}

