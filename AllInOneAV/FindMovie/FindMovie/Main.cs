﻿using DataBaseManager.FindDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Model.FindModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace FindMovie
{
    public partial class Main : Form
    {
        Thread thread;
        List<Match> cacheMovies;

        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            cacheMovies = new List<Match>();
            RefreshCache();
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                listView1.Items.Clear();
                thread = new Thread(doFind);
                thread.Start();
            }
        }

        private void doFind()
        {
            doRecursive();
        }

        private void doRecursive()
        {
            listView1.BeginUpdate();

            foreach (var keyword in textBox1.Text.Split(','))
            {
                var tempKeyword = keyword.Trim().ToUpper();

                foreach (var movie in cacheMovies)
                {
                    if (movie.AvID.Contains(tempKeyword) || movie.AvID == tempKeyword)
                    {
                        ListViewItem lvi = new ListViewItem(tempKeyword);

                        var tempFi = new FileInfo(movie.Location);

                        lvi.SubItems.Add(FileSize.GetAutoSizeString(tempFi.Length, 2));
                        lvi.SubItems.Add(movie.Location);

                        listView1.Items.Add(lvi);
                    }
                }
            }

            listView1.EndUpdate();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            if (info.Item != null)
            {
                System.Diagnostics.Process.Start(@"" + info.Item.SubItems[2].Text);
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
                if (info.Item != null)
                {
                    var result = MessageBox.Show(string.Format("Do you want to delete {0}", info.Item.SubItems[2].Text), "Warning", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(info.Item.SubItems[2].Text);
                        ScanDataBaseManager.DeleteMatch(info.Item.SubItems[2].Text);
                        RefreshCache();
                    }
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    listView1.Items.Clear();
                    thread = new Thread(doFind);
                    thread.Start();
                }
            }
        }

        private void RefreshCache()
        {
            cacheMovies = FindDataBaseManager.GetAllMovies().OrderBy(x => x.AvID).ToList();
        }

        private void initButton_Click(object sender, EventArgs e)
        {
            RefreshCache();
        }
    }
}
