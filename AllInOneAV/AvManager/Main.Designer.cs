namespace AvManager
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnMoveMove = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvMoveDes = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvMoveSrc = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnMoveStart = new System.Windows.Forms.Button();
            this.txtMoveDes = new System.Windows.Forms.TextBox();
            this.txtMoveSrc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.fdMoveSrc = new System.Windows.Forms.FolderBrowserDialog();
            this.fdMoveDes = new System.Windows.Forms.FolderBrowserDialog();
            this.cbAutoReplace = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1978, 1144);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1970, 1111);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MoveFile";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1964, 1105);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.tableLayoutPanel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 120);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1964, 985);
            this.panel3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cbAutoReplace);
            this.panel4.Controls.Add(this.btnMoveMove);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 917);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1964, 68);
            this.panel4.TabIndex = 1;
            // 
            // btnMoveMove
            // 
            this.btnMoveMove.Location = new System.Drawing.Point(1841, 10);
            this.btnMoveMove.Name = "btnMoveMove";
            this.btnMoveMove.Size = new System.Drawing.Size(114, 49);
            this.btnMoveMove.TabIndex = 1;
            this.btnMoveMove.Text = "Move";
            this.btnMoveMove.UseVisualStyleBackColor = true;
            this.btnMoveMove.Click += new System.EventHandler(this.btnMoveMove_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1964, 985);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvMoveDes);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(985, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(976, 979);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Des";
            // 
            // lvMoveDes
            // 
            this.lvMoveDes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvMoveDes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMoveDes.Location = new System.Drawing.Point(3, 22);
            this.lvMoveDes.Name = "lvMoveDes";
            this.lvMoveDes.Size = new System.Drawing.Size(970, 954);
            this.lvMoveDes.TabIndex = 1;
            this.lvMoveDes.UseCompatibleStateImageBehavior = false;
            this.lvMoveDes.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "FileName";
            this.columnHeader3.Width = 500;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Size";
            this.columnHeader4.Width = 100;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvMoveSrc);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(976, 979);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Src";
            // 
            // lvMoveSrc
            // 
            this.lvMoveSrc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvMoveSrc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMoveSrc.Location = new System.Drawing.Point(3, 22);
            this.lvMoveSrc.Name = "lvMoveSrc";
            this.lvMoveSrc.Size = new System.Drawing.Size(970, 954);
            this.lvMoveSrc.TabIndex = 0;
            this.lvMoveSrc.UseCompatibleStateImageBehavior = false;
            this.lvMoveSrc.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "FileName";
            this.columnHeader1.Width = 500;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.Width = 100;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnMoveStart);
            this.panel2.Controls.Add(this.txtMoveDes);
            this.panel2.Controls.Add(this.txtMoveSrc);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1964, 120);
            this.panel2.TabIndex = 0;
            // 
            // btnMoveStart
            // 
            this.btnMoveStart.Location = new System.Drawing.Point(1014, 24);
            this.btnMoveStart.Name = "btnMoveStart";
            this.btnMoveStart.Size = new System.Drawing.Size(123, 74);
            this.btnMoveStart.TabIndex = 1;
            this.btnMoveStart.Text = "Start";
            this.btnMoveStart.UseVisualStyleBackColor = true;
            this.btnMoveStart.Click += new System.EventHandler(this.btnMoveStart_Click);
            // 
            // txtMoveDes
            // 
            this.txtMoveDes.Location = new System.Drawing.Point(53, 75);
            this.txtMoveDes.Name = "txtMoveDes";
            this.txtMoveDes.Size = new System.Drawing.Size(923, 26);
            this.txtMoveDes.TabIndex = 2;
            this.txtMoveDes.Click += new System.EventHandler(this.txtMoveDes_Click);
            // 
            // txtMoveSrc
            // 
            this.txtMoveSrc.Location = new System.Drawing.Point(53, 21);
            this.txtMoveSrc.Name = "txtMoveSrc";
            this.txtMoveSrc.Size = new System.Drawing.Size(923, 26);
            this.txtMoveSrc.TabIndex = 1;
            this.txtMoveSrc.Click += new System.EventHandler(this.txtMoveSrc_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Des";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Src";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1970, 1111);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rename";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // fdMoveSrc
            // 
            this.fdMoveSrc.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // cbAutoReplace
            // 
            this.cbAutoReplace.AutoSize = true;
            this.cbAutoReplace.Location = new System.Drawing.Point(1613, 35);
            this.cbAutoReplace.Name = "cbAutoReplace";
            this.cbAutoReplace.Size = new System.Drawing.Size(206, 24);
            this.cbAutoReplace.TabIndex = 3;
            this.cbAutoReplace.Text = "AutoReplaceSmallerFile";
            this.cbAutoReplace.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1978, 1144);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtMoveDes;
        private System.Windows.Forms.TextBox txtMoveSrc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMoveStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvMoveDes;
        private System.Windows.Forms.ListView lvMoveSrc;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnMoveMove;
        private System.Windows.Forms.FolderBrowserDialog fdMoveSrc;
        private System.Windows.Forms.FolderBrowserDialog fdMoveDes;
        private System.Windows.Forms.CheckBox cbAutoReplace;
    }
}

