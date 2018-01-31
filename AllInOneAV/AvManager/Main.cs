using DataBaseManager.AvManagerDataBaseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvManager
{
    public partial class Main : Form
    {
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();
        private static List<FileInfo> srcFi = new List<FileInfo>();
        private static List<FileInfo> desFi = new List<FileInfo>();

        public Main()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void txtMoveSrc_Click(object sender, EventArgs e)
        {
            MoveSrcClick();
        }

        private void txtMoveDes_Click(object sender, EventArgs e)
        {
            MoveDesClick();
        }

        private void btnMoveMove_Click(object sender, EventArgs e)
        {
            MoveBtnClick();
        }

        private void btnMoveStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMoveDes.Text) && !string.IsNullOrEmpty(txtMoveSrc.Text))
            {
                StartBtnClick();
            }
        }

        private void StartBtnClick()
        {
            srcFi = new List<FileInfo>();
            lvMoveSrc.Items.Clear();

            if (!string.IsNullOrEmpty(txtMoveSrc.Text))
            {
                FileUtility.GetFilesRecursive(txtMoveSrc.Text, formats, excludes, srcFi, 200);

                lvMoveSrc.BeginUpdate();
                foreach (var f in desFi)
                {
                    ListViewItem lvi = new ListViewItem(f.Name);
                    lvi.SubItems.Add(FileSize.GetAutoSizeString(f.Length, 1));

                    if (desFi.Exists(x => x.Name.ToLower() == f.Name.ToLower()))
                    {
                        lvi.BackColor = Color.Red;
                    }

                    lvMoveSrc.Items.Add(lvi);
                }
                lvMoveSrc.EndUpdate();
            }
        }

        private void MoveSrcClick()
        {
            InitFD(fdMoveSrc, txtMoveSrc);
        }

        private void MoveBtnClick()
        {
            if (srcFi.Count > 0 && !string.IsNullOrEmpty(txtMoveDes.Text))
            {
                var des = txtMoveDes.Text;
                DateTime now = DateTime.Now;

                if (cbAutoReplace.Checked)
                {
                    foreach(var fi in srcFi)
                    {
                        var newFi = "";

                        if (desFi.Exists(x => x.Name == fi.Name))
                        {
                            var desF = desFi.Find(x => x.Name == fi.Name);

                            if (fi.Length >= desF.Length)
                            {
                                newFi = des + "/" + fi.Name;
                                File.Delete(desF.FullName);
                                MoveFile(fi.FullName, newFi, now);
                            }
                            else
                            {
                                File.Delete(fi.FullName);
                            }
                        }
                        else
                        {
                            newFi = des + "/" + fi.Name;
                            MoveFile(fi.FullName, newFi, now);
                        }

                        desFi.Add(new FileInfo(newFi));
                    }
                }
                else
                {

                }
            }
        }

        private void MoveFile(string src, string des, DateTime time)
        {
            File.Move(src, des);
            AvManagerDataBaseManager.InserMoveLog(src, des, time);
        }

        private void MoveDesClick()
        {
            InitFD(fdMoveDes, txtMoveDes);
            desFi = new List<FileInfo>();

            if (!string.IsNullOrEmpty(txtMoveDes.Text))
            {
                var files = Directory.GetFiles(txtMoveDes.Text);

                lvMoveDes.BeginUpdate();
                foreach (var f in files)
                {
                    FileInfo fi = new FileInfo(f);

                    ListViewItem lvi = new ListViewItem(fi.Name);
                    lvi.SubItems.Add(FileSize.GetAutoSizeString(fi.Length, 1));

                    lvMoveDes.Items.Add(lvi);
                    desFi.Add(fi);
                }
                lvMoveDes.EndUpdate();
            }
        }

        private void InitFD(FolderBrowserDialog fd, TextBox tb)
        {
            fd.RootFolder = Environment.SpecialFolder.MyComputer;

            if (fd.ShowDialog() == DialogResult.Yes || fd.ShowDialog() == DialogResult.OK)
            {
                tb.Text = fd.SelectedPath;
            }
        }
    }
}
